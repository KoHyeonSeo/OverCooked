using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DeadZone : MonoBehaviour
{
    public float waitTime = 2f;
    private bool playerDead = false;
    private Transform player;
    private float curTime = 0;
    private void Update()
    {
        if (playerDead)
        {
            curTime += Time.deltaTime;
            if (curTime > waitTime)
            {
                player.GetComponent<PlayerInteract>().OnBirth();
                playerDead = false;
                curTime = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
        else
        {
            other.GetComponent<PlayerState>().curState = PlayerState.State.Die;
            if (other.transform.childCount > 1)
                other.transform.GetChild(1).parent = null;
            player = other.transform;
            playerDead = true;
        }
    }
}
