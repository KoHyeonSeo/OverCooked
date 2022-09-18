using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public GameObject interactiveObject;
    public GameObject getObject;
    public GameObject cutTable;
    Vector3 objectPosition;
    private PlayerCreateNew createNew;
    void Start()
    {
        //objectPosition.position = new Vector3(0, 1, 0);
        objectPosition = new Vector3(0f, -0.2f ,0.6f);
        createNew = GetComponent<PlayerCreateNew>();
    }

    void Update()
    {
        CheckRay();
    }

    void CheckRay()
    {
        getObject = GetComponent<PlayerInteract>().GrabbingObjectInfo;
        interactiveObject = GetComponent<PlayerInteract>().PointObject;
        if (getObject)
        {
            getObject.layer = 10;
        }
        else
        {
            
        }
        if (interactiveObject)
        {
            if (interactiveObject.GetComponent<CutBox>())
            {
                cutTable = interactiveObject;
            }
            if (cutTable)
            {
                if (cutTable != interactiveObject)
                {
                    cutTable.GetComponent<CutBox>().isPlayerExit = false;
                    cutTable = null;
                }
            }
        }
        else
        {
            
            if (cutTable)
            {
                cutTable.GetComponent<CutBox>().isPlayerExit = false;
                cutTable = null;
            }
        }
        

        /*ray = new Ray(transform.position, transform.right);
        Debug.DrawRay(transform.position, transform.right, Color.blue);
        RayHit();
        ray = new Ray(transform.position, transform.right * -1);
        Debug.DrawRay(transform.position, transform.right * -1, Color.blue);
        RayHit();
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        RayHit();*/
        if (/*Input.GetKeyDown(KeyCode.E) || */GetComponent<PlayerInput>().LeftClickDown)
        {
            if (interactiveObject)
            {
                if (interactiveObject.name.Contains("ServiceDesk"))
                {
                    if (getObject.GetComponent<Plate>())
                    {
                        OrderSheetManager.instance.CheckOrderSheet(getObject.GetComponent<Plate>());
                    }
                }
                else if (interactiveObject.GetComponent<M_IngredientBox>())
                {
                    InteractiveIngredientBox();
                }
                else if (interactiveObject.GetComponent<CutBox>())
                {
                    InteractiveCutTable();
                }
                else if (interactiveObject.GetComponent<M_Box>())
                {
                    InteractiveTable();
                }
                else if (interactiveObject.GetComponent<FireBox>())
                {
                    InteractiveFireTable();
                }
                else if (interactiveObject.tag == "Food")
                {
                    //SetGetObject(interactiveObject);
                }
            }
            else
            {
                if (getObject)
                {
                    getObject.transform.parent = null;
                    getObject.layer = 0;
                    getObject = null;
                }
            }
        }
        
    }

    void InteractiveTable()
    {
        print("Table 상호작용");
        //뭔가 들고 있을 때
        if (getObject)
        {
            interactiveObject.GetComponent<M_Box>().SetObject(getObject);
            getObject = null;
        }
        //들고있는게 없을 때
        else
        {
            if(interactiveObject.GetComponent<M_Box>().getObject)
            {
                SetGetObject(interactiveObject.GetComponent<M_Box>().getObject);
                interactiveObject.GetComponent<M_Box>().getObject = null;
            }
        }
    }

    void InteractiveIngredientBox()
    {
        //뭔가 들고 있을 때
        if (getObject)
        {

        }
        //들고있는게 없을 때
        //재료 생성 후 getObject에 넣어쥼
        else
        {
            /*GameObject ingredient = Instantiate(interactiveObject.GetComponent<M_IngredientBox>().ingredientPrefab);
                    GetComponent<PlayerCreateNew>().CreatesNewObject(ingredient, "Grab");*/

            //ingredient.transform.parent = transform;
            //ingredient.transform.localPosition = new Vector3(0, -.5f, .5f);
            //ingredient.transform.position = objectPosition.position;

            /*GameObject ingredient = Instantiate(interactiveObject.GetComponent<M_IngredientBox>().ingredientPrefab);
            string[] names = ingredient.name.Split('(');
            ingredient.name = names[0];
            SetGetObject(ingredient);*/
        }
    }

    void InteractiveFireTable()
    {
        //뭔가 들고 있을 때
        if (getObject)
        {
            if (getObject.GetComponent<IngredientDisplay>())
            {
                if(getObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleBake)
                {
                    interactiveObject.GetComponent<FireBox>().SetObject(getObject);
                    getObject = null;
                }
            }
        }
        //들고있는게 없을 때
        else
        {
            if (interactiveObject.GetComponent<FireBox>().getObject)
            {
                SetGetObject(interactiveObject.GetComponent<FireBox>().getObject);
                interactiveObject.GetComponent<FireBox>().getObject = null;
            }
        }
    }

    void InteractiveCutTable()
    {
        
        //뭔가 들고 있을 때
        if (getObject)
        {
            interactiveObject.GetComponent<CutBox>().SetObject(getObject);
            getObject = null;
        }
        //들고있는게 없을 때
        else
        {
            cutTable.GetComponent<CutBox>().isPlayerExit = true;
            if (cutTable.GetComponent<CutBox>().getObject.GetComponent<IngredientDisplay>().isCut)
            {
                getObject = cutTable.GetComponent<CutBox>().getObject;
                SetGetObject(getObject);
                cutTable.GetComponent<CutBox>().getObject = null;
            }
        }
    }

    void RayHit()
    {
        if (Physics.Raycast(ray, out hit, 1))
        {
            //들고 있는게 음식이면 Ray 한 번 더 쏨
            if (getObject && hit.transform.tag == "Food")
            {
                ray = new Ray(hit.transform.position, transform.forward);
                Debug.DrawRay(hit.transform.position, transform.forward, Color.blue);
                if (Physics.Raycast(ray, out hit, 1))
                {
                    interactiveObject = hit.transform.gameObject;
                } 
            }
            else
                interactiveObject = hit.transform.gameObject;
        }
        else
        {
            interactiveObject = null;
        }
    }

    void SetGetObject(GameObject obj)
    {
        //obj.transform.parent = transform;
        createNew.PlayerHaving(obj, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
        obj.transform.position = objectPosition;
        getObject = obj;
    }
}
