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

        map.dragDistance = EditorGUILayout.FloatField("드래그 활성화 거리", map.dragDistance);

        if (GUILayout.Button("바닥 생성"))
        {
            CreateFloor();
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("블록 모두 삭제"))
        {
            bool isCancel = EditorUtility.DisplayDialog(" 주의 ", "\n정말로 모두 삭제하시겠습니까?", "OK", "CANCEL");
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
        if (e.type == EventType.MouseDown && !e.control)
        {
            firstMousePos = e.mousePosition;
        }
        //오브젝트 삭제하기
        else if (e.type == EventType.MouseDown && e.control)
        {
            DeleteObject();
        }
        //================오브젝트 클릭 이벤트====================
        //눌렀다가 바로 뗐을 때 오브젝트 선택 또는 배치(None State)
        if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.None)
        {
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
        //길게 누른 상태로 좌우로 움직인다면 물체 여러개로 늘리기 (Drag시 바로 Drag 상태로)
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
                    Debug.Log("드래그");
                }
            }
            else
            {
                DragAndCreateObjects();
                Debug.Log("드래그");
            }
        }
        //드래그 후 마우스 떼면 모든 것이 초기화
        else if (e.type == EventType.MouseUp
            && e.button == 0
            && mouseState == MouseState.Drag)
        {
            mouseState = MouseState.None;
            selectedObject = null;
        }

        //================오브젝트 갖고 있는 후====================
        //누르지 않았지만 오브젝트 선택 상태라면 선택된 오브젝트를 움직이게 하자
        if (selectState == SelectState.Select && selectedObject)
        {
            Debug.Log("오브젝트 움직임");
            //오브젝트 움직임
            MovingObject();
            if (wheelMode == WheelMode.Change)
            {
                //휠 돌려 오브젝트 변경
                ChangeObject();
            }
            else
            {
                //ZoomMode
            }
        }
        //휠 모드 변경 (Tab)
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            wheelMode = wheelMode == WheelMode.None ? WheelMode.Change : WheelMode.None;
            Debug.Log("휠 모드 변경: " + wheelMode);
        }
    }
}
