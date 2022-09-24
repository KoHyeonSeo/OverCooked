using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    public GameObject getObject; //���� �÷��̾ ��� �ִ� ����
    public GameObject interactiveObject; //���� �÷��̾�� ����ִ� ����, ���̺�
    public GameObject lastCuttingTable; //���� � ���� ���̺�� ��� �ִ���
    public GameObject lastTable;

    void Start()
    {
        
    }

    void Update()
    {
        SetPlayerObject();
        CheckLastTable();
        CheckPlayerInteractive();
    }

    //�÷��̾ ��� �ִ°�, ����ִ� �� üũ
    void SetPlayerObject()
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

    //�������� ���� ���̺�� üũ
    void CheckLastTable()
    {
        if (interactiveObject && interactiveObject.GetComponent<M_Table>())
        {
            if (interactiveObject.GetComponent<M_Table>())
                interactiveObject.GetComponent<M_Table>().BlinkTable();
            //���� ���� å��� �������� ���� å���� �ٸ��ٸ� ������ ����
            if (lastTable && interactiveObject != lastTable)
            {
                interactiveObject.GetComponent<M_Table>().StopBlink();
                lastTable = null;
            }
            //���� ���� å��� �������� ���� å���� �ٸ��� �������̺� ���� ����
            if (lastCuttingTable && interactiveObject != lastCuttingTable)
            {
                lastCuttingTable.GetComponent<CuttingTable>().isPlayerExit = false;
                lastCuttingTable = null;
            }
            lastTable = interactiveObject;
        }
        else if (!interactiveObject)
        {
            if (lastTable && lastTable.GetComponent<M_Table>())
            {
                lastTable.GetComponent<M_Table>().StopBlink();
                lastTable = null;
            }
            if (lastCuttingTable && lastCuttingTable.GetComponent<CuttingTable>())
            {
                lastCuttingTable.GetComponent<CuttingTable>().isPlayerExit = false;
                lastCuttingTable = null;
            }
        }   
    }

    void CheckPlayerInteractive()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //å�� ����� ��
            if (interactiveObject && interactiveObject.GetComponent<M_Table>())
            {
                interactiveObject.GetComponent<M_Table>().StopBlink();
                //��� ���ڶ��
                if (interactiveObject.GetComponent<IngredientBox>())
                    InteractiveIngredientBox();
                //������ �ִ� å���̶��
                else if (interactiveObject.GetComponent<CuttingTable>())
                    InteractiveCuttingTable();
                //���� �ִ� å���̶��
                else if (interactiveObject.GetComponent<FireBox>())
                    InteractiveFireTable();
                //������ �����ϴ� ���̶��
                else if (interactiveObject.name == "ServiceDesk")
                    InteractiveServiceDesk();
                //�Ϲ� ���̺��̶��
                else if (interactiveObject.GetComponent<M_Table>())
                    InteractiveTable();

            }
            //å���� �ƴѰͰ� ��� ��� �ִ� ���� ���� ��
            else if (interactiveObject && !getObject)
            {
                //���� ��°� �÷��̾� �ʿ��� ó��(���߿� �ٲ�)
                HavingSettingObject(interactiveObject);
            }
            //å�� ���� �ʰ� ���𰡸� ��� ���� ��
            else if(!interactiveObject && getObject)
            {
                //�÷��̾�� �������� �Լ� ȣ��
                /*getObject.GetComponent<Rigidbody>().useGravity = true;
                getObject.GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<PlayerInteract>().GrabbingObjectInfo.transform.parent = null;
                GetComponent<PlayerInteract>().GrabbingObjectInfo = null;*/
            }
        }
    }


    void InteractiveServiceDesk()
    {
        if (getObject)
        {
            if (getObject.GetComponent<Plate>())
            {
                OrderSheetManager.instance.CheckOrderSheet(getObject.GetComponent<Plate>());
            }
        }
    }

    void InteractiveTable()
    {
        if (getObject)
        {
            if (getObject.GetComponent<Plate>() && interactiveObject.GetComponent<M_Table>().getObject)
            {
                getObject.GetComponent<Plate>().GetIngredient(interactiveObject.GetComponent<M_Table>().getObject);
            }
            else if (interactiveObject.GetComponent<M_Table>().getObject && interactiveObject.GetComponent<M_Table>().getObject.GetComponent<Plate>())
            {
                if (getObject.GetComponent<IngredientDisplay>())
                {
                    interactiveObject.GetComponent<M_Table>().getObject.GetComponent<Plate>().GetIngredient(getObject);
                }
            }
            else 
                interactiveObject.GetComponent<M_Table>().SetObject(getObject);
        }
        else
        {
            if (interactiveObject.GetComponent<M_Table>().getObject)
            {
                HavingSettingObject(interactiveObject.GetComponent<M_Table>().getObject);
            }
        }
    }

    //��� ����
    void InteractiveIngredientBox()
    {
        GameObject ingredient = interactiveObject.GetComponent<IngredientBox>().CreateIngredient();
        HavingSettingObject(ingredient);
    }

    //�ڸ��� ���̺�
    void InteractiveCuttingTable()
    {
        CuttingTable cuttingTable = interactiveObject.GetComponent<CuttingTable>();
        if (getObject)
        {
            //�ڸ� �� �ִ� ������ ��� ������ ���� ����
            if (getObject.GetComponent<IngredientDisplay>() && getObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut)
                cuttingTable.SetObject(getObject);
            //���ø� ��������鼭 �������̺� �ö� ��ᰡ �߸� ���¶�� ���ÿ� �ø�
            else if (getObject.tag == "Plate" && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
                getObject.GetComponent<Plate>().GetIngredient(cuttingTable.cutTableObject);    
        }
        else
        {
            lastCuttingTable = interactiveObject;
            //���� �� �߸� ��ᰡ ���̺� �ִٸ� �÷��̾ ������
            if (cuttingTable.cutTableObject && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
            {
                HavingSettingObject(cuttingTable.cutTableObject);
                cuttingTable.cutTableObject = null;
            }
            //���� �� �߸��� ���� ��ᰡ ���̺� �ִٸ� ��� ����
            else
                cuttingTable.isPlayerExit = true;
        }
    }

    //ȭ��
    void InteractiveFireTable()
    {
        FireBox fireBox = interactiveObject.GetComponent<FireBox>();
        if (getObject)
        {
            //������ �����鼭 ���� ���� �ƹ��͵� ���� ������ ����� ��
            if (fireBox.cookingTool && !fireBox.cookingTool.GetComponent<FryingPan>().getObject && getObject.GetComponent<IngredientDisplay>())
            {
                //ȭ�� �� �丮 ���� ���� ��� ����
                fireBox.cookingTool.GetComponent<FryingPan>().SetObject(getObject);
            }
            //ȭ�� ���� ������ �����鼭 �÷��̾ ������ ��� ������
            else if (!fireBox.cookingTool && getObject.GetComponent<FryingPan>())
            {
                //ȭ�� ���� ���� ����
                fireBox.SetObject(getObject);
            }
        }
        else
        {   
            //�÷��̾ �� �� ���� ȭ�� ���� ������ ������ �÷��̾ ���
            if (fireBox.cookingTool && !fireBox.cookingTool.GetComponent<FryingPan>().getObject)
            {
                HavingSettingObject(fireBox.cookingTool);
                fireBox.cookingTool = null;
            }       
        }
    }

    //�÷��̾ ��� �ְ� �ϱ�
    void HavingSettingObject(GameObject obj)
    {
        if (!getObject)
            CreateNew.HavingSetting(obj, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
    }
}
