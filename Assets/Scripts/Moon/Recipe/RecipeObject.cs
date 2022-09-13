using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class RecipeObject : ScriptableObject
{
    public string name; //�丮 �̸�
    public GameObject[] cookLevel; //�ܰ躰 �𵨸�
    public IngredientObject[] ingredient; //���
}
