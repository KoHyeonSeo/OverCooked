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
        Player = GameObject.Find("Player");
    }
    private void Update()
    {
        if(!Player)
            Player = GameObject.Find("Player");
    }
    public GameObject Player { get; private set; }
}
