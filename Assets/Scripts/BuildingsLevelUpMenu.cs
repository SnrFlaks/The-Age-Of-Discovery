using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsLevelUpMenu : MonoBehaviour
{
    [SerializeField] private ResourcesData resourcesData;
    public static readonly int[] LevelNow = { 1, 1, 1, 1, 1 };
    private readonly Sprite[][] _sprites = new Sprite[6][];
    private int _selectBuild = -1;
    [SerializeField] private Text[] costText = new Text[9];
    [SerializeField] private Text upgradeButtonText;
    [SerializeField] private Transform content;

    private void Start()
    {
        for (var i = 0; i < _sprites.Length; i++)
        {
            _sprites[i] = BuildingsList.upgradeCostStat[i].levelSprite;
        }
    }

    public void DrillCoalButton()
    {
        _selectBuild = 4;
        for (var i = 0; i < costText.Length; i++)
        {
            costText[i].text = BuildingsList.upgradeCostStat[4].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[4] != 7 ? $"Upgrade to level: \n {LevelNow[4] + 1}" : "Max level!";
    }
    public void DrillTinButton()
    {
        _selectBuild = 0;
        for (var i = 0; i < costText.Length; i++)
        {
            costText[i].text = BuildingsList.upgradeCostStat[0].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[0] != 7 ? $"Upgrade to level: \n {LevelNow[0] + 1}" : "Max level!";
    }
    public void DrillIronButton()
    {
        _selectBuild = 1;
        for (var i = 0; i < costText.Length; i++)
        {
            costText[i].text = BuildingsList.upgradeCostStat[1].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[1] != 7 ? $"Upgrade to level: \n {LevelNow[1] + 1}" : "Max level!";
    }
    public void DrillCopperButton()
    {
        _selectBuild = 2;
        for (var i = 0; i < costText.Length; i++)
        {
            costText[i].text = BuildingsList.upgradeCostStat[2].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[2] != 7 ? $"Upgrade to level: \n {LevelNow[2] + 1}" : "Max level!";
    }
    public void DrillGoldButton()
    {
        _selectBuild = 3;
        for (var i = 0; i < costText.Length; i++)
        {
            costText[i].text = BuildingsList.upgradeCostStat[3].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[3] != 7 ? $"Upgrade to level: \n {LevelNow[3] + 1}" : "Max level!";
    }

    public void UpgradeButton()
    {
        if (_selectBuild == -1) return;
        if (GetPermission())
        {
            ShopMenu.intTokens -= Convert.ToInt32(costText[0].text);
            for (int o = 0; o < resourcesData._oreArray.Length; o++) resourcesData._oreArray[o] -= Convert.ToInt32(costText[o].text);
            for (int i = 0; i < resourcesData._ingotArray.Length; i++) resourcesData._ingotArray[i] -= Convert.ToInt32(costText[i].text);
        }
        else return;
        if (_selectBuild == -1 || LevelNow[_selectBuild] == 7) return;
        LevelNow[_selectBuild]++;
        content.GetChild(_selectBuild).GetChild(0).GetComponent<Image>().sprite = _sprites[_selectBuild][LevelNow[_selectBuild] - 2];
        content.GetChild(_selectBuild).GetChild(2).GetComponent<Image>().sprite = LevelNow[_selectBuild] < 7 ? _sprites[_selectBuild][LevelNow[_selectBuild] - 1] : _sprites[_selectBuild][LevelNow[_selectBuild] - 2];
        upgradeButtonText.text = LevelNow[_selectBuild] != 7 ? $"Upgrade to level: \n {LevelNow[_selectBuild] + 1}" : "Max level!";
    }

    private bool GetPermission()
    {
        bool permission = false;
        for (int t = 0; t < 2; t++)
        {
            for (int r = 0; r < resourcesData._oreArray.Length; r++)
            {
                if (resourcesData._oreArray[r] >= Convert.ToInt32(costText[r].text)) permission = true;
                else return false;
                if (resourcesData._ingotArray[r] >= Convert.ToInt32(costText[r].text)) permission = true;
                else return false;
            }
        }
        return permission;
    }
}
