using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireExtinguisher : MonoBehaviourPun
{
    [SerializeField] private float extinguisher = 8f;
    GameObject player;
    private RaycastHit hit;

    private void Start()
    {
        if (GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    private void Update()
    {
        if (!photonView.IsMine) return;

        if(!player)
            player = GameManager.instance.Player;
        if (player.GetComponent<PlayerInteract>().curInteractState == PlayerInteract.InteractState.FireDistinguish
            && player.GetComponent<PlayerInput>().FireExtinguisher)
        {
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward);

            //소화기 쏘는 부분이 닿았니
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.magenta);
            if (Physics.Raycast(ray, out hit, 5))
            {
                if (hit.transform.name.Contains("FireTable"))
                {
                    if (hit.transform.GetComponent<FireBox>().isFire)
                    {
                        photonView.RPC("FireSuppression",RpcTarget.All, (extinguisher * Time.deltaTime));
                    }
                }
            }
        }
    }
    [PunRPC]
    public void FireSuppression(float ex)
    {
        hit.transform.GetComponent<FireBox>().FireSuppression(ex);
    }
}
