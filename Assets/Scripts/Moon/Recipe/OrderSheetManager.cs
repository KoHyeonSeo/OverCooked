using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//�ֹ��� ����
public class OrderSheetManager : MonoBehaviourPun
{
    public RecipeObject[] recipes;
    public GameObject orderSheetPrefab;
    public List<GameObject> orderSheetList = new List<GameObject>(); //�ֹ��� ����Ʈ
    public GameObject orderSheetPanel;
    int orderCount = 0;
    public UI_ReadyStart readyStart;
    bool isDeleteTime;

    public static OrderSheetManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating("CreateOrderSheet", 2f, 2f);
        }
    }

    void Update()
    {
        /*if (readyStart.IsReady && !isOnce)
        {
            isOnce = true;
            //15�ʸ��� �ֹ��� ����
            InvokeRepeating("CreateOrderSheet", 0f, 15f);
        }*/
    }

    //�ֹ��� ����
    public void CreateOrderSheet()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        orderCount = orderSheetList.Count;
        //�ֹ� 5�� ������ ����
        if (orderCount >= 5)
            return;
        //�����ǵ� �� �������� �ֹ��� ����, �ֹ��� ����Ʈ�� �߰�
        int random = UnityEngine.Random.Range(0, recipes.Length);
        photonView.RPC("RpcMoveOrderSheet", RpcTarget.All, random);
        //StartCoroutine(IeMoveOrderSheet(orderSheet.GetComponent<PhotonView>().ViewID));
    }

    [PunRPC]
    void RpcMoveOrderSheet(int random)
    {
        if (!isDeleteTime)
            StartCoroutine(IeMoveOrderSheet(random));
    }

    //�ֹ��� �̵�
    IEnumerator IeMoveOrderSheet(int random)
    {
        //�ֹ��� ���� ����
        orderCount++;
        GameObject orderSheet = Instantiate(orderSheetPrefab);
        orderSheet.GetComponent<OrderSheet>().recipe = recipes[random];
        orderSheetList.Add(orderSheet);
        //�ֹ��� �ǳڿ� ��ġ
        orderSheet.transform.SetParent(orderSheetPanel.transform);
        //������ġ�� ȭ�� ��
        orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(1920, 0, 0);
        orderSheet.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        float xTargetPos = 0; //������� �̵��ؾ� ��
        float xPos = orderSheet.GetComponent<RectTransform>().position.x; //���� �ֹ����� ��ġ
        for (int i = 0; i < orderSheetList.Count - 1; i++)
        {
            //���� ����Ʈ�� ��� �ֹ��� ���̸�ŭ ���� �ΰ� �̵��ϱ� ���� ���
            xTargetPos += 10 + orderSheetList[i].GetComponent<RectTransform>().rect.width * 0.5f;
        }
        //�ֹ��� �̵�
        while (xTargetPos < xPos)
        {
            if (xTargetPos + 1> xPos)
            {
                xPos = xTargetPos;
                break;
            }
            xPos = Mathf.Lerp(xPos, xTargetPos, 0.1f);
            if (orderSheet)
                orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(xPos, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void DeleteOrderSheet(GameObject orderSheet)
    {
        isDeleteTime = true;
        int orderSheetNum = orderSheetList.IndexOf(orderSheet);
        orderSheetList.Remove(orderSheet);
        for (int i = orderSheetNum; i < orderSheetList.Count; i++)
        {
            float xTargetPos = 0; //������� �̵��ؾ� ��
            float xPos = orderSheetList[i].GetComponent<RectTransform>().position.x; //���� �ֹ����� ��ġ
            for (int j = 0; j < i; j++)
            {
                //���� ����Ʈ�� ��� �ֹ��� ���̸�ŭ ���� �ΰ� �̵��ϱ� ���� ���
                xTargetPos += 10 + orderSheetList[j].GetComponent<RectTransform>().rect.width * 0.5f;
            }
            orderSheetList[i].GetComponent<RectTransform>().localPosition = new Vector3(xTargetPos, 0, 0);
        }
        isDeleteTime = false;
    }

    public IEnumerator IeDeleteOrderSheet(GameObject orderSheet)
    {
        int orderSheetNum = orderSheetList.IndexOf(orderSheet);
        orderSheetList.Remove(orderSheet);
        for (int i = orderSheetNum; i < orderSheetList.Count; i++)
        {
            float xTargetPos = 0; //������� �̵��ؾ� ��
            float xPos = orderSheetList[i].GetComponent<RectTransform>().position.x; //���� �ֹ����� ��ġ
            for (int j = 0; j < i; j++)
            {
                //���� ����Ʈ�� ��� �ֹ��� ���̸�ŭ ���� �ΰ� �̵��ϱ� ���� ���
                xTargetPos += 10 + orderSheetList[j].GetComponent<RectTransform>().rect.width * 0.5f;
            }
            orderSheetList[i].GetComponent<RectTransform>().localPosition = new Vector3(xTargetPos, 0, 0);
            yield return null;
        }
    }

    public void CheckOrderSheet(int id)
    {
        photonView.RPC("RpcCheckOrderSheet", RpcTarget.All, id);
        //RpcCheckOrderSheet(id);
    }

    //�ֹ����� ���� ��
    Plate plate;
    [PunRPC]
    public void RpcCheckOrderSheet(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (!ObjectManager.instance.photonObjectIdList[i])
            {
                ObjectManager.instance.photonObjectIdList.RemoveAt(i);
                continue;
            }
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                plate = ObjectManager.instance.photonObjectIdList[i].GetComponent<Plate>();
            }
        }
        for (int i = 0; i < orderSheetList.Count; i++)
        {
            RecipeObject recipe = orderSheetList[i].GetComponent<OrderSheet>().recipe;
            //������ ��� ������ �ֹ��� �������� ��� ������ �ٸ��� ���� �ֹ�����
            if (plate.ingredientList.Count != recipe.ingredients.Length)
            {
                continue;
            }
            for (int j = 0; j < recipe.ingredients.Length; j++)
            {
                //������ ���� �ֹ��� �������� ��ᰡ ������ ��
                if (!plate.ingredientList.Contains(recipe.ingredients[j]))
                {
                    print("�ٸ� ��ᰡ ��: " + recipe.ingredients[j]);
                    StartCoroutine(WrongPlate(plate));
                    return;
                }
                if (j == recipe.ingredients.Length - 1)
                {
                    orderSheetList[i].GetComponent<OrderSheet>().DestroyOrder();
                    print("����Ʈ�� �ִ� ����");
                    PlateManager.instance.AddDirtyPlate();
                    StageManager.instance.CoinPlus(8);
                    photonView.RPC("RpcDestroyPlate", RpcTarget.All, plate.GetComponent<PhotonView>().ViewID);
                    return;
                }
            }
        }
        print("�ֹ����� ����");
        StartCoroutine(WrongPlate(plate));
    }

    IEnumerator WrongPlate(Plate plate)
    {
        for (int i = 0; i < orderSheetList.Count; i++)
        {
            orderSheetList[i].GetComponent<OrderSheet>().wrongImage.enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < orderSheetList.Count; i++)
        {
            orderSheetList[i].GetComponent<OrderSheet>().wrongImage.enabled = false;
        }
        RpcDestroyPlate(plate.GetComponent<PhotonView>().ViewID);
        //photonView.RPC("RpcDestroyPlate", RpcTarget.All, plate.GetComponent<PhotonView>().ViewID);
        PlateManager.instance.AddDirtyPlate();
        //Destroy(plate.transform.gameObject);
    }

    [PunRPC]
    void RpcDestroyPlate(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (!ObjectManager.instance.photonObjectIdList[i])
            {
                ObjectManager.instance.photonObjectIdList.RemoveAt(i);
                continue;
            }
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                Destroy(ObjectManager.instance.photonObjectIdList[i]);
            }
        }
    }
}
