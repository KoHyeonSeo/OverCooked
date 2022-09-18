using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    [SerializeField] private float extinguisher = 5f;
    GameObject player;
    private RaycastHit hit;

    private void Start()
    {
        if (GameManager.instance.Player)
            player = GameManager.instance.Player;
    }
    private void Update()
    {
        if(!player)
            player = GameManager.instance.Player;
        if (player.GetComponent<PlayerInteract>().curInteractState == PlayerInteract.InteractState.FireDistinguish
            && player.GetComponent<PlayerInput>().FireExtinguisher)
        {
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward);

            //��ȭ�� ��� �κ��� ��Ҵ�
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.magenta);
            if (Physics.Raycast(ray, out hit, 5))
            {
                if (hit.transform.name.Contains("FireTable"))
                {
                    if (hit.transform.GetComponent<FireBox>().isFire)
                    {
                        hit.transform.GetComponent<FireBox>().FireSuppression((int)(extinguisher * Time.deltaTime));
                    }
                }
            }
        }
    }
}
