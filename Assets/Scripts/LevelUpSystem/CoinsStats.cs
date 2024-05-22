using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsStats : MonoBehaviour
{
    [SerializeField] public int CurrentCoins;

    private CoinsManager coinsManager;
    private string prefix = "CoinsStats_"; // Prefix for PlayerPrefs keys

    void Start()
    {
        PlayerPrefs.DeleteAll();

        CurrentCoins = PlayerPrefs.GetInt(prefix + "CurrentCoins", 0); // Default to 0 if not found

        // Get a reference to the XPManager instance in Start
        coinsManager = GetComponent<CoinsManager>();
        if (coinsManager != null)
        {
            coinsManager.OnCoinsChange += HandleCoinsChange;
        }
        else
        {
            Debug.LogError("Could not find CoinsManager component.");
        }

        

    }

    private void OnApplicationQuit()
    {

        PlayerPrefs.SetInt(prefix + "CurrentCoins", CurrentCoins);

        // Save PlayerPrefs
        PlayerPrefs.Save();
    }

    private void HandleCoinsChange(int newCoins)
    {
        CurrentCoins += newCoins;
        
    }

    

}
