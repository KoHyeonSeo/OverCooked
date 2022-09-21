using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
    private void CalcSpawnPos()
    {
        spawnPos = new Vector3[PhotonNetwork.CurrentRoom.PlayerCount];
        float position = (endPos.position.x - startPos.position.x) / PhotonNetwork.CurrentRoom.PlayerCount;
    
        //3. 나머지 애들 재배치시키고
        for(int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = new Vector3(startPos.position.x + position * i, 0.5f, startPos.position.z);
        }
    } 

    //플레이어 생성
    private void Start()
    {
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        //1. 계산하고
        CalcSpawnPos();

        //2. 들어온 친구 배치 시키고
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);


    }
    //현재 방에 있는 Player GameObject를 담아놓자.
    public List<GameObject> players = new List<GameObject>();
    
    public void AddPlayers(GameObject gameObject)
    {
        Debug.Log(gameObject);
        players.Add(gameObject);


        CalcSpawnPos();

        //print("PhotonNetwork Player = " + PhotonNetwork.CurrentRoom.PlayerCount);
        //print("Player = " + players.Count);
        //print("SpawnPos = " + spawnPos.Length);

        //3. 나머지 애들 위치 재배치
        for (int i = 0; i < spawnPos.Length; i++)
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

        //리스트에서 이 아이를 빼야한다.
        for(int i = 0; i < players.Count; i++)
        {
            if (!players[i])
                players.RemoveAt(i);
        }

        //2. 계산한 후
        CalcSpawnPos();

        //3. 나머지 애들 위치 재배치
        for (int i = 0; i < spawnPos.Length; i++)
        {
            players[i].transform.position = spawnPos[i];
        }

    }
}
