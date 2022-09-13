using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class RecipeObject : ScriptableObject
{
    public string name; //요리 이름
    public GameObject[] cookLevel; //단계별 모델링
    public IngredientObject[] ingredient; //재료
}
