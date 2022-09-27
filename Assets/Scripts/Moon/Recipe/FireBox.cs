using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBox : MonoBehaviour
{
    public GameObject cookingTool;
    public Vector3 objectPosition;
    public bool isFire;
    public GameObject fireEffectPrefab;
    public GameObject fireEffect;
    public float fireGauge;
    public GameObject fireGaugeCanvas;
    public Image fireGaugeImage;
    public Image warningImage;

    void Start()
    {
        objectPosition = new Vector3(0, 1, 0);
        if (cookingTool)
        {
            GameObject tool = Instantiate(cookingTool);
            tool.transform.parent = transform;
            tool.transform.localPosition = objectPosition;
            cookingTool = tool;
        }
        fireGaugeImage.GetComponent<Image>().fillAmount = 0;
        fireGaugeCanvas.SetActive(false);
    }

    void Update()
    {
        if (isFire && fireGauge <= 0)
        {
            fireGauge = 0;
            fireGaugeCanvas.SetActive(false);
            Destroy(fireEffect);
            isFire = false;
        }
        if (isFire && fireGauge > 0)
        {
            fireGaugeImage.GetComponent<Image>().fillAmount = fireGauge / 100;
            return;
        }
    }

    public void Fire()
    {
        isFire = true;
        fireEffect = Instantiate(fireEffectPrefab);
        Vector3 firePos = transform.position;
        firePos.y += 1;
        fireEffect.transform.position = firePos;
        fireGauge = 100;
    }

    public void FireSuppression(float i)
    {
        fireGauge -= i;
    }

    public void SetObject(GameObject obj)
    {
        if (!cookingTool && obj.GetComponent<FryingPan>())
        {
            cookingTool = obj;
            cookingTool.transform.parent = transform;
            cookingTool.transform.localPosition = objectPosition;
        }
    }
}
