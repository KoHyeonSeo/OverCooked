using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public static GameManager instance;
    //public Transform playerPos;
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
        //Debug.Log("isMine : " + photonView.IsMine);
        //Player = PhotonNetwork.Instantiate("Shark_Player", playerPos.position + new Vector3(Random.Range(-1,1),0,Random.Range(-2,2)), Quaternion.identity);
        Player = PhotonNetwork.Instantiate("Shark_Player", new Vector3(Random.Range(-10, 10), 5, Random.Range(-10, 10)), Quaternion.identity);
        PhotonNetwork.AutomaticallySyncScene = true;

        //Debug.Log("Player GetInstanceID : " + Player.GetInstanceID());
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (!Player)
            {
                //Player = PhotonNetwork.Instantiate("Shark_Player", playerPos.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-2, 2)), Quaternion.identity);
            }
        }

        #region ���̵�
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (Input.GetKeyDown((KeyCode)(48 + i)))
                {
                    SceneManager.LoadScene(i);
                }
            }
        }
        #endregion
    }
    public GameObject Player { get; private set; }
}
