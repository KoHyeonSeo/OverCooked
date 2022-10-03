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
                myScore.text = "Score: " + curCoin.ToString() + " \n평가: 0별 에반데";
            }
            else if(curCoin >= 20 && curCoin < 40)
            {
                myScore.text = "Score: " + curCoin.ToString() + "\n평가: (2별을 받은 현서가 비웃고 있다)";
                if (curCoin > 1f)
                    ScoreBoard.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            else if( curCoin >= 40 && curCoin < 60)
            {
                myScore.text = "Score: " + curCoin.ToString() + "\n평가: 너무 아쉽고";
                if (curTime2 > 1f)
                    ScoreBoard.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(true);
                if (curTime2 > 2f)
                    ScoreBoard.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true);
            }
            else if (curCoin >= 60)
            {
                myScore.text = "Score: " + curCoin.ToString() + "\n평가: 오 너무 잘하고~";
                if (curTime2 > 1f )
                    ScoreBoard.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(true);
                if (curTime2 > 2f)
                    ScoreBoard.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true);
                if (curTime2 > 3f)
                    ScoreBoard.transform.GetChild(2).GetChild(2).GetChild(1).gameObject.SetActive(true);
                
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
