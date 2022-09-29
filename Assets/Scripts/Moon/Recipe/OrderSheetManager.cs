using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//주문서 관리
public class OrderSheetManager : MonoBehaviourPun
{
    public RecipeObject[] recipes;
    public GameObject orderSheetPrefab;
    public List<GameObject> orderSheetList = new List<GameObject>(); //주문서 리스트
    public GameObject orderSheetPanel;
    int orderCount = 0;
    public UI_ReadyStart readyStart;

    public static OrderSheetManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating("CreateOrderSheet", 2f, 5f);

        }
    }

    void Update()
    {
        /*if (readyStart.IsReady && !isOnce)
        {
            isOnce = true;
            //15초마다 주문서 생성
            InvokeRepeating("CreateOrderSheet", 0f, 15f);
        }*/
    }

    //주문서 생성
    public void CreateOrderSheet()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        orderCount = orderSheetList.Count;
        //주문 5개 까지만 받음
        if (orderCount >= 5)
            return;
        //주문서 개수 증가
        orderCount++;
        //레시피들 중 랜덤으로 주문서 생성, 주문서 리스트에 추가
        int random = UnityEngine.Random.Range(0, recipes.Length);
        photonView.RPC("RpcMoveOrderSheet", RpcTarget.All, random);
        //StartCoroutine(IeMoveOrderSheet(orderSheet.GetComponent<PhotonView>().ViewID));
    }

    [PunRPC]
    void RpcMoveOrderSheet(int random)
    {
        StartCoroutine(IeMoveOrderSheet(random));
    }

    //주문서 이동
    IEnumerator IeMoveOrderSheet(int random)
    {
        GameObject orderSheet = Instantiate(orderSheetPrefab);
        orderSheet.GetComponent<OrderSheet>().recipe = recipes[random];
        orderSheetList.Add(orderSheet);
        //주문서 판넬에 배치
        orderSheet.transform.SetParent(orderSheetPanel.transform);
        //시작위치는 화면 밖
        orderSheet.GetComponent<RectTransform>().localPosition = new Vector3(1920, 0, 0);
        orderSheet.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        /*for (int i = 0; i < orderSheetList.Count; i++)
        {
            if (orderSheetList[i].GetComponent<PhotonView>().ViewID == id)
            {
                orderSheet = orderSheetList[i];
            }
        }*/
        float xTargetPos = 0; //여기까지 이동해야 함
        float xPos = orderSheet.GetComponent<RectTransform>().position.x; //현재 주문서의 위치
        for (int i = 0; i < orderSheetList.Count - 1; i++)
        {
            //현재 리스트에 담긴 주문서 넓이만큼 간격 두고 이동하기 위해 계산
            xTargetPos += 10 + orderSheetList[i].GetComponent<RectTransform>().rect.width * 0.5f;
        }
        //주문서 이동
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
        print("주문서 생성 끝");
    }

    public IEnumerator IeDeleteOrderSheet(GameObject orderSheet)
    {
        print("삭제");
        int orderSheetNum = orderSheetList.IndexOf(orderSheet);
        orderSheetList.Remove(orderSheet);
        for (int i = orderSheetNum; i < orderSheetList.Count; i++)
        {
            float xTargetPos = 0; //여기까지 이동해야 함
            float xPos = orderSheetList[i].GetComponent<RectTransform>().position.x; //현재 주문서의 위치
            for (int j = 0; j < i; j++)
            {
                //현재 리스트에 담긴 주문서 넓이만큼 간격 두고 이동하기 위해 계산
                xTargetPos += 10 + orderSheetList[j].GetComponent<RectTransform>().rect.width * 0.5f;
            }
            orderSheetList[i].GetComponent<RectTransform>().localPosition = new Vector3(xTargetPos, 0, 0);
            yield return null;
            print("줄이기 끝");
        }
    }

    public void CheckOrderSheet(int id)
    {
        photonView.RPC("RpcCheckOrderSheet", RpcTarget.All, id);
    }

    //주문서랑 접시 비교
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
            //접시의 재료 개수와 주문서 레시피의 재료 개수가 다르면 다음 주문서로
            if (plate.ingredientList.Count != recipe.ingredients.Length)
            {
                continue;
            }
            for (int j = 0; j < recipe.ingredients.Length; j++)
            {
                //접시의 재료와 주문서 레시피의 재료가 같은지 비교
                if (!plate.ingredientList.Contains(recipe.ingredients[j]))
                {
                    StartCoroutine(WrongPlate(plate));
                }
                if (j == recipe.ingredients.Length - 1)
                {
                    orderSheetList[i].GetComponent<OrderSheet>().DestroyOrder();
                    print("리스트에 있는 음식");
                    PlateManager.instance.AddDirtyPlate();
                    StageManager.instance.CoinPlus(8);
                    //Destroy(plate.transform.gameObject);
                }
            }
        }
        StartCoroutine(WrongPlate(plate));
        //Destroy(plate.transform.gameObject);
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
        PlateManager.instance.AddDirtyPlate();
        photonView.RPC("RpcDestroyPlate", RpcTarget.All, plate.GetComponent<PhotonView>().ViewID);
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
