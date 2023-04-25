using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float activatePos;

    GameObject player;
    GameObject enemy;

    public Collider2D playerCollider;
    public Collider2D enemyCollider;

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (player != null)
        {
            if (player.transform.position.y > transform.position.y + activatePos)
                playerCollider.isTrigger = false;
            else
                playerCollider.isTrigger = true;
        }

        if (enemy != null)
        {
            if (enemy.transform.position.y > transform.position.y + activatePos)
                enemyCollider.isTrigger = false;
            else
                enemyCollider.isTrigger = true;
        }
    }
}
