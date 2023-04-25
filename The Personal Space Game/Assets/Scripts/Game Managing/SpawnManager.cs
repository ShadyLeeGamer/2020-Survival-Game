using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool ready;

    public int spawnAmount;
    public int currentSpawnAmount;
    public int spawnInc;

    public float speed;
    public float speedInc;
    public float dropSpeed;
    public float[] enemyChance;
    public float[] dropChance;
    float enemyTimer = 1;
    float dropTimer = 1;

    public GameObject shopGUI;
    //public GameObject joystickGUI;
    //public GameObject joystickGUIPrefab;
    //public GameObject buttonsGUI;
    public GameObject gameGUI;
    public GameObject[] enemy;
    public GameObject[] drop;
    public GameObject[] startPos;
    public GameObject[] quitPos;
    public GameObject[] winPos;

    public GameManager gameManager;
    public WaveShopSystem waveShopSystem;
    public PlayerMovement player;
    public Database database;

    public Vector2 paperOffset;

    void Awake()
    {
        startPos = GameObject.FindGameObjectsWithTag("StartPosition");
        quitPos = GameObject.FindGameObjectsWithTag("QuitPosition");
        winPos = GameObject.FindGameObjectsWithTag("WinPosition");

        database = FindObjectOfType<Database>();

        currentSpawnAmount = spawnAmount;
        gameManager.remaining = spawnAmount;
    }

    void Update()
    {
        enemyTimer -= speed * Time.deltaTime;
        dropTimer -= dropSpeed * Time.deltaTime;

        if (currentSpawnAmount > 0 && ready)
        {
            if (enemyTimer <= 0)
                SpawnManagement();
            if (dropTimer <= 0)
                SpawnDrop();
        }

        if (gameManager.remaining < 1 && player)
        {
            ready = false;
            player.canMove = false;

            waveShopSystem.gameObject.SetActive(true);

            //Destroy(joystickGUI);
            shopGUI.SetActive(true);
            gameGUI.SetActive(false);
            //buttonsGUI.SetActive(false);

            gameManager.NoShootBTN();

            if (player.isGrounded)
                player.rb.velocity = new Vector2(0, player.rb.velocity.y);
        }
    }

    void SpawnManagement()
    {
        switch (database.day)
        {
            case int n when n < 3:
                SpawnEnemy(enemy[0]);
                break;
            case 3:
                SpawnEnemy(enemy[1]);
                break;
            case 4:
                SpawnEnemy(enemy[Random.Range(0, 2)]);
                break;
            case 5:
                dropChance[0] = .8f;
                dropChance[1] = .2f;
                SpawnEnemy(enemy[2]);
                break;
            case 6:
                dropChance[0] = .85f;
                dropChance[1] = .15f;
                SpawnEnemy(enemy[Random.Range(0, 3)]);
                break;
            case 7:
                dropChance[0] = .9f;
                dropChance[1] = .1f;
                SpawnEnemy(enemy[Random.Range(2, 4)]);
                break;
            case 8:
                SpawnEnemy(enemy[Random.Range(0, 4)]);
                break;
            case int n when n > 8:
                SpawnEnemy(enemy[Random.Range(0, 15)]);
                break;
        }
        currentSpawnAmount--;
    }

    void SpawnEnemy(GameObject enemy)
    {
        int house = Random.Range(0, startPos.Length);

        if (enemy.GetComponent<EnemyMovement>().type != 3)
        {
            GameObject newEnemy = Instantiate(enemy, startPos[house].transform.position, Quaternion.identity) as GameObject;

            newEnemy.GetComponent<EnemyMovement>().quitPos = quitPos[house].transform;
        }
        else
        {
            float direction = Random.Range(0, 2);

            if (direction == 1)
            {
                GameObject newEnemy = Instantiate(enemy, new Vector2(winPos[0].transform.position.x + 4, 0), Quaternion.identity) as GameObject;
                newEnemy.GetComponent<EnemyMovement>().quitPos = quitPos[house].transform;
                newEnemy.GetComponent<EnemyMovement>().target = winPos[1].transform;
            }
            else
            {
                GameObject newEnemy = Instantiate(enemy, new Vector2(winPos[1].transform.position.x - 4, 0), Quaternion.identity) as GameObject;
                newEnemy.GetComponent<EnemyMovement>().quitPos = quitPos[house].transform;
                newEnemy.GetComponent<EnemyMovement>().target = winPos[0].transform;
            }
        }
        enemyTimer = 1;
    }

    /*void RandomEnemy()
    {
        float randomEnemy = Random.value;

        switch (randomEnemy)
        {
            case float n when n > enemyChance[1] &&
                              n <= enemyChance[0]:
                SpawnEnemy(enemy[0]);
                break;
            case float n when n > enemyChance[2] &&
                              n <= enemyChance[1]:
                SpawnEnemy(enemy[1]);
                break;
            case float n when n > enemyChance[3] &&
                              n <= enemyChance[2]:
                SpawnEnemy(enemy[2]);
                break;
            case float n when n > 0 &&
                              n <= enemyChance[3]:
                SpawnEnemy(enemy[3]);
                break;
        }
    }*/

    void SpawnDrop()
    {
        float randomDrop = Random.value;
        GameObject pickedDrop = drop[0];

        switch (randomDrop)
        {
            case float n when n > dropChance[1] &&
                              n <= dropChance[0]:
                pickedDrop = drop[0];
                break;
            case float n when n > 0 &&
                              n <= dropChance[1]:
                pickedDrop = drop[1];
                break;
        }

        Instantiate(pickedDrop, new Vector2(Random.Range(-paperOffset.x, paperOffset.x), paperOffset.y), Quaternion.identity);
        dropTimer = 1;
    }

    public void Ready()
    {
        ready = true;

        shopGUI.SetActive(false);
        //GameObject newJoystickGUI = Instantiate(joystickGUIPrefab, joystickGUIPrefab.transform.position, Quaternion.identity);
        //joystickGUI = newJoystickGUI;
        gameGUI.SetActive(true);
        //buttonsGUI.SetActive(true);

        spawnAmount += spawnInc;
        gameManager.remaining = spawnAmount;
        currentSpawnAmount = spawnAmount;

        speed += speedInc;

        database.day++;
        enemyTimer = 1;
        dropTimer = 1;

        player.canMove = true;
        //player.joystick = FindObjectOfType<Joystick>();
    }
}