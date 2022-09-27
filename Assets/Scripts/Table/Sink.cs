using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sink : MonoBehaviour
{
    public List<GameObject> dirtyPlateList = new List<GameObject>();
    public List<GameObject> cleanPlateList = new List<GameObject>();
    public int dirtyPlate = 0;
    public int cleanPlate = 0;
    public GameObject sink;
    public Image washGauge;
    public Image washGaugeImage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void WashPlate()
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
