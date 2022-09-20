using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientTable : MonoBehaviour
{
    public GameObject ingredientPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameObject CreateIngredient()
    {
        GameObject ingredient = Instantiate(ingredientPrefab);
        return ingredient;
    }
}
