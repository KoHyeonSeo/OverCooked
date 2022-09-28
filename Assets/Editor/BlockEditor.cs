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
    Map map;
    private void OnEnable()
    {
        map = (Map)target;
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
        GameObject obj = GameObject.Find("Object_Parent");
        DestroyImmediate(obj);
    }
}
