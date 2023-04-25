using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public bool coronaMode;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool menu;

    [HideInInspector] public PlayerMovement player;

    public bool isHit;
    bool quit;

    public int type;
    public int HP;
    public int dropRate;
    int playerHeight;
    int one = 1;
    int maxHP;

    public float moveSpeed;
    public float jumpRate;
    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float stopDist;
    public float jumpDist;
    public float jumpPos;
    public float flashTime;
    float nextTimeToJump;
    float nextTimeToKB;

    public GameObject coronaImg;
    public GameObject drop;
    
    public Transform quitPos;
    public Transform GUI;
    public Transform target;
    public Transform dropPos;

    public Rigidbody2D rb;

    public BoxCollider2D boxCollider;

    public Animator animator;
    public RuntimeAnimatorController[] menuAnimator;

    public SpriteRenderer spriteRenderer;

    public Color enemyDefault;
    public Color enemyHighlight;
    public Color enemyQuitCol;

    public Vector2 GUIOffset;
    public Vector2 knockback;

    public Slider slider;

    public TargetPriority targetPriority;
    public EnemyShooter enemyShooter;
    GameManager gameManager;
    SpawnManager spawnManager;
    Database database;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();
        spawnManager = FindObjectOfType<SpawnManager>();
        database = FindObjectOfType<Database>();

        maxHP = HP;
        slider.maxValue = HP;

        if (menu)
        {
            animator.runtimeAnimatorController = menuAnimator[Random.Range(0, menuAnimator.Length)];

            int facing = Random.Range(0, 2);

            if (facing == 0)
                transform.eulerAngles = new Vector2(0, 180);
            else
                transform.eulerAngles = new Vector2(0, 0);
        }

        if (type == 3)
            animator.SetBool("Shopping", true);
    }

    void Update()
    {
        if (!menu)
        {
            if (player && HP > 0 && player.HP > 0)
            {
                if (type != 3)
                {
                    if (targetPriority)
                        targetPriority.PriorityManager();
                    else if (!targetPriority)
                        target = player.transform;
                }
            }
            else
                quit = true;
        }
        if (quit)
            Quit();

        if (target)
        {
            if (target.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector2(0, 0);
                GUI.eulerAngles = new Vector2(0, 180);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
                GUI.eulerAngles = new Vector2(0, 180);
            }

            if (target.position.y > transform.position.y + jumpPos && isGrounded
                                                                   && rb.velocity.y == 0
                                                                   && Vector2.Distance(transform.position, target.position) < jumpDist
                                                                   && playerHeight > 0)
                Jump();

            Movement();
        }

        SetHealth();

        if (coronaMode)
        {
            enemyDefault = enemyHighlight;
            slider.maxValue = maxHP + 50;
            slider.transform.localScale = new Vector2(1, .75f);
            coronaImg.transform.localScale = new Vector2(.75f, 1);
        }
        else
            enemyDefault = Color.white;

        if (HP <= maxHP && HP > 0)
        {
            coronaMode = false;
            enemyDefault = Color.white;
            slider.maxValue = maxHP;
            slider.transform.localScale = new Vector2(.75f, .75f);
            coronaImg.transform.localScale = Vector2.one;
        }

        if (isHit)
        {
            if (HP > 0)
                StartCoroutine(Flash());

            if (type != 3 && isGrounded)
            {
                if (Time.time >= nextTimeToKB)
                {
                    boxCollider.isTrigger = true;
                    nextTimeToKB = Time.time + 1f / 2;

                    if (transform.eulerAngles == new Vector3(0, 0, 0))
                        rb.AddForce(new Vector2(-knockback.x, knockback.y), ForceMode2D.Impulse);
                    else if (transform.eulerAngles == new Vector3(0, 180, 0))
                        rb.AddForce(new Vector2(knockback.x, knockback.y), ForceMode2D.Impulse);
                }
            }

            isHit = false;
        }
    }

    void FixedUpdate()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y < 0)
            {
                boxCollider.isTrigger = true;

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
            boxCollider.isTrigger = false;

            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }

        if (isJumping)
        {
            if (Time.time >= nextTimeToJump)
            {
                nextTimeToJump = Time.time + 1f / jumpRate;

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
                isJumping = false;
        }
        if (target)
        {
            if (target.position.y > transform.position.y + (jumpPos * 2))
            {
                playerHeight = 2;
                rb.gravityScale = fallMultiplier;
            }
            else if (target.position.y > transform.position.y + jumpPos)
            {
                playerHeight = 1;
                rb.gravityScale = lowJumpMultiplier;
            }
            else
                rb.gravityScale = 5f;
        }
    }

    void Movement()
    {
        if (Vector2.Distance(transform.position, target.position) > stopDist ||
                          target.position.y > transform.position.y + jumpPos ||
                          target.position.y < transform.position.y - jumpPos)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                             new Vector2(target.position.x, transform.position.y),
                             moveSpeed * Time.deltaTime);

            if (type != 3)
                animator.SetFloat("Movement", 1);

            if (targetPriority)
            {
                animator.SetBool("Sneezing", false);
                stopDist = 2;
            }
            else if(enemyShooter)
                animator.SetBool("Coughing", false);
            else if(type == 4)
            {
                stopDist = 1.25f;
                animator.SetBool("Hugging", false);
            }
        }
        else if (isGrounded)
        {
            if (type != 3)
                animator.SetFloat("Movement", -1);

            if (enemyShooter)
            {
                enemyShooter.Shoot();

                if (targetPriority)
                    stopDist = 5;
            }
            if (type == 4)
            {
                stopDist = 2f;
                animator.SetBool("Hugging", true);
            }
        }
    }

    IEnumerator Flash()
    {
        spriteRenderer.color = enemyQuitCol;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = enemyDefault;
    }

    void Jump()
    {
        isJumping = true;
    }

    void SetHealth()
    {
        slider.value = HP;

        GUI.position = new Vector2(transform.position.x + GUIOffset.x, transform.position.y + GUIOffset.y);
    }

    void Quit()
    {
        gameObject.layer = 15;

        stopDist = 0;
        moveSpeed = 10;
        jumpForce = 14;
        jumpRate = 10;

        if (HP < 1)
        {
            spriteRenderer.color = enemyQuitCol;

            if (dropRate > 0)
            {
                Instantiate(drop, dropPos.position, dropPos.rotation);
                dropRate--;
            }
            if (one == 1)
            {
                target = quitPos;
                gameManager.remaining--;
                database.cleaned[type]++;
                coronaImg.SetActive(false);

                if (type == 3)
                {
                    animator.SetBool("Shopping", false);
                    animator.SetFloat("Movement", 1);
                }
                one--;
            }
        }
        else if (one == 1)
        {
            target = spawnManager.winPos[Random.Range(0, 2)].transform;
            one--;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "QuitPosition" || other.tag == "WinPosition")
        {
            if (menu)
                Destroy(gameObject);
            else
            {
                if (HP < 1 || !player || type == 3)
                {
                    Destroy(gameObject);

                    if (type == 3 && HP > 0 && one == 1)
                    {
                        gameManager.remaining--;
                        one--;
                    }
                }
            }
        }
    }
}