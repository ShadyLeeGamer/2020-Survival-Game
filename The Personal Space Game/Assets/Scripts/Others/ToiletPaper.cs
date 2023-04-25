using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaper : MonoBehaviour
{
    public bool forEnemy;
    bool collected;

    public int addAmount;

    public GameObject ToiletPaperEFX;

    Database database;
    SpawnManager spawnManager;

    void Awake()
    {
        database = FindObjectOfType<Database>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        if (!spawnManager.ready)
            collected = true;

        if (collected)
            AddToScore();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            collected = true;
        if (other.tag == "Enemy" && other.GetComponent<EnemyMovement>().HP > 0 && forEnemy)
            Destroy(gameObject);
    }

    void AddToScore()
    {
        float randomAmount = Random.value;
        float fixedAmount = addAmount;

        if (randomAmount > 0 && randomAmount <= database.luckyChance && database.skin == 2)
            fixedAmount = addAmount + 1;

        if (fixedAmount > 0)
        {
            if (spawnManager.gameObject)
            {
                database.paper++;
                database.currentPaper++;
                addAmount--;
            }
        }
        else
        {
            Instantiate(ToiletPaperEFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
