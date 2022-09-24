using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartPlayer : MonoBehaviourPun
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        LobbyManager.instance.photonView.RPC("ChangePosition", RpcTarget.All);
        transform.localEulerAngles = new Vector3(0,180,0);
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            for(int i =0; i <transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
