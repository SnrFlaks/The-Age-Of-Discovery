using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesData : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] Buildings buildings;
    public int[] _oreArray = new int[12];
    public int[] _dustArray = new int[12];
    public int[] _ingotArray = new int[12];
    public int[] _plateArray = new int[12];
    private void Start()
    {
        StartCoroutine(nameof(OncePerSecond));
    }
    private void Update()
    {
        inventory.resourcesDataArray = SetInventoryResourcesCountText();
    }
    private int[][] SetInventoryResourcesCountText()
    {
        int[][] inventoryResourcesCountArray = new int[4][];
        for (int p = 0; p < inventoryResourcesCountArray.Length; p++)
        {
            inventoryResourcesCountArray[p] = new int[12];
            for (int c = 0; c < inventoryResourcesCountArray[p].Length; c++)
            {
                if (p == 0) inventoryResourcesCountArray[p][c] = _oreArray[c];
                else if (p == 1) inventoryResourcesCountArray[p][c] = _dustArray[c];
                else if (p == 2) inventoryResourcesCountArray[p][c] = _ingotArray[c];
                else if (p == 3) inventoryResourcesCountArray[p][c] = _plateArray[c];
            }
        }
        return inventoryResourcesCountArray;
    }
    private IEnumerator OncePerSecond()
    {
        while (true)
        {
            for (int i = 0; i < buildings.ConnectedDrillCount.Length; i++)
            {
                for (int j = 0; j < buildings.ConnectedDrillCount[0].Length; j++)
                {
                    _oreArray[i] += 1 * (i + 1) * buildings.ConnectedDrillCount[i][j];
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
