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
    GameObject player;
    void Start()
    {
        if (GameManager.instance.Player)
        {
            player = GameManager.instance.Player;  
        }
        curObject = Instantiate(ingredientObject.cookLevel[cookLevel]);
        curObject.transform.position = transform.position;
        curObject.transform.parent = transform;
        //curObject.transform.localPosition = new Vector3(0, -.5f, 0);
        //�ڸ��ų� ������ �ϴ� ������Ʈ�� �ƴϸ� �ٷ� ���ÿ� ���� �� ����
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
        curObject = player.GetComponent<PlayerCreateNew>().CreatesNewObject(ingredientObject.cookLevel[cookLevel], "Default", true, transform, new Vector3(0, 0, 0), true);
        //curObject = Instantiate(ingredientObject.cookLevel[cookLevel]);
        //curObject.transform.position = transform.position;
        //curObject.transform.parent = transform;
    }

    void Update()
    {
        if (!player)
            player = GameManager.instance.Player;
    }
}
