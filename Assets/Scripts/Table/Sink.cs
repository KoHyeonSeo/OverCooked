using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    public List<GameObject> cleanPlateList = new List<GameObject>();
    public GameObject sink;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void WashPlate()
    {
        for (int i = 0; i < PlateManager.instance.plateList.Count; i++)
        {
            GameObject plate = PlateManager.instance.plateList[i];
            plate.GetComponent<Plate>().isdirty = false;
            cleanPlateList.Add(plate);
        }
    }

    public void TakeAwayPlate()
    {

        cleanPlateList.Remove(cleanPlateList[cleanPlateList.Count - 1]);
    }
}
