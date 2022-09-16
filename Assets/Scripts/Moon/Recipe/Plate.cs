using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    public List<GameObject> ingredientModels = new List<GameObject>();
    public List<IngredientObject> ingredientList = new List<IngredientObject>();
    //public IngredientObject[] ingredients = new IngredientObject[10];
    public Text ingredientName;
    int count = 0; //���� ��� ��� ��
    public RecipeObject[] recipes; //�̰� �ν����Ϳ��� �ֱ�
    //GameObject[] curIngredients;

    void Start()
    {
        ingredientName.text = "";
    }

    void Update()
    {
        
    }

    //�Ű������� ���� ��Ḧ ����Ʈ�� �ְ� �ؽ�Ʈ �ݿ�
    public void GetIngredient(GameObject ingredient)
    {
        print(ingredient.name);
        if (count > 3)
            return;
        if (CheckIngredientReady(ingredient.GetComponent<IngredientDisplay>()))
        {
            print(ingredient.GetComponent<IngredientDisplay>().ingredientObject.name);
            ingredientList.Add(ingredient.GetComponent<IngredientDisplay>().ingredientObject);
            ingredientName.text = ingredientName.text + ingredientList[count].name + "\n";
            Destroy(ingredient);
            count++;
        }
    }

    public bool CheckIngredientReady(IngredientDisplay ingredient)
    {
        //�߶�� �ϴµ� ���ڸ��Ÿ� return;
        if (ingredient.ingredientObject.isPossibleCut && !ingredient.isCut)
        {
            print("false: " + ingredient.ingredientObject.isPossibleCut + ", " + !ingredient.isCut);
            return false;
        }
        if (ingredient.ingredientObject.isPossibleBake && !ingredient.isBake)
        {
            print("false: " + ingredient.ingredientObject.isPossibleBake + ", " + !ingredient.isBake);
            return false;
        }
        print("Chack: ready");
        return true;
    }
}
