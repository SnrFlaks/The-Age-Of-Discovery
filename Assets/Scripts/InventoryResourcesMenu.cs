using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryResourcesMenu : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ResourcesData resourcesData;
    [SerializeField] private Text resourceNameText;
    [SerializeField] private Text resourceCountText;
    [SerializeField] private Image resourceIcon;
    [SerializeField] private string[] resourceNameArray;
    private Slider _sellSlider;
    private Text _sellCountText;
    private Text _sellMinusCountText;
    private int firstIndex = -1;
    private int secondIndex = -1;
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = resourceIcon.gameObject.GetComponent<RectTransform>();
        _sellSlider = transform.GetChild(3).GetChild(1).GetComponent<Slider>();
        _sellCountText = _sellSlider.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        _sellMinusCountText = transform.GetChild(2).GetChild(0).GetComponent<Text>();
    }
    private void Update()
    {
        if (secondIndex != -1) {
            resourceCountText.text = inventory.resourcesDataArray[firstIndex][secondIndex].ToString();
            _sellSlider.maxValue = inventory.resourcesDataArray[firstIndex][secondIndex];
        }
        if (_sellSlider.value != 0)
        {
            _sellMinusCountText.text = "-" + _sellSlider.value;
            _sellCountText.text = _sellSlider.value.ToString();
        }
        else if (_sellSlider.value == 0)
        {
            _sellMinusCountText.text = "";
            _sellCountText.text = _sellSlider.value.ToString();
        }
    }
    public void SellButton()
    {
        if (firstIndex == 0) resourcesData._oreArray[secondIndex] -= Convert.ToInt32(_sellSlider.value);
        else if (firstIndex == 1) resourcesData._dustArray[secondIndex] -= Convert.ToInt32(_sellSlider.value);
        else if (firstIndex == 2) resourcesData._ingotArray[secondIndex] -= Convert.ToInt32(_sellSlider.value);
        else if (firstIndex == 3) resourcesData._plateArray[secondIndex] -= Convert.ToInt32(_sellSlider.value);
    }
    public void SetName(int type, int name)
    {
        resourceNameText.text = $"{resourceNameArray[name]} {(type == 0 ? "Ore" : type == 1 ? "Dust" : type == 2 ? "Ingot" : "Plate")}";
        firstIndex = type;
        secondIndex = name;
    }
    public void SetIcon(Sprite icon, int type)
    {
        resourceIcon.sprite = icon;
    }
    public void SetLocalScale(Vector3 scale)
    {
        rectTransform.localScale = scale;
    }
}
