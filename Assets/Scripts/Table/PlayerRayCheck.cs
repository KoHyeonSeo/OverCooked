using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRayCheck : MonoBehaviourPun, IPunObservable
{
    public GameObject getObject; //현재 플레이어가 들고 있는 물건
    public GameObject interactiveObject; //현재 플레이어와 닿아있는 물건, 테이블
    public GameObject lastCuttingTable; //현재 어떤 컷팅 테이블과 닿고 있는지
    public GameObject lastTable;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            SetPlayerObject();
            ShootRay();
            CheckLastTable();
            CheckPlayerInteractive();
        }
    }

    void ShootRay()
    {
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.red, 1);
        if (Physics.Raycast(ray, out hit, 1))
        {
           interactiveObject = hit.transform.gameObject;
            
        }
        else
        {
            interactiveObject = null;
        }
        if (interactiveObject && interactiveObject.tag == "Player")
            return;
        if (interactiveObject && interactiveObject.tag == "Food")
        {
            ray = new Ray(interactiveObject.transform.position, transform.forward);
            Debug.DrawRay(interactiveObject.transform.position, transform.forward, Color.red, 1);
            RaycastHit hit2;
            if (Physics.Raycast(ray, out hit2, 1))
            {
                interactiveObject = hit2.transform.gameObject;
            }
        }
        else if (interactiveObject && interactiveObject.GetComponent<Plate>())
        {
            ray = new Ray(interactiveObject.transform.position, transform.forward);
            //Debug.DrawRay(interactiveObject.transform.position, transform.forward, Color.red, 1);
            RaycastHit hit2;
            if (Physics.Raycast(ray, out hit2, 1))
            {
                interactiveObject = hit2.transform.gameObject;
            }
        }
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

    public void CheckPlayerInteractive()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactiveObject && interactiveObject.GetComponent<M_Table>())
            {
                interactiveObject.GetComponent<M_Table>().StopBlink();
                if (interactiveObject.GetComponent<IngredientBox>())
                    InteractiveIngredientBox();
                else if (interactiveObject.GetComponent<CuttingTable>())
                    InteractiveCuttingTable();
                else if (interactiveObject.GetComponent<FireBox>())
                    InteractiveFireTable();
                else if (interactiveObject.name == "ServiceDesk")
                    InteractiveServiceDesk();
                else if (interactiveObject.GetComponent<M_Table>())
                    InteractiveTable();

            }
            else if (interactiveObject && !getObject)
            {
                //물건 드는건 플레이어 쪽에서 처리(나중에 바꿔)
                HavingSettingObject(interactiveObject);
            }
            else if (!interactiveObject && getObject)
            {
                getObject.GetComponent<Rigidbody>().useGravity = true;
                getObject.GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<PlayerInteract>().GrabbingObjectInfo.transform.parent = null;
                GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
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
           // GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
        }
        else
        {
            if (interactiveObject.GetComponent<M_Table>().getObject)
            {
                HavingSettingObject(interactiveObject.GetComponent<M_Table>().getObject);
                interactiveObject.GetComponent<M_Table>().getObject = null;
            }
        }
    }

    //재료 상자
    [PunRPC]
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
            {
                HavingSettingObject(cuttingTable.cutTableObject);
                cuttingTable.cutTableObject = null;
            }
            //만약 다 잘리지 않은 재료가 테이블에 있다면 썰기 시작
            else
            {
                cuttingTable.isPlayerExit = true;
            }
                
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
            {
                HavingSettingObject(fireBox.cookingTool);
                fireBox.cookingTool = null;
            }
                
        }
    }

    [PunRPC]
    void HavingSettingObject(GameObject obj)
    {
        /*if (!getObject)
        {
            //obj.GetComponent<Rigidbody>().useGravity = false;
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, -0.3f, 0.5f);
        }*/
        if (!getObject)
            CreateNew.HavingSetting(obj, "Grab", true, transform, new Vector3(0, -0.3f, 0.5f));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(getObject);
            stream.SendNext(interactiveObject);
        }
        else
        {
            getObject = (GameObject)stream.ReceiveNext();
            interactiveObject = (GameObject)stream.ReceiveNext();
        }
    }
}
