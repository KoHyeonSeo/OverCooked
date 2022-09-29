using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands;
using Unity.VisualScripting;

public class BlockEditor : EditorWindow
{
    
    static GameObject map;
    public static List<Object> ObjectList = new List<Object>();
    Vector2 scrollPosition;
    
    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        var window = GetWindow<BlockEditor>();
        window.maxSize = window.minSize = new Vector2(200, 500);
        map = GameObject.Find("Map");
        Object resource_table = Resources.Load<GameObject>("Editor/Table");
        Object resource_trash = Resources.Load<GameObject>("Editor/Trash Can");
        Object resource_FireExtinguisher = Resources.Load<GameObject>("Editor/FireExtinguisher");

        ObjectList.Add(resource_table);
        ObjectList.Add(resource_trash);
        ObjectList.Add(resource_FireExtinguisher);

    }

    private void OnGUI()
    {
        Event e = Event.current;

        GUILayout.Label("Interact Object", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.BeginScrollView
            (scrollPosition,
            GUILayout.MinWidth(100),
            GUILayout.MaxWidth(300),
            GUILayout.MinHeight(220),
            GUILayout.MaxHeight(700));
        GameObject objectParent = GameObject.Find("Object_Parent");
        if (!objectParent)
        {
            objectParent = new GameObject();
            objectParent.name = "Object_Parent";
        }

        //������Ʈ ������
        GUILayout.Label("Table");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[0])))
        {
            GameObject instantiate = Instantiate(ObjectList[0] as GameObject);
            ObjectSetting(instantiate, Vector3.zero, objectParent.transform);
        }


        GUILayout.Label("Other", EditorStyles.boldLabel); 

        GUILayout.Label("Trash Can");

        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[1])))
        {
            GameObject instantiate = Instantiate(ObjectList[1] as GameObject);
            ObjectSetting(instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Fire Extinguisher");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[2])))
        {
            GameObject instantiate = Instantiate(ObjectList[2] as GameObject);
            ObjectSetting(instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
    }
    public static void ObjectSetting(GameObject instantiate, Vector3 pos, Transform parent)
    {
        EditorGUIUtility.PingObject(map);
        Selection.activeGameObject = map;
        instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
        instantiate.transform.position = pos;
        instantiate.transform.parent = parent;
    }
}


[CustomEditor(typeof(Map))]
public class BlockEditWithEditor : Editor
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

    //������ �����ΰ�?
    SelectState selectState = SelectState.NotSelect;
    //Drag�����ΰ� �ƴѰ�?
    MouseState mouseState = MouseState.None;
    //��� �ִ� ������Ʈ
    GameObject selectedObject = null;
    //Map ����
    Map map;
    //�ʱ� ���콺 ��ġ ����
    Vector2 firstMousePos = Vector2.one;
    //�θ� ������Ʈ ����
    GameObject objectParent;
    //������Ʈ ����Ʈ �߿� ���� �ε���
    int selectIndex = 0;

    private void OnEnable()
    {
        map = (Map)target;
        objectParent = GameObject.Find("Object_Parent");
    }
    //��Ŭ���ϸ� ���� & ��ġ ���� ����
    //��Ŭ���ϰ� �巡�� -> ������ ����
    //��Ŭ���ؼ� ���� �����϶�, ���콺 �� -> ��ü ����

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        map.tileX = EditorGUILayout.IntField("Ÿ�� ����", map.tileX);
        map.tileZ = EditorGUILayout.IntField("Ÿ�� ����", map.tileZ);

        map.tileX = Mathf.Clamp(map.tileX, 1, 500);
        map.tileZ = Mathf.Clamp(map.tileZ, 1, 500);

        map.floorTile = (GameObject)EditorGUILayout.ObjectField("Ÿ��", map.floorTile, typeof(GameObject), false);

        map.dragDistance = EditorGUILayout.FloatField("�巡�� Ȱ��ȭ �Ÿ�", map.dragDistance);

        if(GUILayout.Button("�ٴ� ����"))
        {
            CreateFloor();
        }
        EditorGUILayout.Space();

        if(GUILayout.Button("��� ��� ����"))
        {
            bool isCancel = EditorUtility.DisplayDialog(" ���� ", "\n������ ��� �����Ͻðڽ��ϱ�?", "OK", "CANCEL");
            if(isCancel)
                ClearMapObjects();
        }
    }
    private void OnSceneGUI()
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);

        Event e = Event.current;

        //�巡�� �Ÿ� ����� ���� �ʱ� position�� ����
        if (e.type == EventType.MouseDown && !e.control)
        {
            firstMousePos = e.mousePosition;
        }
        //������Ʈ �����ϱ�
        else if(e.type == EventType.MouseDown && e.control)
        {
            DeleteObject();
        }
        //================������Ʈ Ŭ�� �̺�Ʈ====================
        //�����ٰ� �ٷ� ���� �� ������Ʈ ���� �Ǵ� ��ġ(None State)
        if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.None)
        {
            if (selectState == SelectState.NotSelect)
            {
                //Object ���� �Լ�
                SelectObject();
            }
            else
            {
                //Object ��ġ �Լ�
                CollocatingObject();
            }
            Debug.Log("����");
        }
        //��� ���� ���·� �¿�� �����δٸ� ��ü �������� �ø��� (Drag�� �ٷ� Drag ���·�)
        else if (e.type == EventType.MouseDrag && e.button == 0
            && (mouseState == MouseState.Drag
            || mouseState == MouseState.None))
        {
            if (mouseState == MouseState.None)
            {
                if (Vector2.Distance(e.mousePosition, firstMousePos) > map.dragDistance)
                {
                    mouseState = MouseState.Drag;
                    DragAndCreateObjects();
                    objectParent = GameObject.Find("Object_Parent");
                    Debug.Log("�巡��");
                }
            }
            else
            {
                DragAndCreateObjects();
                Debug.Log("�巡��");
            }
        }
        //�巡�� �� ���콺 ���� ��� ���� �ʱ�ȭ
        else if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.Drag)
        {
            mouseState = MouseState.None;
            selectedObject = null;
        }

        //================������Ʈ ���� �ִ� ��====================
        //������ �ʾ����� ������Ʈ ���� ���¶�� ���õ� ������Ʈ�� �����̰� ����
        //Debug.Log(selectedObject);
        //Debug.Log(selectState);
        if (selectState == SelectState.Select && selectedObject)
        {
            Debug.Log("������Ʈ ������");
            MovingObject();
            ChangeObject();
        }   
    }

    /// <summary>
    /// �ٴ� ���� �Լ�
    /// </summary>
    void CreateFloor()
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
    void ClearMapObjects()
    {
        DestroyImmediate(objectParent);
    }
    /// <summary>
    /// ������Ʈ ���� �Լ� (Left Click)
    /// </summary>
    void SelectObject()
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
                for(int i =0; i<BlockEditor.ObjectList.Count; i++)
                {
                    if(BlockEditor.ObjectList[i].name == selectedObject.name)
                    {
                        selectIndex = i;
                        break;
                    }
                }

                //Debug.Log(selectedObject);
            }
        }
    }

    /// <summary>
    /// ������Ʈ ��ġ ��Ű�� �Լ� (���� ������Ʈ�� �ִ� ���·� Left Click)
    /// </summary>
    void CollocatingObject()
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
    void MovingObject()
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
    void DeleteObject()
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
    void DragAndCreateObjects()
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
    /// </summary>
    private void ChangeObject()
    {
        Event e = Event.current;
        if(e.type == EventType.ScrollWheel)
        {
            if(e.delta.y > 0)
            {
                selectIndex = selectIndex - 1 < 0 ? BlockEditor.ObjectList.Count - 1 : selectIndex - 1;
                DestroyImmediate(selectedObject);
                Object resource = Resources.Load<GameObject>("Editor/" + BlockEditor.ObjectList[selectIndex].name);
                //Debug.Log(resource);
                BlockEditor.ObjectSetting(resource as GameObject, Vector3.zero, objectParent.transform);
                selectedObject = resource as GameObject;
                Debug.Log(selectedObject);

            }
            else
            {
                selectIndex = selectIndex + 1 > BlockEditor.ObjectList.Count - 1 ? 0 : selectIndex + 1;
                DestroyImmediate(selectedObject);
                Object resource = Resources.Load<GameObject>("Editor/" + BlockEditor.ObjectList[selectIndex].name);
                //Debug.Log(resource);
                //BlockEditor.ObjectSetting(resource as GameObject, Vector3.zero, objectParent.transform);
                selectedObject = resource as GameObject;
                Debug.Log(selectedObject);
            }
        }
    }

}
