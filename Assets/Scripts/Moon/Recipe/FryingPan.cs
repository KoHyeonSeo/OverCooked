using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryingPan : MonoBehaviour
{
    public GameObject getObject;
    Vector3 objectPosition = new Vector3(0, -0.2f, 0);

    //����
    float bakeTime = 10;
    public float time = 0;
    public GameObject bakeGauge;
    public Image bakeGaugeImage;

    //��
    float fireTime = 18;
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
        if (!bakeGauge.activeSelf && !getObject.GetComponent<IngredientDisplay>().isBake)
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
        getObject.GetComponent<IngredientDisplay>().isBake = true;
        getObject.GetComponent<IngredientDisplay>().CookLevelUp();
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
        bakeGauge.SetActive(false);
        for (float i = 1; i < 10; i++) 
        {
            for (int j = 0; j < 2; j++)
            {
                burnWarning.SetActive(false);
                yield return new WaitForSeconds(Mathf.Lerp(0.4f, 0f, i / 10f));
                burnWarning.SetActive(true);
                yield return new WaitForSeconds(Mathf.Lerp(0.4f, 0f, i / 10f));
            }
        }
    }
}
