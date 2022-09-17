using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public GameObject interactiveObject;
    public GameObject getObject;
    public Transform objectPosition;

    void Start()
    {

    }

    void Update()
    {
        if (getObject)
        {
            //getObject.transform.position = objectPosition.position;
        }
        CheckRay();
    }

    void CheckRay()
    {
        ray = new Ray(transform.position, transform.right);
        Debug.DrawRay(transform.position, transform.right, Color.blue);
        RayHit();
        ray = new Ray(transform.position, transform.right * -1);
        Debug.DrawRay(transform.position, transform.right * -1, Color.blue);
        RayHit();
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        RayHit();
        if (interactiveObject)
        {
            if (Input.GetKeyDown(KeyCode.E))//GetComponent<PlayerInput>().LeftClickDown)
            {
                if (interactiveObject.GetComponent<M_IngredientBox>())
                {
                    GameObject ingredient = Instantiate(interactiveObject.GetComponent<M_IngredientBox>().ingredientPrefab);
                    ingredient.transform.parent = transform;
                    //ingredient.transform.localPosition = new Vector3(0, -.5f, .5f);
                    ingredient.transform.position = objectPosition.position;
                    string[] names = ingredient.name.Split('(');
                    ingredient.name = names[0];
                    getObject = ingredient;
                }
                
            }
        }
        

    }

    void RayHit()
    {
        if (Physics.Raycast(ray, out hit, 1))
        {
            interactiveObject = hit.transform.gameObject;
        }
        else
        {
            interactiveObject = null;
        }
    }
}
