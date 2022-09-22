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

        //2. 계산한 후
        CalcSpawnPos();

        //3. 재배치
        ChangePosition();
    }
    public void OnClickMiddleMap()
    {

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("MiddleScene");
    }
}
