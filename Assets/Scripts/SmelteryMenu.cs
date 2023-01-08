using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmelteryMenu : MonoBehaviour
{
    [SerializeField] private ResourcesData resourcesData;
    [SerializeField] private Buildings buildings;
    [SerializeField] private Text finalNumber;
    private Slider _slider;
    private Toggle _toggle;

    void Start()
    {
        _toggle = transform.GetChild(5).GetComponent<Toggle>();
        _slider = transform.GetChild(3).GetComponent<Slider>();
    }

    void Update() => finalNumber.text = "Remelting: \n" + Mathf.Round(_slider.value);

    public void NumberOfOre(int ore)
    {
        for (int i = 0; i < 4; i++)
        {
            if (ore == i) _slider.maxValue = resourcesData._oreArray[i] >= buildings.ConnectedFurnaceCount ? buildings.ConnectedFurnaceCount + 0.0001f : Mathf.Round(resourcesData._oreArray[i]) + 0.0001f;
        }
    }

    public void Remelt(int ore)
    {
        for (int i = 0; i < 4; i++)
        {
            if (ore == i && resourcesData._oreArray[i] >= _slider.value)
            {
                resourcesData._oreArray[i] -= Convert.ToInt32(Mathf.Round(_slider.value));
                resourcesData._ingotArray[i] += Convert.ToInt32(Mathf.Round(_slider.value));
                _slider.maxValue = resourcesData._oreArray[i] >= buildings.ConnectedFurnaceCount ? buildings.ConnectedFurnaceCount + 0.0001f : Mathf.Round(resourcesData._oreArray[i]) + 0.0001f;
            } 
        }
    }

    public void Auto(int ore)
    {
        if (_toggle.isOn) StartCoroutine(Toggle(ore));
        else StopCoroutine(Toggle(ore));
    }

    private IEnumerator Toggle(int ore)
    {
        while (true)
        {
            Remelt(ore);
            yield return new WaitForSeconds(1);
        }
    }
}