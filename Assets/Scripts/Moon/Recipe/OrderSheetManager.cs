using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ֹ��� ����
public class OrderSheetManager : MonoBehaviour
{
    public List<GameObject> orderSheetList = new List<GameObject>(); //�ֹ��� ����Ʈ

    public static OrderSheetManager instance;
    int orderCount = 0;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Destroy(gameObject, 10);
    }

    void Update()
    {
        
    }
    //�ֹ��� ����
    void CreateOrderSheet()
    {
        if (orderCount >= 4)
            return;
        
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
