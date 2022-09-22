using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class IngredientObject : ScriptableObject
{
    public string ingredientName; //재료 이름
    public GameObject[] model; //단계별 모델링
    public Sprite recipeIcon; //레시피용 아이콘
    public bool isPossibleCut;
    public bool isPossibleBake;
   
}
