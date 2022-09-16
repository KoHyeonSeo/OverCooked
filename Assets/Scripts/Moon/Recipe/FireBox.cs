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
        //오브젝트가 프라이팬이고 프라이팬에 음식이 있으면 시간이 흐름
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
                        //상태를 자른걸로 변환
                        ChangeStateBake();
                    }
                }
            }
        }
    }

    void ChangeStateBake()
    {
        print("구움: " + getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().isBake = true;
        time = 0;
    }

    void Fire()
    {
        print("불 남");
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
