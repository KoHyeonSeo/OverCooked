using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Plate : MonoBehaviourPun
{
    public List<GameObject> ingredientModels = new List<GameObject>();
    public List<IngredientObject> ingredientList = new List<IngredientObject>();
    //public Text ingredientName;
    int count = 0; //���� ��� ��� ��
    public RecipeObject[] recipes; //�̰� �ν����Ϳ��� �ֱ�
    public bool isdirty;
    public GameObject topBread;
    bool isBreadExist;
    //�̹��� ǥ��
    public GameObject canvas;
    public GameObject[] ingredientImages;
    public GameObject cleanPlate;
    public GameObject dirtyPlate;


    void Start()
    {
        ObjectManager.instance.SetPhotonObject(gameObject);
    }

    private void Update()
    {
        canvas.transform.LookAt(new Vector3(transform.position.x, 10, 0));
        if (isdirty && cleanPlate.activeSelf)
        {
            cleanPlate.SetActive(false);
            dirtyPlate.SetActive(true);
        }
        else if (!isdirty && dirtyPlate.activeSelf)
        {
            cleanPlate.SetActive(true);
            dirtyPlate.SetActive(false);
        }
    }

    public void AddPlate(GameObject plate)
    {
        plate.transform.parent = transform;
        plate.transform.localPosition = new Vector3(0, 0.2f, 0);
        plate.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void PlateKinematicOff()
    {
        if (transform.childCount > 1)
            transform.GetChild(1).GetComponent<Rigidbody>().isKinematic = false;
    }

    public void GetIngredient(int id)
    {
        photonView.RPC("RpcGetIngredient", RpcTarget.All, id);
    }

    //�Ű������� ���� ��Ḧ ����Ʈ�� �ְ� �ؽ�Ʈ �ݿ�
    GameObject ingredient;
    [PunRPC]
    public void RpcGetIngredient(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (!ObjectManager.instance.photonObjectIdList[i])
                continue;
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                ingredient = ObjectManager.instance.photonObjectIdList[i];
            }
        }
        if (isdirty || ingredient.GetComponent<IngredientDisplay>().isBurn)
            return;
        if (count > 3)
            return;
        if (CheckIngredientReady(ingredient.GetComponent<IngredientDisplay>()))
        {
            ingredientImages[count].SetActive(true);
            if (ingredient.GetComponent<IngredientDisplay>().ingredientObject.name == "Bread")
            {
                isBreadExist = true;
                ingredientList.Insert(0, ingredient.GetComponent<IngredientDisplay>().ingredientObject);
                ingredientImages[count].GetComponent<Image>().sprite = ingredientList[0].recipeIcon;
            }
            else
            {
                ingredientList.Add(ingredient.GetComponent<IngredientDisplay>().ingredientObject);
                ingredientImages[count].GetComponent<Image>().sprite = ingredientList[count].recipeIcon;
            }
            Destroy(ingredient);
            ModelActive();
            count++;
        }
    }

    void ModelActive()
    {
        for (int i = 0; i < ingredientModels.Count; i++)
        {
            Destroy(ingredientModels[i]);
            ingredientModels.RemoveAt(i);
        }
        for (int i = 0; i < ingredientList.Count; i++)
        {
            GameObject ingredientModel = Instantiate(ingredientList[i].model[ingredientList[i].model.Length - 1]);
            ingredientModel.transform.parent = transform;
            ingredientModel.transform.localPosition = new Vector3 (0, 0.2f * (i + 1), 0);
            ingredientModels.Add(ingredientModel);
        }
        CheckBread();
    }

    public void CheckBread()
    {
        if (isBreadExist)
        {
            GameObject ingredientModel = Instantiate(topBread);
            ingredientModel.transform.parent = transform;
            ingredientModel.transform.localPosition = new Vector3(0, 0.2f * (count + 1), 0);
            ingredientModels.Add(ingredientModel);
        }
        isBreadExist = false;
    }

    public bool CheckIngredientReady(IngredientDisplay ingredient)
    {
        //�߶�� �ϴµ� ���ڸ��Ÿ� return;
        if (ingredient.ingredientObject.isPossibleCut && !ingredient.isCut)
        {
            return false;
        }
        if (ingredient.ingredientObject.isPossibleBake && !ingredient.isBake)
        {
            return false;
        }
        return true;
    }

    public void DestroyThisPlate()
    {
        photonView.RPC("RpcDestroyThisPlate", RpcTarget.All);
    }

    [PunRPC]
    void RpcDestroyThisPlate()
    {
        Destroy(gameObject);
    }
}
