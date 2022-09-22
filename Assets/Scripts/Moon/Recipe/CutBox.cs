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
        
        //�������� �ö�� ������Ʈ�� �����̸鼭 �ڸ��� ���� ���¶�� �ð��� �帧
        if (getObject && getObject.GetComponent<IngredientDisplay>()/*&&Player�� �տ� ������*/)
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
                    //���¸� �ڸ��ɷ� ��ȯ
                    ChangeStateCut();
                }
            }
        }

    }

    void ChangeStateCut()
    {
        player.GetComponent<PlayerState>().curState = PlayerState.State.Idle;

        print("�߸�: " + getObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        getObject.GetComponent<IngredientDisplay>().isCut = true;
        getObject.GetComponent<IngredientDisplay>().CookLevelUp(); 
        time = 0;
        cutGauge.SetActive(false);
    }

    public void SetObject(GameObject obj)
    {
        //�ڽ� ���� ������Ʈ�� ������ ���� ������Ʈ ����
        if (!getObject)
        {
            getObject = obj;
            getObject.transform.position = objectPosition.position;
            getObject.transform.parent = transform;
        }
    }
}
