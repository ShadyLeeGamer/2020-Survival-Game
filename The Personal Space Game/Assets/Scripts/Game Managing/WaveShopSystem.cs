using UnityEngine;
using UnityEngine.UI;

public class WaveShopSystem : MonoBehaviour
{
    public int[] itemCost;
    public int[] ammoCost;
    public int healCost;
    public int[] addAmmo;

    public GameObject healBTN;
    public GameObject fullHPTXT;
    public GameObject[] ammoPanel;
    public GameObject[] itemBTN;

    public RectTransform personalSpaceIMG;

    public Text[] ammoTXT;

    public Image healIMG;

    public AudioSource audioSource;

    public PlayerMovement player;
    public Shooter shooter;
    Database database;

    void Start()
    {
        database = FindObjectOfType<Database>();
        healIMG.sprite = database.skinSprite;
    }

    void Update()
    {
        personalSpaceIMG.sizeDelta = new Vector2(player.HP, player.HP);

        if (player.HP == 5)
        {
            healBTN.SetActive(false);
            fullHPTXT.SetActive(true);
        }
        else
        {
            healBTN.SetActive(true);
            fullHPTXT.SetActive(false);
        }

        for (int i = 0; i < ammoTXT.Length; i++)
            ammoTXT[i].text = shooter.currentAmmo[i + 1] + " + " + addAmmo[i];
    }

    public void Heal()
    {
        if (database.currentPaper >= healCost)
        {
            database.currentPaper -= healCost;
            audioSource.Play();

            player.HP = 5;
        }
    }

    public void GetItem(int item)
    {
        if (database.currentPaper >= itemCost[item])
        {
            database.currentPaper -= itemCost[item];
            audioSource.Play();

            Destroy(itemBTN[item]);
            ammoPanel[item].SetActive(true);
        }
    }

    public void EquipItem(int item)
    {
        shooter.type[1] = item;
    }

    public void GetAmmo(int item)
    {
        if (database.currentPaper >= ammoCost[item])
        {
            database.currentPaper -= ammoCost[item];
            audioSource.Play();

            shooter.currentAmmo[item + 1] += addAmmo[item];
        }
    }
}
