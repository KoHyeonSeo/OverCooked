using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayer : MonoBehaviour
{
    private void Start()
    {
        LobbyManager.instance.AddPlayers(gameObject);
    }
}
