using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    public List<GameObject> plateList = new List<GameObject>();
    public GameObject platePrefab;

    public static PlateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddPlate()
    {
        GameObject plate = Instantiate(platePrefab);
        plate.GetComponent<Plate>().isdirty = true;
        //접시 반환 테이블에 접시가 하나도 없으면 하나 추가
        if (plateList.Count == 0)
        {
            GetComponent<M_Table>().SetObject(plate);
            plate.transform.parent = transform;
            plate.transform.localPosition = new Vector3(0, 1, 0);
            plateList.Add(plate);
        }
        else
        {
            //이전 접시위에 올리기
            plateList[plateList.Count - 1].GetComponent<Plate>().AddPlate(plate);
            plateList.Add(plate);

        }
    }

    public void TakeAwayPlate()
    {

    }
}
