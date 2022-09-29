using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Box : MonoBehaviour
{
    /*public GameObject getObject;
    public Transform objectPosition;
    public PlayerInput playerInput;

    void Start()
    {
        *//*if (GameManager.instance.Player)
        {
            playerInput = GameManager.instance.Player.GetComponent<PlayerInput>();
        }*//*
        
    }

    void Update()
    {
        *//*if(playerInput)
        {
            if (playerInput.LeftClickDown)
                print("playerClick");
        }
        else
        {
            playerInput = GameManager.instance.Player.GetComponent<PlayerInput>();
        }*//*
    }

    public void SetObject(GameObject obj)
    {
        //박스 위에 오브젝트가 없으면 받은 오브젝트 셋팅
        if (!getObject) 
        {
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 1, 0);
            //getObject.transform.position = objectPosition.position;
            
            getObject = obj;
        }
        else
        {
            if (obj.tag == "Food" && getObject.GetComponent<Plate>())
            {
                getObject.GetComponent<Plate>().GetIngredient(obj);
            }
            else if (obj.GetComponent<Plate>() && getObject.GetComponent<IngredientDisplay>())
            {
                obj.GetComponent<Plate>().GetIngredient(getObject);
            }
        }
        
    }*/
}
