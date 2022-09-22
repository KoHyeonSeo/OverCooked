using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutBox : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;
    float cutTime = 2;
    float time = 0;
    public bool isPlayerExit;
    public GameObject cutGauge;
    public Image cutGaugeImage;
    private GameObject player;

    void Start()
    {
        cutGaugeImage.GetComponent<Image>().fillAmount = time / cutTime;
        cutGauge.SetActive(false);
        if (GameManager.instance.Player)
            player = GameManager.instance.Player;
    }

    void Update()
    {
        if(!player)
            player = GameManager.instance.Player;

        if (GetComponent<Table>() && GetComponent<Table>().transform.childCount == 3)
        {
            getObject = GetComponent<Table>().transform.GetChild(2).gameObject;
        }
        
        //도마위에 올라온 오브젝트가 음식이면서 자르지 않은 상태라면 시간이 흐름
        if (getObject && getObject.GetComponent<IngredientDisplay>()/*&&Player가 앞에 있으면*/)
        {
            if (getObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut && !getObject.GetComponent<IngredientDisplay>().isCut)
            {
                player.GetComponent<PlayerState>().curState = PlayerState.State.Chop;
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
        player.GetComponent<PlayerState>().curState = PlayerState.State.Idle;

        print("잘림: " + getObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        getObject.GetComponent<IngredientDisplay>().isCut = true;
        getObject.GetComponent<IngredientDisplay>().CookLevelUp(); 
        time = 0;
        cutGauge.SetActive(false);
    }

    public void SetObject(GameObject obj)
    {
        //박스 위에 오브젝트가 없으면 받은 오브젝트 셋팅
        if (!getObject)
        {
            getObject = obj;
            getObject.transform.position = objectPosition.position;
            getObject.transform.parent = transform;
        }
    }
}
