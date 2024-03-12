using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetXPTest : MonoBehaviour
{
    public TMP_Text CurrentLevel;
    public XPStats xpStats;
    public Slider XPSlider;
    int XPAmount = 100;

    private void FixedUpdate()
    {
        CurrentLevel.text = xpStats.CurrentLevel.ToString();
        XPSlider.maxValue = xpStats.MaxXP;

      // Add XPAmount to the current value of the slider
        XPSlider.value = xpStats.CurrentXP;

    }
   public void FinishedTaskXP()
   {
            XPManager.instance.AddExperience(XPAmount);
            //Debug.Log("Pressed button");
    
   }
}
