using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{

    public static CoinsManager instance;

    public delegate void CoinsChangeHandler(int amount);
    public event CoinsChangeHandler OnCoinsChange;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // public TMP_Text CurrentLevel;
    public CoinsStats coinsStats;
    public SaveData saveData;

    public TMP_Text coinsToClaimText;


    private void FixedUpdate()
    {
        // Get the current XP value from xpStats and convert it to a string
        string coinValueText = coinsStats.CurrentCoins.ToString();

        // Concatenate "XP" with the XP value string
        string formattedCurrentCoinsText = "Coins " + coinValueText;

    }

    public void UpdateExperienceAmount(float Coins)
    {
        // Check if valueToCheck is between minValue and maxValue
        if (Coins > 0 && Coins < 20)
        {
            coinsToClaimText.text = 20.ToString() + " Coins";
            saveData.AdjustMoney(20);
            Debug.Log("Gave 20 Coins");
        }
        else if (Coins > 20 && Coins < 40)
        {
            coinsToClaimText.text = 40.ToString() + " Coins";
            saveData.AdjustMoney(40);
            Debug.Log("Gave 40 Coins");
        }
        else if (Coins > 40 && Coins < 60)
        {
            coinsToClaimText.text = 160.ToString() + " Coins";
            saveData.AdjustMoney(160);
            Debug.Log("Gave 160 Coins");
        }
        else if (Coins > 60 && Coins < 80)
        {
            coinsToClaimText.text = 280.ToString() + " Coins";
            saveData.AdjustMoney(280);
            Debug.Log("Gave 280 Coins");
        }
        else if (Coins > 80 && Coins < 100)
        {
            coinsToClaimText.text = 400.ToString() + " Coins";
            saveData.AdjustMoney(400);
            Debug.Log("Gave 400 Coins");
        }
    }

    


    
}
