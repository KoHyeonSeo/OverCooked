using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryingPan : MonoBehaviour
{
    public GameObject getObject;
    Vector3 objectPosition = new Vector3(0, -0.2f, 0);

    //����
    float bakeTime = 1;
    public float time = 0;
    public GameObject bakeGauge;
    public Image bakeGaugeImage;

    //��
    float fireTime = 15;
    public GameObject burnWarning;

    void Start()
    {
        bakeGauge.SetActive(false);
    }

    void Update()
    {
        //ȭ������ �ҳ��� �ƹ��͵� ���� X
        if (transform.parent && transform.parent.GetComponent<FireBox>() && 
            transform.parent.GetComponent<FireBox>().isFire)
            return;  
        //ȭ�� ���� �ְ� ������ �ִٸ�
        if (transform.parent && transform.parent.GetComponent<FireBox>() && getObject)
        {
            Bake();
        }
    }

    void Bake()
    {
        //ź �����̸� �� ���� ��
        if (getObject.GetComponent<IngredientDisplay>().isBurn)
            return;
        time += Time.deltaTime;
        if (!bakeGauge.activeSelf)
            bakeGauge.SetActive(true);
        bakeGaugeImage.GetComponent<Image>().fillAmount = time / bakeTime;
        if (time > fireTime)
        {
            transform.parent.GetComponent<FireBox>().Fire();
            getObject.GetComponent<IngredientDisplay>().isBurn = true;
            bakeGauge.SetActive(false);
            burnWarning.SetActive(false);
        }
        else if (time > bakeTime && !getObject.GetComponent<IngredientDisplay>().isBake)
        {
            getObject.GetComponent<IngredientDisplay>().isBake = true;
            StartCoroutine(BurnWarning());
            ChangeStateBake();
        }
        
    }

    void ChangeStateBake()
    {
        getObject.GetComponent<IngredientDisplay>().CookLevelUp();
        getObject.GetComponent<IngredientDisplay>().isBake = true;
        bakeGauge.SetActive(false);
    }

    public void SetObject(GameObject obj)
    {
        //�ڽ� ���� ������Ʈ�� ������ ���� ������Ʈ ����
        if (!getObject && obj.GetComponent<IngredientDisplay>() && 
            obj.GetComponent<IngredientDisplay>().isCut && 
            obj.GetComponent<IngredientDisplay>().ingredientObject.isPossibleBake)
        {
            time = 0;
            getObject = obj;
            getObject.transform.parent = transform;
            getObject.transform.localPosition = objectPosition;
        }
    }

    IEnumerator BurnWarning()
    {
        for (int i = 1; i < 20; i++) 
        {
            burnWarning.SetActive(false);
            yield return new WaitForSeconds(0.5f - (i * i / 500));
            burnWarning.SetActive(true);
            yield return new WaitForSeconds(0.5f - (i * i / 500));
        }
    }
}
