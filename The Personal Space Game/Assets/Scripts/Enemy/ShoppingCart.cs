using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x <= -31 && transform.position.x > -56 ||
            transform.position.x >= 31 && transform.position.x < 56)
            GetComponent<Collider2D>().isTrigger = true;
        else
            GetComponent<Collider2D>().isTrigger = false;
    }
}
