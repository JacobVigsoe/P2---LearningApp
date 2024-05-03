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
    public GameObject buyButton;
    public GameObject chooseButton;
    public GameObject chosenBorder;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the avataravatarIndex of the avatar equal to its position in the hierarchy
        avatarIndex = this.gameObject.transform.GetSiblingIndex();
        
        UnChoose();

        //If character is bought and chosen
        if (SaveData.instance.currentCharacter == avatarIndex)
        {
            previousCharacter = this.gameObject;
            ChooseAvatar();
            Debug.Log("Choosing" + gameObject.name);
        }

        //If character is bought but NOT chosen
        if(SaveData.instance.charactersUnlocked[avatarIndex] == true && SaveData.instance.currentCharacter != avatarIndex)
        {
            buyButton.gameObject.SetActive(false);
            chooseButton.gameObject.SetActive(true);
            //Debug.Log("Unlocked but not chosen " + avatarIndex);
        }

        //If character is NOT bought or chosen
        if(SaveData.instance.charactersUnlocked[avatarIndex] == false)
        {
            buyButton.SetActive(true);
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

        //Saving data
        SaveData.instance.SaveUserData();
    }

}
