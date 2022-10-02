using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlateManager : MonoBehaviourPun
{
    public List<GameObject> plateList = new List<GameObject>();
    public GameObject platePrefab;
    public int plateCount = 0;
    public static PlateManager instance;

    private void Awake()
    {
        instance = this;
    }

    GameObject plate;
    public void AddDirtyPlate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        plate = PhotonNetwork.Instantiate(platePrefab.name, transform.position, Quaternion.identity);
        StartCoroutine(DirtyPlateSetting(plate));
    }

    IEnumerator DirtyPlateSetting(GameObject plate)
    {
        yield return new WaitForSeconds(0.1f);
        photonView.RPC("RpcDirtyPlateSetting", RpcTarget.All, plate.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    void RpcDirtyPlateSetting(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (!ObjectManager.instance.photonObjectIdList[i])
            {
                ObjectManager.instance.photonObjectIdList.RemoveAt(i);
                i--;
                continue;
            }
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                plate = ObjectManager.instance.photonObjectIdList[i];
            }
        }
        plate.GetComponent<Plate>().isdirty = true;
        //plate.SetActive(true);
        //���� ��ȯ ���̺� ���ð� �ϳ��� ������ �ϳ� �߰�
        if (plateList.Count == 0)
        {
            GetComponent<M_Table>().SetObject(plate.GetComponent<PhotonView>().ViewID);
            plate.transform.parent = transform;
            plate.transform.localPosition = new Vector3(0.5f, 0.7f, 0.5f);
            plateList.Add(plate);
        }
        else
        {
            plate.GetComponent<BoxCollider>().enabled = false;
            //���� �������� �ø���
            plateList[plateList.Count - 1].GetComponent<Plate>().AddPlate(plate);
            plateList.Add(plate);

        }
        plateCount++;
    }

    public void RemovePlateList()
    {
        photonView.RPC("RpcRemovePlateList", RpcTarget.All);
    }    

    [PunRPC]
    public void RpcRemovePlateList()
    {
        plateList.Clear();
        plateCount = 0;
    }
}
