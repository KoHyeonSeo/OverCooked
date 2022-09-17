using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestText : MonoBehaviour
{
    [SerializeField] private Text text;
    private bool isOne = true;
    private void Update()
    {
        if (isOne)
        {
            Color color = text.color;
            color.a = Mathf.Lerp(text.color.a, 0, Time.deltaTime * 5);
            text.color = color;
            if (color.a < 0.1f)
                isOne = false;
        }
        else
        {
            Color color = text.color;
            color.a = Mathf.Lerp(text.color.a, 1, Time.deltaTime * 5);
            text.color = color;
            if (color.a > 0.9f)
                isOne = true;
        }
    }
}
