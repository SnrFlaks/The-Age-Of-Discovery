using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    [SerializeField] private Pipes pipes;
    private Camera _mainCam;
    private Tilemap _ground;
    [SerializeField] private Sprite cannonHb;
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite generator;
    [SerializeField] private HotBar hotBar;

    private void Start()
    {
        _mainCam = Camera.main;
        _ground = transform.parent.GetChild(0).GetComponent<Tilemap>();
    }

    private void Update()
    {
        var cellPosition = _ground.WorldToCell(_mainCam.ScreenToWorldPoint(Input.mousePosition));
        gameObject.transform.position = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
        TileBase tile = _ground.GetTile(cellPosition);
        string tileName = _ground.GetTile(cellPosition).name;
        Sprite sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        for (var i = 0; i < 9; i++)
        {
            if (!hotBar.HotBarSelect[i]) continue;
            if (tile != null)
            {
                Sprite hBSprite = hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite;
                TileBase changedTile;
                if (hBSprite == cannonHb) changedTile = null;
                else if (hBSprite == empty) changedTile = null;
                else if (hBSprite == pipes.pipesSprite[10]) changedTile = pipes.pipesArray[0];
                else changedTile = BuildingsList.buildings[Array.IndexOf(BuildingsList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)];
                if (changedTile != null)
                {
                    if (changedTile == BuildingsList.buildings[0])
                    {
                        if (tileName == "ironRandomTile") sprite = BuildingsLevelUpMenu.LevelNow[1] == 1 ? BuildingsList.buildingsIcon[0] : BuildingsList.upgradeCostStat[1].levelSprite[BuildingsLevelUpMenu.LevelNow[1] - 2];
                        else if (tileName == "goldRandomTile") sprite = BuildingsLevelUpMenu.LevelNow[3] == 1 ? BuildingsList.buildingsIcon[1] : BuildingsList.upgradeCostStat[3].levelSprite[BuildingsLevelUpMenu.LevelNow[3] - 2];
                        else if (tileName == "tinRandomTile") sprite = BuildingsLevelUpMenu.LevelNow[0] == 1 ? BuildingsList.buildingsIcon[2] : BuildingsList.upgradeCostStat[0].levelSprite[BuildingsLevelUpMenu.LevelNow[0] - 2];
                        else if (tileName == "copperRandomTile") sprite = BuildingsLevelUpMenu.LevelNow[2] == 1 ? BuildingsList.buildingsIcon[3] : BuildingsList.upgradeCostStat[2].levelSprite[BuildingsLevelUpMenu.LevelNow[2] - 2];
                        else if (tileName == "coalRandomTile") sprite = BuildingsLevelUpMenu.LevelNow[4] == 1 ? BuildingsList.buildingsIcon[7] : BuildingsList.upgradeCostStat[4].levelSprite[BuildingsLevelUpMenu.LevelNow[4] - 2];
                        else sprite = hotBar.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
                    }
                    else if (changedTile == pipes.pipesArray[0]) sprite = pipes.pipesSprite[10];
                }
                else sprite = hotBar.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = hotBar.spriteNN[i] == cannonHb ? new Color(1, 1, 1, 0.2f) : new Color(1f, 1f, 1f, 0f);
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = hotBar.spriteNN[i] == generator ? new Color(1, 1, 1, 0.2f) : new Color(1f, 1f, 1f, 0f);
            }
        }
    }
}
