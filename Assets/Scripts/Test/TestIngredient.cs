using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIngredient : MonoBehaviour
{
    private void Update()
    {
        if (transform.parent == null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
