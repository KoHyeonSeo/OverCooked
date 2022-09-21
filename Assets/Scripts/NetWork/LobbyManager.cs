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
    
        //3. ������ �ֵ� ���ġ��Ű��
        for(int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = new Vector3(startPos.position.x + position * i, 0.5f, startPos.position.z);
        }
    } 

    //�÷��̾� ����
    private void Start()
    {
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        //1. ����ϰ�
        CalcSpawnPos();

        //2. ���� ģ�� ��ġ ��Ű��
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);


    }
    //���� �濡 �ִ� Player GameObject�� ��Ƴ���.
    public List<GameObject> players = new List<GameObject>();
    
    public void AddPlayers(GameObject gameObject)
    {
        Debug.Log(gameObject);
        players.Add(gameObject);


        CalcSpawnPos();

        //print("PhotonNetwork Player = " + PhotonNetwork.CurrentRoom.PlayerCount);
        //print("Player = " + players.Count);
        //print("SpawnPos = " + spawnPos.Length);

        //3. ������ �ֵ� ��ġ ���ġ
        for (int i = 0; i < spawnPos.Length; i++)
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

        //����Ʈ���� �� ���̸� �����Ѵ�.
        for(int i = 0; i < players.Count; i++)
        {
            if (!players[i])
                players.RemoveAt(i);
        }

        //2. ����� ��
        CalcSpawnPos();

        //3. ������ �ֵ� ��ġ ���ġ
        for (int i = 0; i < spawnPos.Length; i++)
        {
            players[i].transform.position = spawnPos[i];
        }

    }
}
