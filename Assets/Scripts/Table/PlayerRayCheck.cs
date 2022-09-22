using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    public GameObject getObject; //현재 플레이어가 들고 있는 물건
    public GameObject interactiveObject; //현재 플레이어와 닿아있는 물건, 테이블
    public GameObject lastCuttingTable; //현재 어떤 컷팅 테이블과 닿고 있는지
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

    void CheckLastTable()
    {
        if (interactiveObject && interactiveObject.GetComponent<M_Table>())
        {
            if (interactiveObject.GetComponent<M_Table>())
                interactiveObject.GetComponent<M_Table>().BlinkTable();
            //현재 닿은 책상과 마지막에 닿은 책상이 다르다면 깜빡임 멈춤
            if (lastTable && interactiveObject != lastTable)
            {
                interactiveObject.GetComponent<M_Table>().StopBlink();
                lastTable = null;
            }
            //현재 닿은 책상과 마지막에 닿은 책상이 다르면 컷팅테이블 조건 삭제
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

    //재료 상자
    void InteractiveIngredientBox()
    {
        GameObject ingredient = interactiveObject.GetComponent<IngredientBox>().CreateIngredient();
        HavingSettingObject(ingredient);
    }

    //자르는 테이블
    void InteractiveCuttingTable()
    {
        CuttingTable cuttingTable = interactiveObject.GetComponent<CuttingTable>();
        if (getObject)
        {
            //들고 있는 오브젝트 테이블에 내려 놓기
            //자를 수 있는 음식을 들고 있으면 내려 놓음
            if (getObject.GetComponent<IngredientDisplay>() && getObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut)
                cuttingTable.SetObject(getObject);
            //접시를 들고있으면서 컷팅테이블에 올라간 재료가 잘린 상태라면
            else if (getObject.tag == "Plate" && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
                getObject.GetComponent<Plate>().GetIngredient(cuttingTable.cutTableObject);    
        }
        else
        {
            lastCuttingTable = interactiveObject;
            //만약 다 잘린 재료가 테이블에 있다면 플레이어가 가져감
            if (cuttingTable.cutTableObject && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
                HavingSettingObject(cuttingTable.cutTableObject);
            //만약 다 잘리지 않은 재료가 테이블에 있다면 썰기 시작
            else
                cuttingTable.isPlayerExit = true;
        }
    }

    //화덕
    void InteractiveFireTable()
    {
        FireBox fireBox = interactiveObject.GetComponent<FireBox>();
        if (getObject)
        {
            //도구가 있으면서 도구 위에 아무것도 없고 가진게 재료일 때
            if (fireBox.cookingTool && !fireBox.cookingTool.GetComponent<FryingPan>().getObject && getObject.GetComponent<IngredientDisplay>())
            {
                //화덕 위 요리 도구 위에 재료 셋팅
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
