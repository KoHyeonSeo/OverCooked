using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if(!Player)
            Player = GameObject.FindGameObjectWithTag("Player");

        #region ���̵�
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (Input.GetKeyDown((KeyCode)(48 + i)))
            {
                SceneManager.LoadScene(i);
            }
        }
        #endregion
    }
    public GameObject Player { get; private set; }
}
