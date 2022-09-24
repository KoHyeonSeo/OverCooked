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

    //플레이어가 들고 있는것, 닿아있는 것 체크
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

    //마지막에 닿은 테이블들 체크
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
            //책상에 닿았을 때
            if (interactiveObject && interactiveObject.GetComponent<M_Table>())
            {
                interactiveObject.GetComponent<M_Table>().StopBlink();
                //재료 상자라면
                if (interactiveObject.GetComponent<IngredientBox>())
                    InteractiveIngredientBox();
                //도마가 있는 책상이라면
                else if (interactiveObject.GetComponent<CuttingTable>())
                    InteractiveCuttingTable();
                //불이 있는 책상이라면
                else if (interactiveObject.GetComponent<FireBox>())
                    InteractiveFireTable();
                //음식을 제출하는 곳이라면
                else if (interactiveObject.name == "ServiceDesk")
                    InteractiveServiceDesk();
                //일반 테이블이라면
                else if (interactiveObject.GetComponent<M_Table>())
                    InteractiveTable();

            }
            //책상이 아닌것과 닿고 들고 있는 것이 없을 때
            else if (interactiveObject && !getObject)
            {
                //물건 드는건 플레이어 쪽에서 처리(나중에 바꿔)
                HavingSettingObject(interactiveObject);
            }
            //책상에 닿지 않고 무언가를 들고 있을 때
            else if(!interactiveObject && getObject)
            {
                //플레이어에서 내려놓는 함수 호출
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
            //자를 수 있는 음식을 들고 있으면 내려 놓음
            if (getObject.GetComponent<IngredientDisplay>() && getObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut)
                cuttingTable.SetObject(getObject);
            //접시를 들고있으면서 컷팅테이블에 올라간 재료가 잘린 상태라면 접시에 올림
            else if (getObject.tag == "Plate" && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
                getObject.GetComponent<Plate>().GetIngredient(cuttingTable.cutTableObject);    
        }
        else
        {
            lastCuttingTable = interactiveObject;
            //만약 다 잘린 재료가 테이블에 있다면 플레이어가 가져감
            if (cuttingTable.cutTableObject && cuttingTable.cutTableObject.GetComponent<IngredientDisplay>().isCut)
            {
                HavingSettingObject(cuttingTable.cutTableObject);
                cuttingTable.cutTableObject = null;
            }
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
            //화덕 위에 도구가 없으면서 플레이어가 도구를 들고 있으면
            else if (!fireBox.cookingTool && getObject.GetComponent<FryingPan>())
            {
                //화덕 위에 도구 셋팅
                fireBox.SetObject(getObject);
            }
        }
        else
        {   
            //플레이어가 든 게 없고 화덕 위에 도구만 있으면 플레이어가 든다
            if (fireBox.cookingTool && !fireBox.cookingTool.GetComponent<FryingPan>().getObject)
            {
                HavingSettingObject(fireBox.cookingTool);
                fireBox.cookingTool = null;
            }       
        }
    }

    //플레이어가 들고 있게 하기
    void HavingSettingObject(GameObject obj)
    {
        if (!getObject)
            CreateNew.HavingSetting(obj, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
    }
}
