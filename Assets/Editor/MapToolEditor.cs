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
        //�ٸ� ������Ʈ ���� �ȵǵ��� ����
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);

        Event e = Event.current;


        //�巡�� �Ÿ� ����� ���� �ʱ� position�� ����
        //Save initialize position for calculating Drag Activation Distance
        if (e.type == EventType.MouseDown && !e.control)
        {
            firstMousePos = e.mousePosition;
        }
        //������Ʈ �����ϱ�
        /// Left Ctrl + Left Click -> Object Delete
        else if (e.type == EventType.MouseDown && e.control)
        {
            DeleteObject();
        }
        //================Object Click Event====================
        //�����ٰ� �ٷ� ���� �� ������Ʈ ���� �Ǵ� ��ġ(None State)
        /// Left Click -> Place Object (None State)
        if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.None)
        {
            if (selectState == SelectState.NotSelect)
            {
                //Object ���� �Լ�
                /// Left Click the Object -> Object Select (in NotSelect State) => Change to Select State
                SelectObject();
            }
            else
            {
                //Object ��ġ �Լ�
                ///Left Click the Object -> CollocatingObject (in Select State) => Change to NotSelect State
                CollocatingObject();
            }
        }
        //��� ���� ���·� �¿�� �����δٸ� ��ü �������� �ø��� (Drag�� �ٷ� Drag ���·�)
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
        //�巡�� �� ���콺 ���� ��� ���� �ʱ�ȭ
        ///Mouse Up in Drag State -> initialization
        else if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.Drag)
        {
            mouseState = MouseState.None;
            selectedObject = null;
        }

        //================After Selecting Object====================
        //������ �ʾ����� ������Ʈ ���� ���¶�� ���õ� ������Ʈ�� �����̰� ����
        ///     - Select object and Move mouse -> Object Move
        if (selectState == SelectState.Select && selectedObject)
        {
            //������Ʈ ������
            MovingObject();
            /// In select State and Change Mode -> Change Object
            if (wheelMode == ChangeMode.Change)
            {
                //�θ� ã��
                objectParent = GameObject.Find("Object_Parent");
                //������Ʈ ����
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
