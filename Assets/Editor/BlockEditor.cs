using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlockEditor : EditorWindow
{
    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        var window = GetWindow<BlockEditor>();
        window.maxSize = window.minSize = new Vector2(200, 500);
    }

    Vector2 scrollPosition;
    private void OnGUI()
    {
        GUILayout.Label("Interact Object", EditorStyles.boldLabel);
        Object resource_table = Resources.Load<GameObject>("Table");
        GUILayout.BeginHorizontal();
        GUILayout.BeginScrollView
            (scrollPosition,
            GUILayout.MinWidth(100),
            GUILayout.MaxWidth(300),
            GUILayout.MinHeight(220),
            GUILayout.MaxHeight(700));

        GUILayout.Label("Table");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_table)))
        {
            EditorGUIUtility.PingObject(resource_table);
            GameObject instantiate = Instantiate(resource_table as GameObject);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }

        GUILayout.Label("Trash Can");
        Object resource_trash = Resources.Load<GameObject>("Trash Can");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_trash)))
        {
            EditorGUIUtility.PingObject(resource_trash);
            GameObject instantiate = Instantiate(resource_trash as GameObject);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }

        GUILayout.Label("Fire Extinguisher");
        Object resource_FireExtinguisher = Resources.Load<GameObject>("FireExtinguisher");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_FireExtinguisher)))
        {
            EditorGUIUtility.PingObject(resource_FireExtinguisher);
            GameObject instantiate = Instantiate(resource_FireExtinguisher as GameObject);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }

        GUILayout.Label("Other", EditorStyles.boldLabel); 

        GUILayout.Label("Deadzone");
        Object resource_DeadZone = Resources.Load<GameObject>("Deadzone");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(resource_DeadZone)))
        {
            EditorGUIUtility.PingObject(resource_DeadZone);
            GameObject instantiate = Instantiate(resource_DeadZone as GameObject);
            instantiate.gameObject.name = instantiate.gameObject.name.Split('(')[0];
            instantiate.transform.position = Vector3.zero;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
    }
}
