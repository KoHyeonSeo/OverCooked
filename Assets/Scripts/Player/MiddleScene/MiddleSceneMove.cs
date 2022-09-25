using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MiddleSceneMove : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float lerpSpeed = 5;
    [SerializeField] private float turnSpeed = 5;
    Vector3 recievePos;
    Quaternion recieveRot;
    private void Start()
    {
        
    }
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 dir = Vector3.right * h + Vector3.forward * v;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.position += dir.normalized * moveSpeed * Time.deltaTime;

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, recievePos, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, recieveRot, Time.deltaTime * lerpSpeed);
        }

        if (isPortal && Input.GetKeyDown(KeyCode.Space))
        {
            //PhotonNetwork.LoadLevel(4);
            if (other.gameObject.name.Contains("Portal"))
            {
                string[] names = other.gameObject.name.Split('/');
                PhotonNetwork.LoadLevel(names[1]);
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            recievePos = (Vector3)stream.ReceiveNext();
            recieveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    bool isPortal = false;
    Collider other;
    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isPortal = true;
            this.other = other;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Slope"))
        {
            moveSpeed = 18f;
            GetComponent<Rigidbody>().drag = 10f;
        }
        else
        {
            moveSpeed = 10f;
            GetComponent<Rigidbody>().drag = 0f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isPortal = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        //PhotonNetwork.LoadLevel(4);
        //        if (other.gameObject.name.Contains("Portal"))
        //        {
        //            string[] names = other.gameObject.name.Split('/');
        //            PhotonNetwork.LoadLevel(names[1]);
        //        }
        //    }
        //}
    }
    //[PunRPC]
    //public void RPC_PlayerMove()
    //{
    //    print("1");
    //    float h = Input.GetAxisRaw("Horizontal");
    //    float v = Input.GetAxisRaw("Vertical");

    //    Vector3 dir = Vector3.right * h + Vector3.forward * v;

    //    Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0);
    //    transform.rotation = Quaternion.LookRotation(newDir);
    //    transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    //}
}
