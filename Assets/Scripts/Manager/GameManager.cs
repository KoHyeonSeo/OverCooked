using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;

public class GameManager : MonoBehaviourPun
{
    public static GameManager instance;
    public List<Transform> playerPoss = new List<Transform>();
    LobbyManager.ArrayJson<LobbyManager.PlayerCharactor> arrayJson;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        //나중에 나의 캐릭터를 찾아야할 방법으로 바꿔야한다.
        //Debug.Log("isMine : " + photonView.IsMine);

        //씬 전환 후 동기화 
        PhotonNetwork.AutomaticallySyncScene = true;

        DataLoad();
        StartPlayer[] FindObjects = GameObject.FindObjectsOfType<StartPlayer>();
        GameObject myObject = null;

        for (int j = 0; j < FindObjects.Length; j++)
        {
            if (FindObjects[j].gameObject.GetComponent<PhotonView>().IsMine)
            {
                myObject = FindObjects[j].gameObject;
            }
        }
        for (int i = 0; i < arrayJson.data.Count; i++)
        {
            if (arrayJson.data[i].myViewId == myObject?.GetComponent<PhotonView>().ViewID)
            {
                //if (photonView.IsMine)
                    print("생성: " + arrayJson.data[i].name + " / " + playerPoss[i].position);
                Player =
                    PhotonNetwork.Instantiate(arrayJson.data[i].name, playerPoss[i].position, Quaternion.identity);
            }

        }

    }
    public List<GameObject> players = new List<GameObject>();
    private void Update()
    {
        //if (photonView.IsMine)
        //{
        //    if (!Player)
        //    {
        //        Player = PhotonNetwork.Instantiate("Shark_Player", playerPos.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-2, 2)), Quaternion.identity);
        //    }
        //}

        #region 씬이동
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
    public void DataLoad()
    {
        string path = Application.dataPath + "/Data";
        string jsonData = File.ReadAllText(path + "/PlayerData.txt");
        print(jsonData);
        arrayJson = 
            JsonUtility.FromJson<LobbyManager.ArrayJson<LobbyManager.PlayerCharactor>>(jsonData);
        
    }
    public GameObject Player { get; private set; }
}
