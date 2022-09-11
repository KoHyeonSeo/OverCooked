using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Food"))
        {
            Debug.Log("¿Ü ¾Êµµ¤Ó");
            GameObject food = Instantiate(collision.gameObject);
            food.layer = LayerMask.NameToLayer("Table");
            food.transform.parent = transform;
            food.transform.position = new Vector3(0, 1, 0);
            food.GetComponent<Rigidbody>().useGravity = false;
            Destroy(collision.gameObject);
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
                food.layer = LayerMask.NameToLayer("Player");
                food.transform.parent = collision.transform;
                food.transform.position = Vector3.up + collision.transform.forward * 2f;
                food.GetComponent<Rigidbody>().useGravity = false;
                food.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Destroy(transform.GetChild(0).gameObject);
            }

        }
    }
}
