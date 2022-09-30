using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SinkPlateTable : MonoBehaviourPun
{
    public int cleanPlate;
    public GameObject PlatePrefab;
    public List<GameObject> plateList = new List<GameObject>();

    public void CreateCleanPlates()
    {
        photonView.RPC("RpcCreateCleanPlates", RpcTarget.All);
    }

    [PunRPC]
    public void RpcCreateCleanPlates()
    {
        for (int i = 0; i < plateList.Count; i++)
        {
            Destroy(plateList[i]);
            plateList.RemoveAt(i);
        }
        for (int i = 0; i < cleanPlate; i++)
        {
            GameObject plate = Instantiate(PlatePrefab);
            plate.transform.parent = transform;
            plate.transform.localPosition = new Vector3(0, 0.2f * (i + 1), 0);
            plateList.Add(plate);
        }
    }

    public GameObject plate;
    public void CreatePlate(int id)
    {
        photonView.RPC("RpcCreatePlate", RpcTarget.All, id);
    }

    GameObject player;
    [PunRPC]
    public void RpcCreatePlate(int id)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().ViewID == id)
            {
                player = players[i];
            }
        }
        cleanPlate--;
        plate = Instantiate(PlatePrefab);
        player.GetComponent<PlayerRayCheck>().HavingSettingObject(plate);
        //plate = PhotonNetwork.Instantiate(PlatePrefab.name, transform.position, Quaternion.identity);
        CreateCleanPlates();
    }
}
