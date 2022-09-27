using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    public List<GameObject> plateList = new List<GameObject>();
    public GameObject platePrefab;
    public int plateCount = 0;
    public static PlateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddDirtyPlate()
    {
        GameObject plate = Instantiate(platePrefab);
        plate.GetComponent<Plate>().isdirty = true;
        //���� ��ȯ ���̺� ���ð� �ϳ��� ������ �ϳ� �߰�
        if (plateList.Count == 0)
        {
            GetComponent<M_Table>().SetObject(plate);
            plate.transform.parent = transform;
            plate.transform.localPosition = new Vector3(0, 0.2f, 0);
            plateList.Add(plate);
        }
        else
        {
            //���� �������� �ø���
            plateList[plateList.Count - 1].GetComponent<Plate>().AddPlate(plate);
            plateList.Add(plate);

        }
        plateCount++;
    }

    public void RemovePlateList()
    {
        plateList.Clear();
        plateCount = 0;
    }
}
