using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireExtinguisher : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float extinguisher = 8f;
    GameObject player;
    private RaycastHit hit;
    private Vector3 recievePos;
    private Quaternion recieveRot;

    private bool isFireParticle = false;

    private void Start()
    {
        ObjectManager.instance.SetPhotonObject(gameObject);
        if (GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    private void Update()
    {
        if (!player)
            player = GameManager.instance.Player;
        if (photonView.IsMine)
        {
            if (player.transform.childCount> 1
                && player.transform.GetChild(1).name==transform.name
                )
            {
                if (player.GetComponent<PlayerInput>().FireExtinguisher)
                {
                    Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward);

                    photonView.RPC("FireParticle", RpcTarget.All, true);

                    //소화기 쏘는 부분이 닿았니
                    Debug.DrawRay(ray.origin, ray.direction * 5, Color.magenta);
                    if (Physics.Raycast(ray, out hit, 5))
                    {
                        if (hit.transform.name.Contains("FireTable"))
                        {
                            if (hit.transform.GetComponent<FireBox>().isFire)
                            {
                                //hit.transform.GetComponent<FireBox>().FireSuppression(extinguisher * Time.deltaTime);
                                photonView.RPC("FireSuppression", RpcTarget.All, (extinguisher * Time.deltaTime));
                            }
                        }
                    }
                }
                else
                {
                    photonView.RPC("FireParticle", RpcTarget.All, false);
                }
            }
        }
        else if(transform.parent == null)
        {
            transform.position = Vector3.Lerp(transform.position, recievePos, Time.deltaTime * 7);
            transform.rotation = Quaternion.Lerp(transform.rotation, recieveRot, Time.deltaTime * 7);
        }

    }
    [PunRPC]
    public void FireSuppression(float ex)
    {
        hit.transform.GetComponent<FireBox>().FireSuppression(ex);
    }
    [PunRPC]
    public void FireParticle(bool isFire)
    {
        if (transform.childCount == 1)
        {
            if (transform.GetChild(0).childCount == 1)
                transform.GetChild(0).GetChild(0).gameObject.SetActive(isFire);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (transform.parent != null) return;

        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient && !transform.parent)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
        }
        else
        {
            recievePos = (Vector3)stream.ReceiveNext();
            recieveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
