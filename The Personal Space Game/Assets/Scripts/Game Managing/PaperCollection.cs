using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaperCollection : MonoBehaviour
{
    public int spawnAmount;
    public int currentPaper;

    public float maxSpeed;
    public float speedInc;
    public float speed;
    float paperTimer = 1;

    public GameObject paper;

    public Vector2 offset;

    public Color lockedCol;

    public TextMeshProUGUI paperCounter;

    public Image[] achievementIcon;

    Database database;

    void Start()
    {
        database = FindObjectOfType<Database>();

        spawnAmount = PlayerPrefs.GetInt("Paper", 0);
    }

    void Update()
    {
        paperCounter.text = currentPaper.ToString();

        paperTimer -= speed * Time.deltaTime;

        if (paperTimer <= 0 && spawnAmount > 0)
            SpawnManagement();

        if (speed < maxSpeed)
            speed += speedInc * Time.deltaTime;

        for(int i = 0; i < database.unlocked.Length; i++)
        {
            if (database.unlocked[i] == 1)
                achievementIcon[i].color = Color.white;
            else
                achievementIcon[i].color = lockedCol;
        }
    }

    void SpawnManagement()
    {
        Instantiate(paper, new Vector2(Random.Range(-offset.x, offset.x), offset.y), Quaternion.Euler(0, 0, Random.Range(-75, 75)));
        spawnAmount--;
        paperTimer = 1;
    }
}
