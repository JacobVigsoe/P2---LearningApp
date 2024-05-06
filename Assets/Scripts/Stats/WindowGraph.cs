using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;
using System.Linq;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<List<GameObject>> gameObjectLists; // List of lists to store game objects for each graph
    public TaskManager taskManager;

    [SerializeField] private int ShowLastListAmount = -1;

    private List<int> valueListPercent;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent <RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        gameObjectLists = new List<List<GameObject>>(); // Initialize the list of lists

    }

    public void UpdateGraph()
    {
        ClearGraph();
        // Get accuracy values from AccuracyManager and convert them to integers
        if(taskManager.lastClickedTask != "xxxxxxxxx")
            valueListPercent = taskManager.GetPercentage();


        if (gameObjectLists.Count == 0)
        {
            ShowGraphPercentage(valueListPercent, ShowLastListAmount, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
            ShowGraphPercentage(CreateRandomValueList(8), ShowLastListAmount, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
        }

    }
    private List<int> CreateRandomValueList(int length)
    {
        List<int> valueList = new List<int>();
        for (int i = 0; i < length; i++)
        {
            valueList.Add(UnityEngine.Random.Range(0, 100));
        }
        return valueList;
    }
    private void ClearGraph()
    {
        // Clear the game objects for each graph
        foreach (var gameObjectList in gameObjectLists)
        {
            foreach (var gameObject in gameObjectList)
            {
                Destroy(gameObject);
            }
            gameObjectList.Clear();
        }
        // Clear the list of lists
        gameObjectLists.Clear();
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) 
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer,false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = new Color(46f / 255f, 112f / 255f, 170f / 255f, 1f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(22, 22);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    private void ShowGraphPercentage(List<int> valueList, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {

        if(getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }

        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }
        

        List<GameObject> gameObjectList = new List<GameObject>(); // Create a new list to store game objects for this graph
        gameObjectLists.Add(gameObjectList); // Add the list to the list of lists

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }

        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) //hvis vi har mindre end 5 values i vores data set s� ville den returne error (valueList.Count - maxVisibleValueAmount vil g� i minus). Mathf.Max g�r at den istedet returner 0 s� vi ikke f�r en error.
        {
            int value = valueList[i];

            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        float xSize = graphWidth / (maxVisibleValueAmount + 1); // +1 s� den ikke er helt oppe af h�jre side p� grafen

        int xIndex = 0;

        GameObject lastCircleGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) // hvis maxVisibleValueAmount er fx 5 vil den vise de sidste 5 v�rdier 
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circleGameObject);
            if(lastCircleGameObject != null )
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition3D = new Vector3(xPosition, -35f, 0f);
            labelX.localScale = Vector3.one;
            labelX.GetComponent<TMP_Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition3D = new Vector3(xPosition, -35f, 0f);
            gameObjectList.Add(dashY.gameObject);

            xIndex++;
        }

        int seperatorCount = 5;
        for (int i = 0;i <= seperatorCount;i++) 
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / seperatorCount;
            labelY.anchoredPosition3D = new Vector3(-35f, normalizedValue * graphHeight, 0f);
            labelY.localScale = Vector3.one;
            labelY.GetComponent<TMP_Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition3D = new Vector3(-35f, normalizedValue * graphHeight, 0f);
            gameObjectList.Add(dashX.gameObject);
        }
    }

    
    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof (Image));
        gameObject.transform.SetParent(graphContainer,false);
        gameObject.GetComponent<Image>().color = new Color(46f/255f, 112f/255f, 170f/255f, 1f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 10f);
        rectTransform.anchoredPosition = dotPositionA + dir  * distance * .5f; // S� den bliver positioned i midten af de 2 points (.5f)
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        return gameObject;
    }

}
