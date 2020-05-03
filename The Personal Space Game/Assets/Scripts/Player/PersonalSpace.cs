using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalSpace : MonoBehaviour
{
    public GameObject safeSpace;
    public GameObject dangerSpace;

    public Color safeSpaceColA;
    public Color safeSpaceColB;
    public Color dangerSpaceCol;
    public Color enemyHighlight;

    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();

        GetComponent<CircleCollider2D>().radius = player.maxSpace / 2;

        safeSpace.GetComponent<SpriteRenderer>().color = safeSpaceColA;
        dangerSpace.GetComponent<SpriteRenderer>().color = dangerSpaceCol;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && other.GetComponent<Enemy>().HP > 0)
        {
            player.noSpace = true;

            safeSpace.GetComponent<SpriteRenderer>().color = safeSpaceColB;
            other.GetComponent<SpriteRenderer>().color = enemyHighlight;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            player.noSpace = false;

            safeSpace.GetComponent<SpriteRenderer>().color = safeSpaceColA;
            other.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
