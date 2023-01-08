using System;
using UnityEngine;

public class HotKeys : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private GameObject menuWindow;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!inventoryWindow.activeSelf && !menuWindow.activeSelf)
                {
                    inventoryWindow.SetActive(true);
                }
                else if (inventoryWindow.activeSelf && !menuWindow.activeSelf)
                {
                    inventoryWindow.SetActive(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                for (int i = 0; i < 1; i++)
                {
                    if (inventoryWindow.activeSelf)
                    {
                        inventoryWindow.SetActive(false);
                        return;
                    }
                    else if (menuWindow.activeSelf)
                    {
                        menuWindow.SetActive(false);
                        return;
                    }
                }
                menuWindow.SetActive(true);
                return;
            }
        }
    }
    private void KeyPress(KeyCode pressedKey)
    {
        if (pressedKey == KeyCode.E)
        {
            if (!inventoryWindow.activeSelf)
            {
                inventoryWindow.SetActive(true);
            }
        }
        else if (pressedKey == KeyCode.Escape)
        {
            for (int i = 0; i < 1; i++)
            {
                if (inventoryWindow.activeSelf)
                {
                    inventoryWindow.SetActive(false);
                    return;
                }
                else if (menuWindow.activeSelf)
                {
                    menuWindow.SetActive(false);
                    return;
                }
            }
            menuWindow.SetActive(true);
            return;
        }
    }
}
