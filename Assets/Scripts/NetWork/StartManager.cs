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
                //나중에 UI로 구현
                Debug.Log("코드를 입력해주세요.");
            }
            else if (friendCode_InputField.text != "" && createCode_InputField.text != "")
            {
                //나중에 UI로 구현
                Debug.Log("둘 중 하나의 코드만 입력해주세요");
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
