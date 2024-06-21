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
    /*
    public void UpdateExperienceAmount(float percent)
    {
        // Check if valueToCheck is between minValue and maxValue
        if (percent >= 0 && percent < 20)
        {
            coinsToClaimText.text = 20.ToString();
            saveData.AdjustMoney(20);
            Debug.Log("Gave 20 Coins");
        }
        else if (percent > 20 && percent < 40)
        {
            coinsToClaimText.text = 40.ToString();
            saveData.AdjustMoney(40);
            Debug.Log("Gave 40 Coins");
        }
        else if (percent > 40 && percent < 60)
        {
            coinsToClaimText.text = 160.ToString();
            saveData.AdjustMoney(160);
            Debug.Log("Gave 160 Coins");
        }
        else if (percent > 60 && percent < 80)
        {
            coinsToClaimText.text = 280.ToString();
            saveData.AdjustMoney(280);
            Debug.Log("Gave 280 Coins");
        }
        else if (percent > 80 && percent < 100)
        {
            coinsToClaimText.text = 400.ToString();
            saveData.AdjustMoney(400);
            Debug.Log("Gave 400 Coins");
        }
    }
    */
    public void UpdateExperienceAmount(float percent)
    {
        int caseSwitch = (int)percent / 20;

        switch (caseSwitch)
        {
            case 0:
            case 1:
                coinsToClaimText.text = 20.ToString();
                saveData.AdjustMoney(20);
                Debug.Log("Gave 20 Coins");
                break;
            case 2:
                coinsToClaimText.text = 40.ToString();
                saveData.AdjustMoney(40);
                Debug.Log("Gave 40 Coins");
                break;
            case 3:
                coinsToClaimText.text = 160.ToString();
                saveData.AdjustMoney(160);
                Debug.Log("Gave 160 Coins");
                break;
            case 4:
                coinsToClaimText.text = 280.ToString();
                saveData.AdjustMoney(280);
                Debug.Log("Gave 280 Coins");
                break;
            case 5:
                coinsToClaimText.text = 400.ToString();
                saveData.AdjustMoney(400);
                Debug.Log("Gave 400 Coins");
                break;
            case 6: // 120 > percent >= 100
                coinsToClaimText.text = 280.ToString();
                saveData.AdjustMoney(280);
                Debug.Log("Gave 500 Coins");
                break;
            case 7: // 140 > percent >= 120
                coinsToClaimText.text = 160.ToString();
                saveData.AdjustMoney(160);
                Debug.Log("Gave 600 Coins");
                break;
                case 8: // 160 > percent >= 140
                coinsToClaimText.text = 40.ToString();
                saveData.AdjustMoney(40);
                Debug.Log("Gave 700 Coins");
                break;
            case 9: // 180 > percent >= 160
                coinsToClaimText.text = 20.ToString();
                saveData.AdjustMoney(20);
                Debug.Log("Gave 800 Coins");
                break;
                
            default:
                Debug.Log("Invalid percentage");
                break;
        }
    }

    


    
}
