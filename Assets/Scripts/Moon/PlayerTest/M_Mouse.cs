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
                        
                    //재료 상자 클릭하면 재료 생성
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
                    //만약 재료를 든 상태에서
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
                    //뭔가 든 상태에서 박스를 누르면 박스 위에 올라감
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
                    //아니면 그냥 그곳에 두기
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

    //재료 상자에서 재료 꺼내기
    void TakeOutIngredient(RaycastHit hit)
    {
        hitObject = Instantiate(hitObject.GetComponent<M_IngredientBox>().ingredientPrefab);
        hitObject.transform.position = hit.transform.position;
    }

    //들고 있는 오브젝트 움직이기
    void ObjectMove()
    {
        if (hitObject)
        {
            mouseWorldPos.y = 2f;
            hitObject.transform.position = mouseWorldPos;
        }
    }
}
