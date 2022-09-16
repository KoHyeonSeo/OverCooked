using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class RecipeObject : ScriptableObject
{
    public string recipeName; //�丮 �̸�
    public GameObject[] cookLevel; //�ܰ躰 �𵨸�
    public Sprite recipeSprite; //�ϼ� ���� �𵨸�
    public IngredientObject[] ingredients; //���
    public Sprite[] ingredientSprites; //��� �̹���
}
