using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class UI_TimeOver : MonoBehaviourPunCallbacks
{

    [SerializeField] private float speed = 3f;
    [SerializeField] private float endingTime = 5f;
    [SerializeField] private List<GameObject> removeUI = new List<GameObject>();
    [SerializeField] private GameObject ScoreBoard;
    [SerializeField] private Text myScore;
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private string currentMap;
    GameObject player;
    float curTime = 0;
    float curTime2 = 0;
    bool isEnding = false;
    private void Awake()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }
    private void Start()
    {
        if (GameManager.instance.Player)
        {
            player = GameManager.instance.Player;
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            button1.SetActive(false);
            button2.SetActive(false);
        }
    }
    private void Update()
    {
        if (!player)
        {
            player = GameManager.instance.Player;
        }
        curTime += Time.deltaTime;
        if (curTime < endingTime)
        {
            player.GetComponent<PlayerInput>().playerControl = true;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;

            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed);
            if (Vector3.Distance(transform.localScale, new Vector3(1, 1, 1)) < 0.01f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            ScoreBoard.SetActive(true);
            curTime2 += Time.deltaTime;
            float curCoin = StageManager.instance.curCoin;
            //20 40 60
            for(int i = 0; i < removeUI.Count; i++)
            {
                removeUI[i].SetActive(false);
            }
            if (curCoin < 20)
            {
                myScore.text = "Score: " + curCoin.ToString();
            }
            else if(curCoin >= 20 && curCoin < 40)
            {
                ScoreBoard.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                myScore.text = "Score: " + curCoin.ToString();
            }
            else if( curCoin >= 40 && curCoin < 60)
            {
                ScoreBoard.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                if (curTime2 > 1.5f)
                {
                    ScoreBoard.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                    myScore.text = "Score: " + curCoin.ToString();
                }
            }
            else if (curCoin >= 60)
            {
                ScoreBoard.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                if (curTime2 > 1.5f)
                {
                    ScoreBoard.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                }
                if (curTime2 > 3)
                {
                    ScoreBoard.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                    myScore.text = "Score: " + curCoin.ToString();
                }
            }
        }
    }
    [PunRPC]
    void Restart()
    {
        PhotonNetwork.LoadLevel(currentMap);
    }
    [PunRPC]
    void MiddleMap()
    {
        PhotonNetwork.LoadLevel("MiddleScene");
    }
    public void OnClickReStart()
    {
        photonView.RPC("Restart", RpcTarget.All);
    }
    public void OnClickMiddleMap()
    {
        photonView.RPC("MiddleMap", RpcTarget.All);
    }
}
