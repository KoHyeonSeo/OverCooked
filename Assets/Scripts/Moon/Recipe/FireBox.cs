using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBox : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;
    float fireTime = 5;
    float bakeTime = 2;
    float time = 0;
    public GameObject fireEffectPrefab;
    public GameObject fireEffect;
    public float fireGauge;
    public bool isFire;
    public GameObject bakeGauge;
    public Image bakeGaugeImage;

    void Start()
    {
        bakeGaugeImage.GetComponent<Image>().fillAmount = time / bakeTime;
        bakeGauge.SetActive(false);
    }

    void Update()
    {
        if (isFire && fireGauge <= 0)
        {
            fireGauge = 0;
            print("꺼짐");
            Destroy(fireEffect);
            isFire = false;
            time = 0;
        }
        if (isFire && fireGauge > 0)
        {  
            return;
        }
        if (getObject && getObject.GetComponent<IngredientDisplay>().isBake)
        {
            time += Time.deltaTime;
            if (fireTime < time)
            {
                Fire();
            }
        }
        //오브젝트가 프라이팬이고 프라이팬에 음식이 있으면 시간이 흐름
        if (getObject /*&& getObject.GetComponent<FryingPan>() && getObject.GetComponent<FryingPan>().getObject*/)
        {
            //GameObject fryObject = getObject.GetComponent<FryingPan>().getObject;
            GameObject fryObject = getObject;
            if (fryObject && fryObject.GetComponent<IngredientDisplay>())
            {
                if (fryObject.GetComponent<IngredientDisplay>().ingredientObject.isPossibleBake && !fryObject.GetComponent<IngredientDisplay>().isBake)
                {
                    time += Time.deltaTime;
                    bakeGaugeImage.GetComponent<Image>().fillAmount = time / bakeTime;
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
        /*print("구움: " + getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().isBake = true;*/
        print("구움: " + getObject.GetComponent<IngredientDisplay>().name);
        getObject.GetComponent<IngredientDisplay>().CookLevelUp();
        getObject.GetComponent<IngredientDisplay>().isBake = true;
        bakeGauge.SetActive(false);
        //time = 0;
    }

    void Fire()
    {
        getObject.GetComponent<IngredientDisplay>().isBake = false;
        fireEffect = Instantiate(fireEffectPrefab);
        Vector3 firePos = transform.position;
        firePos.y += 1;
        fireEffect.transform.position = firePos;
        isFire = true;
        fireGauge = 100;
        Destroy(getObject);
        getObject = null;
        print("불 남");
    }

    public void FireSuppression(float i)
    {
        fireGauge -= i;
    }

    public void SetObject(GameObject obj)
    {
        //박스 위에 오브젝트가 없으면 받은 오브젝트 셋팅
        if (!getObject && !isFire && !obj.GetComponent<IngredientDisplay>().isBake)
        {
            time = 0;
            bakeGauge.SetActive(true);
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 1, 0);
            getObject = obj;
            //getObject.transform.position = objectPosition.position;
            //getObject.transform.parent = transform;
        }

    }
}
