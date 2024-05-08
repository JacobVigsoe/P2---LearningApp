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
    public static GameObject previousCharacter;

    // children
    public GameObject avatarImage;
    public GameObject buyButton;
    public GameObject chooseButton;
    public GameObject chosenBorder;
    public GameObject lockedImage;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the avataravatarIndex of the avatar equal to its position in the hierarchy
        avatarIndex = this.gameObject.transform.GetSiblingIndex();
        
        UnChoose();

        //If character is bought and chosen
        if (SaveData.instance.currentCharacter == avatarIndex)
        {
            priceTxt.gameObject.SetActive(false);
            previousCharacter = this.gameObject;
            lockedImage.gameObject.SetActive(false);
            ChooseAvatar();
        }

        //If character is bought but NOT chosen
        if(SaveData.instance.charactersUnlocked[avatarIndex] == true && SaveData.instance.currentCharacter != avatarIndex)
        {
            priceTxt.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            lockedImage.gameObject.SetActive(false);
            chooseButton.gameObject.SetActive(true);
            //Debug.Log("Unlocked but not chosen " + avatarIndex);
        }

        //If character is NOT bought or chosen
        if(SaveData.instance.charactersUnlocked[avatarIndex] == false)
        {
            buyButton.SetActive(true);
            lockedImage.SetActive(true);
            chooseButton.SetActive(false);
            chosenBorder.SetActive(false);
        }



        //Getting the price of the avatar
        price = int.Parse(priceTxt.text);
    }

    private void UnChoose()
    {
        //Disabling Chosen Border
        chosenBorder.SetActive(false);

        //Enabling Choose button
        chooseButton.SetActive(true);

        //Disabling Buy button
        buyButton.SetActive(false);
        lockedImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if money is equal to or greater than the price of the avatar and thereby making the Buy button interactable.
        if (SaveData.instance.money >= price)
        {
            if(SaveData.instance.charactersUnlocked[avatarIndex] == false)
                buyButton.SetActive(true);
            //buyButton.GetComponent<Button>().interactable = true;
            //buyButton.GetComponent<Image>().color = Color.white;

            lockedImage.SetActive(false);
        }
        else
        {
            buyButton.SetActive(false);

            if(SaveData.instance.charactersUnlocked[avatarIndex] == false)
                lockedImage.SetActive(true);
            //buyButton.GetComponent<Button>().interactable = false;
            //buyButton.GetComponent<Image>().color = Color.gray;
        }

        if(lockedImage.activeSelf == false)
        {
            avatarImage.SetActive(true);
        }
        else
        {
            avatarImage.SetActive(false);
        }

    }
    public void BuyAvatar()
    {
        //Adjusting money and noting that character is unlocked
        SaveData.instance.AdjustMoney(-price);
        SaveData.instance.charactersUnlocked[avatarIndex] = true;

        //Disabling Buy button
        buyButton.gameObject.SetActive(false);
        priceTxt.gameObject.SetActive(false);

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
        previousCharacter.GetComponent<AvatarSelection>().UnChoose();

        //Disabling Choose button
        chooseButton.gameObject.SetActive(false);

        //Enabling Chosen Border
        chosenBorder.gameObject.SetActive(true);

        buyButton.gameObject.SetActive(false);

        //Choosing character
        SaveData.instance.currentCharacter = avatarIndex;

        SaveData.instance.charactersUnlocked[avatarIndex] = true;

        previousCharacter = this.gameObject;

        AvatarDisplay.instance.UpdateAvatar(avatarIndex);

        //Saving data
        SaveData.instance.SaveUserData();
    }

    public void Confirm()
    {

    }

}
