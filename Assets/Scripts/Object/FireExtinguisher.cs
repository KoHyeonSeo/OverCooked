using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    GameObject player;
    private RaycastHit hit;
    private void Start()
    {
        if (GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    private void Update()
    {
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
                Debug.Log(hit.transform.gameObject);
            }
        }
    }
}
