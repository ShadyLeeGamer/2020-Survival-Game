using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool noSpace;
    public bool shrink;
    public bool isGrounded;
    public bool isFalling;
    public bool isFiring;

    public int jumpCount;
    public int HP;

    public float projectileSpeed;
    public float jumpForce;
    public float fireRate;
    public float speed;
    public float shrinkSpeed;
    public float growSpeed;
    public float minSpace;
    public float maxSpace;
    float minSpace_;
    float maxSpace_;
    float nextTimeToFire;

    public GameObject projectile;
    public GameObject dangerSpace;

    public Transform personalSpace;
    public Transform currnetFirePoint;
    public Transform groundCheck;
    public Transform[] firePoint;

    Rigidbody2D rb;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        shrink = true;

        jumpCount = 1;
        minSpace_ = minSpace;
        maxSpace_ = maxSpace;

        personalSpace.localScale = new Vector2(minSpace, minSpace);
    }
    
    void Update()
    {
        if (HP <= 0)
            Destroy(gameObject);

        if (Input.GetButtonDown("Jump") && isGrounded && jumpCount > 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.P))
        {
            isFiring = true;
            if (Time.time >= nextTimeToFire)
                    Shoot();
        }
        else
            isFiring = false;

        if (noSpace)
        {
            dangerSpace.SetActive(true);

            if (personalSpace.localScale.x < minSpace)
                shrink = false;
            else if (personalSpace.localScale.x == maxSpace)
                shrink = true;
        }
        else
        {
            dangerSpace.SetActive(false);
            shrink = false;
        }

        if(shrink == true)
            personalSpace.localScale = new Vector2(maxSpace_ -= shrinkSpeed * Time.deltaTime,
                                                   maxSpace_ -= shrinkSpeed * Time.deltaTime);
        else
            personalSpace.localScale = new Vector3(minSpace_ += growSpeed * Time.deltaTime,
                                                   minSpace_ += growSpeed * Time.deltaTime);

        if (personalSpace.localScale.x <= minSpace)
        {
            personalSpace.localScale = new Vector2(minSpace, minSpace);
            minSpace_ = minSpace;
            HP--;
            shrink = false;
        }

        if (personalSpace.localScale.x >= maxSpace)
        {
            personalSpace.localScale = new Vector2(maxSpace, maxSpace);
            maxSpace_ = maxSpace;

        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");

        Vector2 move = new Vector3(x * speed, rb.velocity.y);
        rb.velocity = move;

        if (x > 0)
            transform.eulerAngles = new Vector2(0, 0);
        if (x < 0)
            transform.eulerAngles = new Vector2(0, 180);
        if (x == 0)
        {
            currnetFirePoint = firePoint[0];

            animator.SetFloat("Movement", -1);
        }
        else if (isGrounded)
        {
            currnetFirePoint = firePoint[1];

            animator.SetFloat("Movement", 1);
        }

        if (!isGrounded)
        {
            currnetFirePoint = firePoint[2];

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
            animator.SetBool("Falling", false);
            animator.SetBool("Jumping", false);
        }

        if (isFiring)
            animator.SetBool("Shooting", true);
        else
            animator.SetBool("Shooting", false);

    }

    void Shoot()
    {
        GameObject bullet_ = Instantiate(projectile, currnetFirePoint.position
                                                           , Quaternion.identity);

        Rigidbody2D bulletRB = bullet_.GetComponent<Rigidbody2D>();

        nextTimeToFire = Time.time + 1f / fireRate;

        bullet_.transform.position = currnetFirePoint.position
                                   + currnetFirePoint.transform.forward;

        bulletRB.velocity = currnetFirePoint.transform.right
                          * projectileSpeed;
    }
}