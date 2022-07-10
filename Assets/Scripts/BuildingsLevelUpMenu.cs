using System.Linq;
using UnityEngine;

public class BuildingsLevelUpMenu : MonoBehaviour
{
    [SerializeField] private int[] levelNow = {1, 1, 1, 1, 1, 1};
    private readonly Sprite[][] sprites = new Sprite[6][];
    private readonly int[] _preLoadCount = new int[9];
    private int _selectBuild = -1;

    private void Start()
    {
        for (var i = 0; i < sprites.Length; i++) {
            sprites[i] = ItemList.upgradeCostStat[i].levelSprite;
        }
    }

    public void DrillTinButton() {
        _selectBuild = 0;
    }

    public void UpgradeButton()
    {
        _preLoadCount[0] = ShopMenu.intTokens;
        _preLoadCount[2] = (int)Buildings._tin;
        _preLoadCount[3] = (int)Buildings._iron;
        _preLoadCount[4] = (int)Buildings._copper;
        _preLoadCount[5] = (int)Buildings._gold;
        _preLoadCount[6] = Buildings._tinIngot;
        _preLoadCount[7] = Buildings._ironIngot;
        _preLoadCount[8] = Buildings._copperIngot;
        _preLoadCount[9] = Buildings._goldIngot;
        if (_selectBuild == -1) return;
        if (levelNow[_selectBuild] == 1 && _preLoadCount.Where((t, i) => t < ItemList.upgradeCostStat[_selectBuild].level1Costs[i]).Any()) return;
        if (levelNow[_selectBuild] == 2 && _preLoadCount.Where((t, i) => t < ItemList.upgradeCostStat[_selectBuild].level2Costs[i]).Any()) return;
        if (levelNow[_selectBuild] == 3 && _preLoadCount.Where((t, i) => t < ItemList.upgradeCostStat[_selectBuild].level3Costs[i]).Any()) return;
        if (levelNow[_selectBuild] == 4 && _preLoadCount.Where((t, i) => t < ItemList.upgradeCostStat[_selectBuild].level4Costs[i]).Any()) return;
        if (levelNow[_selectBuild] == 5 && _preLoadCount.Where((t, i) => t < ItemList.upgradeCostStat[_selectBuild].level5Costs[i]).Any()) return;
        if (levelNow[_selectBuild] == 6 && _preLoadCount.Where((t, i) => t < ItemList.upgradeCostStat[_selectBuild].level6Costs[i]).Any()) return;
        if (levelNow[_selectBuild] == 7 && _preLoadCount.Where((t, i) => t < ItemList.upgradeCostStat[_selectBuild].level7Costs[i]).Any()) return;
    }

    private void GetUpgradeCost(int build)
    {
        
    }
}
