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
    //플레이어 선택 캐릭터 정보를 저장
    public Dictionary<int, string> playerInfo = new Dictionary<int, string>();

    //Json 파일에 저장할 플레이어와 선택한 캐릭터 정보에 대한 구조체
    [System.Serializable]
    public struct PlayerCharactor
    {
        public int myViewId;
        public string name;
    }

    //구조체를 담을 리스트 클래스 생성
    [System.Serializable]
    public class ArrayJson<T>
    {
        public List<T> data;
    }

    ArrayJson<PlayerCharactor> arrayJson = new ArrayJson<PlayerCharactor>();
    //SpawnPos 계산
    private void CalcSpawnPos()
    {
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.PlayerCount];
        float position = (endPos.position.x - startPos.position.x) / PhotonNetwork.CurrentRoom.PlayerCount;
    
        //Spawn 거리 계산
        for(int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = new Vector3(startPos.position.x + position * i, 0.5f, startPos.position.z);
        }
    } 

    //플레이어 생성
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        //1. 계산하고
        CalcSpawnPos();

        //2. 들어온 친구 배치 시키고
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
        //계산하고
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
        print(newPlayer.NickName + "님이 방에 들어왔");

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        ////2. 계산한 후
        //CalcSpawnPos();

        ////3. 재배치
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

        //플레이어 정보 저장
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
