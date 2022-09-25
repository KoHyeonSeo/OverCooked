using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRayCheck : MonoBehaviourPun, IPunObservable
{
    public GameObject getObject; //���� �÷��̾ ��� �ִ� ����
    public GameObject interactiveObject; //���� �÷��̾�� ����ִ� ����, ���̺�
    public GameObject lastCuttingTable; //���� � ���� ���̺�� ��� �ִ���
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
                //���� ��°� �÷��̾� �ʿ��� ó��(���߿� �ٲ�)
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
            {
                HavingSettingObject(cuttingTable.cutTableObject);
                cuttingTable.cutTableObject = null;
            }
            //���� �� �߸��� ���� ��ᰡ ���̺� �ִٸ� ��� ����
            else
            {
                cuttingTable.isPlayerExit = true;
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
