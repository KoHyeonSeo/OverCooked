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
    void Start()
    {
        curObject = Instantiate(ingredientObject.cookLevel[0]);
        curObject.transform.position = transform.position;
        curObject.transform.parent = transform;
        //curObject.transform.localPosition = new Vector3(0, -.5f, 0);
        //�ڸ��ų� ������ �ϴ� ������Ʈ�� �ƴϸ� �ٷ� ���ÿ� ���� �� ����
        if(!ingredientObject.isPossibleBake && !ingredientObject.isPossibleCut)
        {
            isReady = true;
        }
    }

    void Update()
    {
        
    }
}
