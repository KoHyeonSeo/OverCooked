using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectManager : MonoBehaviourPun
{
    public List<int> photonObjectIdList = new List<int>();

    public static ObjectManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void SetViewID(int id)
    {
        photonObjectIdList.Add(id);
    }
}
