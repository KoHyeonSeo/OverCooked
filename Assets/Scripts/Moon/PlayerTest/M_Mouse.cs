using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_Mouse : MonoBehaviour
{
    Vector3 mouseWorldPos;
    public GameObject hitObject;
    //GameObject hitObject;
    float startY;

    void Start()
    {
        
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (!hitObject)
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "Plane")
                        return;
                    hitObject = hit.transform.gameObject;
                    if (hitObject.transform.parent)
                    {
                        
                        hitObject = hitObject.transform.parent.gameObject;
                    }
                        
                    //��� ���� Ŭ���ϸ� ��� ����
                    if (hitObject.GetComponent<M_IngredientBox>())
                    {
                        TakeOutIngredient(hit);
                    }
                    startY = hitObject.transform.position.y;
                }
            }
            else if (hitObject)
            {
                
                Ray ray = new Ray(hitObject.transform.position, hitObject.transform.up * -1);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    print(hit.transform.name);
                    //���� ��Ḧ �� ���¿���
                    if (hit.transform.GetComponent<Plate>())
                    {
                        if (hitObject.GetComponent<IngredientDisplay>())
                        {
                            print("11111");
                                hit.transform.GetComponent<Plate>().GetIngredient(hitObject);
                        }
                    }
                    else if (hit.transform.name == "ServiceDesk")
                    {
                        if (hitObject.GetComponent<Plate>())
                        {
                            print("dffffffffffff");
                            OrderSheetManager.instance.CheckOrderSheet(hitObject.GetComponent<Plate>());
                        }
                    }
                    //���� �� ���¿��� �ڽ��� ������ �ڽ� ���� �ö�
                    else if(hit.transform.GetComponent<M_Box>())
                    {
                        if (!hit.transform.GetComponent<M_Box>().getObject)
                        {
                            hit.transform.GetComponent<M_Box>().SetObject(hitObject);
                            hitObject = null;
                        }
                    }
                    else if (hit.transform.GetComponent<CutBox>())
                    {
                        if (!hit.transform.GetComponent<CutBox>().getObject)
                        {
                            hit.transform.GetComponent<CutBox>().SetObject(hitObject);
                            hitObject = null;
                        }
                    }
                    else if (hit.transform.parent && hit.transform.parent.GetComponent<M_Box>())
                    {
                        print("dfdfdf");
                        hitObject = hit.transform.gameObject;
                        hit.transform.parent.GetComponent<M_Box>().getObject = null;
                        hit.transform.parent = null;
                    }
                    //�ƴϸ� �׳� �װ��� �α�
                    else
                    {
                        mouseWorldPos.y = startY;
                        hitObject.transform.position = mouseWorldPos;
                        hitObject = null;
                    }
                }
            }
        }

        ObjectMove();
    }

    //��� ���ڿ��� ��� ������
    void TakeOutIngredient(RaycastHit hit)
    {
        hitObject = Instantiate(hitObject.GetComponent<M_IngredientBox>().ingredientPrefab);
        hitObject.transform.position = hit.transform.position;
    }

    //��� �ִ� ������Ʈ �����̱�
    void ObjectMove()
    {
        if (hitObject)
        {
            mouseWorldPos.y = 2f;
            hitObject.transform.position = mouseWorldPos;
        }
    }
}
