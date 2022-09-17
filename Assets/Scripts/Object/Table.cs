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
    [Header("Table�� �ʱ� �ڽ��� ���� (��Ḧ ���� ����)")]
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
        //������ �ö� ���, ������ �ö󰡵���
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
            //����
            //1. �÷��̾�� �ڽ��� �־���Ѵ�(��Ḧ ������ �ִ� ����)
            //2. �÷��̾��� �ڽ��� table�� ray�� ���� ���̿����Ѵ�.
            //3. �÷��̾�� ��Ŭ���� �ؾ��Ѵ�. (����Ű�� �������Ѵ�.)
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
                //��Ŭ���Ͽ� ����
                if (hit.transform.gameObject.GetComponent<PlayerInput>().LeftClickDown)
                {
                    player.GetComponent<PlayerCreateNew>().
                        CreatesNewObject(transform.GetChild(tableChild).gameObject, "Food", true, hit.transform, new Vector3(0, -0.5f, 0.5f));
                  
                }
            }
        }
    }
}
