using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IngredientBox : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public GameObject player;
    bool isPlayerExit;

    void Start()
    {/*
        if (GameManager.instance.Player)
        {
            player = GameManager.instance.Player;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (player && isPlayerExit)
        {
            if (Input.GetKeyDown(KeyCode.E))//player.GetComponent<PlayerInput>().LeftClickDown)
            {
                print(player.name);
                GameObject ingredient = Instantiate(ingredientPrefab);
                ingredient.transform.parent = player.transform;
                ingredient.transform.localPosition = new Vector3(0, -.5f, .5f);
                string[] names = ingredient.name.Split('(');
                ingredient.name = names[0];
            }
        }
        else
        {
            player = GameManager.instance.Player;
        }*/
    }

    public void CheckClick()
    {
        /*if (player.GetComponent<PlayerInput>().LeftClickDown)
        {
            GameObject ingredient = Instantiate(ingredientPrefab);
            ingredient.transform.parent = player.transform;
            ingredient.transform.localPosition = new Vector3(0, -.5f, .5f);
            string[] names = ingredient.name.Split('(');
            ingredient.name = names[0];
        }*/
    }

    void OnDestroy()
    {
        Debug.Log("Test Destroy");
    }

    //재료 생성
    public void CreateIngredient()
    {
        /*ingredient = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
        //박스 클릭하면 재료 생성
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ingredient = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
        }*/
    }

}
