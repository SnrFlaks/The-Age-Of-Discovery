using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingsLevelUpMenu : MonoBehaviour
{
    public static readonly int[] LevelNow = {1,1,1,1};
    private readonly Sprite[][] _sprites = new Sprite[6][];
    private int _selectBuild = -1;
    [SerializeField] private Text[] costText = new Text[9];
    [SerializeField] private Text upgradeButtonText;
    [FormerlySerializedAs("_ground")] [SerializeField] private Tilemap _objInGround;

    private void Start()
    {
        for (var i = 0; i < _sprites.Length; i++) {
            _sprites[i] = ItemList.upgradeCostStat[i].levelSprite;
        }
    }

    public void DrillTinButton() {
        _selectBuild = 0;
        for (var i = 0; i < costText.Length; i++) {
            costText[i].text = ItemList.upgradeCostStat[0].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[0] != 7 ? $"Upgrade to level: \n {LevelNow[0] + 1}" : "Max level!";
    }
    public void DrillIronButton() {
        _selectBuild = 1;
        for (var i = 0; i < costText.Length; i++) {
            costText[i].text = ItemList.upgradeCostStat[1].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[1] != 7 ? $"Upgrade to level: \n {LevelNow[1] + 1}" : "Max level!";
    }
    public void DrillCopperButton() {
        _selectBuild = 2;
        for (var i = 0; i < costText.Length; i++) {
            costText[i].text = ItemList.upgradeCostStat[2].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[2] != 7 ? $"Upgrade to level: \n {LevelNow[2] + 1}" : "Max level!";
    }
    public void DrillGoldButton() {
        _selectBuild = 3;
        for (var i = 0; i < costText.Length; i++) {
            costText[i].text = ItemList.upgradeCostStat[3].level2Costs[i].ToString();
        }
        upgradeButtonText.text = LevelNow[3] != 7 ? $"Upgrade to level: \n {LevelNow[3] + 1}" : "Max level!";
        
    }

    public void UpgradeButton()
    {
        if (_selectBuild == -1) return;
        if (GetPermission()) {
            ShopMenu.intTokens -= Convert.ToInt32(costText[0].text);
            Buildings._tin -= Convert.ToInt32(costText[1].text);
            Buildings._iron -= Convert.ToInt32(costText[2].text);
            Buildings._copper -= Convert.ToInt32(costText[3].text);
            Buildings._gold -= Convert.ToInt32(costText[4].text);
            Buildings._tinIngot -= Convert.ToInt32(costText[5].text);
            Buildings._ironIngot -= Convert.ToInt32(costText[6].text);
            Buildings._copperIngot -= Convert.ToInt32(costText[7].text);
            Buildings._goldIngot -= Convert.ToInt32(costText[8].text);
        }
        else return;
        if (_selectBuild == 0 && LevelNow[0] != 7) 
        {
            LevelNow[0]++;
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _sprites[0][LevelNow[0] - 2];
            transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = LevelNow[0] < 7 ? _sprites[0][LevelNow[0] - 1] : _sprites[0][LevelNow[0] - 2];
            upgradeButtonText.text = LevelNow[0] != 7 ? $"Upgrade to level: \n {LevelNow[0] + 1}" : "Max level!";
        }
        else if (_selectBuild == 1 && LevelNow[1] != 7)
        {
            LevelNow[1]++;
            transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = _sprites[1][LevelNow[1] - 2];
            transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = LevelNow[1] < 7 ? _sprites[1][LevelNow[1] - 1] : _sprites[1][LevelNow[1] - 2];
            upgradeButtonText.text = LevelNow[1] != 7 ? $"Upgrade to level: \n {LevelNow[1] + 1}" : "Max level!";
        } 
        else if (_selectBuild == 2 && LevelNow[2] != 7)
        {
            LevelNow[2]++;
            transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = _sprites[2][LevelNow[2] - 2];
            transform.GetChild(2).GetChild(2).GetComponent<Image>().sprite = LevelNow[2] < 7 ? _sprites[2][LevelNow[2] - 1] : _sprites[2][LevelNow[2] - 2];
            upgradeButtonText.text = LevelNow[2] != 7 ? $"Upgrade to level: \n {LevelNow[2] + 1}" : "Max level!";
        } 
        else if (_selectBuild == 3 && LevelNow[3] != 7)
        {
            LevelNow[3]++;
            transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = _sprites[3][LevelNow[3] - 2];
            transform.GetChild(3).GetChild(2).GetComponent<Image>().sprite = LevelNow[3] < 7 ? _sprites[3][LevelNow[3] - 1] : _sprites[3][LevelNow[3] - 2];
            upgradeButtonText.text = LevelNow[3] != 7 ? $"Upgrade to level: \n {LevelNow[3] + 1}" : "Max level!";
        }
        UpdateBuildings();
    }

    private void UpdateBuildings()
    {
        for (var x = 0; x < WorldGeneration.coord.x; x++)
        {
            for (var y = 0; y < WorldGeneration.coord.y; y++)
            {
                if (_selectBuild == 0 && _objInGround.GetTile(new Vector3Int(x, y, 0)) != null && _objInGround.GetTile(new Vector3Int(x, y, 0)))
                {
                    
                    _objInGround.GetTile(new Vector3Int(x, y, 0));
                }
            }
        }
    }

    private bool GetPermission() {
        return ShopMenu.intTokens >= Convert.ToInt32(costText[0].text) && 
               Buildings._tin >= Convert.ToInt32(costText[1].text) && 
               Buildings._iron >= Convert.ToInt32(costText[2].text) && 
               Buildings._copper >= Convert.ToInt32(costText[3].text) &&
               Buildings._gold >= Convert.ToInt32(costText[4].text) &&
               Buildings._tinIngot >= Convert.ToInt32(costText[5].text) &&
               Buildings._ironIngot >= Convert.ToInt32(costText[6].text) &&
               Buildings._copperIngot >= Convert.ToInt32(costText[7].text) &&
               Buildings._goldIngot >= Convert.ToInt32(costText[8].text);
    }
}
