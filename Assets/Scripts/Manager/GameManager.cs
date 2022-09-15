using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //나중에 나의 캐릭터를 찾아야할 방법으로 바꿔야한다.
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if(!Player)
            Player = GameObject.FindGameObjectWithTag("Player");
    }
    public GameObject Player { get; private set; }
}
