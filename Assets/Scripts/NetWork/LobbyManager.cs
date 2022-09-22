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

        //2. ����� ��
        CalcSpawnPos();

        //3. ���ġ
        ChangePosition();
    }
    public void OnClickMiddleMap()
    {

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("MiddleScene");
    }
}
