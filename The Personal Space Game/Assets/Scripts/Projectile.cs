using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Ground")
        {
            Instantiate(hitEFX, transform.position
                              , Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().HP--;
            Instantiate(hitEFX, transform.position
                              , Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
