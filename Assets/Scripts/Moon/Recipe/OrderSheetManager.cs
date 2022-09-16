using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ֹ��� ����
public class OrderSheetManager : MonoBehaviour
{
    public RecipeObject[] recipes;
    public GameObject orderSheetPrefab;
    public List<GameObject> orderSheetList = new List<GameObject>(); //�ֹ��� ����Ʈ
    public GameObject orderSheetPanel;
    int orderCount = 0;

    public static OrderSheetManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //15�ʸ��� �ֹ��� ����
        InvokeRepeating("CreateOrderSheet", 0f, 15f);
    }

    void Update()
    {
        
    }

    //�ֹ��� ����
    void CreateOrderSheet()
    {
        //�ֹ� 5�� ������ ����
        if (orderCount >= 5)
            return;
        //�ֹ��� ���� ����
        orderCount++;
        //�����ǵ� �� �������� �ֹ��� ����, �ֹ��� ����Ʈ�� �߰�
        GameObject orderSheet = Instantiate(orderSheetPrefab);
        int random = UnityEngine.Random.Range(0, recipes.Length);
        orderSheet.GetComponent<OrderSheet>().recipe = recipes[random];
        orderSheetList.Add(orderSheet);
        //�ֹ��� �ǳڿ� ��ġ
        orderSheet.transform.SetParent(orderSheetPanel.transform);
        //������ġ�� ȭ�� ��
        orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(1920, 0, 0);
        orderSheet.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine(IeMoveOrderSheet(orderSheet));
    }

    //�ֹ��� �̵�
    IEnumerator IeMoveOrderSheet(GameObject orderSheet)
    {
        float xTargetPos = 0; //������� �̵��ؾ� ��
        float xPos = orderSheet.GetComponent<RectTransform>().position.x; //���� �ֹ����� ��ġ
        for (int i = 0; i < orderSheetList.Count - 1; i++)
        {
            //���� ����Ʈ�� ��� �ֹ��� ���̸�ŭ ���� �ΰ� �̵��ϱ� ���� ���
            xTargetPos += 10 + orderSheetList[i].GetComponent<RectTransform>().rect.width * 0.5f;
        }
        //�ֹ��� �̵�
        while (xTargetPos <= xPos)
        {
            xPos = Mathf.Lerp(xPos, xTargetPos, 0.1f);
            orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(xPos, 0, 0);
            yield return new WaitForSeconds(0.01f);
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
                //�󸶳� ���� ������ ����Ʈ
                print(plate.ingredientList.Count + ", " + recipe.ingredients.Length);
                continue;
            }
            print(plate.ingredientList.Count);
            for (int j = 0; j < recipe.ingredients.Length; j++)
            {
                //������ ���� �ֹ��� �������� ��ᰡ ������ ��
                if (plate.ingredientList.Contains(recipe.ingredients[j]))
                {
                    print(plate.ingredientList[j].name);
                }
                else
                {
                    print("�߸��� ����");
                    Destroy(plate.transform.gameObject);
                }
            }
            print("����Ʈ�� �ִ� ����");

        }
    }
}
