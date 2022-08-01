using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmelteryMenu : MonoBehaviour
{
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
        if (ore == 0) _slider.maxValue = Buildings._tin >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._tin) + 0.0000000001f;
        else if (ore == 1) _slider.maxValue = Buildings._iron >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._iron) + 0.0000000001f;
        else if (ore == 2) _slider.maxValue = Buildings._copper >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._gold) + 0.0000000001f;
        else if (ore == 3) _slider.maxValue = Buildings._gold >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._gold) + 0.0000000001f;
    }

    public void Remelt(int ore)
    {
        if (ore == 0 && Buildings._tin >= _slider.value)
        {
            Buildings._tin -= Mathf.Round(_slider.value);
            Buildings._tinIngot += (int) Mathf.Round(_slider.value);
            _slider.maxValue = Buildings._tin >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._tin) + 0.0000000001f;
        }
        else if (ore == 1 && Buildings._iron >= _slider.value)
        {
            Buildings._iron -= Mathf.Round(_slider.value);
            Buildings._ironIngot += (int) Mathf.Round(_slider.value);
            _slider.maxValue = Buildings._iron >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._iron) + 0.0000000001f;
        }
        else if (ore == 2 && Buildings._copper >= _slider.value)
        {
            Buildings._copper -= Mathf.Round(_slider.value);
            Buildings._copperIngot += (int) Mathf.Round(_slider.value);
            _slider.maxValue = Buildings._copper >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._gold) + 0.0000000001f;
        }
        else if (ore == 3 && Buildings._gold >= _slider.value)
        {
            Buildings._gold -= Mathf.Round(_slider.value);
            Buildings._goldIngot += (int) Mathf.Round(_slider.value);
            _slider.maxValue = Buildings._gold >= Buildings.ConnectedFurnaceCount ? Buildings.ConnectedFurnaceCount + 0.0000000001f : Mathf.Round(Buildings._gold) + 0.0000000001f;
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