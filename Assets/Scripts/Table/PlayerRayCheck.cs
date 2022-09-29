using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRayCheck : MonoBehaviourPun, IPunObservable
{
    public GameObject getObject; //���� �÷��̾ ��� �ִ� ����
    public GameObject interactiveObject; //���� �÷��̾�� ����ִ� ����, ���̺�
    public GameObject lastCuttingTable; //���� � ���� ���̺�� ��� �ִ���
    public GameObject sink;
    public GameObject lastTable;
    public int cleanPlate;
    public int dirtyPlate;

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
        Vector3 playerPos = transform.position;
        playerPos.y = playerPos.y * 2 / 3 ;
        ray = new Ray(playerPos, transform.forward);
        Debug.DrawRay(playerPos, transform.forward, Color.red, 1);
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
            else
                interactiveObject = null;
        }
        else if (interactiveObject && interactiveObject.GetComponent<Plate>())
        {
            ray = new Ray(interactiveObject.transform.position, transform.forward);
            Debug.DrawRay(interactiveObject.transform.position, transform.forward, Color.red, 1);
            RaycastHit hit2;
            if (Physics.Raycast(ray, out hit2, 1))
            {
                interactiveObject = hit2.transform.gameObject;
            }
            else
                interactiveObject = null;
        }
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
                lastCuttingTable.GetComponent<CuttingTable>().CheckPlayerExist(false);
                lastCuttingTable = null;
            }
            if (sink && interactiveObject != sink )
            {
                sink.GetComponent<Sink>().isPlayerExit = false;
                sink = null;
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
                lastCuttingTable.GetComponent<CuttingTable>().CheckPlayerExist(false);
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
                {
                    InteractiveCuttingTable();
                    //photonView.RPC("InteractiveCuttingTable", RpcTarget.All);
                }
                else if (interactiveObject.GetComponent<FireBox>())
                    InteractiveFireTable();
                else if (interactiveObject.name == "ServiceDesk")
                    InteractiveServiceDesk();
                else if (interactiveObject.GetComponent<Sink>())
                    InteractiveSink();
                else if (interactiveObject.GetComponent<SinkPlateTable>())
                    InteractiveSinkPlateTable();
                else if (interactiveObject.GetComponent<M_Table>())
                    InteractiveTable();
            }
            else if (interactiveObject && !getObject)
            {
                //���� ��°� �÷��̾� �ʿ��� ó��(���߿� �ٲ�)
                HavingSettingObject(interactiveObject);
            }
            else if (!interactiveObject && getObject)
            {
                GetComponent<PlayerInteract>().GrabbingObjectInfo.transform.parent = null;
                getObject.GetComponent<Rigidbody>().useGravity = true;
                getObject.GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
            }
        }
    }

    void InteractiveSink()
    {
        if (getObject && getObject.GetComponent<Plate>() && dirtyPlate > 0)
        {
            if (getObject.GetComponent<Plate>().isdirty)
            {
                interactiveObject.GetComponent<Sink>().SetPlate(dirtyPlate);
                dirtyPlate = 0;
                Destroy(getObject);
            }
        }
        else if (!getObject)
        {
            sink = interactiveObject;
            interactiveObject.GetComponent<Sink>().isPlayerExit = true;
        }
    }

    void InteractiveSinkPlateTable()
    {
        if (!getObject)
        {
            if (interactiveObject.GetComponent<SinkPlateTable>().cleanPlate > 0)
            {
                //HavingSettingObject(ObjectManager.instance.photonObjectIdList.FindIndex()
                //HavingSettingObject(interactiveObject.GetComponent<SinkPlateTable>().CreatePlate());
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
                else if (getObject.GetComponent<FryingPan>() && getObject.GetComponent<FryingPan>().getObject)
                {
                    interactiveObject.GetComponent<M_Table>().getObject.GetComponent<Plate>().GetIngredient(getObject.GetComponent<FryingPan>().getObject);
                }
            }
            else
            {
                if (getObject.GetComponent<Plate>() && getObject.GetComponent<Plate>().isdirty)
                    return;
                interactiveObject.GetComponent<M_Table>().SetObject(getObject.GetComponent<PhotonView>().ViewID);
                print("View ID: " + getObject.GetComponent<PhotonView>().ViewID);
            }  
            GetComponent<PlayerInteract>().GrabbingObjectInfo = null;
        }
        else
        {
            if (interactiveObject.GetComponent<M_Table>().getObject)
            {
                HavingSettingObject(interactiveObject.GetComponent<M_Table>().getObject);
                interactiveObject.GetComponent<M_Table>().getObject = null;
                //�������� ���õ��
                if (interactiveObject.GetComponent<PlateManager>())
                {
                    dirtyPlate = PlateManager.instance.plateCount;
                    PlateManager.instance.plateList.Clear();
                }
                    
            }
        }
    }

    //��� ����
    [PunRPC]
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
            {
                cuttingTable.SetObject(getObject.GetComponent<PhotonView>().ViewID);
            }
            //���ø� ��������鼭 �������̺� �ö� ��ᰡ �߸� ���¶��
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
            {
                cuttingTable.GetComponent<CuttingTable>().CheckPlayerExist(true);
            }
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
                print("22222");
                //ȭ�� �� �丮 ���� ���� ��� ����
                fireBox.cookingTool.GetComponent<FryingPan>().SetObject(getObject.GetComponent<PhotonView>().ViewID);
            }
            else if (!fireBox.cookingTool && getObject.GetComponent<FryingPan>())
            {
                fireBox.SetObject(getObject.GetComponent<PhotonView>().ViewID);
            }
        }
        else
        {
            if (fireBox.cookingTool)
            {
                print("Fire" + fireBox.cookingTool);
                HavingSettingObject(fireBox.cookingTool);
                fireBox.cookingTool = null;
            }
                
        }
    }

    [PunRPC]
    void HavingSettingObject(GameObject obj)
    {
        int id = obj.GetComponent<PhotonView>().ViewID;
        print("iD" + id);
        GetComponent<PlayerInteract>().CallGrabOnTable_RPC(id);
        obj.GetComponent<PhotonTransformView>().enabled = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
