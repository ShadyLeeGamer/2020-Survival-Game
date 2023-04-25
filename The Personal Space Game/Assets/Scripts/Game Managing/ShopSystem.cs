using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    [HideInInspector] public PlayerMovement player;
    [HideInInspector] public Shooter shooter;

    public int[] key;
    public int[] skinCost1;
    public int[] skinCost2;
    public int selectedSkin;
    int[] fixedCost;

    public GameObject unlockGUI;
    public GameObject failedGUI;
    public GameObject[] buyPanel;
    public GameObject[] skinBTN;

    public MenuManager menuManager;
    public Database database;

    public void CheckCosts(int item)
    {
        key[item] = 1;
        switch (item)
        {
            case 0:
                fixedCost = skinCost1;
                break;
            case 1:
                fixedCost = skinCost2;
                break;
        }
        for (int i = 0; i < database.shopCleaned.Length; i++)
        {
            if (database.shopCleaned[i] < fixedCost[i])
            {
                key[item] = 0;
                skinBTN[item].SetActive(true);
                buyPanel[item].SetActive(true);
                failedGUI.SetActive(true);
            }
        }
        if (key[item] == 1)
        {
            unlockGUI.SetActive(true);
            selectedSkin = item;
        }
    }

    public void GetSkin(int item)
    {
        item = selectedSkin;

        for (int i = 0; i < database.shopCleaned.Length; i++)
            database.shopCleaned[i] -= fixedCost[i];

        UnlockSkin(selectedSkin);
        database.SetShopCleaned();

        for (int i = 0; i < menuManager.shopCleanedCounter.Length; i++)
            menuManager.shopCleanedCounter[i].text = PlayerPrefs.GetInt("ShopCleaned" + i, 0).ToString();

        PlayerPrefs.SetInt("Skin" + selectedSkin, key[selectedSkin]);
        //database.GPGS.OpenSave(true);
    }

    public void UnlockSkin(int item)
    {
        Destroy(skinBTN[item]);
        Destroy(buyPanel[item]);
    }

    public void EquipSkin(int skin)
    {
        menuManager.skin = skin;
    }
}
