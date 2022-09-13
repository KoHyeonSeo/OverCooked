using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Mouse : MonoBehaviour
{
    Vector3 mouseWorldPos;
<<<<<<< HEAD
    public GameObject hitObject;
=======
    GameObject hitObject;
>>>>>>> parent of 5e48f9e (Merge pull request #13 from KoHyeonSeo/revert-12-MOON)
    float startY;

    void Start()
    {
        
    }

<<<<<<< HEAD
=======
    // Update is called once per frame
>>>>>>> parent of 5e48f9e (Merge pull request #13 from KoHyeonSeo/revert-12-MOON)
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
<<<<<<< HEAD
                    if (hitObject.GetComponent<M_IngredientBox>())
                    {
                        hitObject.GetComponent<M_IngredientBox>().CreateIngredient();
                        hitObject = hitObject.GetComponent<M_IngredientBox>().ingredient;
                    }
=======
>>>>>>> parent of 5e48f9e (Merge pull request #13 from KoHyeonSeo/revert-12-MOON)
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
