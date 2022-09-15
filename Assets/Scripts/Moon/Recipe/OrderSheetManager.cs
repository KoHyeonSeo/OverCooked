using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주문서 관리
public class OrderSheetManager : MonoBehaviour
{
    public List<GameObject> orderSheetList = new List<GameObject>(); //주문서 리스트

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
    //주문서 생성
    void CreateOrderSheet()
    {
        if (orderCount >= 4)
            return;
        
    }

    //주문서랑 접시 비교
    public void CheckOrderSheet(Plate plate)
    {
        print("orderSheetList.Count: " + orderSheetList.Count);
        for (int i = 0; i < orderSheetList.Count; i++)
        {
            RecipeObject recipe = orderSheetList[i].GetComponent<OrderSheet>().recipe;
            //접시의 재료 개수와 주문서 레시피의 재료 개수가 다르면 다음 주문서로
            if (plate.ingredientList.Count != recipe.ingredients.Length)
            {
                print(plate.ingredientList.Count + ", " + recipe.ingredients.Length);
                continue;
            }
            print(plate.ingredientList.Count);
            for (int j = 0; j < recipe.ingredients.Length; j++)
            {
                //접시의 재료와 주문서 레시피의 재료가 같은지 비교
                if (plate.ingredientList.Contains(recipe.ingredients[j]))
                {
                    print(plate.ingredientList[j].name);
                }
                else
                {
                    print("잘못된 음식");
                    Destroy(plate.transform.gameObject);
                }
            }
            print("리스트에 있는 음식");
        }
    }
}
