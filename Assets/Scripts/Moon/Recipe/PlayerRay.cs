using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    public GameObject getObject; //현재 플레이어가 들고 있는 것
    public GameObject interactiveObject; //현재 플레이어와 닿아있는 것
    public GameObject lastTable; //마지막으로 닿아있던 테이블
    public GameObject cutTable; //지금 닿아있는 컷팅 테이블
    public GameObject sinkTable; //지금 닿아있는 싱크대

    /*void Start()
    {

    }

    void Update()
    {
        CheckPlayer(); //플레이어가 들고 있는 것, 닿아 있는 테이블 찾기
        CheckRay(); //
        CheckPlayerClick();
    }

    void CheckPlayer()
    {
        //플레이가 뭘 들고 있으면 getObject에 넣음
        if (GetComponent<PlayerInteract>().GrabbingObjectInfo)
            getObject = GetComponent<PlayerInteract>().GrabbingObjectInfo;
        else
            getObject = null;
        //플레어어가 어디에 닿아있으면 interactiveObject에 넣음
        if (GetComponent<PlayerInteract>().PointObject)
            interactiveObject = GetComponent<PlayerInteract>().PointObject;
        else
            interactiveObject = null;
    }

    void CheckRay()
    {
        CheckTable();
        if (getObject)
        {
            if (interactiveObject)
            {
                if (interactiveObject.GetComponent<M_Table>())
                {
                    M_Table();
                }
            }
            else
            {

            }
        }

        



        if (interactiveObject)
        {
            if (interactiveObject.GetComponent<CuttingTable>())
            {
                cutTable = interactiveObject;
            }
            if (cutTable)
            {
                if (cutTable != interactiveObject)
                {
                    cutTable.GetComponent<CuttingTable>().isPlayerExit = false;
                    cutTable = null;
                }
            }
        }
        else
        {
            
            if (cutTable)
            {
                cutTable.GetComponent<CuttingTable>().isPlayerExit = false;
                cutTable = null;
            }
        }

        if (GetComponent<PlayerInput>().LeftClickDown)
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
                    //InteractiveIngredientBox();
                }
                else if (interactiveObject.GetComponent<CuttingTable>())
                {
                    //InteractiveCutTable();
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
                *//*if (getObject)
                {
                    getObject.transform.parent = null;
                    getObject.layer = 0;
                    getObject = null;
                }*//*
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
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
                    //InteractiveIngredientBox();
                }
                else if (interactiveObject.GetComponent<CuttingTable>())
                {
                    InteractiveCutTable();
                }
                else if (interactiveObject.GetComponent<M_Box>())
                {
                    //InteractiveTable();
                }
                else if (interactiveObject.GetComponent<FireBox>())
                {
                    //InteractiveFireTable();
                }
                else if (interactiveObject.tag == "Food")
                {
                    //SetGetObject(interactiveObject);
                }
            }
            else
            {
                *//*if (getObject)
                {
                    getObject.transform.parent = null;
                    getObject.layer = 0;
                    getObject = null;
                }*//*
            }
        }
    }

    void CheckTable()
    {
        //책상에 닿아있다면
        if (interactiveObject && interactiveObject.GetComponent<M_Table>())
        {
            //깜빡거림
            lastTable = interactiveObject;
            interactiveObject.GetComponent<M_Table>().BlinkTable();
        }
        else if (lastTable)
        {
            lastTable.GetComponent<M_Table>().StopBlink();
            lastTable = null;
        }
    }

    void M_Table()
    {
        //깜빡거림
        lastTable = interactiveObject;
        interactiveObject.GetComponent<M_Table>().BlinkTable();
        if (getObject)
        {

        }
        else
        {

        }
    }

    void CheckPlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (getObject)
            {
                if (!interactiveObject)
                {
                    getObject = null;

                }
                else if (interactiveObject.GetComponent<M_Table>())
                {
                    interactiveObject.GetComponent<M_Table>().SetObject(getObject);
                }
            }
            else if (!getObject)
            {
                if (!interactiveObject)
                {
                    
                }
                else if (interactiveObject.GetComponent<M_Table>())
                {
                    if (interactiveObject.GetComponent<M_Table>().getObject)
                    {
                        CreateNew.HavingSetting(interactiveObject.GetComponent<M_Table>().getObject, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
                    }    
                }
                else if (interactiveObject.GetComponent<IngredientBox>())
                {
                    GameObject ingredient = interactiveObject.GetComponent<IngredientBox>().CreateIngredient();
                    CreateNew.HavingSetting(ingredient, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
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
            //getObject = null;
            GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
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
            *//*GameObject ingredient = Instantiate(interactiveObject.GetComponent<M_IngredientBox>().ingredientPrefab);
                    GetComponent<PlayerCreateNew>().CreatesNewObject(ingredient, "Grab");*//*

            //ingredient.transform.parent = transform;
            //ingredient.transform.localPosition = new Vector3(0, -.5f, .5f);
            //ingredient.transform.position = objectPosition.position;

            *//*GameObject ingredient = Instantiate(interactiveObject.GetComponent<M_IngredientBox>().ingredientPrefab);
            string[] names = ingredient.name.Split('(');
            ingredient.name = names[0];
            SetGetObject(ingredient);*//*
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
            interactiveObject.GetComponent<CuttingTable>().SetObject(getObject);
            //getObject = null;
            GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
        }
        //들고있는게 없을 때
        else
        {
            cutTable.GetComponent<CuttingTable>().isPlayerExit = true;
            if (cutTable.GetComponent<CuttingTable>().cutTableObject.GetComponent<IngredientDisplay>().isCut)
            {
                getObject = cutTable.GetComponent<CuttingTable>().cutTableObject;
                SetGetObject(getObject);
                //cutTable.GetComponent<CutBox>().getObject = null;
                cutTable.GetComponent<Table>().transform.GetChild(0).parent = null;
            }
        }
    }

    void SetGetObject(GameObject obj)
    {
        //createNew.PlayerHaving(obj, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
        //obj.transform.parent = transform;
        //obj.transform.position = objectPosition;
        GetComponent<PlayerInteract>().GrabbingObjectInfo = obj;
    }*/
}
