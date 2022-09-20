using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class StartManager : MonoBehaviourPun
{
    public InputField friendCode_InputField;
    public InputField createCode_InputField;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (friendCode_InputField.text == "" && createCode_InputField.text == "")
            {
                //���߿� UI�� ����
                Debug.Log("�ڵ带 �Է����ּ���.");
            }
            else if (friendCode_InputField.text != "" && createCode_InputField.text != "")
            {
                //���߿� UI�� ����
                Debug.Log("�� �� �ϳ��� �ڵ常 �Է����ּ���");
            }
            else if (friendCode_InputField.text != "")
            {
                EnterRoom();
            }
            else if(createCode_InputField.text != "")
            {
                CreateRoom();
            }
        }
    }
    private void EnterRoom()
    {

    }
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
    }
}
