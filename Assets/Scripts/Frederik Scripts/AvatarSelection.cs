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
    private Button buyButton;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the avataravatarIndex of the avatar equal to its position in the hierarchy
        avatarIndex = this.gameObject.transform.GetSiblingIndex();

        //Checking if the avatar is was chosen 
        if (SaveData.instance.currentCharacter == avatarIndex)
        {
            ChooseAvatar();
        }
        else
        {
            Debug.LogFormat("Avatar " + avatarIndex + "is not the Current Character selected");
        }

        //Getting the price of the avatar
        price = int.Parse(priceTxt.text);

        //Finding the button attached to the avatar and calling the BuyAvatar method on click
        BuyButton();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if money is equal to or greater than the price of the avatar and thereby making the Buy button interactable.

        45if (SaveData.instance.money >= price)
        {
            this.gameObject.transform.GetChild(3).interactable = true;
            this.gameObject.transform.GetChild(3).GetComponent<Image>().color = Color.white;
        }
        else
        {
            this.gameObject.transform.GetChild(3).interactable = false;
            this.gameObject.transform.GetChild(3).GetComponent<Image>().color = Color.gray;
        }


        //If the button is not interactable, the color is grayed out and if the button is interactable, the color is white.
        if (SaveData.instance.money >= price)
        {
            this.gameObject.transform.GetChild(3).interactable = true;
            this.gameObject.transform.GetChild(3).GetComponent<Image>().color = Color.white;
        }
        else
        {
            this.gameObject.transform.GetChild(3).interactable = false;
            this.gameObject.transform.GetChild(3).GetComponent<Image>().color = Color.gray;
        }
        
    }

    public void BuyAvatar()
    {
        //Adjusting money and noting that character is unlocked
        SaveData.instance.money -= price;
        SaveData.instance.charactersUnlocked[avatarIndex] = true;


        //Disabling Buy button
        buyButton.gameObject.SetActive(false);
        Debug.Log("Buy button for Avatar" + avatarIndex + "disabled");

        //Choosing character
        SaveData.instance.currentCharacter = avatarIndex;

        //Enabling Chosen Border
        this.gameObject.transform.GetChild(5).gameObject.SetActive(true);

        //Saving data
        SaveData.instance.SaveUserData();
    }

    public void ChooseAvatar()
    {
        //Disabling Choose button
        this.gameObject.transform.GetChild(4).gameObject.SetActive(false);

        //Enabling Chosen Border
        this.gameObject.transform.GetChild(5).gameObject.SetActive(true);

        //Disabling previous active Chosen Border
        GameObject.Find("Avatar" + SaveData.instance.currentCharacter).transform.GetChild(5).gameObject.SetActive(false);
      
        //Enabling previous Choose button
        GameObject.Find("Avatar" + SaveData.instance.currentCharacter).transform.GetChild(4).gameObject.SetActive(true);

        //Choosing character
        SaveData.instance.currentCharacter = avatarIndex;

        //Saving data
        SaveData.instance.SaveUserData();
    }

    //A method that finds the button attached to the avatar and calls the BuyAvatar method on click
    public void BuyButton()
    {
        buyButton = this.gameObject.transform.GetChild(3).GetComponent<Button>();
        buyButton.gameObject.GetComponent<Button>().onClick.AddListener(BuyAvatar);
    }

    //A method that finds the BuyButton attached and sets the buyButton variable to be that and calls the BuyAvatar method on click


}
