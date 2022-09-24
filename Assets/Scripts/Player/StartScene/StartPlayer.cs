using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartPlayer : MonoBehaviourPun
{
    private void Start()
    {
        LobbyManager.instance.photonView.RPC("ChangePosition", RpcTarget.All);
        transform.localEulerAngles = new Vector3(0,180,0);
    }
}
