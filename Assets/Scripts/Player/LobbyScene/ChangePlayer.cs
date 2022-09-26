using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChangePlayer : MonoBehaviourPun
{
    private int selectCnt = 0;
    private int maxCnt = 2;

    private void Start()
    {
        if (photonView.IsMine)
        {
            LobbyManager.instance.playerInfo[photonView.ViewID] = null;
            transform.GetChild(1).gameObject.SetActive(true);
        }
        transform.GetChild(3).GetChild(0).GetComponent<Text>().text = photonView.Owner.NickName;
    }
    private void Update()
    {
        Show();

        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("Checking", RpcTarget.AllBuffered, true);
            LobbyManager.instance.Info(photonView.ViewID, transform.GetChild(0).GetChild(selectCnt).gameObject.name);
        }
    }
    public void OnClickLeftButton()
    {
        photonView.RPC("RPC_SelectCount", RpcTarget.AllBuffered, false);
        photonView.RPC("Checking", RpcTarget.AllBuffered, false);
    }
    public void OnClickRightButton()
    {
        photonView.RPC("RPC_SelectCount", RpcTarget.AllBuffered, true);
        photonView.RPC("Checking", RpcTarget.AllBuffered, false);
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
    [PunRPC]
    public void Checking(bool isCheck)
    {
        if (isCheck)
            transform.GetChild(2).gameObject.SetActive(true);
        else
            transform.GetChild(2).gameObject.SetActive(false);
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
