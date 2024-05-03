using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AvatarDisplay : MonoBehaviour
{   
    public GameObject mainMenuAvatar;
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
        mainMenuAvatar.GetComponent<Image>().sprite = characterSprites[characterIndex];
    }

}
