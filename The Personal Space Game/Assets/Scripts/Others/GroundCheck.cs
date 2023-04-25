using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public string checkFor;

    bool isGrounded;

    public GameObject controller;

    PlayerMovement player;
    EnemyMovement enemy;

    void Update()
    {
        if (checkFor == "Player")
        {
            player = controller.GetComponent<PlayerMovement>();
            if (isGrounded)
                player.isGrounded = true;
            else
                player.isGrounded = false;
        }

        if (checkFor == "Enemy")
        {
            enemy = controller.GetComponent<EnemyMovement>();

            if (isGrounded)
                enemy.isGrounded = true;
            else
                enemy.isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "Platform")
            isGrounded = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "Platform")
            isGrounded = false;
    }
}
