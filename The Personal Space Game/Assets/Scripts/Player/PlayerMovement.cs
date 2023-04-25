using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true;
    public bool isGrounded;
    public bool invincibile;
    public bool isHit;
    public bool isHugged;
    bool isJumping;

    public int HP;

    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float speed;

    public float invFrameDelay;
    public float invFrame;
    public float x;

    public Transform groundCheck;

    public Animator animator;

    public Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    public SpriteRenderer[] skinSpriteAnim;

    public Sprite[] skinSprite1;
    public Sprite[] skinSprite2;
    public Sprite[] skinSprite3;

    public Vector2 knockback;

    public PersonalSpace personalSpace;
    //public Joystick joystick;
    public Shooter shooter;

    void Start()
    {
        //joystick = FindObjectOfType<Joystick>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && isGrounded && rb.velocity.y == 0 && canMove)
        //if (joystick.Vertical >= 0.45f && isGrounded && rb.velocity.y == 0 && canMove)
            Jump();
    }

    void FixedUpdate()
    {
        if (canMove)
            Movement();

        if (isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = false;
        }
        else if(isHugged)
        {
            speed = 2;
            jumpForce = 7;
        }

        if (rb.velocity.y < 0)
            rb.gravityScale = fallMultiplier;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        //else if (rb.velocity.y > 0 && joystick.Vertical < 0.6f)
            rb.gravityScale = lowJumpMultiplier;
        else
            rb.gravityScale = 5f;
    }

    void Movement()
    {
        //x = joystick.Horizontal * speed;

        /*switch(joystick.Horizontal)
        {
            case float n when n > 0.3f && n < 0.5f:
                x = speed - 1;
                break;
            case float n when n < -0.3f && n > -0.5f:
                x = -speed + 1;
                break;
            case float n when n > -0.25f && n < 0.25f:
                x = 0;
                break;
        }*/

        /*if (joystick.Horizontal > 0.3f)
            x = speed;
        else if (joystick.Horizontal < -0.3f)
            x = -speed;
        else
         x = 0;*/

        float x = Input.GetAxisRaw("Horizontal") * speed;

        Vector2 move = new Vector2(x, rb.velocity.y);

        if (x > 0)
            transform.eulerAngles = new Vector2(0, 0);
        if (x < 0)
            transform.eulerAngles = new Vector2(0, 180);
        if (x == 0)
        {
            shooter.currnetFirePoint = shooter.firePoint[0];

            animator.SetFloat("Movement", -1);
        }
        else if (isGrounded)
        {
            shooter.currnetFirePoint = shooter.firePoint[1];

            animator.SetFloat("Movement", 1);
        }

        if (!isGrounded)
        {
            shooter.currnetFirePoint = shooter.firePoint[2];

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
            if (transform.eulerAngles == new Vector3(0, 0, 0))
                rb.velocity = new Vector2(-knockback.x, knockback.y);
            else if (transform.eulerAngles == new Vector3(0, 180, 0))
                rb.velocity = new Vector2(knockback.x, knockback.y);

            if (!isGrounded)
                isHit = false;
        }
    }

    void Jump()
    {
        isJumping = true;
    }
}