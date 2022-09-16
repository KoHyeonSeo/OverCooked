using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBox : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;
    float fireTime = 5;
    float bakeTime = 2;
    float time = 0;
    void Start()
    {
        
    }

    void Update()
    {
        //������Ʈ�� ���������̰� �������ҿ� ������ ������ �ð��� �帧
        if (getObject && getObject.GetComponent<FryingPan>() && getObject.GetComponent<FryingPan>().getObject)
        {
            
            GameObject fryObject = getObject.GetComponent<FryingPan>().getObject;
            if (fryObject && fryObject.GetComponent<IngredientDisplay>())
            {
                if (fryObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleBake && !fryObject.GetComponent<IngredientDisplay>().isBake)
                {
                    time += Time.deltaTime;
                    if (fireTime < time)
                    {
                        Fire();
                    }
                    else if (bakeTime < time)
                    {
                        //���¸� �ڸ��ɷ� ��ȯ
                        ChangeStateBake();
                    }
                }
            }
        }
    }

    void ChangeStateBake()
    {
        print("����: " + getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().isBake = true;
        time = 0;
    }

    void Fire()
    {
        print("�� ��");
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
