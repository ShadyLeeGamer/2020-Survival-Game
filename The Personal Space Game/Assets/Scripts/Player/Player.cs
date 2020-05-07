using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool invincibile;
    public bool noSpace;
    public bool shrink;
    public bool isGrounded;
    public bool isFalling;
    public bool isHit;

    public int jumpCount;
    public float HP;

    public float projectileSpeed;
    public float jumpForce;
    public float fireRate;
    public float speed;
    public float shrinkSpeed;
    public float growSpeed;
    public float[] space;
    public int currentSpace;
    public float minSpace;
    public float maxSpace;
    public float minSpace_;
    public float maxSpace_;
    public float invFrameDelay;
    float nextTimeToFire;
    float invFrame;

    public GameObject projectile;
    public GameObject dangerSpace;

    public Transform safeSpace;
    public Transform currnetFirePoint;
    public Transform groundCheck;
    public Transform[] firePoint;

    public Animator animator;

    Rigidbody2D rb;

    public Material outline;

    PersonalSpace personalSpace;
    SpriteRenderer spriteRenderer;

    public Vector2 knockback;
    public Vector2 currentKnockback;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        personalSpace = FindObjectOfType<PersonalSpace>();

        jumpCount = 1;
        currentSpace = space.Length - 1;
        maxSpace = space[currentSpace];

        minSpace_ = minSpace;
        maxSpace_ = maxSpace;

        HP = space.Length;

        outline.SetFloat("_Thickness", 0);
        outline.SetColor("_Color", personalSpace.safeSpaceColB);

        safeSpace.localScale = new Vector2(minSpace, minSpace);

        currentKnockback = new Vector2(-knockback.x, knockback.y);
    }
    
    void Update()
    {
        Jump();

        Shoot();

        PersonalSpace();

        dangerSpace.transform.localScale = new Vector2(space[currentSpace],
                                                       space[currentSpace]);

        personalSpace.GetComponent<CircleCollider2D>().radius = space[currentSpace] / 2;

        if (HP <= 0)
            Destroy(gameObject);
        else if (HP < 2)
        {
            outline.SetFloat("_Thickness", 0.005f);
            safeSpace.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        if (invincibile)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, personalSpace.safeSpaceColB.a);
        else
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 255);
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");

        Vector2 move = new Vector3(x * speed, rb.velocity.y);

        if (x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
            currentKnockback.x = -knockback.x;
        }
        if (x < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
            currentKnockback.x = knockback.x;
        }
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

        if (!isHit)
            rb.velocity = move;
        else
        {
            rb.velocity = new Vector2(currentKnockback.x, currentKnockback.y);

            if (!isGrounded)
                isHit = false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && jumpCount > 0)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    void Shoot()
    {
        if (Input.GetKey(KeyCode.P))
        {
            animator.SetBool("Shooting", true);

            if (Time.time >= nextTimeToFire)
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
        else
            animator.SetBool("Shooting", false);
    }

    void PersonalSpace()
    {
        if (Time.time >= invFrame)
        {
            invincibile = false;
            if (noSpace)
            {
                if (HP > 1)
                    dangerSpace.SetActive(true);
                else
                    outline.SetColor("_Color", personalSpace.dangerSpaceCol);
                if (safeSpace.localScale.x <= minSpace)
                {
                    invFrame = Time.time + 1f * invFrameDelay;

                    isHit = true;

                    minSpace_ = minSpace;
                    HP--;

                    safeSpace.localScale = new Vector2(minSpace, minSpace);

                    if (currentSpace > 0)
                        maxSpace = space[currentSpace -= 1];

                    shrink = false;
                }
                else if (safeSpace.localScale.x == maxSpace)
                    shrink = true;
            }
            else
            {
                dangerSpace.SetActive(false);

                if (HP < 2)
                    outline.SetColor("_Color", personalSpace.safeSpaceColB);

                shrink = false;
            }
        }
        else
            invincibile = true;

        if (shrink == true)
            safeSpace.localScale = new Vector2(maxSpace_ -= shrinkSpeed * Time.deltaTime,
                                               maxSpace_ -= shrinkSpeed * Time.deltaTime);
        else
            safeSpace.localScale = new Vector3(minSpace_ += growSpeed * Time.deltaTime,
                                               minSpace_ += growSpeed * Time.deltaTime);

        if (safeSpace.localScale.x >= maxSpace)
        {
            safeSpace.localScale = new Vector2(maxSpace, maxSpace);
            maxSpace_ = maxSpace;

        }
    }
}
