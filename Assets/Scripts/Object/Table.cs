using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    GameObject player;
    RaycastHit hit;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;
    RaycastHit hit5;
    private void Start()
    {
        if(GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    private void Update()
    {
        if (!player)
            player = GameManager.instance.Player;
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        LayerMask layer = 1 << LayerMask.NameToLayer("Table");
        if (Physics.Raycast(transform.position, transform.position + transform.forward, out hit, 1,~layer))
        {
            HitRay(hit);
        }
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.black);
        if (Physics.Raycast(transform.position, transform.position + transform.right, out hit2, 1, ~layer))
        {
            HitRay(hit2);
        }
        Debug.DrawLine(transform.position, transform.position - transform.right, Color.blue);
        if (Physics.Raycast(transform.position, transform.position - transform.right, out hit3, 1,~layer))
        {
            HitRay(hit3);
        }
        Debug.DrawLine(transform.position, transform.position - transform.forward, Color.green);
        if (Physics.Raycast(transform.position, transform.position - transform.forward, out hit4, 1, ~layer))
        {
            HitRay(hit4);
        }
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.magenta);
        if (Physics.Raycast(transform.position, transform.position + transform.up, out hit5, 1, ~layer))
        {
            HitRayUP(hit5);
        }
    }
    private void HitRayUP(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Food") && transform.childCount == 0)
        {
            GameObject food = Instantiate(hit.transform.gameObject);
            food.layer = LayerMask.NameToLayer("Table");
            food.transform.parent = transform;
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            food.transform.localPosition = new Vector3(0, 1, 0);
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            food.GetComponent<Rigidbody>().useGravity = false;
            Destroy(hit.transform.gameObject);
        }
    }
    private void HitRay(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Food") && transform.childCount == 0)
        {
            if (player.transform.childCount != 0
                && player.transform.GetChild(0) == hit.transform
                && player.GetComponent<PlayerInput>().LeftClickDown)
            {
                GameObject food = Instantiate(hit.transform.gameObject);
                food.layer = LayerMask.NameToLayer("Table");
                food.transform.parent = transform;
                food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                food.transform.localPosition = new Vector3(0, 1, 0);
                food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                food.GetComponent<Rigidbody>().useGravity = false;
                Destroy(hit.transform.gameObject);
            }
        }
        else if (hit.transform.CompareTag("Player"))
        {
            if (transform.childCount == 0 || (hit.transform.childCount != 0 && hit.transform.GetChild(0).CompareTag("Food")))
            {
                return;
            }
            else
            {
                //우클릭하여 집기
                if (hit.transform.gameObject.GetComponent<PlayerInput>().LeftClickDown)
                {
                    GameObject food = Instantiate(transform.GetChild(0).gameObject);
                    food.layer = LayerMask.NameToLayer("Player");
                    food.transform.parent = hit.transform;
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    food.transform.localPosition = Vector3.up * 0.5f + hit.transform.forward * 1.5f;
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    food.GetComponent<Rigidbody>().useGravity = false;
                    Destroy(transform.GetChild(0).gameObject);
                }
            }
        }
    }
}
