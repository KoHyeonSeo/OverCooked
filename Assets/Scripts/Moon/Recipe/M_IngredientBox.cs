using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IngredientBox : MonoBehaviour
{
    public GameObject ingredientPrefab;
<<<<<<< HEAD
    public GameObject ingredient;
=======
    GameObject ingredient;
>>>>>>> parent of 5e48f9e (Merge pull request #13 from KoHyeonSeo/revert-12-MOON)

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        
    }

    //��� ����
    public void CreateIngredient()
    {
        ingredient = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
=======
        //�ڽ� Ŭ���ϸ� ��� ����
        if (Input.GetMouseButtonDown(0))
        {
            ingredient = Instantiate(ingredientPrefab);
        }
>>>>>>> parent of 5e48f9e (Merge pull request #13 from KoHyeonSeo/revert-12-MOON)
    }
}
