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
        //���߿� ���� ĳ���͸� ã�ƾ��� ������� �ٲ���Ѵ�.
        Player = GameObject.Find("Player");
    }
    private void Update()
    {
        if(!Player)
            Player = GameObject.Find("Player");
    }
    public GameObject Player { get; private set; }
}
