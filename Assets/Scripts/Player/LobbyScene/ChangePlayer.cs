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
        {
            LobbyManager.instance.playerInfo[photonView.ViewID] = null;
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        Show();

        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LobbyManager.instance.Info(photonView.ViewID, transform.GetChild(0).GetChild(selectCnt).gameObject.name);
        }
    }
    public void OnClickLeftButton()
    {
        photonView.RPC("RPC_SelectCount", RpcTarget.All, false);
    }
    public void OnClickRightButton()
    {
        photonView.RPC("RPC_SelectCount", RpcTarget.All, true);
    }
    [PunRPC]
    public void RPC_SelectCount(bool isInCrease)
    {
        if (isInCrease)
        {
            if (selectCnt == maxCnt)
                selectCnt = 0;
            else
                selectCnt++;
        }
        else
        {
            if (selectCnt == 0)
                selectCnt = maxCnt;
            else
                selectCnt--;
        }
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
