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
        //다른 오브젝트 선택 안되도록 설정
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);

        Event e = Event.current;


        //드래그 거리 계산을 위해 초기 position을 저장
        //Save initialize position for calculating Drag Activation Distance
        if (e.type == EventType.MouseDown && !e.control)
        {
            firstMousePos = e.mousePosition;
        }
        //오브젝트 삭제하기
        /// Left Ctrl + Left Click -> Object Delete
        else if (e.type == EventType.MouseDown && e.control)
        {
            DeleteObject();
        }
        //================Object Click Event====================
        //눌렀다가 바로 뗐을 때 오브젝트 선택 또는 배치(None State)
        /// Left Click -> Place Object (None State)
        if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.None)
        {
            if (selectState == SelectState.NotSelect)
            {
                //Object 선택 함수
                /// Left Click the Object -> Object Select (in NotSelect State) => Change to Select State
                SelectObject();
            }
            else
            {
                //Object 배치 함수
                ///Left Click the Object -> CollocatingObject (in Select State) => Change to NotSelect State
                CollocatingObject();
            }
        }
        //길게 누른 상태로 좌우로 움직인다면 물체 여러개로 늘리기 (Drag시 바로 Drag 상태로)
        /// Left Click and Drag -> Place multiple objects (Drag State)
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
                }
            }
            else
            {
                DragAndCreateObjects();
            }
        }
        //드래그 후 마우스 떼면 모든 것이 초기화
        ///Mouse Up in Drag State -> initialization
        else if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.Drag)
        {
            mouseState = MouseState.None;
            selectedObject = null;
        }

        //================After Selecting Object====================
        //누르지 않았지만 오브젝트 선택 상태라면 선택된 오브젝트를 움직이게 하자
        ///     - Select object and Move mouse -> Object Move
        if (selectState == SelectState.Select && selectedObject)
        {
            //오브젝트 움직임
            MovingObject();
            /// In select State and Change Mode -> Change Object
            if (wheelMode == ChangeMode.Change)
            {
                //부모 찾기
                objectParent = GameObject.Find("Object_Parent");
                //오브젝트 변경
                ChangeObject();
            }
        }
        ///Change the ChangeMode (Tab)
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            wheelMode = wheelMode == ChangeMode.None ? ChangeMode.Change : ChangeMode.None;
        }
    }
}
