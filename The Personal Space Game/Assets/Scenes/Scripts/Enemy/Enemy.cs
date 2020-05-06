using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isGrounded;

    public int jumpCount;
    public int HP;

    public float moveSpeed;
    public float moveSpeedQ;
    public float jumpForce;
    public float stopDist;
    public float jumpDist;
    public float jumpPos;

    public Transform quitPos;
    public Transform target;

    public Animator animator;
    
    Rigidbody2D rb;

    Player player;

    public Color enemyQuitCol;

    void Start()
    {
        player = FindObjectOfType<Player>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        jumpCount = 1;
    }

    void Update()
    {
        if (HP > 0 && player != null)
            target = player.transform;
        else
        {
            stopDist = 0;
            moveSpeed = moveSpeedQ;
            target = quitPos;

            if(player != null)
                gameObject.GetComponent<SpriteRenderer>().color = enemyQuitCol;

            //Quit();
        }

        if (target.position.x > transform.position.x)
            transform.eulerAngles = new Vector2(0, 0);
        else
            transform.eulerAngles = new Vector2(0, 180);

        Movement();

        Jump();

        if (!isGrounded)
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);

            }
            else
            {
                animator.SetBool("Jumping", true);
                animator.SetBool("Falling", false);

            }
        }
        else
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);

        }
    }
    
    void Movement()
    {
        if (Vector2.Distance(transform.position, target.position) > stopDist)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                             new Vector2(target.position.x, transform.position.y),
                             moveSpeed * Time.deltaTime);

            animator.SetFloat("Movement", 1);
        }
        else
            animator.SetFloat("Movement", -1);

    }

    void Jump()
    {
        if (target.position.y > transform.position.y + jumpPos && isGrounded
                                                               && Vector2.Distance(transform.position, target.position) < jumpDist
                                                               && jumpCount > 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "QuitPosition" && HP < 1 || other.tag == "QuitPosition" && player == null)
            Destroy(gameObject);
    }

    /*void Quit()
    {
        GameObject[] quitPos = GameObject.FindGameObjectsWithTag("QuitPosition");

        for (int i = 0; i < quitPos.Length; i++)
        {
            if (Vector3.Distance(transform.position,
                     quitPos[i].transform.position) <= closeDistance)
            {
                target = quitPos[i].transform;
            }
        }
    }*/
}
