using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SinkPlateTable : MonoBehaviour
{
    public int cleanPlate;
    public GameObject PlatePrefab;
    public List<GameObject> plateList = new List<GameObject>();

    public void CreateCleanPlates()
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

    public GameObject CreatePlate()
    {
        cleanPlate--;
        GameObject plate = PhotonNetwork.Instantiate(PlatePrefab.name, transform.position, Quaternion.identity);
        //ObjectManager.instance.photonObjectIdList.Add(plate.GetComponent<PhotonView>().ViewID);
        CreateCleanPlates();
        return plate;
    }
}
