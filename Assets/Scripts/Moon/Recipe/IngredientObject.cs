using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class IngredientObject : ScriptableObject
{
    public string name; //��� �̸�
    public GameObject[] cookLevel; //�ܰ躰 �𵨸�
    public bool isPossibleCut;
    public bool isPossibleBake;
    public bool isCut;
    public bool isBake;
    public bool isReady;
}
