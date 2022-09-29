using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectManager : MonoBehaviourPun
{
    public List<GameObject> photonObjectIdList = new List<GameObject>();

    public static ObjectManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetPhotonObject(GameObject obj)
    {
        photonObjectIdList.Add(obj);
    }
}
