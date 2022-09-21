using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_MiddleMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7;
    void Update()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    }
}
