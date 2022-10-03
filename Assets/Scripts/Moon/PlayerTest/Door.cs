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
            print("������ �ٸ�");
            WrongPlate();
            return;
        }

            for (int j = 0; j < recipe.ingredients.Length; j++)
            {
                //������ ���� �ֹ��� �������� ��ᰡ ������ ��
                if (!plate.ingredientList.Contains(recipe.ingredients[j]))
                {
                    print("�ٸ� ��ᰡ ��: " + recipe.ingredients[j]);
                WrongPlate();
                return;
                }
                if (j == recipe.ingredients.Length - 1)
                {
                //���� ����
                Destroy(zombie);
                zombie = null;
                    print("����Ʈ�� �ִ� ����");
                    //StageManager.instance.CoinPlus(8);
                recipe = null;
                recipeImageObject.SetActive(false);
                return;
                }
            }
        print("�ֹ����� ����");
        WrongPlate();
    }

    void WrongPlate()
    {
        Destroy(plate.gameObject);
        //���� ����
    }
    public void Hit()
    {
        hp -= 5;
        if (hp < 0)
        {
            PhotonNetwork.Destroy(gameObject);
            print("���� ����");
        }

    }
}
