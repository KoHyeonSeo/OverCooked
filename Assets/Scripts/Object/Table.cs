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
        //������ �ö� ���, ������ �ö󰡵���
        if (hit.transform.CompareTag("Food") && transform.childCount == 0)
        {
            GameObject food = Instantiate(hit.transform.gameObject);
            //layer�� Table���Ͽ� Table�� ���� �ʰԲ�
            food.layer = LayerMask.NameToLayer("Table");
            //�̸��� Clone�� �����.
            string[] names = food.name.Split('(');
            food.name = names[0];
            //Food�� Table�� �ڽ����� ����
            food.transform.parent = transform;
            //����ó�� & ��ġ ����
            food.GetComponent<Rigidbody>().isKinematic = true;
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            food.transform.localPosition = new Vector3(0, 1, 0);
            food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            food.GetComponent<Rigidbody>().useGravity = false;
            
            //������ ���� �����
            Destroy(hit.transform.gameObject);
        }
    }
    private void HitRay(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Food") && transform.childCount == 0)
        {
            //����
            //1. �÷��̾�� �ڽ��� �־���Ѵ�(��Ḧ ������ �ִ� ����)
            //2. �÷��̾��� �ڽ��� table�� ray�� ���� ���̿����Ѵ�.
            //3. �÷��̾�� ��Ŭ���� �ؾ��Ѵ�. (����Ű�� �������Ѵ�.)
            if (player.transform.childCount != 1
                && player.transform.GetChild(1) == hit.transform
                && player.GetComponent<PlayerInput>().LeftClickDown)
            {
                GameObject food = Instantiate(hit.transform.gameObject);
                //layer�� Table���Ͽ� Table�� ���� �ʰԲ�
                food.layer = LayerMask.NameToLayer("Table");
                //�̸��� Clone�� �����.
                string[] names = food.name.Split('(');
                food.name = names[0];
                //����� �θ� Table�� ��´�.
                food.transform.parent = transform;
                //����ó�� & ��ġ ����
                food.GetComponent<Rigidbody>().isKinematic = true;
                food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                food.transform.localPosition = new Vector3(0, 1, 0);
                food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                food.GetComponent<Rigidbody>().useGravity = false;

                //������ ���� �����
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
                //��Ŭ���Ͽ� ����
                if (hit.transform.gameObject.GetComponent<PlayerInput>().LeftClickDown)
                {
                    GameObject food = Instantiate(transform.GetChild(0).gameObject);
                    //Clone ���� �����
                    string[] names = food.name.Split('(');
                    food.name = names[0];
                    //layer�� Player�� �����Ͽ� Player�� �浹ó���� �ȵǵ���
                    food.layer = LayerMask.NameToLayer("Player");
                    //food�� �θ� Player�� �ٲ۴�.
                    food.transform.parent = hit.transform;
                    //����ó�� �� ��ġ����
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    food.transform.localPosition = new Vector3(0, -0.3f, 0) + transform.forward * 1f;
                    food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    food.GetComponent<Rigidbody>().useGravity = false;
                    //������ ���� �����.
                    Destroy(transform.GetChild(0).gameObject);
                }
            }
        }
    }
}
