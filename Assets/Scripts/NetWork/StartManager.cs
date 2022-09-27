using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class StartManager : MonoBehaviourPunCallbacks
{
    public InputField friendCode_InputField;
    public InputField createCode_InputField;
    public InputField nickName_InputField;
    private void Start()
    {
        OnConnect();
    }

    public void OnConnect()
    {
        //������ ������ ���� ��û
        PhotonNetwork.ConnectUsingSettings();
    }
    //������ ������ ���� ����, �κ� ���� �� ������ �� �� ���� ����
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //print("OnConnected");
    }
    //������ ������ ����, �κ� ���� �� ������ ������ ����
    //�̶� �κ� �����ؾ���
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //print("OnConnectedToMaster");

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nickName_InputField.text.Length > 0)
            {
                if (friendCode_InputField?.text.Length == 0 && createCode_InputField?.text.Length == 0)
                {
                    //���߿� UI�� ����
                    Debug.Log("�ڵ带 �Է����ּ���.");
                }
                else if (friendCode_InputField?.text.Length != 0 && createCode_InputField?.text.Length != 0)
                {
                    //���߿� UI�� ����
                    Debug.Log("�� �� �ϳ��� �ڵ常 �Է����ּ���");
                }
                else if (friendCode_InputField?.text.Length != 0)
                {
                    Debug.Log("���� ���� ģ����~~");
                    //�г��� ����
                    PhotonNetwork.NickName = nickName_InputField.text;
                    JoinRoom();
                }
                else if (createCode_InputField?.text.Length != 0)
                {
                    Debug.Log("��~���ڰ�~~");
                    //�г��� ����
                    PhotonNetwork.NickName = nickName_InputField.text;
                    CreateRoom();
                }
            }
        }
    }
    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(friendCode_InputField.text);
    }
    //�� ������ �������� �� �Ҹ��� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 3;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(createCode_InputField.text, roomOptions);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        //����� ���� UI�� �˷��ֱ�
    }
    //�� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        //�� ���� ���� UI�� �˷��ֱ�
    }


}
