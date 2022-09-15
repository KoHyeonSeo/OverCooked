using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutBox : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;
    float cutTime = 2;
    float time = 0;
    void Start()
    {
        
    }

    void Update()
    {
        
        //�������� �ö�� ������Ʈ�� �����̸鼭 �ڸ��� ���� ���¶�� �ð��� �帧
        if (getObject && getObject.GetComponent<IngredientDisplay>()/*&&Player�� �տ� ������*/)
        {
            if (getObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleCut && !getObject.GetComponent<IngredientDisplay>().isCut)
            {
                time += Time.deltaTime;
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
        print("�߸�: " + getObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        getObject.GetComponent<IngredientDisplay>().isCut = true;
        time = 0;
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
