using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalSpace : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();

        GetComponent<CircleCollider2D>().radius = player.maxSpace / 2;
    }
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
            player.noSpace = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
            player.noSpace = false;
    }
}
