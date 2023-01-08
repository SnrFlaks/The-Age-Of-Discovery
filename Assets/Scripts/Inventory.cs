using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ResourcesData resourcesData;
    [SerializeField] private Buildings buildings;
    public int[][] resourcesDataArray = new int[4][];
    private Text[][] resourcesCountArrayText;
    private GameObject inventory;
    private void Awake()
    {
        inventory = transform.gameObject;
        for (int i = 0; i < resourcesDataArray.Length; i++) resourcesDataArray[i] = new int[12];
        FillCountArrayText();
    }
    private void Update()
    {
        if (inventory.activeSelf) UpdateResourcesData();
    }
    private void UpdateResourcesData()
    {
        for (int i = 0; i < resourcesDataArray.Length; i++)
        {
            for (int j = 0; j < resourcesDataArray[i].Length; j++)
            {
                resourcesCountArrayText[i][j].text = resourcesDataArray[i][j] < 9999 ? resourcesDataArray[i][j].ToString() : "9999+";
            }
        }
    }
    private void FillCountArrayText()
    {
        resourcesCountArrayText = new Text[transform.childCount - 2][];
        Transform[] pageArray = new Transform[transform.childCount - 2];
        for (int p = 0; p < pageArray.Length; p++)
        {
            pageArray[p] = transform.GetChild(p);
            resourcesCountArrayText[p] = new Text[pageArray[p].childCount - 1];
            for (int c = 0; c < resourcesCountArrayText[p].Length; c++)
            {
                resourcesCountArrayText[p][c] = pageArray[p].GetChild(c).GetChild(1).GetComponent<Text>();
            }
        }
    }
}
