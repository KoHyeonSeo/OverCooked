using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.IO;

public class Cam_MiddleMove : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float lerpSpeed = 5;
    public Transform target;
    private float difTargetY;
    Vector3 recievePos;
    private void Start()
    {
        difTargetY = Mathf.Abs(transform.position.y - target.position.y);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            //photonView.RPC("RPC_Cam_Move", RpcTarget.All);
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 dir = Vector3.right * h + Vector3.forward * v;
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, target.position.y + difTargetY, transform.position.z);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, recievePos, Time.deltaTime * lerpSpeed);
        }


    }
    //[PunRPC]
    //public void RPC_Cam_Move()
    //{
    //    print("2");
    //    float h = Input.GetAxisRaw("Horizontal");
    //    float v = Input.GetAxisRaw("Vertical");

    //    Vector3 dir = Vector3.right * h + Vector3.forward * v;
    //    transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    //}
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            recievePos = (Vector3)stream.ReceiveNext();
        }
    }
}
