using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Photon.Pun;

public class DeadZone : MonoBehaviourPun
{
    public float waitTime = 2f;
    private bool playerDead = false;
    private Transform player;
    private float curTime = 0;
    private void Update()
    {
        if (playerDead)
        {
            curTime += Time.deltaTime;
            if (curTime > waitTime)
            {
                player.GetComponent<PhotonView>().RPC("RPC_OnBirth", RpcTarget.All);
                playerDead = false;
                curTime = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
        else
        {
            other.GetComponent<PhotonView>().RPC("RPC_ChangeState", RpcTarget.All, PlayerState.State.Die);
            if (other.transform.childCount > 1)
                other.transform.GetChild(1).parent = null;
            player = other.transform;
            playerDead = true;
        }
    }
}
