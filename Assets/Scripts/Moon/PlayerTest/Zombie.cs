using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Zombie : MonoBehaviourPun
{
    //앞으로만 움직임 
    //10초동안 다가옴
    //그 후 10초마다 문 -5씩 두번 공격 

    void Start()
    {
        ObjectManager.instance.SetPhotonObject(gameObject);
        StartCoroutine(AttackDoor());
    }

    void Update()
    {
        if (door)
        {
            if (Vector3.Distance(door.transform.position, transform.position) < 2)
                return;
            Vector3 dir = door.transform.position - transform.position;
            transform.position += dir * Time.deltaTime / 3;
        }
    }

    GameObject door;
    public void SetDoor(int id)
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
                door = ObjectManager.instance.photonObjectIdList[i];
            }
        }
    }

    IEnumerator AttackDoor()
    {
        for (int i = 0; i < 50; i++)
        {
            if(door)
            {
                door.GetComponent<Door>().Hit();
                yield return new WaitForSeconds(1f);
                door.GetComponent<Door>().Hit();
            }
            yield return new WaitForSeconds(4f);
        }
    }
}
