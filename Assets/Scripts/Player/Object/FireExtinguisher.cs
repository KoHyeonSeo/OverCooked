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
    private bool isFire = false;

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

        if (isFire)
        {
            hit.transform.GetComponent<FireBox>().FireSuppression(extinguisher * Time.deltaTime);
        }

        if (photonView.IsMine)
        {
            if (player.transform.childCount > 1
                && player.transform.GetChild(1).name == transform.name
                )
            {
                if (player.GetComponent<PlayerInput>().FireExtinguisher)
                {
                    photonView.RPC("FireParticle", RpcTarget.All, true);
                    photonView.RPC("FireSuppression", RpcTarget.All,
                        new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z),
                        transform.forward, true);
                }
                else
                {
                    photonView.RPC("FireSuppression", RpcTarget.All,
                        new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z),
                        transform.forward, false);
                    photonView.RPC("FireParticle", RpcTarget.All, false);
                }
            }
        }
        else if (transform.parent == null)
        {
            transform.position = Vector3.Lerp(transform.position, recievePos, Time.deltaTime * 7);
            transform.rotation = Quaternion.Lerp(transform.rotation, recieveRot, Time.deltaTime * 7);
        }

    }
    [PunRPC]
    public void FireSuppression(Vector3 origin, Vector3 dir, bool push)
    {
        if (push)
        {
            Ray ray = new Ray(origin, dir);
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.magenta);
            LayerMask layer = 1 << LayerMask.NameToLayer("FireExtinguisher") & LayerMask.NameToLayer("grab");
            //소화기 쏘는 부분이 닿았니
            if (Physics.Raycast(ray, out hit, 5, ~layer))
            {
                Debug.Log(hit.transform);
                if (hit.transform.name.Contains("FireTable"))
                {
                    if (hit.transform.GetComponent<FireBox>().isFire)
                    {
                        isFire = true;
                    }
                }
            }
        }
        else
        {
            isFire = false;
        }
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
