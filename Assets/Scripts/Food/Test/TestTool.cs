using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTool : MonoBehaviour
{
    public string function = "Bake";
    private void OnTriggerEnter(Collider other)
    {
        if (other?.GetComponent<Food>())
        {
            other.GetComponent<Food>().OrderCooking(function);
        }
    }
}
