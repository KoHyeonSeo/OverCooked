using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Door : MonoBehaviourPun
{
    public int hp = 100;
    public GameObject[] Board;
    public RecipeObject[] recipes;
    public RecipeObject recipe;
    public GameObject recipeImageObject;
    public Image recipeImage;

    void Start()
    {
        recipeImageObject.SetActive(false);
        ObjectManager.instance.SetPhotonObject(gameObject);
    }

    public void SetRecipe()
    {
        int random = Random.Range(0, 2);
        recipe = recipes[random];
    }

    public void CheckOrder(int id)
    {
        photonView.RPC("RpcCheckOrder", RpcTarget.All, id);
    }

    public void setZombie(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (!ObjectManager.instance.photonObjectIdList[i])
            {
                ObjectManager.instance.photonObjectIdList.RemoveAt(i);
                i--;
                continue;
            }
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                zombie = ObjectManager.instance.photonObjectIdList[i];
            }
        }
    }

    public Plate plate;
    public GameObject zombie;
    [PunRPC]
    public void RpcCheckOrder(int id)
    {
        print("1");
        if (!recipe)
            return;
        print("2");
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (!ObjectManager.instance.photonObjectIdList[i])
            {
                ObjectManager.instance.photonObjectIdList.RemoveAt(i);
                i--;
                continue;
            }
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                plate = ObjectManager.instance.photonObjectIdList[i].GetComponent<Plate>();
            }
        }
        if (recipe.ingredients.Length != plate.ingredientList.Count)
        {
            print("개수가 다름");
            WrongPlate();
            return;
        }

            for (int j = 0; j < recipe.ingredients.Length; j++)
            {
                //접시의 재료와 주문서 레시피의 재료가 같은지 비교
                if (!plate.ingredientList.Contains(recipe.ingredients[j]))
                {
                    print("다른 재료가 들어감: " + recipe.ingredients[j]);
                WrongPlate();
                return;
                }
                if (j == recipe.ingredients.Length - 1)
                {
                //좀비 삭제
                Destroy(zombie);
                zombie = null;
                    print("리스트에 있는 음식");
                    //StageManager.instance.CoinPlus(8);
                recipe = null;
                recipeImageObject.SetActive(false);
                return;
                }
            }
        print("주문서에 없음");
        WrongPlate();
    }

    void WrongPlate()
    {
        Destroy(plate.gameObject);
        //접시 삭제
    }
    public void Hit()
    {
        hp -= 5;
        if (hp < 0)
        {
            PhotonNetwork.Destroy(gameObject);
            print("게임 오버");
        }

    }
}
