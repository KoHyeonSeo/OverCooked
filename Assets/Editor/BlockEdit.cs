using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BlockEditWithEditor;

/// <summary>
/// MapTool의 BlockEdit 관련 함수와 변수
/// </summary>
[CustomEditor(typeof(MapTool))]
public class BlockEdit : Editor
{
    public enum SelectState
    {
        Select,
        NotSelect
    }

    public enum MouseState
    {
        None,
        Drag
    }

    public enum WheelMode
    {
        None,
        Change
    }
    //특정 오브젝트를 선택한 상태인가 아닌가? (잡고 잇는 상태인가 아닌가?)
    protected SelectState selectState = SelectState.NotSelect;
    //Drag상태인가 아닌가?
    protected MouseState mouseState = MouseState.None;
    //휠 모드 설정
    protected WheelMode wheelMode = WheelMode.None;
    //들고 있는 오브젝트
    protected GameObject selectedObject = null;
    //초기 마우스 위치 저장
    protected Vector2 firstMousePos = Vector2.one;
    //부모 오브젝트 변수
    protected GameObject objectParent;
    //오브젝트 리스트 중에 현재 인덱스
    protected int selectIndex = 0;
    //Map 변수
    protected MapTool map;
    /// <summary>
    /// 바닥 생성 함수
    /// </summary>
    protected void CreateFloor()
    {
        GameObject floor = GameObject.Find("Tile");
        if (floor)
        {
            DestroyImmediate(floor);
        }
        floor = (GameObject)PrefabUtility.InstantiatePrefab(map.floorTile);
        floor.transform.localScale = new Vector3(map.tileX, 1, map.tileZ);
    }
    /// <summary>
    /// 맵에 배치한 오브젝트 모두 삭제하는 함수
    /// </summary>
    protected void ClearMapObjects()
    {
        DestroyImmediate(objectParent);
    }
    /// <summary>
    /// 오브젝트 선택 함수 (Left Click)
    /// </summary>
    protected void SelectObject()
    {
        //Select 상태
        selectState = SelectState.Select;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Tile"))
            {
                //레이어를 SelectObject로 바꾸자
                hit.transform.gameObject.layer = LayerMask.NameToLayer("SelectObject");
                //selectedObject에 클릭한 물체를 넣어두자
                selectedObject = hit.transform.gameObject;
                //현재 들고 있는 오브젝트가 리스트 중 몇 번째 인덱스인가
                for (int i = 0; i < BlockEditor.ObjectList.Count; i++)
                {
                    if (BlockEditor.ObjectList[i].name == selectedObject.name)
                    {
                        selectIndex = i;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 오브젝트 배치 시키는 함수 (잡은 오브젝트가 있는 상태로 Left Click)
    /// </summary>
    protected void CollocatingObject()
    {
        //Not Select상태로 변경
        if (selectedObject)
        {
            selectState = SelectState.NotSelect;
            selectedObject.layer = LayerMask.NameToLayer("Default");
            selectedObject = null;
        }
    }
    /// <summary>
    /// 오브젝트 움직이게 하는 함수 (잡은 오브젝트가 있는 상태로 마우스 움직이기)
    /// </summary>
    protected void MovingObject()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        //오브젝트 격자형태로 움직이기
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tile")
                || hit.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                //닿은 격자에 넣어두자 Layer는 SelectObject인데 걸러야한다. 이 레이어는
                Vector3 p = new Vector3((int)hit.point.x, hit.point.y, (int)hit.point.z);
                selectedObject.transform.position = p;
            }
        }
    }

    /// <summary>
    /// 오브젝트 삭제하는 함수 (Left Control + Left Click)
    /// </summary>
    protected void DeleteObject()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Tile"))
            {
                DestroyImmediate(hit.transform.gameObject);
            }
        }
    }

    /// <summary>
    /// 드래그 하여 여러 오브젝트 생성하는 함수
    /// </summary>
    protected void DragAndCreateObjects()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        if (!selectedObject)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Tile"))
                {
                    selectedObject = hit.transform.gameObject;
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tile"))
                {
                    Object resource = Resources.Load<GameObject>("Editor/" + selectedObject.name);

                    GameObject instantiate = Instantiate(resource as GameObject);
                    instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
                    instantiate.transform.position = new Vector3((int)hit.point.x, hit.point.y, (int)hit.point.z);
                    instantiate.transform.parent = objectParent.transform;
                }
            }
        }
    }
    /// <summary>
    /// 휠을 돌리면 오브젝트가 바뀐다.
    /// 휠을 돌리면 오브젝트가 바뀐다.
    /// </summary>
    protected private void ChangeObject()
    {
        Event e = Event.current;
        #region Scroll(Regacy)
        //if(e.type == EventType.ScrollWheel)
        //{
        //    if(e.delta.y > 0)
        //    {
        //        //인덱스 감소
        //        selectIndex = selectIndex - 1 < 0 ? BlockEditor.ObjectList.Count - 1 : selectIndex - 1;
        //        ChaingSelectObject(selectIndex);
        //    }
        //    else
        //    {
        //        selectIndex = selectIndex + 1 > BlockEditor.ObjectList.Count - 1 ? 0 : selectIndex + 1;
        //        ChaingSelectObject(selectIndex);
        //    }
        //}
        #endregion

        #region KeyBoardArrow
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.RightArrow)
        {
            //인덱스 증가
            selectIndex = selectIndex + 1 > BlockEditor.ObjectList.Count - 1 ? 0 : selectIndex + 1;
            ChaingSelectObject(selectIndex);
        }
        else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow)
        {
            //인덱스 감소
            selectIndex = selectIndex - 1 < 0 ? BlockEditor.ObjectList.Count - 1 : selectIndex - 1;
            ChaingSelectObject(selectIndex);
        }
        #endregion
    }
    /// <summary>
    /// 오브젝트 변경 시 선택한 오브젝트 넣기
    /// </summary>
    protected void ChaingSelectObject(int index)
    {

        //기존의 잡고 있는 오브젝트는 삭제
        DestroyImmediate(selectedObject);
        //선택한 오브젝트의 인덱스 변경하여 생성
        Object resource = Resources.Load<GameObject>("Editor/" + BlockEditor.ObjectList[index].name);
        //프리팹 생성
        GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(resource);
        //선택한 오브젝트 변경
        selectedObject = instantiate;
        //오브젝트 셋팅
        BlockEditor.ObjectSetting(instantiate, Vector3.zero, objectParent.transform);
    }
}
