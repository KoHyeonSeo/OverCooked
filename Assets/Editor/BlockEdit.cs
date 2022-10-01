using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// MapTool�� BlockEdit ���� �Լ��� ����
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
    //Ư�� ������Ʈ�� ������ �����ΰ� �ƴѰ�? (��� �մ� �����ΰ� �ƴѰ�?)
    /// Is a specific object selected?
    public static SelectState selectState = SelectState.NotSelect;
    //Drag�����ΰ� �ƴѰ�?
    /// Is it a Drag State?
    public static MouseState mouseState = MouseState.None;
    //Change ��� ����
    /// Setting Change Mode
    public static ChangeMode changeMode = ChangeMode.None;
    //��� �ִ� ������Ʈ
    /// Holding object
    public static GameObject selectedObject = null;
    //�ʱ� ���콺 ��ġ ����
    /// Save initializable mouse position 
    protected Vector2 firstMousePos = Vector2.one;
    //�θ� ������Ʈ ����
    /// Parent object value
    protected GameObject objectParent;
    //������Ʈ ����Ʈ �߿� ���� �ε���
    /// Current index in the object list
    protected int selectIndex = 0;
    //Map ����
    /// map value
    protected MapTool map;
    /// <summary>
    /// �ٴ� ���� �Լ�
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
    /// �ʿ� ��ġ�� ������Ʈ ��� �����ϴ� �Լ�
    /// A function that deletes all objects placed on the map
    /// </summary>
    protected void ClearMapObjects()
    {
        DestroyImmediate(objectParent);
    }
    /// <summary>
    /// ������Ʈ ���� �Լ� (Left Click)
    /// Object Selection Function (Left Click)
    /// </summary>
    protected void SelectObject()
    {
        //Select ����
        //Select State
        selectState = SelectState.Select;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Tile"))
            {
                //���̾ SelectObject�� �ٲ���
                /// Let's change the layer to SelectObject
                hit.transform.gameObject.layer = LayerMask.NameToLayer("SelectObject");
                //selectedObject�� Ŭ���� ��ü�� �־����
                ///Put the object you clicked on in the selectedObject
                selectedObject = hit.transform.gameObject;
                //���� ��� �ִ� ������Ʈ�� ����Ʈ �� �� ��° �ε����ΰ�
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
    /// ������Ʈ ��ġ ��Ű�� �Լ� (���� ������Ʈ�� �ִ� ���·� Left Click)
    /// A function to place objects (Left Click while holding objects)
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
    /// A function that moves an object (moving the mouse with the object you hold)
    /// </summary>
    protected void MovingObject()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        //������Ʈ �������·� �����̱�
        /// Moving objects in grid form
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tile")
                || hit.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                //���� ���ڿ� �־����
                Vector3 p = new Vector3((int)hit.point.x, hit.point.y, (int)hit.point.z);
                selectedObject.transform.position = p;
            }
        }
    }

    /// <summary>
    /// ������Ʈ �����ϴ� �Լ� (Left Control + Left Click)
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
    /// �巡�� �Ͽ� ���� ������Ʈ �����ϴ� �Լ�
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
    /// Z | C�� ������ Object ����
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

        #region KeyBoard(Z | C)
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.C)
        {
            //�ε��� ����
            ///Increase Index
            selectIndex = selectIndex + 1 > BlockEditorWindow.ObjectList.Count - 1 ? 0 : selectIndex + 1;
            ChaingSelectObject(selectIndex);
        }
        else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
        {
            //�ε��� ����
            ///Decrease Index
            selectIndex = selectIndex - 1 < 0 ? BlockEditorWindow.ObjectList.Count - 1 : selectIndex - 1;
            ChaingSelectObject(selectIndex);
        }
        #endregion
    }
    /// <summary>
    /// ������Ʈ ���� �� ������ ������Ʈ �ֱ�
    /// Insert selected object when changing object
    /// </summary>
    protected void ChaingSelectObject(int index)
    {

        //������ ��� �ִ� ������Ʈ�� ����
        ///Delete an existing holding object
        DestroyImmediate(selectedObject);
        //������ ������Ʈ�� �ε��� �����Ͽ� ����
        ///Change Index and Load Object
        Object resource = Resources.Load<GameObject>("Editor/" + BlockEditorWindow.ObjectList[index].name);
        //������ ����
        ///Create Prefab
        GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(resource);
        //������ ������Ʈ ����
        ///Change SelectedObject value
        selectedObject = instantiate;
        //������Ʈ ����
        ///Setting Object
        EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
    }
}
