using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AvatarSelection : MonoBehaviour
{

    public TextMeshProUGUI priceTxt;
    private int price;


    // Start is called before the first frame update
    void Start()
    {
      price = int.Parse(priceTxt.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyAvatar()
    {
        SaveData.instance.money -= price;
        SaveData.instance.SaveUserData();
    }

}
