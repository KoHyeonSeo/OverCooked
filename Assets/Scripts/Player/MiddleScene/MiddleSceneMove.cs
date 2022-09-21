using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MiddleSceneMove : MonoBehaviourPun
{
    [SerializeField] private float turnSpeed = 5;
    [SerializeField] private float moveSpeed = 7;
    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        
    }
}
