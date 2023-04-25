using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalSpace : MonoBehaviour
{
    bool shrink;
    bool isEmpty;

    public int noSpace;
    public int currentSpace;

    public float minSpace;
    public float maxSpace;
    float minSpace_;
    float maxSpace_;
    public float shrinkSpeed;
    public float growSpeed;
    public float[] space;

    public Transform safeSpace;
    public GameObject dangerSpace;

    public Material outline;

    public Color safeSpaceColA;
    public Color safeSpaceColB;
    public Color dangerSpaceCol;

    public PlayerMovement player;
    public GameManager gameManager;
    Database database;

    SpriteRenderer safeSpaceColour;

    void Start()
    {
        database = FindObjectOfType<Database>();

        player.HP = space.Length;

        GetComponent<CircleCollider2D>().radius = maxSpace / 2;

        safeSpaceColour = safeSpace.GetComponent<SpriteRenderer>();
        safeSpaceColour.color = safeSpaceColA;
        dangerSpace.GetComponent<SpriteRenderer>().color = dangerSpaceCol;

        minSpace_ = minSpace;
        maxSpace_ = maxSpace;

        outline.SetFloat("_Thickness", 0);
        outline.SetColor("_Color", safeSpaceColB);

        safeSpace.localScale = new Vector2(minSpace, minSpace);

        if (database.skin == 1)
            shrinkSpeed = database.shrinkBuff;
        else if (database.skin == 0)
            growSpeed = database.growBuff;
    }

    void Update()
    {
        currentSpace = player.HP - 1;
        maxSpace = space[currentSpace];

        PersonalSpaceSystem();

        dangerSpace.transform.localScale = new Vector2(space[currentSpace],
                                               space[currentSpace]);

        GetComponent<CircleCollider2D>().radius = space[currentSpace] / 2;

        if (player.HP < 2)
        {
            outline.SetFloat("_Thickness", 0.01f);
            safeSpaceColour.color = new Color(0, 0, 0, 0);
        }
        else
            outline.SetFloat("_Thickness", 0);

        if (player.invincibile)
            player.spriteRenderer.color = new Color(player.spriteRenderer.color.r,
                                             player.spriteRenderer.color.g,
                                             player.spriteRenderer.color.b, safeSpaceColB.a);
        else
            player.spriteRenderer.color = new Color(player.spriteRenderer.color.r,
                                             player.spriteRenderer.color.g,
                                             player.spriteRenderer.color.b, 255);
    }

    void FixedUpdate()
    {
        isEmpty = true;
    }

    void PersonalSpaceSystem()
    {
        if (!isEmpty)
        {
            if (player.HP > 1)
                dangerSpace.SetActive(true);
            else
            {
                outline.SetColor("_Color", dangerSpaceCol);
                dangerSpace.SetActive(false);
            }
            if (safeSpace.localScale.x <= minSpace)
            {
                player.invFrame = Time.time + 1f * player.invFrameDelay;

                player.isHit = true;

                minSpace_ = minSpace;
                player.HP--;

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
            safeSpaceColour.color = safeSpaceColA;

            if (player.HP < 2)
                outline.SetColor("_Color", safeSpaceColB);

            shrink = false;
        }

        if (shrink == true)
        {
            player.invincibile = false;

            safeSpace.localScale = new Vector2(maxSpace_ -= shrinkSpeed * Time.deltaTime,
                                               maxSpace_ -= shrinkSpeed * Time.deltaTime);
        }
        else
        {
            safeSpace.localScale = new Vector3(minSpace_ += growSpeed * Time.deltaTime,
                                               minSpace_ += growSpeed * Time.deltaTime);

            if (safeSpace.localScale.x < maxSpace)
                player.invincibile = true;
            else
                player.invincibile = false;
        }

        if (safeSpace.localScale.x >= maxSpace)
        {
            safeSpace.localScale = new Vector2(maxSpace, maxSpace);
            maxSpace_ = maxSpace;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<EnemyMovement>().HP > 0)
                AddToSpace(other);
            else
                RemoveFromSpace(other);
        }
        if (other.tag == "Projectile" && other.GetComponent<Projectile>().type != 1)
            AddToSpace(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<EnemyMovement>().HP > 0)
                RemoveFromSpace(other);
        }

        if (other.tag == "Projectile" && other.GetComponent<Projectile>().type != 1)
            RemoveFromSpace(other);
    }

    void AddToSpace(Collider2D other_)
    {
        safeSpaceColour.color = safeSpaceColB;

        if (other_.tag == "Enemy")
        {
            EnemyMovement enemy = other_.GetComponent<EnemyMovement>();
            if (enemy.spriteRenderer.color != enemy.enemyQuitCol)
                enemy.spriteRenderer.color = Color.red;
        }
        else
            other_.GetComponent<SpriteRenderer>().color = Color.red;

        isEmpty = false;
    }

    void RemoveFromSpace(Collider2D other_)
    {
        EnemyMovement enemy = other_.GetComponent<EnemyMovement>();
        safeSpaceColour.color = safeSpaceColA;

        if (other_.transform.tag == "Enemy")
        {
            if (enemy.HP > 0)
                enemy.spriteRenderer.color = enemy.enemyDefault;
        }
        else
            other_.GetComponent<SpriteRenderer>().color = Color.white;
    }
    /*    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile" && other.GetComponent<Projectile>().type == 2)
            AddToSpace(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        EnemyMovement enemy = other.GetComponent<EnemyMovement>();

        if (other.tag == "Enemy")
        {
            isEmpty = false;
            if (enemy.HP > 0)
            {
                if (enemy.inSpace < 1)
                {
                    AddToSpace(other);
                    enemy.inSpace++;
                }
            }
            else
            {
                if (enemy.inSpace >= 1)
                {
                    RemoveFromSpace(other);
                    enemy.inSpace = 0;
                }
            }
        }
        if (other.tag == "Projectile" && other.GetComponent<Projectile>().type == 2)
            isEmpty = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        EnemyMovement enemy = other.GetComponent<EnemyMovement>();

        if (other.tag == "Enemy")
        {
            if (enemy.HP > 0)
            {
                if (enemy.inSpace >= 1)
                {
                    RemoveFromSpace(other);
                    enemy.inSpace = 0;
                }
            }
        }

        if (other.tag == "Projectile" && other.GetComponent<Projectile>().type == 2)
            RemoveFromSpace(other);
    }

    void AddToSpace(Collider2D other_)
    {
        safeSpaceColour.color = safeSpaceColB;
        other_.GetComponent<SpriteRenderer>().color = Color.red;

        noSpace++;
    }

    void RemoveFromSpace(Collider2D other_)
    {
        safeSpaceColour.color = safeSpaceColA;

        if (other_.transform.tag == "Enemy")
            other_.GetComponent<SpriteRenderer>().color = other_.GetComponent<EnemyMovement>().enemyDefault;
        else
            other_.GetComponent<SpriteRenderer>().color = Color.white;

        noSpace--;
    }*/
}
