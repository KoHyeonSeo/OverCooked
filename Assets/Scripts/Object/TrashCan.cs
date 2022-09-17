using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    //���ó� �������� �ȿ� �ִ� "����"�� ���� �� �ִ�.
    //���ĸ� ��� ���� ���� ������ ���� �� �ִ�.

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
        //��� �� ��ü�� ���
        if (hit.transform.childCount == 0)
        {
            if (hit.transform.CompareTag("Food"))
            {
                //����
                //1. �÷��̾�� �ڽ��� �־���Ѵ�(��Ḧ ������ �ִ� ����)
                //2. �÷��̾��� �ڽ��� table�� ray�� ���� ���̿����Ѵ�.
                //3. �÷��̾�� ��Ŭ���� �ؾ��Ѵ�. (����Ű�� �������Ѵ�.)
                if (player.transform.childCount != 1
                    && player.transform.GetChild(1) == hit.transform
                    && player.GetComponent<PlayerInput>().LeftClickDown)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        //�丮�� ���������� ������� ����� ���
        else
        {
            for (int i = 0; i < hit.transform.childCount; i++)
            {
                if (hit.transform.GetChild(i).CompareTag("Food"))
                {
                    //����
                    //1. �÷��̾�� �ڽ��� �־���Ѵ�(��Ḧ ������ �ִ� ����)
                    //2. �÷��̾��� �ڽ��� table�� ray�� ���� ���̿����Ѵ�.
                    //3. �÷��̾�� ��Ŭ���� �ؾ��Ѵ�. (����Ű�� �������Ѵ�.)
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
