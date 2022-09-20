using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class IngredientObject : ScriptableObject
{
    public string ingredientName; //��� �̸�
    public GameObject[] model; //�ܰ躰 �𵨸�
    public Sprite recipeIcon; //�����ǿ� ������
    public bool isPossibleCut;
    public bool isPossibleBake;
   
}
