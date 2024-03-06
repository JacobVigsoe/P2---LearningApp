using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetXPTest : MonoBehaviour
{
    int XPAmount = 100;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            XPManager.instance.AddExperience(XPAmount);
            //Debug.Log("Pressed button");
        }
    }
}
