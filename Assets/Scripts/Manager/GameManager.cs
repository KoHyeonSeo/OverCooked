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
        //나중에 나의 캐릭터를 찾아야할 방법으로 바꿔야한다.
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if(!Player)
            Player = GameObject.FindGameObjectWithTag("Player");

        #region 씬이동
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
