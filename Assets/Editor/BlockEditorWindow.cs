using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class BlockEditorWindow : EditorWindow
{
    //Creating window setting
    static GameObject map;
    public static List<Object> ObjectList = new List<Object>();
    Vector2 scrollPosition;

    //ToolBar Setting
    string[] toolList = { "Create", "Manual" };
    int toolBarIdx = 0;


    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        var window = GetWindow<BlockEditorWindow>();
        window.maxSize = window.minSize = new Vector2(400, 500);

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
        toolBarIdx = GUILayout.Toolbar(toolBarIdx, toolList);

        switch (toolBarIdx)
        {
            case 0:
                OnGUI_Create();
                break;
            case 1:
                OnGUI_ControlWindow();
                break;
        }
    }
    /// <summary>
    /// Control Manual
    /// </summary>
    private void OnGUI_ControlWindow()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Left Click -> Place Object");
        GUILayout.Label("Left Ctrl + Left Click -> Object Delete");
        GUILayout.Label("Left Click the Object -> Object Select");
        GUILayout.Label("\t - Select object and Move mouse -> Object Move");
        GUILayout.Label("\t - In select State and Change Mode -> Change Object");
        GUILayout.Label("\t\t - Z -> Change Pre Object");
        GUILayout.Label("\t\t - C -> Change Next Object");
        GUILayout.Label("\t\t - Tab -> Change the ChangeMode");
        GUILayout.Label("Left Click and Drag -> Place multiple objects");
        GUILayout.Label("\t - Mouse Up in Drag State -> initialization");
        GUILayout.EndVertical();
    }
    /// <summary>
    /// Object creating window
    /// </summary>
    private void OnGUI_Create() {

        GUILayout.BeginVertical();

        GUILayout.BeginScrollView
            (scrollPosition,
            GUILayout.MinWidth(0),
            GUILayout.MaxWidth(400),
            GUILayout.MinHeight(0),
            GUILayout.MaxHeight(1000));

        Event e = Event.current;
        EditorStyles.boldLabel.normal.textColor = Color.cyan;
        GUILayout.Label("Interact Object", EditorStyles.boldLabel);
        GameObject objectParent = GameObject.Find("Object_Parent");
        if (!objectParent)
        {
            objectParent = new GameObject();
            objectParent.name = "Object_Parent";
        }

        
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

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}