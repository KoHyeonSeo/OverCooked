using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IngredientBox : MonoBehaviour
{
    public GameObject ingredientPrefab;
    GameObject ingredient;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�ڽ� Ŭ���ϸ� ��� ����
        if (Input.GetMouseButtonDown(0))
        {
            ingredient = Instantiate(ingredientPrefab);
        }
    }
}
