using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TimeOver : MonoBehaviour
{

    [SerializeField] private float speed = 3f;
    GameObject player;
    private void Awake()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }
    private void Start()
    {
        if (GameManager.instance.Player)
        {
            player = GameManager.instance.Player;
        }
    }
    private void Update()
    {
        if (!player)
        {
            player = GameManager.instance.Player;
        }

        player.GetComponent<PlayerInput>().playerControl = true;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed);
        if(Vector3.Distance(transform.localScale, new Vector3(1, 1, 1)) < 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
