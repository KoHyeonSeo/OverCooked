using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands;
using Unity.VisualScripting;

public class BlockEditor : EditorWindow
{
    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        var window = GetWindow<BlockEditor>();
        window.maxSize = window.minSize = new Vector2(200, 500);
    }

    Vector2 scrollPosition;

    //public List<string> objectList = new List<string>();

    private void OnGUI()
    {
        Event e = Event.current;
        //if (e.type == EventType.MouseDown)
        //{
        //    Debug.Log(e.button);
        //}

        GUILayout.Label("Interact Object", EditorStyles.boldLabel);
        Object resource_table = Resources.Load<GameObject>("Editor/Table");
        GUILayout.BeginHorizontal();
        GUILayout.BeginScrollView
            (scrollPosition,
            GUILayout.MinWidth(100),
            GUILayout.MaxWidth(300),
            GUILayout.MinHeight(220),
            GUILayout.MaxHeight(700));

        GameObject map = GameObject.Find("Object_Parent");
        if (!map)
        {
            map = new GameObject();
            map.name = "Object_Parent";
        }

        GUILayout.Label("Table");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_table)))
        {
            EditorGUIUtility.PingObject(resource_table);
            GameObject instantiate = Instantiate(resource_table as GameObject);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.parent = map.transform;
        }


        GUILayout.Label("Other", EditorStyles.boldLabel); 

        GUILayout.Label("Trash Can");
        Object resource_trash = Resources.Load<GameObject>("Editor/Trash Can");

        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_trash)))
        {
            EditorGUIUtility.PingObject(resource_trash);
            GameObject instantiate = Instantiate(resource_trash as GameObject);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.parent = map.transform;
        }

        GUILayout.Label("Fire Extinguisher");
        Object resource_FireExtinguisher = Resources.Load<GameObject>("Editor/FireExtinguisher");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_FireExtinguisher)))
        {
            EditorGUIUtility.PingObject(resource_FireExtinguisher);
            GameObject instantiate = Instantiate(resource_FireExtinguisher as GameObject);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.parent = map.transform;
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

    SelectState selectState = SelectState.NotSelect;
    GameObject selectedObject = null;
    Map map;
    private void OnEnable()
    {
        map = (Map)target;
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

        //눌렀다가 뗐을 때 오브젝트 선택 또는 배치
        if (e.type == EventType.MouseUp && e.button == 0) {
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
            Debug.Log("선택");
        }
        //길게 누른 상태로 좌우로 움직인다면 물체 여러개로 늘리기
        else if(e.type == EventType.MouseDrag && e.button == 0)
        {
            Debug.Log("드래그");
        }
        //누르지 않았지만 오브젝트 선택 상태라면 선택된 오브젝트를 움직이게 하자
        if(selectState == SelectState.Select && selectedObject)
        {
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
        GameObject obj = GameObject.Find("Object_Parent");
        DestroyImmediate(obj);
    }
    /// <summary>
    /// 오브젝트 선택 함수
    /// </summary>
    void SelectObject()
    {
        //레이어를 SelectObject로 바꾸자

        //selectedObject에 클릭한 물체를 넣어두자

    }

    /// <summary>
    /// 오브젝트 배치 시키는 함수
    /// </summary>
    void CollocatingObject()
    {
        //닿은 격자에 넣어두자 Layer는 SelectObject인데 걸러야한다. 이 레이어는
    }


}
