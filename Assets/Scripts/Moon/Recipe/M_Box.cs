using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Box : MonoBehaviour
{
    public GameObject getObject;
    public Transform objectPosition;
    public PlayerInput playerInput;
    void Start()
    {
        /*if (GameManager.instance.Player)
        {
            playerInput = GameManager.instance.Player.GetComponent<PlayerInput>();
        }*/
        
    }

    void Update()
    {
        /*if(playerInput)
        {
            if (playerInput.LeftClickDown)
                print("playerClick");
        }
        else
        {
            playerInput = GameManager.instance.Player.GetComponent<PlayerInput>();
        }*/
    }

    public void SetObject(GameObject obj)
    {
        //�ڽ� ���� ������Ʈ�� ������ ���� ������Ʈ ����
        if (!getObject)
        {
            getObject = obj;
            getObject.transform.position = objectPosition.position;
            getObject.transform.parent = transform;
        }
        
    }
}
