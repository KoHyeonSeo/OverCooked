using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주문서 관리
public class OrderSheetManager : MonoBehaviour
{
    public RecipeObject[] recipes;
    public GameObject orderSheetPrefab;
    public List<GameObject> orderSheetList = new List<GameObject>(); //주문서 리스트
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
    //주문서 생성
    void CreateOrderSheet()
    {
        if (orderCount >= 5)
            return;
        //레시피들 중 랜덤으로 주문서 생성
        orderCount++;
        GameObject orderSheet = Instantiate(orderSheetPrefab);
        int random = UnityEngine.Random.Range(0, recipes.Length);
        orderSheet.GetComponent<OrderSheet>().recipe = recipes[random];
        orderSheetList.Add(orderSheet);
        //콘텐츠 하위에 배치
        orderSheet.transform.SetParent(orderSheetPanel.transform);
        orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(1920, 0, 0);
        orderSheet.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //print(orderSheet.GetComponent<RectTransform>().position);
        StartCoroutine(IeMoveOrderSheet(orderSheet));
    }

    //주문서 이동
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
