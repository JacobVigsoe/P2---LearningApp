using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvatarSelection : MonoBehaviour
{
    public TextMeshProUGUI priceTxt;
    private int price;
    private int avatarIndex;

    // children
    public GameObject buyButton;
    public GameObject chooseButton;
    public GameObject chosenBorder;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the avataravatarIndex of the avatar equal to its position in the hierarchy
        avatarIndex = this.gameObject.transform.GetSiblingIndex();

        //Checking if the avatar is was chosen 
        if(SaveData.instance.charactersUnlocked[avatarIndex] == true)
        {
            buyButton.gameObject.SetActive(false);
            chooseButton.gameObject.SetActive(true);
            Debug.Log("Unlocked but not chosen " + avatarIndex);
        }

        if (SaveData.instance.currentCharacter == avatarIndex)
        {
            ChooseAvatar();
        }

        /*
        else
        {
            buyButton.gameObject.SetActive(true);
            chooseButton.gameObject.SetActive(false);

        }
        */

        //Getting the price of the avatar
        price = int.Parse(priceTxt.text);
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if money is equal to or greater than the price of the avatar and thereby making the Buy button interactable.

        if (SaveData.instance.money >= price)
        {
            buyButton.GetComponent<Button>().interactable = true;
            buyButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            buyButton.GetComponent<Button>().interactable = false;
            buyButton.GetComponent<Image>().color = Color.gray;
        }
    }

    public void BuyAvatar()
    {
        //Adjusting money and noting that character is unlocked
        SaveData.instance.money -= price;
        SaveData.instance.charactersUnlocked[avatarIndex] = true;


        //Disabling Buy button
        buyButton.gameObject.SetActive(false);

        //Choosing character
        SaveData.instance.currentCharacter = avatarIndex;

        //Enabling Chosen Border
        chosenBorder.gameObject.SetActive(true);

        ChooseAvatar();

        //Saving data
        SaveData.instance.SaveUserData();
    }

    public void ChooseAvatar()
    {
        //Disabling Choose button
        chooseButton.gameObject.SetActive(false);

        //Enabling Chosen Border
        chosenBorder.gameObject.SetActive(true);

        if(SaveData.instance.currentCharacter != avatarIndex)
        {
            // bruh virker ikke
            //Disabling previous active Chosen Border
            GameObject.Find("Avatar" + SaveData.instance.currentCharacter).transform.GetChild(5).gameObject.SetActive(false);

            //Enabling previous Choose button
            GameObject.Find("Avatar" + SaveData.instance.currentCharacter).transform.GetChild(4).gameObject.SetActive(true);
        }

        //Choosing character
        SaveData.instance.currentCharacter = avatarIndex;

        //Saving data
        SaveData.instance.SaveUserData();
    }

}
