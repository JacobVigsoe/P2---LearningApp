using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{

    public TextMeshProUGUI moneyTxt;
    
    // Update is called once per frame
    void Update()
    {
        moneyTxt.text = SaveData.instance.money.ToString();
    }

    public void AddMoney(int money)
    {
        SaveData.instance.AdjustMoney(money);
    }
}
