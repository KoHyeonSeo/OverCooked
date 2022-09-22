using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    public GameObject getObject; //���� �÷��̾ ��� �ִ� ��
    public GameObject interactiveObject; //���� �÷��̾�� ����ִ� ��
    public GameObject lastTable; //���������� ����ִ� ���̺�
    public GameObject cutTable; //���� ����ִ� ���� ���̺�
    public GameObject sinkTable; //���� ����ִ� ��ũ��

    /*void Start()
    {

    }

    void Update()
    {
        CheckPlayer(); //�÷��̾ ��� �ִ� ��, ��� �ִ� ���̺� ã��
        CheckRay(); //
        CheckPlayerClick();
    }

    void CheckPlayer()
    {
        //�÷��̰� �� ��� ������ getObject�� ����
        if (GetComponent<PlayerInteract>().GrabbingObjectInfo)
            getObject = GetComponent<PlayerInteract>().GrabbingObjectInfo;
        else
            getObject = null;
        //�÷��� ��� ��������� interactiveObject�� ����
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
        //å�� ����ִٸ�
        if (interactiveObject && interactiveObject.GetComponent<M_Table>())
        {
            //�����Ÿ�
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
        //�����Ÿ�
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
        print("Table ��ȣ�ۿ�");
        //���� ��� ���� ��
        if (getObject)
        {
            interactiveObject.GetComponent<M_Box>().SetObject(getObject);
            //getObject = null;
            GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
        }
        //����ִ°� ���� ��
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
        //���� ��� ���� ��
        if (getObject)
        {

        }
        //����ִ°� ���� ��
        //��� ���� �� getObject�� �־���
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
        //���� ��� ���� ��
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
        //����ִ°� ���� ��
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
        
        //���� ��� ���� ��
        if (getObject)
        {
            interactiveObject.GetComponent<CuttingTable>().SetObject(getObject);
            //getObject = null;
            GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
        }
        //����ִ°� ���� ��
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
