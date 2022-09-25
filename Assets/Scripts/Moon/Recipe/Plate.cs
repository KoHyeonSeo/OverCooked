using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Plate : MonoBehaviourPun
{
    public List<GameObject> ingredientModels = new List<GameObject>();
    public List<IngredientObject> ingredientList = new List<IngredientObject>();
    public Text ingredientName;
    int count = 0; //���� ��� ��� ��
    public RecipeObject[] recipes; //�̰� �ν����Ϳ��� �ֱ�
    public bool isdirty;

    //�̹��� ǥ��
    public GameObject canvas;
    public GameObject[] ingredientImages;

    void Start()
    {
        ingredientName.text = "";
    }

    private void Update()
    {
        canvas.transform.LookAt(new Vector3(transform.position.x, 10, 0));
    }

    public void AddPlate(GameObject plate)
    {
        plate.transform.parent = transform;
        print(transform);
        plate.transform.localPosition = new Vector3(0, 2, 0);
        plate.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void PlateKinematicOff()
    {
        if (transform.childCount > 1)
            transform.GetChild(1).GetComponent<Rigidbody>().isKinematic = false;
    }

    //�Ű������� ���� ��Ḧ ����Ʈ�� �ְ� �ؽ�Ʈ �ݿ�
    public void GetIngredient(GameObject ingredient)
    {
        if (isdirty)
            return;
        print(ingredient.name);
        if (count > 3)
            return;
        if (CheckIngredientReady(ingredient.GetComponent<IngredientDisplay>()))
        {
            print(ingredient.GetComponent<IngredientDisplay>().ingredientObject.name);
            ingredientList.Add(ingredient.GetComponent<IngredientDisplay>().ingredientObject);
            ingredientImages[count].SetActive(true);
            ingredientImages[count].GetComponent<Image>().sprite = ingredientList[count].recipeIcon;
            //ingredientName.text = ingredientName.text + ingredientList[count].name + "\n";
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
