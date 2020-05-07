using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEFX;

    public AudioClip splatSFX;
    public AudioClip hitSFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Ground")
        {
            hitEFX.GetComponent<AudioSource>().clip = splatSFX;
            hitEFX.GetComponent<AudioSource>().Play();

            GameObject hitEFX_ = Instantiate(hitEFX, transform.position, Quaternion.identity) as GameObject;

            Destroy(gameObject);
        }

        if (other.transform.tag == "Enemy" && other.GetComponent<Enemy>().HP > 0)
        {
            hitEFX.GetComponent<AudioSource>().clip = hitSFX;
            hitEFX.GetComponent<AudioSource>().Play();

            other.gameObject.GetComponent<Enemy>().HP--;

            GameObject hitEFX_ = Instantiate(hitEFX, transform.position, Quaternion.identity) as GameObject;

            Destroy(gameObject);
        }
    }
}
