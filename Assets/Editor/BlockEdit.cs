using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// MapTool의 BlockEdit 관련 함수와 변수
/// </summary>
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

    public enum ChangeMode
    {
        None,
        Change
    }
    //특정 오브젝트를 선택한 상태인가 아닌가? (잡고 잇는 상태인가 아닌가?)
    /// Is a specific object selected?
    public static SelectState selectState = SelectState.NotSelect;
    //Drag상태인가 아닌가?
    /// Is it a Drag State?
    public static MouseState mouseState = MouseState.None;
    //Change 모드 설정
    /// Setting Change Mode
    public static ChangeMode changeMode = ChangeMode.None;
    //들고 있는 오브젝트
    /// Holding object
    public static GameObject selectedObject = null;
    //초기 마우스 위치 저장
    /// Save initializable mouse position 
    protected Vector2 firstMousePos = Vector2.one;
    //부모 오브젝트 변수
    /// Parent object value
    protected GameObject objectParent;
    //오브젝트 리스트 중에 현재 인덱스
    /// Current index in the object list
    protected int selectIndex = 0;
    //Map 변수
    /// map value
    protected MapTool map;
    /// <summary>
    /// 바닥 생성 함수
    /// A function that creates floor
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
    /// A function that deletes all objects placed on the map
    /// </summary>
    protected void ClearMapObjects()
    {
        DestroyImmediate(objectParent);
    }
    /// <summary>
    /// 오브젝트 선택 함수 (Left Click)
    /// Object Selection Function (Left Click)
    /// </summary>
    protected void SelectObject()
    {
        //Select 상태
        //Select State
        selectState = SelectState.Select;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Tile"))
            {
                //레이어를 SelectObject로 바꾸자
                /// Let's change the layer to SelectObject
                hit.transform.gameObject.layer = LayerMask.NameToLayer("SelectObject");
                //selectedObject에 클릭한 물체를 넣어두자
                ///Put the object you clicked on in the selectedObject
                selectedObject = hit.transform.gameObject;
                //현재 들고 있는 오브젝트가 리스트 중 몇 번째 인덱스인가
                ///Which index of the list is the object you are holding?
                for (int i = 0; i < BlockEditorWindow.ObjectList.Count; i++)
                {
                    if (BlockEditorWindow.ObjectList[i].name == selectedObject.name)
                    {
                        selectIndex = i;
                        break;
                    }
                }
            }
            else
            {
                selectState = SelectState.NotSelect;
            }
        }
    }

    /// <summary>
    /// 오브젝트 배치 시키는 함수 (잡은 오브젝트가 있는 상태로 Left Click)
    /// A function to place objects (Left Click while holding objects)
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
    /// A function that moves an object (moving the mouse with the object you hold)
    /// </summary>
    protected void MovingObject()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        //오브젝트 격자형태로 움직이기
        /// Moving objects in grid form
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tile")
                || hit.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                //닿은 격자에 넣어두자
                Vector3 p = new Vector3((int)hit.point.x, hit.point.y, (int)hit.point.z);
                selectedObject.transform.position = p;
            }
        }
    }

    /// <summary>
    /// 오브젝트 삭제하는 함수 (Left Control + Left Click)
    /// A function that delete object
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
    /// A function that generates multiple objects by dragging themselves
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
    /// Z | C를 누르면 Object 변경
    /// Press Z | C to Change Object
    /// </summary>
    protected private void ChangeObject()
    {
        Event e = Event.current;
        #region Scroll(Legacy)
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

        #region KeyBoard(Z | C)
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.C)
        {
            //인덱스 증가
            ///Increase Index
            selectIndex = selectIndex + 1 > BlockEditorWindow.ObjectList.Count - 1 ? 0 : selectIndex + 1;
            ChaingSelectObject(selectIndex);
        }
        else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
        {
            //인덱스 감소
            ///Decrease Index
            selectIndex = selectIndex - 1 < 0 ? BlockEditorWindow.ObjectList.Count - 1 : selectIndex - 1;
            ChaingSelectObject(selectIndex);
        }
        #endregion
    }
    /// <summary>
    /// 오브젝트 변경 시 선택한 오브젝트 넣기
    /// Insert selected object when changing object
    /// </summary>
    protected void ChaingSelectObject(int index)
    {

        //기존의 잡고 있는 오브젝트는 삭제
        ///Delete an existing holding object
        DestroyImmediate(selectedObject);
        //선택한 오브젝트의 인덱스 변경하여 생성
        ///Change Index and Load Object
        Object resource = Resources.Load<GameObject>("Editor/" + BlockEditorWindow.ObjectList[index].name);
        //프리팹 생성
        ///Create Prefab
        GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(resource);
        //선택한 오브젝트 변경
        ///Change SelectedObject value
        selectedObject = instantiate;
        //오브젝트 셋팅
        ///Setting Object
        EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
    }
}
