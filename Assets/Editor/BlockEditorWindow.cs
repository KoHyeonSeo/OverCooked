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
    static MapTool map;
    public static List<Object> ObjectList = new List<Object>();
    Vector2 scrollPosition;

    //ToolBar Setting
    string[] toolList = { "Create", "View" , "Manual"};
    int toolBarIdx = 0;


    [MenuItem("EditorWindow/BlockEditor")]
    static void Init()
    {
        var window = GetWindow<BlockEditorWindow>();
        window.maxSize = window.minSize = new Vector2(400, 500);

        map = GameObject.Find("Map").GetComponent<MapTool>();
        Object resource_table = Resources.Load<GameObject>("Editor/TableTest");
        Object resource_trash = Resources.Load<GameObject>("Editor/Trash Can");
        Object resource_FireExtinguisher = Resources.Load<GameObject>("Editor/FireExtinguisher");
        Object resource_BunBox = Resources.Load<GameObject>("Editor/BunBox");
        Object resource_CutTable = Resources.Load<GameObject>("Editor/CutTable");
        Object resource_FireTable = Resources.Load<GameObject>("Editor/FireTable");
        Object resource_LettuceBox = Resources.Load<GameObject>("Editor/LettuceBox");
        Object resource_MeatBox = Resources.Load<GameObject>("Editor/MeatBox");
        Object resource_PlateTable = Resources.Load<GameObject>("Editor/PlateTable");
        Object resource_ServiceDesk = Resources.Load<GameObject>("Editor/ServiceDesk");
        Object resource_Sink = Resources.Load<GameObject>("Editor/Sink");
        Object resource_Table = Resources.Load<GameObject>("Editor/Table");
        Object resource_TomatoBox = Resources.Load<GameObject>("Editor/TomatoBox");
        Object resource_Table2 = Resources.Load<GameObject>("Editor/Table2");

        ObjectList.Add(resource_table);
        ObjectList.Add(resource_trash);
        ObjectList.Add(resource_FireExtinguisher);
        
        ObjectList.Add(resource_BunBox);
        ObjectList.Add(resource_LettuceBox);
        ObjectList.Add(resource_TomatoBox);
        ObjectList.Add(resource_MeatBox);

        ObjectList.Add(resource_CutTable);
        ObjectList.Add(resource_FireTable);
        ObjectList.Add(resource_PlateTable);
        ObjectList.Add(resource_ServiceDesk);
        ObjectList.Add(resource_Sink);
        ObjectList.Add(resource_Table);
        ObjectList.Add(resource_Table2);

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
                OnGUI_View();
                break;
            case 2:
                OnGUI_ControlWindow();
                break;
        }
    }


    private void OnGUI_View()
    {
        GUILayout.Label("Select State: " + BlockEdit.selectState);
        GUILayout.Space(10);
        GUILayout.Label("Mouse State: " + BlockEdit.mouseState);
        GUILayout.Space(10);
        GUILayout.Label("Change Object Mode: " + BlockEdit.changeMode);
        GUILayout.Space(10);
        GUILayout.Label("Selected Object: " + BlockEdit.selectedObject);
        
        this.Repaint();
    }

    /// <summary>
    /// Control Manual
    /// </summary>
    private void OnGUI_ControlWindow()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginScrollView
            (scrollPosition,
            GUILayout.MinWidth(0),
            GUILayout.MaxWidth(400),
            GUILayout.MinHeight(500),
            GUILayout.MaxHeight(1000));

        GUILayout.BeginHorizontal();
        GUILayout.Label("\n\nLeft Click -> Place Object");
        GUI.DrawTexture(new Rect(330, 5, 60, 60), map.arrangeMentTexture);
        GUILayout.EndHorizontal();

        DrawHorizontalLine(1, new Vector2(20, 5));

        GUILayout.BeginHorizontal();
        GUILayout.Label("\nLeft Ctrl + Left Click -> Object Delete");
        GUI.DrawTexture(new Rect(330, 75, 60, 60), map.deleteTexture);
        GUILayout.EndHorizontal();

        DrawHorizontalLine(1, new Vector2(28, 5));

        GUILayout.BeginHorizontal();
        GUILayout.Label("\nLeft Click the Object -> Object Select");
        GUI.DrawTexture(new Rect(330, 145, 60, 60), map.selectTexture);
        GUILayout.EndHorizontal();

        DrawHorizontalLine(1, new Vector2(31, 5));

        GUILayout.BeginHorizontal();
        GUILayout.Label("\n\t - Select object and Move mouse \n\t\t -> Object Move");
        GUI.DrawTexture(new Rect(330, 215, 60, 60), map.moveTexture);
        GUILayout.EndHorizontal();

        DrawHorizontalLine(1, new Vector2(13, 5));

        GUILayout.Label("\t - In select State and Change Mode");

        GUILayout.Label("\t\t - Z -> Change Pre Object");
        GUILayout.Label("\t\t - C -> Change Next Object");
        GUILayout.BeginHorizontal();
        GUI.DrawTexture(new Rect(330, 285, 60, 60), map.changeTexture);
        GUILayout.EndHorizontal();

        DrawHorizontalLine(1, new Vector2(3, 5));

        GUILayout.BeginHorizontal();
        GUILayout.Label("\n\t\t - Tab -> Change the ChangeMode");
        GUI.DrawTexture(new Rect(330, 355, 60, 60), map.swapTexture);
        GUILayout.EndHorizontal();

        DrawHorizontalLine(1, new Vector2(28, 5));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Left Click and Drag  -> Place multiple objects");
        GUI.DrawTexture(new Rect(330, 425, 60, 60), map.dragTexture);
        GUILayout.EndHorizontal();

        GUILayout.Label("\t - Mouse Up in Drag State -> initialization");

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        
    }
    /// <summary>
    /// Draw HorizontalLine
    /// </summary>
    /// <param name="height"></param>
    /// <param name="margin"></param>
    public void DrawHorizontalLine(float height, Vector2 margin)
    {
        GUILayout.Space(margin.x);

        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), Color.black);

        GUILayout.Space(margin.y);
    }

    /// <summary>
    /// Object creating window
    /// </summary>
    private void OnGUI_Create() {

        GUILayout.BeginVertical();

        scrollPosition = GUILayout.BeginScrollView
            (scrollPosition,
            GUILayout.MinWidth(0),
            GUILayout.MaxWidth(400),
            GUILayout.MinHeight(0),
            GUILayout.MaxHeight(1000));

        Event e = Event.current;
        GameObject objectParent = GameObject.Find("Object_Parent");
        if (!objectParent)
        {
            objectParent = new GameObject();
            objectParent.name = "Object_Parent";
        }

        EditorStyles.boldLabel.normal.textColor = Color.cyan;
        GUILayout.Label("Ingredient Box", EditorStyles.boldLabel);

        GUILayout.Label("Bun Box");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[3])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[3]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Lettuce Box");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[4])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[4]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Tomato Box");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[5])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[5]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Meat Box");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[6])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[6]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }



        GUILayout.Label("Interact Object", EditorStyles.boldLabel);

        //GUILayout.Label("Table");
        //if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[0])))
        //{
        //    GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[0]);
        //    EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        //}

        GUILayout.Label("CutTable");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[7])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[7]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }
        GUILayout.Label("FireTable");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[8])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[8]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }
        GUILayout.Label("PlateTable");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[9])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[9]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }
        GUILayout.Label("ServiceDesk");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[10])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[10]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }
        GUILayout.Label("Sink");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[11])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[11]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }
        GUILayout.Label("Table");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[12])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[12]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Table2");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[13])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[12]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Other", EditorStyles.boldLabel);

        GUILayout.Label("Trash Can");

        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[1])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[1]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.Label("Fire Extinguisher");
        if (GUILayout.Button(AssetPreview.GetMiniThumbnail(ObjectList[2])))
        {
            GameObject instantiate = (GameObject)PrefabUtility.InstantiatePrefab(ObjectList[2]);
            EditUtility.ObjectSetting(map.gameObject, instantiate, Vector3.zero, objectParent.transform);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}