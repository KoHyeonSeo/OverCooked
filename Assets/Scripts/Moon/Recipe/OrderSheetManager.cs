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
    private bool isOnce = false;
    

    public static OrderSheetManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating("CreateOrderSheet", 0f, 15f);
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
        //�ֹ��� ���� ����
        orderCount++;
        //�����ǵ� �� �������� �ֹ��� ����, �ֹ��� ����Ʈ�� �߰�
        GameObject orderSheet = PhotonNetwork.Instantiate(orderSheetPrefab.name, transform.position, Quaternion.identity);
        int random = UnityEngine.Random.Range(0, recipes.Length);
        orderSheet.GetComponent<OrderSheet>().recipe = recipes[random];
        orderSheetList.Add(orderSheet);
        //�ֹ��� �ǳڿ� ��ġ
        orderSheet.transform.SetParent(orderSheetPanel.transform);
        //������ġ�� ȭ�� ��
        orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(1920, 0, 0);
        orderSheet.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine(IeMoveOrderSheet(orderSheet.GetComponent<PhotonView>().ViewID));
    }

    //�ֹ��� �̵�
    GameObject orderSheet;
    [PunRPC]
    IEnumerator IeMoveOrderSheet(int id)
    {
        for (int i = 0; i < orderSheetList.Count; i++)
        {
            if (orderSheetList[i].GetComponent<PhotonView>().ViewID == id)
            {
                orderSheet = orderSheetList[i];
            }
        }
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
        print("�ֹ��� ���� ��");
    }

    public IEnumerator IeDeleteOrderSheet(GameObject orderSheet)
    {
        print("����");
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
            print("���̱� ��");
        }
    }

    //�ֹ����� ���� ��
    public void CheckOrderSheet(Plate plate)
    {
        print("orderSheetList.Count: " + orderSheetList.Count);
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
                    StartCoroutine(WrongPlate());
                    Destroy(plate.transform.gameObject);
                }
                if (j == recipe.ingredients.Length - 1)
                {
                    orderSheetList[i].GetComponent<OrderSheet>().DestroyOrder();
                    print("����Ʈ�� �ִ� ����");
                    PlateManager.instance.AddDirtyPlate();
                    Destroy(plate.transform.gameObject);
                    StageManager.instance.CoinPlus(8);
                    break;
                }
            }
            if (i == orderSheetList.Count - 1)
            {
                //StartCoroutine(WrongPlate());
                
                //Destroy(plate.transform.gameObject);
            }
        }
        StartCoroutine(WrongPlate());
        Destroy(plate.transform.gameObject);
    }

    IEnumerator WrongPlate()
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
        PlateManager.instance.AddDirtyPlate();
    }
}
