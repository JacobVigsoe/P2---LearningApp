using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AvatarDisplay : MonoBehaviour
{   
    public GameObject mainMenuAvatar;
    public GameObject profileAvatar;
    public GameObject shopAvatar;
    public GameObject idleAvatar;
    public GameObject taskStatsAvatar;
    public GameObject setTimeAvatar;
    public GameObject finishedTaskAvatar;
    public Sprite[] characterSprites;
    public static AvatarDisplay instance;
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    void Start()
    {
        UpdateAvatar(SaveData.instance.currentCharacter);
    }
    public void UpdateAvatar(int characterIndex)
    {
        if(characterSprites.Length > 0)
        {
            mainMenuAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
            profileAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
            shopAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
            idleAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
            taskStatsAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
            setTimeAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
            finishedTaskAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
        }
    }

}
