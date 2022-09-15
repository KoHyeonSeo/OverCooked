using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
        else
        {

            other.GetComponent<PlayerState>().curState = PlayerState.State.Die;
            //다시 부활해야함
        }
    }
}
