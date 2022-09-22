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
            if (interactiveObject && interactiveObject.GetComponent<M_Table>())
            {
                interactiveObject.GetComponent<M_Table>().StopBlink();
                if (interactiveObject.GetComponent<IngredientBox>())
                    InteractiveIngredientBox();
                if (interactiveObject.GetComponent<CuttingTable>())
                    InteractiveCuttingTable();
                if (interactiveObject.GetComponent<FireBox>())
                    InteractiveFireTable();
            }
            else if (interactiveObject && !getObject)
            {
                HavingSettingObject(interactiveObject);
            }
            else if(!interactiveObject)
            {
                //GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
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
            //��� �ִ� ������Ʈ ���̺� ���� ����
            //�ڸ� �� �ִ� ������ ��� ������ ���� ����
            if (getObject.GetComponent<IngredientDisplay>() && getObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut)
                cuttingTable.SetObject(getObject);
            //���ø� ��������鼭 �������̺� �ö� ��ᰡ �߸� ���¶��
            else if (getObject.tag == "Plate" && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
                getObject.GetComponent<Plate>().GetIngredient(cuttingTable.cutTableObject);    
        }
        else
        {
            lastCuttingTable = interactiveObject;
            //���� �� �߸� ��ᰡ ���̺� �ִٸ� �÷��̾ ������
            if (cuttingTable.cutTableObject && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
                HavingSettingObject(cuttingTable.cutTableObject);
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
            else if (!fireBox.cookingTool && getObject.GetComponent<FryingPan>())
            {
                
                fireBox.SetObject(getObject);
            }
        }
        else
        {
            if (fireBox.cookingTool)
                HavingSettingObject(fireBox.cookingTool);
        }
    }

    void HavingSettingObject(GameObject obj)
    {
        CreateNew.HavingSetting(obj, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
    }
}
