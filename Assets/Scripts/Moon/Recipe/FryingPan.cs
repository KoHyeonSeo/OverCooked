using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    public GameObject getObject;
    public Vector3 objectPosition = new Vector3(0, 1, 0);

    public void SetObject(GameObject obj)
    {
        //�ڽ� ���� ������Ʈ�� ������ ���� ������Ʈ ����
        if (!getObject && obj.GetComponent<IngredientDisplay>() && obj.GetComponent<IngredientDisplay>().isCut && obj.GetComponent<IngredientDisplay>().ingredientObject.isPossibleBake)
        {
            getObject = obj;
            getObject.transform.parent = transform;
            getObject.transform.localPosition = objectPosition;
        }

    }
}
