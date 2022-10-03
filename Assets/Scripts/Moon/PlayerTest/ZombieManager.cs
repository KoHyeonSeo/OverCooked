using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ZombieManager : MonoBehaviourPun
{
    //3�� �� ����
    //���� ���� ��ġ�� �� 3����
    //������ ���� ��ȯ
    //���̺�1 10, 30, 50 / 1 3 2
    //���̺�2 90 110 120  130 / 2 3 1 2
    //���̺�3 180 210 /1,2 2,3
    //���̺�4 260 290 300 310/ 1,2 3 1 2
    public Transform[] spawnTransform;
    Vector3[] spawnPosition = new Vector3[3];
    public GameObject[] zombiePrefab; // 1 = 3����, 2 = 4����
    public GameObject[] door;
    public Text waveText;
    public GameObject playerPrefab;
    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(5f, 2f, 6f), Quaternion.identity);
        for (int i = 0; i < 3; i++)
        {
            spawnPosition[i] = spawnTransform[i].position;
        }
        StartCoroutine(Wave_1());
    }

    IEnumerator Wave_1()
    {
        waveText.text = "ù��° ����";
        yield return new WaitForSeconds(1f);
        SpawnZombie(0, 1);
        yield return new WaitForSeconds(2f);
        SpawnZombie(2, 0);
        yield return new WaitForSeconds(2f);
        SpawnZombie(1, 0);
    }

    GameObject zombie;
    void SpawnZombie(int positionNum, int zombieNum)
    {
        zombie = PhotonNetwork.Instantiate(zombiePrefab[zombieNum].name, spawnPosition[positionNum], Quaternion.identity);
        StartCoroutine(SetZombie(positionNum));
    }

    IEnumerator SetZombie(int positionNum)
    {
        yield return new WaitForSeconds(0.1f);
        zombie.GetComponent<Zombie>().SetDoor(door[positionNum].GetComponent<PhotonView>().ViewID);
        door[positionNum].GetComponent<Door>().setZombie(zombie.GetComponent<PhotonView>().ViewID);
        door[positionNum].GetComponent<Door>().SetRecipe();
        door[positionNum].GetComponent<Door>().recipeImageObject.SetActive(true);
    }
}
