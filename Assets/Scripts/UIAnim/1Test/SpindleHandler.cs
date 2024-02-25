using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpindleHandler : MonoBehaviour
{
    private int currentBoxIndex = 0;

    public List<GameObject> boxes = new List<GameObject>();

    public static bool openBox;

    public GameObject nextButton;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            boxes.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        boxes[0].SetActive(true);
    }

    private void Update()
    {
        if (openBox == true)
            nextButton.SetActive(false);
        else
            nextButton.SetActive(true);
    }

    public void CycleNext()
    {
        if(boxes.Count == 0)
        {
            Debug.Log("No boxes in list");
            return;
        }

        boxes[currentBoxIndex].SetActive(false);

        currentBoxIndex++;

        if(currentBoxIndex >= boxes.Count)
        {
            currentBoxIndex = 0;
        }

        boxes[currentBoxIndex].SetActive(true);
    }

}
