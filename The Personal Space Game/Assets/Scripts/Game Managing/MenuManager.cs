using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public bool mute;

    public int skin;
    public int spawnAmount;

    public GameObject enemyStart;
    public GameObject[] enemy;

    public Transform[] quitPos;

    public Text volumePercent;
    public Text[] maxCleanedCounter;
    public Text[] shopCleanedCounter;
    public Text[] pumpsCounter;
    public TextMeshProUGUI bestDayCounter;

    public Image muteIMG;

    public Sprite[] muteSprites;
    public Sprite[] skinSprite;

    public Vector2 enemyStartOffset;

    public Database database;
    public ShopSystem shopSystem;
    public SceneManaging sceneManaging;

    public AudioMixer audioMixer;

    void Start()
    {
        enemy = GameObject.FindGameObjectsWithTag("EnemyStart");

        bestDayCounter.text = "BEST: " + PlayerPrefs.GetInt("BestDay");

        for (int i = 0; i < pumpsCounter.Length; i++)
        {
            database.pumps[i] = PlayerPrefs.GetInt("Pumps" + i, 0);
            pumpsCounter[i].text = database.pumps[i] + " PUMPS";
        }
        for (int i = 0; i < maxCleanedCounter.Length; i++)
        {
            database.maxCleaned[i] = PlayerPrefs.GetInt("MaxCleaned" + i, 0);
            maxCleanedCounter[i].text = database.maxCleaned[i] + " CLEANED";
        }
        for (int i = 0; i < shopCleanedCounter.Length; i++)
        {
            database.shopCleaned[i] = PlayerPrefs.GetInt("ShopCleaned" + i, 0);
            shopCleanedCounter[i].text = database.shopCleaned[i].ToString();
        }
        for (int i = 0; i < shopSystem.key.Length; i++)
            shopSystem.key[i] = PlayerPrefs.GetInt("Skin" + i, 0);

        for (int i = 0; i < shopSystem.key.Length; i++)
        {
            if (shopSystem.key[i] == 1)
                shopSystem.UnlockSkin(i);
        }
    }

    void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag("EnemyStart");

        if (Input.GetKeyDown(KeyCode.E))
            ResetData();

        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].GetComponent<EnemyMovement>().HP < 1)
            {
                //database.cleaned[enemy[i].GetComponent<EnemyMovement>().type]++;
                enemy[i].GetComponent<SpriteRenderer>().color =
                enemy[i].GetComponent<EnemyMovement>().enemyQuitCol;
                EnemyEnd(i);
            }
        }

        if (spawnAmount > 0)
        {
            Instantiate(enemyStart, new Vector2(Random.Range(-enemyStartOffset.x,
                                                              enemyStartOffset.x),
                                                              enemyStartOffset.y), Quaternion.identity);
            spawnAmount--;
        }
    }

    public void Quarantine()
    {
        database.skin = skin;
        database.skinSprite = skinSprite[skin];

        for (int i = 0; i < enemy.Length; i++)
            EnemyEnd(i);

        sceneManaging.LoadScene("GAME");
    }

    void EnemyEnd(int i_)
    {
        if (!enemy[i_].GetComponent<EnemyMovement>().target)
        {
            DontDestroyOnLoad(enemy[i_]);
            enemy[i_].GetComponent<EnemyMovement>().target = quitPos[Random.Range(0, 3)];
            enemy[i_].layer = 15;
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        volumePercent.text = Mathf.Round(volume + 80) + "%";
    }

    public void MuteBTN()
    {
        if (mute)
        {
            audioMixer.SetFloat("volume", 0);
            muteIMG.sprite = muteSprites[0];
            mute = false;
        }
        else
        {
            audioMixer.SetFloat("volume", -80);
            muteIMG.sprite = muteSprites[1];
            mute = true;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    void ResetData()
    {
        PlayerPrefs.DeleteAll();
        bestDayCounter.text = "BEST: 0";

        for (int i = 0; i < pumpsCounter.Length; i++)
            pumpsCounter[i].text = "0 PUMPS";
        for (int i = 0; i < maxCleanedCounter.Length; i++)
            maxCleanedCounter[i].text = "0 CLEANED";
        for (int i = 0; i < shopCleanedCounter.Length; i++)
            shopCleanedCounter[i].text = "0";
        for (int i = 0; i < shopSystem.key.Length; i++)
            shopSystem.key[i] = 0;

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}