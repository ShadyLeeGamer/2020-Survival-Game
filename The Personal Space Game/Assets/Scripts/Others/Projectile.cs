using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public bool shopProj;

    public int damage;
    public int type;

    public float speed;

    public GameObject hitEFX;

    public AudioClip splatSFX;
    public AudioClip hitSFX;

    Rigidbody2D rb;

    SpawnManager spawnManager;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();

        rb = GetComponent<Rigidbody2D>();

        rb.transform.position = transform.position + transform.transform.forward;

        rb.velocity = transform.transform.right * speed;
    }

    void Update()
    {
        Vector2 dir = rb.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (spawnManager != null && !spawnManager.ready && !shopProj && type != 3)
            Splash(splatSFX);
    }

    void Splash(AudioClip SFX)
    {
        hitEFX.GetComponent<AudioSource>().clip = SFX;
        GameObject hitEFX_ = Instantiate(hitEFX, transform.position, Quaternion.identity) as GameObject;

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyMovement enemy = other.GetComponent<EnemyMovement>();
        if (other.transform.tag == "Ground" && type != 3)
            Splash(splatSFX);

        if (type == 1)
        {
            if (other.transform.tag == "Enemy" && enemy.HP > 0 ||
                other.transform.tag == "EnemyStart" && enemy.HP > 0)
            {
                enemy.HP -= damage;

                Splash(hitSFX);

                enemy.isHit = true;
            }
        }

        if (type == 3)
        {
            if (other.transform.tag == "Enemy" && enemy.HP > 0)
            {
                if (!enemy.coronaMode && enemy.type != 2)
                {
                    enemy.coronaMode = true;

                    /*enemy.moveSpeed *= 1.25f;
                    enemy.jumpForce *= 1.25f;
                    enemy.fireRate *= 1.25f;*/
                    enemy.HP = 150;

                    enemy.coronaImg.GetComponent<Image>().color = Color.red;
                    enemy.spriteRenderer.color = enemy.enemyHighlight;

                    // Destroy(gameObject);
                }
                //GameObject hitEFX_ = Instantiate(hitEFX, transform.position, Quaternion.identity) as GameObject;
            }
        }
    }
}