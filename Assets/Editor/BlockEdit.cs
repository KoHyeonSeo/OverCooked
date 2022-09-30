using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BlockEditWithEditor;

/// <summary>
/// MapTool�� BlockEdit ���� �Լ��� ����
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
    //Ư�� ������Ʈ�� ������ �����ΰ� �ƴѰ�? (��� �մ� �����ΰ� �ƴѰ�?)
    protected SelectState selectState = SelectState.NotSelect;
    //Drag�����ΰ� �ƴѰ�?
    protected MouseState mouseState = MouseState.None;
    //�� ��� ����
    protected WheelMode wheelMode = WheelMode.None;
    //��� �ִ� ������Ʈ
    protected GameObject selectedObject = null;
    //�ʱ� ���콺 ��ġ ����
    protected Vector2 firstMousePos = Vector2.one;
    //�θ� ������Ʈ ����
    protected GameObject objectParent;
    //������Ʈ ����Ʈ �߿� ���� �ε���
    protected int selectIndex = 0;
    //Map ����
    protected MapTool map;
    /// <summary>
    /// �ٴ� ���� �Լ�
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
    /// �ʿ� ��ġ�� ������Ʈ ��� �����ϴ� �Լ�
    /// </summary>
    protected void ClearMapObjects()
    {
        DestroyImmediate(objectParent);
    }
    /// <summary>
    /// ������Ʈ ���� �Լ� (Left Click)
    /// </summary>
    protected void SelectObject()
    {
        //Select ����
        selectState = SelectState.Select;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Tile"))
            {
                //���̾ SelectObject�� �ٲ���
                hit.transform.gameObject.layer = LayerMask.NameToLayer("SelectObject");
                //selectedObject�� Ŭ���� ��ü�� �־����
                selectedObject = hit.transform.gameObject;
                //���� ��� �ִ� ������Ʈ�� ����Ʈ �� �� ��° �ε����ΰ�
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
    /// ������Ʈ ��ġ ��Ű�� �Լ� (���� ������Ʈ�� �ִ� ���·� Left Click)
    /// </summary>
    protected void CollocatingObject()
    {
        //Not Select���·� ����
        if (selectedObject)
        {
            selectState = SelectState.NotSelect;
            selectedObject.layer = LayerMask.NameToLayer("Default");
            selectedObject = null;
        }
    }
    /// <summary>
    /// ������Ʈ �����̰� �ϴ� �Լ� (���� ������Ʈ�� �ִ� ���·� ���콺 �����̱�)
    /// </summary>
    protected void MovingObject()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        //������Ʈ �������·� �����̱�
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tile")
                || hit.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                //���� ���ڿ� �־���� Layer�� SelectObject�ε� �ɷ����Ѵ�. �� ���̾��
                Vector3 p = new Vector3((int)hit.point.x, hit.point.y, (int)hit.point.z);
                selectedObject.transform.position = p;
            }
        }
    }

    /// <summary>
    /// ������Ʈ �����ϴ� �Լ� (Left Control + Left Click)
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
    /// �巡�� �Ͽ� ���� ������Ʈ �����ϴ� �Լ�
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
    /// ���� ������ ������Ʈ�� �ٲ��.
    /// ���� ������ ������Ʈ�� �ٲ��.
    /// </summary>
    protected private void ChangeObject()
    {
        Event e = Event.current;
        #region Scroll(Regacy)
        //if(e.type == EventType.ScrollWheel)
        //{
        //    if(e.delta.y > 0)
        //    {
        //        //�ε��� ����
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
            //�ε��� ����
            selectIndex = selectIndex + 1 > BlockEditor.ObjectList.Count - 1 ? 0 : selectIndex + 1;
            ChaingSelectObject(selectIndex);
        }
        else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow)
        {
            //�ε��� ����
            selectIndex = selectIndex - 1 < 0 ? BlockEditor.ObjectList.Count - 1 : selectIndex - 1;
            ChaingSelectObject(selectIndex);
        }
        #endregion
    }
    /// <summary>
    /// ������Ʈ ���� �� ������ ������Ʈ �ֱ�
    /// </summary>
    protected void ChaingSelectObject(int index)
    {

        //������ ��� �ִ� ������Ʈ�� ����
        DestroyImmediate(selectedObject);
        //������ ������Ʈ�� �ε��� �����Ͽ� ����
        Object resource = Resources.Load<GameObject>("Editor/" + BlockEditor.ObjectList[index].name);
        //������ ����
        GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(resource);
        //������ ������Ʈ ����
        selectedObject = instantiate;
        //������Ʈ ����
        BlockEditor.ObjectSetting(instantiate, Vector3.zero, objectParent.transform);
    }
}
