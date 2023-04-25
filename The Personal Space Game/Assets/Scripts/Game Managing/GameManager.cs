using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int remaining;

    public GameObject gameOverScreen;
    public GameObject gameScreen;

    public Transform[] winPos;

    public TextMeshPro HPCounter;

    public TextMeshProUGUI[] scoreCounter;
    public TextMeshProUGUI ammoCounter;
    public Text[] dayCounter;
    public Text remainingCounter;
    public Text[] resultsTXT;

    //public Image handIMG;
    public Image soapAmmoIMG;
    //public Image[] soapSwitchIMG;

    public Sprite[] soapSprite1;
    public Sprite[] soapSprite2;

    public Vector2 offset;

    public Texture2D[] skinTex;

    //public Joystick joystick;

    public AudioClip[] deathSFX;
    AudioSource audio;

    public SpawnManager spawnManager;
    public WaveShopSystem waveShopSystem;
    public Shooter shooter;
    public PersonalSpace personalSpace;
    public PlayerMovement player;
    Database database;
    MenuMusic music;

    void Start()
    {
        database = FindObjectOfType<Database>();
        music = FindObjectOfType<MenuMusic>();
        audio = GetComponent<AudioSource>();
        music.PlayGameMusic();
        database.one = 1;

        //handIMG.sprite = shooter.handUp[database.skin];

        for (int i = 0; i < player.skinSpriteAnim.Length; i++)
        {
            switch (database.skin)
            {
                case 0:
                    player.skinSpriteAnim[i].sprite = player.skinSprite1[i];
                    break;
                case 1:
                    player.skinSpriteAnim[i].sprite = player.skinSprite2[i];
                    break;
                case 2:
                    player.skinSpriteAnim[i].sprite = player.skinSprite3[i];
                    break;
            }
        }

        personalSpace.outline.SetTexture("MainTex_", skinTex[database.skin]);
    }

    void Update()
    {
        remainingCounter.text = remaining + " Remaining";

        if (player)
        {
            HPCounter.text = "" + player.HP;
            HPCounter.transform.position = new Vector2(player.transform.position.x + offset.x,
                                                       player.transform.position.y + offset.y);
        }

        if (player.HP <= 0 && database.day > 0 && player)
            GameOver();

        if (shooter.currentType == 0)
            ammoCounter.text = "∞";
        else
            ammoCounter.text = "" + shooter.currentAmmo[shooter.currentType];

        if (shooter.pump)
            soapAmmoIMG.sprite = soapSprite2[shooter.currentType];
        else
            soapAmmoIMG.sprite = soapSprite1[shooter.currentType];

        /*if (shooter.currentType == 0)
        {
            soapSwitchIMG[0].sprite = shooter.soapSprite1[shooter.type[0]];

            if (shooter.upMode)
                soapSwitchIMG[1].sprite = shooter.soapSprite1[shooter.type[2]];
            else
                soapSwitchIMG[1].sprite = shooter.soapSprite1[shooter.type[1]];
        }
        else
        {
            soapSwitchIMG[1].sprite = shooter.soapSprite1[shooter.type[0]];

            if (shooter.upMode)
                soapSwitchIMG[0].sprite = shooter.soapSprite1[shooter.type[2]];
            else
                soapSwitchIMG[0].sprite = shooter.soapSprite1[shooter.type[1]];
        }*/

        for (int i = 0; i < dayCounter.Length; i++)
            dayCounter[i].text = "DAY " + database.day;

        for (int i = 0; i < scoreCounter.Length; i++)
            scoreCounter[i].text = database.currentPaper.ToString();
    }

    void GameOver()
    {
        spawnManager.ready = false;

        resultsTXT[0].text = (database.day - 1).ToString();
        resultsTXT[1].text = (database.cleaned[0] + database.cleaned[1] + database.cleaned[2]).ToString();
        resultsTXT[2].text = database.paper.ToString();

        spawnManager.gameObject.SetActive(false);
        //spawnManager.buttonsGUI.SetActive(false);
        spawnManager.shopGUI.SetActive(false);
        HPCounter.gameObject.SetActive(false);
        dayCounter[0].gameObject.SetActive(false);
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);

        music.GetComponent<AudioSource>().Pause();

        SetDatabase();

        //Destroy(spawnManager.joystickGUI);
        Destroy(player.gameObject);
        audio.clip = deathSFX[Random.Range(0, deathSFX.Length - 1)];
        audio.Play();

        Restart();
    }

    public void SetDatabase()
    {
        for (int i = 0; i < database.maxCleaned.Length; i++)
        {
            database.maxCleaned[i] += database.cleaned[i];
            database.shopCleaned[i] += database.cleaned[i];
        }

        database.SetMaxCleaned();
        database.SetShopCleaned();
        database.SetBest();
        database.SetPaper();
        database.SetPumps();

        //database.GPGS.OpenSave(true);
    }

    public void Restart()
    {
        database.day = 1;
        database.paper = 0;
        database.currentPaper = 0;
    }

    public void Pause()
    {
        music.GetComponent<AudioSource>().Pause();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        music.GetComponent<AudioSource>().UnPause();
        Time.timeScale = 1;
    }

    public void ShootBTN()
    {
        shooter.Shoot();
    }

    public void NoShootBTN()
    {
        shooter.NoShoot();
    }

    public void SwitchBTN()
    {
        shooter.SwitchSoap();
    }
}