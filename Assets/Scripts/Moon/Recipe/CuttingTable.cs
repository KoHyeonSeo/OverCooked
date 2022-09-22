using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingTable : MonoBehaviour
{
    public GameObject cutTableObject; //���̺� �� ������Ʈ
    public Vector3 objectPosition; //������Ʈ ��ġ
    float cutTime = 2; //�ڸ��� �ð�
    float time = 0; //���� �ð�
    public bool isPlayerExit; //�÷��̾ ���� �ϴ���
    public GameObject cutGauge; //�󸶳� �߷ȴ���
    public Image cutGaugeImage; //�󸶳� �߷ȴ��� �̹����� ǥ��

    void Start()
    {
        objectPosition = new Vector3(0, 1, 0);
        cutGaugeImage.GetComponent<Image>().fillAmount = time / cutTime;
        cutGauge.SetActive(false);
    }

    void Update()
    { 
        //�������� �ö�� ������Ʈ�� �����̸鼭 �ڸ��� ���� ���¶�� �ð��� �帧
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
                    //���¸� �ڸ��ɷ� ��ȯ
                    ChangeStateCut();
                }
            }
        }
    }

    void ChangeStateCut()
    {
        print("�߸�: " + cutTableObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        cutTableObject.GetComponent<IngredientDisplay>().isCut = true;
        cutTableObject.GetComponent<IngredientDisplay>().CookLevelUp(); 
        time = 0;
        cutGauge.SetActive(false);
    }

    public void SetObject(GameObject obj)
    {
        //�ڽ� ���� ������Ʈ�� ������ ���� ������Ʈ ����
        if (!cutTableObject)
        {
            cutTableObject = obj;
            cutTableObject.transform.parent = transform;
            
            objectPosition.y = cutTableObject.transform.localScale.y / 2;
            cutTableObject.transform.localPosition = objectPosition;
        }
    }
}
