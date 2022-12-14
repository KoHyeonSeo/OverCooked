using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapTool))]
public class MapToolEditor : BlockEdit
{
    /// <summary>
    /// [Function List]
    /// Left Click -> Place Object
    /// Left Ctrl + Left Click -> Object Delete
    /// Left Click the Object -> Object Select
    ///     - Select object and Move mouse -> Object Move
    ///     - In select State and Change Mode -> Change Object
    ///         - Z -> Change Pre Object
    ///         - C -> Change Next Object
    ///         - Tab -> Change the ChangeMode 
    /// Left Click and Drag -> Place multiple objects
    ///     - Mouse Up in Drag State -> initialization
    /// </summary>
    private void OnEnable()
    {
        map = (MapTool)target;
        objectParent = GameObject.Find("Object_Parent");
    }
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //tile infomation value of map
        map.tileX = EditorGUILayout.IntField("Tile Width", map.tileX);
        map.tileZ = EditorGUILayout.IntField("Tile Height", map.tileZ);

        map.tileX = Mathf.Clamp(map.tileX, 1, 500);
        map.tileZ = Mathf.Clamp(map.tileZ, 1, 500);
        //floorTile Object value of map
        map.floorTile = (GameObject)EditorGUILayout.ObjectField("Tile Object", map.floorTile, typeof(GameObject), false);
        //dragDistance value of map
        map.dragDistance = EditorGUILayout.FloatField("Drag Activation Distance", map.dragDistance);
        
        //Texture Value of map
        map.changeTexture = (Texture)EditorGUILayout.ObjectField("Chagne Texture", map.changeTexture, typeof(Texture), false);
        map.deleteTexture = (Texture)EditorGUILayout.ObjectField("Delete Texture", map.deleteTexture, typeof(Texture), false);
        map.dragTexture = (Texture)EditorGUILayout.ObjectField("Drag Texture", map.dragTexture, typeof(Texture), false);
        map.moveTexture = (Texture)EditorGUILayout.ObjectField("Move Texture", map.moveTexture, typeof(Texture), false);
        map.swapTexture = (Texture)EditorGUILayout.ObjectField("Swap Texture", map.swapTexture, typeof(Texture), false);
        map.selectTexture = (Texture)EditorGUILayout.ObjectField("Select Texture", map.selectTexture, typeof(Texture), false);
        map.arrangeMentTexture = (Texture)EditorGUILayout.ObjectField("Arrangement Texture", map.arrangeMentTexture, typeof(Texture), false);

        //Create Floor Button
        if (GUILayout.Button("Create Floor"))
        {
            CreateFloor();
        }
        EditorGUILayout.Space();
        //Delete Button
        if (GUILayout.Button("Delete All Objects"))
        {
            bool isCancel = EditorUtility.DisplayDialog(" Caution ", "\nAre you sure you want to delete all?", "OK", "CANCEL");
            if (isCancel)
                ClearMapObjects();
        }
    }
    private void OnSceneGUI()
    {
        //???? ???????? ???? ???????? ????
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);

        Event e = Event.current;


        //?????? ???? ?????? ???? ???? position?? ????
        //Save initialize position for calculating Drag Activation Distance
        if (e.type == EventType.MouseDown && !e.control)
        {
            firstMousePos = e.mousePosition;
        }
        //???????? ????????
        /// Left Ctrl + Left Click -> Object Delete
        else if (e.type == EventType.MouseDown && e.control)
        {
            selectState = SelectState.NotSelect;
            DeleteObject();
        }
        //================Object Click Event====================
        //???????? ???? ???? ?? ???????? ???? ???? ????(None State)
        /// Left Click -> Place Object (None State)
        if (Vector2.Distance(e.mousePosition, firstMousePos) < map.dragDistance 
            && e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.None)
        {
            if (selectState == SelectState.NotSelect)
            {
                //Object ???? ????
                /// Left Click the Object -> Object Select (in NotSelect State) => Change to Select State
                SelectObject();
            }
            else
            {
                //Object ???? ????
                ///Left Click the Object -> CollocatingObject (in Select State) => Change to NotSelect State
                CollocatingObject();
            }
        }
        //???? ???? ?????? ?????? ?????????? ???? ???????? ?????? (Drag?? ???? Drag ??????)
        /// Left Click and Drag -> Place multiple objects (Drag State)
        else if (e.type == EventType.MouseDrag && e.button == 0)
        {
            if (mouseState == MouseState.None)
            {
                if (Vector2.Distance(e.mousePosition, firstMousePos) > map.dragDistance)
                {
                    mouseState = MouseState.Drag;
                    objectParent = GameObject.Find("Object_Parent");
                    DragAndCreateObjects();
                }
            }
            else
            {
                DragAndCreateObjects();
            }
        }


        //?????? ?? ?????? ???? ???? ???? ??????
        ///Mouse Up in Drag State -> initialization
        if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.Drag)
        {
            mouseState = MouseState.None;
            selectedObject = null;
        }

        //================After Selecting Object====================
        //?????? ???????? ???????? ???? ???????? ?????? ?????????? ???????? ????
        ///     - Select object and Move mouse -> Object Move
        if (selectState == SelectState.Select && selectedObject)
        {
            //???????? ??????
            MovingObject();
            /// In select State and Change Mode -> Change Object
            if (changeMode == ChangeMode.Change)
            {
                //???? ????
                objectParent = GameObject.Find("Object_Parent");
                //???????? ????
                ChangeObject();
            }
        }
        ///Change the ChangeMode (Tab)
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            changeMode = changeMode == ChangeMode.None ? ChangeMode.Change : ChangeMode.None;
        }
    }
}
