using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ZombieManager : MonoBehaviourPun
{
    //3초 뒤 시작
    //좀비 스폰 위치는 총 3군데
    //순서에 맞춰 소환
    //웨이브1 10, 30, 50 / 1 3 2
    //웨이브2 90 110 120  130 / 2 3 1 2
    //웨이브3 180 210 /1,2 2,3
    //웨이브4 260 290 300 310/ 1,2 3 1 2
    public Transform[] spawnTransform;
    Vector3[] spawnPosition = new Vector3[3];
    public GameObject[] zombiePrefab; // 1 = 3가지, 2 = 4가지
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
        waveText.text = "첫번째 진격";
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
