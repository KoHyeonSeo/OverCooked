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
    private void Start()
    {
        if(GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    public float greenLength = 1;
    private void Update()
    {
        if (!player)
            player = GameManager.instance.Player;
        LayerMask layer = 1 << LayerMask.NameToLayer("Table");
        Ray ray = new Ray(transform.position, - transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit4, greenLength, ~layer))
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
        if (hit.transform.CompareTag("Food") && transform.childCount == 0)
        {
            GameObject food = Instantiate(hit.transform.gameObject);
            //layer를 Table로하여 Table과 닿지 않게끔
            food.layer = LayerMask.NameToLayer("Table");
            //이름의 Clone을 지운다.
            string[] names = food.name.Split('(');
            food.name = names[0];
            //Food를 Table의 자식으로 설정
            food.transform.parent = transform;
            //물리처리 & 위치 조정
            food.GetComponent<Rigidbody>().isKinematic = true;
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            food.transform.localPosition = new Vector3(0, 1, 0);
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            food.GetComponent<Rigidbody>().useGravity = false;
            
            //기존의 것은 지우기
            Destroy(hit.transform.gameObject);
        }
    }
    private void HitRay(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Food") && transform.childCount == 0)
        {
            //조건
            //1. 플레이어는 자식이 있어야한다(재료를 가지고 있는 상태)
            //2. 플레이어의 자식이 table의 ray를 맞은 아이여야한다.
            //3. 플레이어는 좌클릭을 해야한다. (놓기키를 눌러야한다.)
            if (player.transform.childCount != 1
                && player.transform.GetChild(1) == hit.transform
                && player.GetComponent<PlayerInput>().LeftClickDown)
            {
                GameObject food = Instantiate(hit.transform.gameObject);
                //layer를 Table로하여 Table과 닿지 않게끔
                food.layer = LayerMask.NameToLayer("Table");
                //이름의 Clone을 지운다.
                string[] names = food.name.Split('(');
                food.name = names[0];
                //재료의 부모를 Table로 삼는다.
                food.transform.parent = transform;
                //물리처리 & 위치 조정
                food.GetComponent<Rigidbody>().isKinematic = true;
                food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                food.transform.localPosition = new Vector3(0, 1, 0);
                food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                food.GetComponent<Rigidbody>().useGravity = false;

                //기존의 것은 지우기
                Destroy(hit.transform.gameObject);
            }
        }
        else if (hit.transform.CompareTag("Player"))
        {
            if (transform.childCount == 0 || (hit.transform.childCount != 1 && hit.transform.GetChild(1).CompareTag("Food")))
            {
                return;
            }
            else
            {
                //우클릭하여 집기
                if (hit.transform.gameObject.GetComponent<PlayerInput>().LeftClickDown)
                {
                    GameObject food = Instantiate(transform.GetChild(0).gameObject);
                    //Clone 글자 지우기
                    string[] names = food.name.Split('(');
                    food.name = names[0];
                    //layer를 Player로 변경하여 Player와 충돌처리가 안되도록
                    food.layer = LayerMask.NameToLayer("Player");
                    //food의 부모를 Player로 바꾼다.
                    food.transform.parent = hit.transform;
                    //물리처리 및 위치조정
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    food.transform.localPosition = new Vector3(0, -0.3f, 0) + transform.forward * 1f;
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    food.GetComponent<Rigidbody>().useGravity = false;
                    //기존의 것은 지운다.
                    Destroy(transform.GetChild(0).gameObject);
                }
            }
        }
    }
}
