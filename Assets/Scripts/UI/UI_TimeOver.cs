using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TimeOver : MonoBehaviour
{

    [SerializeField] private float speed = 3f;
    private void Awake()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed);
        if(Vector3.Distance(transform.localScale, new Vector3(1, 1, 1)) < 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
