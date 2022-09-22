using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBox : MonoBehaviour
{
    //public GameObject getObject;
    public Vector3 objectPosition;
    float fireTime = 15;
    float bakeTime = 10;
    float time = 0;
    public GameObject fireEffectPrefab;
    public GameObject fireEffect;
    public float fireGauge;
    public bool isFire;
    public GameObject bakeGauge;
    public Image bakeGaugeImage;
    public GameObject cookingTool;
    FryingPan fryingPan;

    void Start()
    {
        objectPosition = new Vector3(0, 1, 0);
        bakeGaugeImage.GetComponent<Image>().fillAmount = time / bakeTime;
        bakeGauge.SetActive(false);
    }

    void Update()
    {
        if (isFire && fireGauge <= 0)
        {
            fireGauge = 0;
            print("����");
            Destroy(fireEffect);
            isFire = false;
            time = 0;
        }
        if (isFire && fireGauge > 0)
        {  
            return;
        }
        if (cookingTool && cookingTool.GetComponent<FryingPan>() && cookingTool.GetComponent<FryingPan>().getObject
            && cookingTool.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().isBake)
        {
            time += Time.deltaTime;
            if (fireTime < time)
            {
                Fire();
            }
        }
        //������Ʈ�� ���������̰� �������ҿ� ������ ������ �ð��� �帧
        else if (cookingTool && cookingTool.GetComponent<FryingPan>().getObject/*&& getObject.GetComponent<FryingPan>() && getObject.GetComponent<FryingPan>().getObject*/)
        {
            GameObject fryObject = cookingTool.GetComponent<FryingPan>().getObject;
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
                        //���¸� �ڸ��ɷ� ��ȯ
                        ChangeStateBake();
                    }
                }
            }
        }
    }

    void ChangeStateBake()
    {
        /*print("����: " + getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().ingredientObject.name);
        getObject.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().isBake = true;*/
        print("����: " + cookingTool.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().name);
        cookingTool.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().CookLevelUp();
        cookingTool.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().isBake = true;
        bakeGauge.SetActive(false);
        //time = 0;
    }

    void Fire()
    {
        cookingTool.GetComponent<FryingPan>().getObject.GetComponent<IngredientDisplay>().isBake = false;
        fireEffect = Instantiate(fireEffectPrefab);
        Vector3 firePos = transform.position;
        firePos.y += 1;
        fireEffect.transform.position = firePos;
        isFire = true;
        fireGauge = 100;
        Destroy(cookingTool.GetComponent<FryingPan>().getObject);
        cookingTool.GetComponent<FryingPan>().getObject = null;
        print("�� ��");
    }

    public void FireSuppression(float i)
    {
        fireGauge -= i;
    }

    public void SetObject(GameObject obj)
    {
        if (!cookingTool && obj.GetComponent<FryingPan>())
        {
            fryingPan = obj.GetComponent<FryingPan>();
            cookingTool = obj;
            cookingTool.transform.parent = transform;
            objectPosition.y = 0.52f;
            cookingTool.transform.localPosition = objectPosition;
        }
        //�ڽ� ���� ������Ʈ�� ������ ���� ������Ʈ ����
        /*if (!getObject && obj.tag == "Food" && cookingTool && !obj.GetComponent<IngredientDisplay>().isBake)
        {
            time = 0;
            bakeGauge.SetActive(true);
            getObject = obj;
            getObject.transform.parent = transform;
            objectPosition.y = getObject.transform.localScale.y / 2;
            getObject.transform.localPosition = objectPosition;
        }*/

    }
}
