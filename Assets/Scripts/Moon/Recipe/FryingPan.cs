using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    public GameObject getObject;
    public Vector3 objectPosition = new Vector3(0, 1, 0);

    public void SetObject(GameObject obj)
    {
        //박스 위에 오브젝트가 없으면 받은 오브젝트 셋팅
        if (!getObject && obj.GetComponent<IngredientDisplay>() && obj.GetComponent<IngredientDisplay>().isCut && obj.GetComponent<IngredientDisplay>().ingredientObject.isPossibleBake)
        {
            getObject = obj;
            getObject.transform.parent = transform;
            getObject.transform.localPosition = objectPosition;
        }

    }
}
