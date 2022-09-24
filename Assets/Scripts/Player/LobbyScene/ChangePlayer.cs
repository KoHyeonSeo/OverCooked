using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangePlayer : MonoBehaviourPun
{
    int selectCnt = 0;
    int maxCnt = 2;

    private void Start()
    {
        if (photonView.IsMine)
            LobbyManager.instance.playerInfo[photonView.ViewID] = null;
    }
    private void Update()
    {
        Show();

        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LobbyManager.instance.playerInfo[photonView.ViewID] = transform.GetChild(0).GetChild(selectCnt).gameObject.name;
            print(photonView.ViewID);
            print(LobbyManager.instance.playerInfo[photonView.ViewID]);
        }
    }
    public void OnClickLeftButton()
    {
        if (selectCnt == 0)
            selectCnt = maxCnt;
        else
            selectCnt--;
    }
    public void OnClickRightButton()
    {
        if(selectCnt == maxCnt)
            selectCnt = 0;
        else
            selectCnt++;
    }
    private void Show()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(0).GetChild(selectCnt).gameObject.SetActive(true);
    }
}
