using System;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelButton : MonoBehaviour
{
    [SerializeField] private GameObject[] statPanel;
    [SerializeField] private GameObject[] statButton;
    [SerializeField] private Sprite[] sprite;
    public void StatPanButton()
    {
        for (var i = 0; i < statPanel.Length; i++)
        {
            if (i == Convert.ToInt32(transform.name[transform.name.Length - 1].ToString()) - 1)
            {
                statPanel[i].SetActive(true);
                statButton[i].GetComponent<Image>().sprite = (i == 0) ? sprite[3] :  sprite[1];
            }
            else if (i != Convert.ToInt32(transform.name[transform.name.Length - 1].ToString()) - 1)
            {
                statPanel[i].SetActive(false);
                statButton[i].GetComponent<Image>().sprite = (i == 0) ? sprite[2] :  sprite[0];
            }
        }
    }
    
}
