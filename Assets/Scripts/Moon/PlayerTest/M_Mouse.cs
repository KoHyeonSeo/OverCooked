using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Mouse : MonoBehaviour
{
    Vector3 mouseWorldPos;
    public GameObject hitObject;
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
                    hitObject = hit.transform.gameObject;
                    if (hitObject.GetComponent<M_IngredientBox>())
                    {
                        hitObject.GetComponent<M_IngredientBox>().CreateIngredient();
                        hitObject = hitObject.GetComponent<M_IngredientBox>().ingredient;
                    }
                    startY = hitObject.transform.position.y;
                    print(hit.transform.name);
                }
            }
            else
            {
                mouseWorldPos.y = startY;
                hitObject.transform.position = mouseWorldPos;
                hitObject = null;
            }
        }
        if (hitObject)
        {
            mouseWorldPos.y = 2f;
            hitObject.transform.position = mouseWorldPos;
        }
    }
}
