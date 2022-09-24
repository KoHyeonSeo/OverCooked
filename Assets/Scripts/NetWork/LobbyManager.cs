using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance;

    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    private Vector3[] spawnPos;

    private void Awake()
    {
        if(!instance)
            instance = this;
    }
    //�÷��̾� ���� ĳ���� ������ ����
    public Dictionary<int, string> playerInfo = new Dictionary<int, string>();

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
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        //1. ����ϰ�
        CalcSpawnPos();

        //2. ���� ģ�� ��ġ ��Ű��
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
    }
    public void Info(int key, string value)
    {
        photonView.RPC("RPC_ChangedInfo", RpcTarget.All, key, value);
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
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "���� �濡 ����");

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        ////2. ����� ��
        //CalcSpawnPos();

        ////3. ���ġ
        //ChangePosition();
    }
    public void OnClickMiddleMap()
    {
        Save();

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
                PhotonNetwork.LoadLevel("MiddleScene");
            }
        }
    }
    public void Save()
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
        print(jsonData);
        string path = Application.dataPath + "/Data";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "/PlayerData.txt", jsonData);
    }
}
