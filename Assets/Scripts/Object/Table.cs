using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Food"))
        {
            GameObject food = Instantiate(collision.gameObject);
            food.transform.parent = transform;
            food.transform.position = new Vector3(0, 1, 0);
            food.GetComponent<Rigidbody>().useGravity = false;
        }
        else if (collision.collider.CompareTag("Player"))
        {
            if (collision.transform.GetChild(0).CompareTag("Food")
                || transform.childCount == 0)
            {
                return;
            }
            else
            {
                GameObject food = Instantiate(collision.transform.GetChild(0).gameObject);
                food.transform.parent = collision.transform;
                food.transform.position = collision.transform.forward;
                food.GetComponent<Rigidbody>().useGravity = false;
                Destroy(transform.GetChild(0).gameObject);
            }

        }
    }
}
