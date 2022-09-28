using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FireBox : MonoBehaviourPun
{
    public GameObject cookingTool;
    public Vector3 objectPosition;
    public bool isFire;
    public GameObject fireEffectPrefab;
    public GameObject fireEffect;
    public float fireGauge;
    public GameObject fireGaugeCanvas;
    public Image fireGaugeImage;
    GameObject tool;

    void Start()
    {
        objectPosition = new Vector3(0, 1, 0);
        if (PhotonNetwork.IsMasterClient && cookingTool)
        {
            tool = PhotonNetwork.Instantiate(cookingTool.name, transform.position, Quaternion.identity);
            photonView.RPC("RpcToolSetting", RpcTarget.All, tool.GetComponent<PhotonView>().ViewID);
        }
        fireGaugeImage.GetComponent<Image>().fillAmount = 0;
        fireGaugeCanvas.SetActive(false);
    }

    [PunRPC]
    void RpcToolSetting(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            print("ViewId" + ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID);
            print("ID" + id);
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                print(id);
                tool = ObjectManager.instance.photonObjectIdList[i];
            }
        }
        tool.transform.parent = transform;
        tool.transform.localPosition = objectPosition;
        cookingTool = tool;
    }

    void Update()
    {
        if (isFire && fireGauge <= 0)
        {
            fireGauge = 0;
            fireGaugeCanvas.SetActive(false);
            Destroy(fireEffect);
            isFire = false;
        }
        if (isFire && fireGauge > 0)
        {
            fireGaugeImage.GetComponent<Image>().fillAmount = fireGauge / 100;
            return;
        }
    }

    public void Fire()
    {
        isFire = true;
        fireEffect = Instantiate(fireEffectPrefab);
        Vector3 firePos = transform.position;
        firePos.y += 1;
        fireEffect.transform.position = firePos;
        fireGauge = 100;
    }

    public void FireSuppression(float i)
    {
        fireGauge -= i;
    }

    public void SetObject(int id)
    {
        photonView.RPC("RpcSetObject", RpcTarget.All, id);
    }

    GameObject obj;
    [PunRPC]
    public void RpcSetObject(int id)
    {
        for (int i = 0; i < ObjectManager.instance.photonObjectIdList.Count; i++)
        {
            if (ObjectManager.instance.photonObjectIdList[i].GetComponent<PhotonView>().ViewID == id)
            {
                obj = ObjectManager.instance.photonObjectIdList[i];
            }
        }
        if (!cookingTool && obj.GetComponent<FryingPan>())
        {
            cookingTool = obj;
            cookingTool.transform.parent = transform;
            cookingTool.transform.localPosition = objectPosition;
        }
    }
}
