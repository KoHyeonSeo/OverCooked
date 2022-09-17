using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    //접시나 조리도구 안에 있는 "음식"을 버릴 수 있다.
    //음식만 들고 있을 때도 음식을 버릴 수 있다.

    GameObject player;
    RaycastHit hit;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;
    RaycastHit hit5;
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
        ray = new Ray(transform.position, transform.up);
        Debug.DrawRay(ray.origin, ray.direction, Color.magenta);
        if (Physics.Raycast(ray, out hit5, 1, ~layer))
        {
            HitRayUP(hit5);
        }
    }
    private void HitRayUP(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Food"))
        {
            Destroy(hit.transform.gameObject);
        }
    }
    private void HitRay(RaycastHit hit)
    {
        //재료 그 자체인 경우
        if (hit.transform.childCount == 0)
        {
            if (hit.transform.CompareTag("Food"))
            {
                //조건
                //1. 플레이어는 자식이 있어야한다(재료를 가지고 있는 상태)
                //2. 플레이어의 자식이 table의 ray를 맞은 아이여야한다.
                //3. 플레이어는 좌클릭을 해야한다. (놓기키를 눌러야한다.)
                if (player.transform.childCount != 1
                    && player.transform.GetChild(1) == hit.transform
                    && player.GetComponent<PlayerInput>().LeftClickDown)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        //요리나 조리도구에 담겨져온 재료의 경우
        else
        {
            for (int i = 0; i < hit.transform.childCount; i++)
            {
                if (hit.transform.GetChild(i).CompareTag("Food"))
                {
                    //조건
                    //1. 플레이어는 자식이 있어야한다(재료를 가지고 있는 상태)
                    //2. 플레이어의 자식이 table의 ray를 맞은 아이여야한다.
                    //3. 플레이어는 좌클릭을 해야한다. (놓기키를 눌러야한다.)
                    if (player.transform.childCount != 1
                        && player.transform.GetChild(1) == hit.transform.GetChild(i)
                        && player.GetComponent<PlayerInput>().LeftClickDown)
                    {
                        Destroy(hit.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
    }
}
