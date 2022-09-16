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
    int count = 0; //현재 담긴 재료 수
    public RecipeObject[] recipes; //이건 인스펙터에서 넣기
    //GameObject[] curIngredients;

    void Start()
    {
        ingredientName.text = "";
    }

    void Update()
    {
        
    }

    //매개변수로 들어온 재료를 리스트에 넣고 텍스트 반영
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
        //잘라야 하는데 안자른거면 return;
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
