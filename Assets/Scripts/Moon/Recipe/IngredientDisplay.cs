using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDisplay : MonoBehaviour
{
    public IngredientObject ingredientObject;
    public GameObject curObject;
    public bool isRaw;
    public bool isCut;
    public bool isBake;
    public bool isReady;
    int cookLevel = 0;
    int maxCookLevel;
    void Start()
    {
        curObject = Instantiate(ingredientObject.cookLevel[cookLevel]);
        curObject.transform.position = transform.position;
        curObject.transform.parent = transform;
        //curObject.transform.localPosition = new Vector3(0, -.5f, 0);
        //자르거나 구워야 하는 오브젝트가 아니면 바로 접시에 담을 수 있음
        if (ingredientObject.isPossibleBake)
            maxCookLevel = 2;
        if (ingredientObject.isPossibleCut)
            maxCookLevel = 1;
        if(!ingredientObject.isPossibleBake && !ingredientObject.isPossibleCut)
        {
            isReady = true;
        }
    }

    public void CookLevelUp()
    {
        if (cookLevel < maxCookLevel)
            cookLevel++;
        Destroy(curObject);
        curObject = Instantiate(ingredientObject.cookLevel[cookLevel]);
        curObject.transform.position = transform.position;
        curObject.transform.parent = transform;
    }

    void Update()
    {
        
    }
}
