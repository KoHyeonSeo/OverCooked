using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBox : MonoBehaviour
{
    public GameObject ingredientPrefab;

    public GameObject CreateIngredient()
    {
        GameObject ingredient = Instantiate(ingredientPrefab);
        return ingredient;
    }
}
