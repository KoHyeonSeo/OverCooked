using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class BlockEditorWindow : EditorWindow
{
    
    static GameObject map;
    public static List<Object> ObjectList = new List<Object>();
    Vector2 scrollPosition;
    
    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        var window = GetWindow<BlockEditorWindow>();
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
        EditorStyles.boldLabel.normal.textColor = Color.cyan;
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

        //오브젝트 생성부
        GUILayout.Label("Table");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[0])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[0]);
            EditUtility.ObjectSetting(map, instantiate, Vector3.zero, objectParent.transform);
        }


        GUILayout.Label("Other", EditorStyles.boldLabel); 

        GUILayout.Label("Trash Can");

        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[1])))
        {
            GameObject instantiate = Instantiate(ObjectList[1] as GameObject);
            EditUtility.ObjectSetting(map, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Fire Extinguisher");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[2])))
        {
            GameObject instantiate = Instantiate(ObjectList[2] as GameObject);
            EditUtility.ObjectSetting(map, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
    }
}


