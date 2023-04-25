using UnityEngine;
//using GooglePlayGames;

public class Database : MonoBehaviour
{
    public int one = 1;
    public int[] unlocked;
    public int[] pumps;
    public int[] cleaned;
    public int[] maxCleaned;
    public int[] shopCleaned;
    public int day;
    public int skin;
    public int maxPaper;
    public int currentPaper;
    public int paper;

    public float luckyChance;
    public float shrinkBuff;
    public float growBuff;

    public Sprite[] soapSprite1;
    public Sprite[] soapSprite2;
    public Sprite skinSprite;

    static Database instance;

    void Awake()
    {
        maxPaper = PlayerPrefs.GetInt("Paper", 0);

        for (int i = 0; i < unlocked.Length; i++)
            unlocked[i] = PlayerPrefs.GetInt("Unlocked" + i, 0);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (maxPaper + paper >= 20 && unlocked[0] == 0)
        {
            //GPGS.UnlockToiletPaperAmateur();
            PlayerPrefs.SetInt("Unlocked" + 0, unlocked[0]);
            unlocked[0] = 1;
        }
        if (maxPaper + paper >= 100 && unlocked[1] == 0)
        {
            //GPGS.UnlockToiletPaperPro();
            PlayerPrefs.SetInt("Unlocked" + 1, unlocked[1]);
            unlocked[1] = 1;
        }
        if (maxPaper + paper >= 300 && unlocked[2] == 0)
        {
            //GPGS.UnlockToiletPaperMaster();
            PlayerPrefs.SetInt("Unlocked" + 2, unlocked[2]);
            unlocked[2] = 1;
        }
        if (maxPaper + paper >= 700 && unlocked[3] == 0)
        {
            //GPGS.UnlockToiletPaperChampion();
            PlayerPrefs.SetInt("Unlocked" + 3, unlocked[3]);
            unlocked[3] = 1;
        }
        if (maxPaper + paper >= 1000 && unlocked[4] == 0)
        {
            //GPGS.UnlockToiletPaperLegend();
            PlayerPrefs.SetInt("Unlocked" + 4, unlocked[4]);
            unlocked[4] = 1;
        }
    }

    public void SetPaper()
    {
        maxPaper += paper;
        PlayerPrefs.SetInt("Paper", maxPaper);
    }

    public void SetPumps()
    {
        for (int i = 0; i < pumps.Length; i++)
            PlayerPrefs.SetInt("Pumps" + i, pumps[i]);
    }

    public void SetMaxCleaned()
    {
        for (int i = 0; i < maxCleaned.Length; i++)
            PlayerPrefs.SetInt("MaxCleaned" + i, maxCleaned[i]);
    }

    public void SetShopCleaned()
    {
        for (int i = 0; i < shopCleaned.Length; i++)
            PlayerPrefs.SetInt("ShopCleaned" + i, shopCleaned[i]);
    }

    public void SetBest()
    {
        if (day - 1 > PlayerPrefs.GetInt("BestDay"))
        {
            PlayerPrefs.SetInt("BestDay", day - 1);
            //GPGS.UpdateLeaderboardScore(day - 1);
        }
    }

    /*public void CheckUserAuthentication(int type)
    {
        switch(type)
        {
            case 0:
                if (PlayGamesPlatform.Instance.IsAuthenticated())
                    GPGS.OpenLeaderboardsUI();
                else
                    GPGS.SignIn();
                    break;
            case 1:
                if (PlayGamesPlatform.Instance.IsAuthenticated())
                    GPGS.OpenAchievementsPanel();
                else
                    GPGS.SignIn();
                break;
        }
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }*/
}
