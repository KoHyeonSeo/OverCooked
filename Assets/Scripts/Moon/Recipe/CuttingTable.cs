using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingTable : MonoBehaviour
{
    public GameObject cutTableObject; //테이블 위 오브젝트
    public Vector3 objectPosition; //오브젝트 배치
    float cutTime = 2; //자르는 시간
    float time = 0; //현재 시간
    public bool isPlayerExit; //플레이어가 존재 하는지
    public GameObject cutGauge; //얼마나 잘렸는지
    public Image cutGaugeImage; //얼마나 잘렸는지 이미지로 표현

    void Start()
    {
        objectPosition = new Vector3(0, 1, 0);
        cutGaugeImage.GetComponent<Image>().fillAmount = time / cutTime;
        cutGauge.SetActive(false);
    }

    void Update()
    { 
        //도마위에 올라온 오브젝트가 음식이면서 자르지 않은 상태라면 시간이 흐름
        if (cutTableObject && cutTableObject.GetComponent<IngredientDisplay>())
        {
            if (cutTableObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut && !cutTableObject.GetComponent<IngredientDisplay>().isCut)
            {
                if (isPlayerExit)
                {
                    cutGauge.SetActive(true);
                    time += Time.deltaTime;
                    cutGaugeImage.GetComponent<Image>().fillAmount = time / cutTime;
                }  
                if (cutTime < time)
                {
                    //상태를 자른걸로 변환
                    ChangeStateCut();
                }
            }
        }
    }

    void ChangeStateCut()
    {
        print("잘림: " + cutTableObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        cutTableObject.GetComponent<IngredientDisplay>().isCut = true;
        cutTableObject.GetComponent<IngredientDisplay>().CookLevelUp(); 
        time = 0;
        cutGauge.SetActive(false);
    }

    public void SetObject(GameObject obj)
    {
        //박스 위에 오브젝트가 없으면 받은 오브젝트 셋팅
        if (!cutTableObject)
        {
            cutTableObject = obj;
            cutTableObject.transform.parent = transform;
            
            objectPosition.y = cutTableObject.transform.localScale.y / 2;
            cutTableObject.transform.localPosition = objectPosition;
        }
    }
}
