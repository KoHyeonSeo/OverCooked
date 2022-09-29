using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands;
using Unity.VisualScripting;

public class BlockEditor : EditorWindow
{
    
    static GameObject map;
    
    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        var window = GetWindow<BlockEditor>();
        window.maxSize = window.minSize = new Vector2(200, 500);
        map = GameObject.Find("Map");    
    }

    Vector2 scrollPosition;

    private void OnGUI()
    {
        Event e = Event.current;

        GUILayout.Label("Interact Object", EditorStyles.boldLabel);
        Object resource_table = Resources.Load<GameObject>("Editor/Table");
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

        GUILayout.Label("Table");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_table)))
        {
            GameObject instantiate = Instantiate(resource_table as GameObject);
            EditorGUIUtility.PingObject(map);
            Selection.activeGameObject = map;
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.parent = objectParent.transform;
        }


        GUILayout.Label("Other", EditorStyles.boldLabel); 

        GUILayout.Label("Trash Can");
        Object resource_trash = Resources.Load<GameObject>("Editor/Trash Can");

        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_trash)))
        {
            GameObject instantiate = Instantiate(resource_trash as GameObject);
            EditorGUIUtility.PingObject(map);
            Selection.activeGameObject = map;
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.parent = objectParent.transform;
        }

        GUILayout.Label("Fire Extinguisher");
        Object resource_FireExtinguisher = Resources.Load<GameObject>("Editor/FireExtinguisher");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_FireExtinguisher)))
        {
            GameObject instantiate = Instantiate(resource_FireExtinguisher as GameObject);
            EditorGUIUtility.PingObject(map);
            Selection.activeGameObject = map;
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.parent = objectParent.transform;
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
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

    SelectState selectState = SelectState.NotSelect;
    MouseState mouseState = MouseState.None;
    GameObject selectedObject = null;
    Map map;
    //초기 마우스 위치 저장
    Vector2 firstMousePos = Vector2.one;
    GameObject objectParent;
    private void OnEnable()
    {
        map = (Map)target;
        objectParent = GameObject.Find("Object_Parent");
    }
    //우클릭하면 선택 & 위치 변경 가능
    //좌클릭하고 드래그 -> 여러개 생성
    //우클릭해서 선택 상태일때, 마우스 휠 -> 물체 변경

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        map.tileX = EditorGUILayout.IntField("타일 가로", map.tileX);
        map.tileZ = EditorGUILayout.IntField("타일 세로", map.tileZ);

        map.tileX = Mathf.Clamp(map.tileX, 1, 500);
        map.tileZ = Mathf.Clamp(map.tileZ, 1, 500);

        map.floorTile = (GameObject)EditorGUILayout.ObjectField("타일", map.floorTile, typeof(GameObject), false);

        map.dragDistance = EditorGUILayout.FloatField("드래그 활성화 거리", map.dragDistance);

        if(GUILayout.Button("바닥 생성"))
        {
            CreateFloor();
        }
        EditorGUILayout.Space();

        if(GUILayout.Button("블록 모두 삭제"))
        {
            bool isCancel = EditorUtility.DisplayDialog(" 주의 ", "\n정말로 모두 삭제하시겠습니까?", "OK", "CANCEL");
            if(isCancel)
                ClearMapObjects();
        }
    }
    private void OnSceneGUI()
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);

        Event e = Event.current;

        //드래그 거리 계산을 위해 초기 position을 저장
        if (e.type == EventType.MouseDown && !e.control)
        {
            firstMousePos = e.mousePosition;
        }
        //오브젝트 삭제하기
        else if(e.type == EventType.MouseDown && e.control)
        {
            DeleteObject();
        }
        //================오브젝트 클릭 이벤트====================
        //눌렀다가 바로 뗐을 때 오브젝트 선택 또는 배치(None State)
        if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.None)
        {
            if (selectState == SelectState.NotSelect)
            {
                //Object 선택 함수
                SelectObject();
            }
            else
            {
                //Object 배치 함수
                CollocatingObject();
            }
            //Debug.Log("선택");
        }
        //길게 누른 상태로 좌우로 움직인다면 물체 여러개로 늘리기 (Drag시 바로 Drag 상태로)
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
                    Debug.Log("드래그");
                }
            }
            else
            {
                DragAndCreateObjects();
                Debug.Log("드래그");
            }
        }
        //드래그 후 마우스 떼면 모든 것이 초기화
        else if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.Drag)
        {
            mouseState = MouseState.None;
            selectedObject = null;
        }

        //================오브젝트 갖고 있는 후====================
        //누르지 않았지만 오브젝트 선택 상태라면 선택된 오브젝트를 움직이게 하자
        //Debug.Log(selectedObject);
        //Debug.Log(selectState);
        if (selectState == SelectState.Select && selectedObject)
        {
            Debug.Log("오브젝트 움직임");
            MovingObject();
        }   
    }
    /// <summary>
    /// 바닥 생성 함수
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
    /// 맵에 배치한 오브젝트 모두 삭제하는 함수
    /// </summary>
    void ClearMapObjects()
    {
        DestroyImmediate(objectParent);
    }
    /// <summary>
    /// 오브젝트 선택 함수 (Left Click)
    /// </summary>
    void SelectObject()
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
                //Debug.Log(selectedObject);
            }
        }
    }

    /// <summary>
    /// 오브젝트 배치 시키는 함수 (잡은 오브젝트가 있는 상태로 Left Click)
    /// </summary>
    void CollocatingObject()
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
    void MovingObject()
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
    /// 드래그 하여 여러 오브젝트 생성하는 함수
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

}
