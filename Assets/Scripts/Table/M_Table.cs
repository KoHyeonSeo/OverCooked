using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Table : MonoBehaviour
{
    Color startColor;
    Color endColor;

    void Start()
    {
        startColor = GetComponent<Renderer>().material.color;
        endColor = new Color(startColor.r + 0.4f, startColor.g + 0.4f, startColor.b + 0.4f);
        print(startColor+ ", "+ endColor);
    }

    void Update()
    {
        
    }

    public void BlinkTable()
    {
        print(Time.deltaTime);
        GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * 2.5f, 1));
    }
}
