using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance;

    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject nextUI;

    private Vector3[] spawnPos;
    private bool isStartUI = false;

    private void Awake()
    {
        if(!instance)
            instance = this;
    }
    //�÷��̾� ���� ĳ���� ������ ����
    public Dictionary<int, string> playerInfo = new Dictionary<int, string>();
    //public SortedList<int, string> playerInfo = new SortedList<int, string>();

    //Json ���Ͽ� ������ �÷��̾�� ������ ĳ���� ������ ���� ����ü
    [System.Serializable]
    public struct PlayerCharactor
    {
        public int myViewId;
        public string name;
    }

    //����ü�� ���� ����Ʈ Ŭ���� ����
    [System.Serializable]
    public class ArrayJson<T>
    {
        public List<T> data;
    }

    ArrayJson<PlayerCharactor> arrayJson = new ArrayJson<PlayerCharactor>();
    //SpawnPos ���
    private void CalcSpawnPos()
    {
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.PlayerCount];
        float position = (endPos.position.x - startPos.position.x) / PhotonNetwork.CurrentRoom.PlayerCount;
    
        //Spawn �Ÿ� ���
        for(int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = new Vector3(startPos.position.x + position * i, 0.5f, startPos.position.z);
        }
    } 

    //�÷��̾� ����
    private void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
            startUI.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.SendRate = 10;
        //1. ����ϰ�
        CalcSpawnPos();

        //2. ���� ģ�� ��ġ ��Ű��
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
    }
    private void Update()
    {
        if (isStartUI)
        {
            nextUI.SetActive(true);
            nextUI.transform.localScale = Vector3.Lerp(nextUI.transform.localScale, new Vector3(40, 40, 40), Time.deltaTime * 0.5f);
            if(Vector3.Distance(nextUI.transform.localScale, new Vector3(40, 40, 40)) < 35f)
            {
                isStartUI = false;
                OnNextStage();
            }
        }
    }
    public void Info(int key, string value)
    {
        photonView.RPC("RPC_ChangedInfo", RpcTarget.AllBuffered, key, value);
    }

    [PunRPC]
    public void RPC_ChangedInfo(int key, string value)
    {
        playerInfo[key] = value;
    }

    [PunRPC]
    public void ChangePosition()
    {
        //����ϰ�
        CalcSpawnPos();

        List<GameObject> players = new List<GameObject>();
        StartPlayer[] obj = GameObject.FindObjectsOfType<StartPlayer>();
        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i].CompareTag("Player"))
            {
                players.Add(obj[i].gameObject);
            }
        }
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = spawnPos[i];
        }
    }
    [PunRPC]
    public void RPC_StartUI()
    {
        isStartUI = true;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "���� �濡 ����");

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
    public void OnNextStage()
    {
        photonView.RPC("RPC_Save", RpcTarget.All);
        PhotonNetwork.LoadLevel("MiddleScene");
        //PhotonNetwork.LoadLevel("Stage1");
    }
    public void OnClickMiddleMap()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            bool isChecking = true;
            StartPlayer [] players = GameObject.FindObjectsOfType<StartPlayer>();
            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].transform.GetChild(2).gameObject.activeSelf)
                {
                    isChecking = false;
                }
            }
            if (isChecking)
            {
                photonView.RPC("RPC_StartUI", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void RPC_Save()
    {
        List<int> list = new List<int>(playerInfo.Keys);
        
        arrayJson.data = new List<PlayerCharactor>();

        //�÷��̾� ���� ����
        foreach (int i in list)
        { 
            PlayerCharactor playerCharactor = new PlayerCharactor();
            
            playerCharactor.myViewId = i;
            playerCharactor.name = playerInfo[i];
            arrayJson.data.Add(playerCharactor);
        }
        arrayJson.data.Sort((struct1, struct2) => struct1.myViewId.CompareTo(struct2.myViewId));

        string jsonData = JsonUtility.ToJson(arrayJson, true);
        //print(jsonData);
        string path = Application.dataPath + "/Data";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "/PlayerData.txt", jsonData);

    }
    //public void Save()
    //{
    //    List<int> list = new List<int>(playerInfo.Keys);

    //    arrayJson.data = new List<PlayerCharactor>();

    //    //�÷��̾� ���� ����
    //    foreach (int i in list)
    //    {
    //        PlayerCharactor playerCharactor = new PlayerCharactor();

    //        playerCharactor.myViewId = i;
    //        playerCharactor.name = playerInfo[i];
    //        arrayJson.data.Add(playerCharactor);
    //    }
    //    //arrayJson.data.Sort((struct1, struct2) => struct1.myViewId.CompareTo(struct2.myViewId));

    //    string jsonData = JsonUtility.ToJson(arrayJson, true);
    //    print(jsonData);
    //    string path = Application.dataPath + "/Data";
    //    if (!Directory.Exists(path))
    //    {
    //        Directory.CreateDirectory(path);
    //    }
    //    File.WriteAllText(path + "/PlayerData.txt", jsonData);
    //}
}
