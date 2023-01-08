using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryResourcesButton : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryResourcesMenu inventoryResourcesMenu;
    private Sprite buttonIcon;
    private int firstIndex;
    private int secondIndex;
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        firstIndex = Convert.ToInt32(transform.parent.name.Substring(4, 1).ToString()) - 1;
        secondIndex = Convert.ToInt32(transform.name.Trim(new char[] { 'C', 'e', 'l' })) - 1;
    }

    public void OnClick()
    {
        if (!inventoryResourcesMenu.gameObject.activeSelf) inventoryResourcesMenu.gameObject.SetActive(true);
        inventoryResourcesMenu.SetName(firstIndex, secondIndex);
        inventoryResourcesMenu.SetIcon(transform.GetChild(0).GetComponent<Image>().sprite, firstIndex);
        inventoryResourcesMenu.SetLocalScale(rectTransform.localScale);
    }
}
