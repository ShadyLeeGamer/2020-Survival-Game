using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public string checkFor;

    bool isGrounded;

    public GameObject controller;

    Player player;
    Enemy enemy;

    void Start()
    {

    }

    void Update()
    {
        if (checkFor == "Player")
        {
            player = controller.GetComponent<Player>();
            if (isGrounded)
                player.isGrounded = true;
            else
                player.isGrounded = false;
        }

        if (checkFor == "Enemy")
        {
            enemy = controller.GetComponent<Enemy>();

            if (isGrounded)
                enemy.isGrounded = true;
            else
                enemy.isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "Platform")
        {
            isGrounded = true;

            if (checkFor == "Player")
            {
                player = controller.GetComponent<Player>();
                player.jumpCount = 1;
            }
            if (checkFor == "Enemy")
            {
                enemy = controller.GetComponent<Enemy>();
                enemy.jumpCount = 1;
            }
        }


    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "Platform")
        {
            isGrounded = false;

            if (checkFor == "Player")
            {
                player = controller.GetComponent<Player>();
                player.jumpCount = 0;
            }

            if (checkFor == "Enemy")
            {
                enemy = controller.GetComponent<Enemy>();
                enemy.jumpCount = 0;
            }
        }
    }
}
