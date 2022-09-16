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

    public static OrderSheetManager instance;
    int orderCount = 0;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating("CreateOrderSheet", 1f, 1f);
        //CreateOrderSheet();
        //Destroy(gameObject, 100);
    }

    void Update()
    {
        
    }
    //�ֹ��� ����
    void CreateOrderSheet()
    {
        if (orderCount >= 5)
            return;
        //�����ǵ� �� �������� �ֹ��� ����
        orderCount++;
        GameObject orderSheet = Instantiate(orderSheetPrefab);
        int random = UnityEngine.Random.Range(0, recipes.Length);
        orderSheet.GetComponent<OrderSheet>().recipe = recipes[random];
        orderSheetList.Add(orderSheet);
        //������ ������ ��ġ
        orderSheet.transform.SetParent(orderSheetPanel.transform);
        orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(1920, 0, 0);
        orderSheet.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //print(orderSheet.GetComponent<RectTransform>().position);
        StartCoroutine(IeMoveOrderSheet(orderSheet));
    }

    //�ֹ��� �̵�
    IEnumerator IeMoveOrderSheet(GameObject orderSheet)
    {
        float xTargetPos = 0;
        float xPos = orderSheet.GetComponent<RectTransform>().position.x;
        for (int i = 0; i < orderSheetList.Count - 1; i++)
        {
            xTargetPos += 10 + orderSheetList[i].GetComponent<RectTransform>().rect.width * 0.5f;
        }
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
