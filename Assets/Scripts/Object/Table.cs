using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Table : MonoBehaviour
{
    GameObject player;
    RaycastHit hit;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;
    RaycastHit hit5;
    [Header("Table의 초기 자식의 갯수 (재료를 들지 않은)")]
    public int tableChild = 0;
    private void Start()
    {
        if(GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    private void Update()
    {
        if (!player)
            player = GameManager.instance.Player;
        LayerMask layer = 1 << LayerMask.NameToLayer("Table");
        Ray ray = new Ray(transform.position, - transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit4, 1, ~layer))
        {
            HitRay(hit4);
        }
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray, out hit, 1,~layer))
        {
            HitRay(hit);
        }
        ray = new Ray(transform.position, transform.right);
        Debug.DrawRay(ray.origin, ray.direction, Color.black);
        if (Physics.Raycast(ray, out hit2, 1, ~layer))
        {
            HitRay(hit2);
        }
        ray = new Ray(transform.position , - transform.right);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue);
        if (Physics.Raycast(ray, out hit3, 1,~layer))
        {
            HitRay(hit3);
        }
        ray = new Ray(transform.position, transform.up);
        Debug.DrawRay(ray.origin, ray.direction, Color.magenta);
        if (Physics.Raycast(ray, out hit5, 1, ~layer))
        {
            HitRayUP(hit5);
        }
    }
    private void HitRayUP(RaycastHit hit)
    {
        //던져서 올라갈 경우, 무조건 올라가도록
        if (hit.transform.CompareTag("Food") &&
            hit.transform.gameObject.layer == LayerMask.NameToLayer("Default") &&
            transform.childCount == tableChild)
        {
            player.GetComponent<PlayerCreateNew>().
                CreatesNewObject(hit.transform.gameObject, "Table", true, transform, new Vector3(0, 1, 0));
       
        }
    }
    private void HitRay(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Food") && transform.childCount == tableChild)
        {
            //조건
            //1. 플레이어는 자식이 있어야한다(재료를 가지고 있는 상태)
            //2. 플레이어의 자식이 table의 ray를 맞은 아이여야한다.
            //3. 플레이어는 좌클릭을 해야한다. (놓기키를 눌러야한다.)
            if (player.transform.childCount != 1
                && player.transform.GetChild(1) == hit.transform
                && player.GetComponent<PlayerInput>().LeftClickDown)
            {
                player.GetComponent<PlayerCreateNew>().
                    CreatesNewObject(hit.transform.gameObject, "Table", true, transform, new Vector3(0, 1, 0));

            }
        }
        else if (hit.transform.CompareTag("Player"))
        {
            if (transform.childCount == tableChild || (hit.transform.childCount != 1 && hit.transform.GetChild(1).CompareTag("Food")))
            {
                return;
            }
            else
            {
                //우클릭하여 집기
                if (hit.transform.gameObject.GetComponent<PlayerInput>().LeftClickDown)
                {
                    player.GetComponent<PlayerCreateNew>().
                        CreatesNewObject(transform.GetChild(tableChild).gameObject, "Food", true, hit.transform, new Vector3(0, -0.5f, 0.5f));
                  
                }
            }
        }
    }
}
