using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    GameObject player;
    RaycastHit hit;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;
    private void Start()
    {
        if (GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    private void Update()
    {
        if (!player)
            player = GameManager.instance.Player;
        LayerMask layer = 1 << LayerMask.NameToLayer("Table");
        Ray ray = new Ray(transform.position, -transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit4, 1, ~layer))
        {
            HitRay(hit4);
        }
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, 1, ~layer))
        {
            HitRay(hit);
        }
        ray = new Ray(transform.position, transform.right);
        Debug.DrawRay(ray.origin, ray.direction, Color.black);
        if (Physics.Raycast(ray, out hit2, 1, ~layer))
        {
            HitRay(hit2);
        }
        ray = new Ray(transform.position, -transform.right);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue);
        if (Physics.Raycast(ray, out hit3, 1, ~layer))
        {
            HitRay(hit3);
        }
    }
    private void HitRay(RaycastHit hit)
    {
        
    }
}
