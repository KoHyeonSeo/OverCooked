using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapToolEditor : BlockEdit
{
    private void OnEnable()
    {
        map = (MapTool)target;
        objectParent = GameObject.Find("Object_Parent");
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

        map.dragDistance = EditorGUILayout.FloatField("�巡�� Ȱ��ȭ �Ÿ�", map.dragDistance);

        if (GUILayout.Button("�ٴ� ����"))
        {
            CreateFloor();
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("��� ��� ����"))
        {
            bool isCancel = EditorUtility.DisplayDialog(" ���� ", "\n������ ��� �����Ͻðڽ��ϱ�?", "OK", "CANCEL");
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
        if (e.type == EventType.MouseDown && !e.control)
        {
            firstMousePos = e.mousePosition;
        }
        //������Ʈ �����ϱ�
        else if (e.type == EventType.MouseDown && e.control)
        {
            DeleteObject();
        }
        //================������Ʈ Ŭ�� �̺�Ʈ====================
        //�����ٰ� �ٷ� ���� �� ������Ʈ ���� �Ǵ� ��ġ(None State)
        if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.None)
        {
            if (selectState == SelectState.NotSelect)
            {
                //Object ���� �Լ�
                SelectObject();
            }
            else
            {
                //Object ��ġ �Լ�
                CollocatingObject();
            }
            Debug.Log("����");
        }
        //��� ���� ���·� �¿�� �����δٸ� ��ü �������� �ø��� (Drag�� �ٷ� Drag ���·�)
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
                    Debug.Log("�巡��");
                }
            }
            else
            {
                DragAndCreateObjects();
                Debug.Log("�巡��");
            }
        }
        //�巡�� �� ���콺 ���� ��� ���� �ʱ�ȭ
        else if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.Drag)
        {
            mouseState = MouseState.None;
            selectedObject = null;
        }

        //================������Ʈ ���� �ִ� ��====================
        //������ �ʾ����� ������Ʈ ���� ���¶�� ���õ� ������Ʈ�� �����̰� ����
        if (selectState == SelectState.Select && selectedObject)
        {
            Debug.Log("������Ʈ ������");
            //������Ʈ ������
            MovingObject();
            if (wheelMode == WheelMode.Change)
            {
                //�� ���� ������Ʈ ����
                ChangeObject();
            }
            else
            {
                //ZoomMode
            }
        }
        //�� ��� ���� (Tab)
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            wheelMode = wheelMode == WheelMode.None ? WheelMode.Change : WheelMode.None;
            Debug.Log("�� ��� ����: " + wheelMode);
        }
    }
}
