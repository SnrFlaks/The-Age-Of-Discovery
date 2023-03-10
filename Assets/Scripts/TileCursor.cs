using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    [SerializeField] private Pipes pipes;
    private Camera _mainCam;
    private Tilemap _ground;
    private Tilemap _objectInGround;
    [SerializeField] private Sprite cannonHb;
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite generator;
    [SerializeField] private HotBar hotBar;
    [SerializeField] private Transform cannonGroup;

    private void Start()
    {
        _mainCam = Camera.main;
        _ground = transform.parent.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.parent.GetChild(1).GetComponent<Tilemap>();
    }

    private void Update()
    {
        var cellPosition = _ground.WorldToCell(_mainCam.ScreenToWorldPoint(Input.mousePosition));
        gameObject.transform.position = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
        if ((cellPosition.x >= 0 && cellPosition.x < 500) && (cellPosition.y >= 0 && cellPosition.y < 500))
        {
            TileBase tile = _ground.GetTile(cellPosition);
            string tileName = _ground.GetTile(cellPosition).name;
            for (var i = 0; i < 9; i++)
            {
                if (!hotBar.HotBarSelect[i]) continue;
                Sprite hotBarSprite = hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite;
                TileBase changedTile;
                changedTile = GetChangedTile(i, hotBarSprite);
                if (hotBarSprite == cannonHb && cannonGroup.Find($"{new Vector3Int(cellPosition.x, cellPosition.y, 0)}") != null) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = empty;
                else if (hotBarSprite == cannonHb) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cannonHb;
                else if (changedTile == null) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = empty;
                else if (_objectInGround.GetTile(cellPosition) == changedTile) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = empty;
                else if (_objectInGround.GetTile(cellPosition) != null && _objectInGround.GetTile(cellPosition).name.StartsWith("TA_Pipes_")) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = empty;
                else if (changedTile != null)
                {
                    if (changedTile == BuildingsList.buildings[0])
                    {
                        if (tileName == "tinRandomTile") ChangeSprite(0);
                        else if (tileName == "ironRandomTile") ChangeSprite(1);
                        else if (tileName == "copperRandomTile") ChangeSprite(2);
                        else if (tileName == "goldRandomTile") ChangeSprite(3);
                        else transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = hotBar.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
                    }
                    else if (changedTile == pipes.pipesArray[0]) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = pipes.pipesSprite[0];
                    else transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = hotBar.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
                }
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = hotBar.spriteNN[i] == cannonHb ? new Color(1, 1, 1, 0.2f) : new Color(1f, 1f, 1f, 0f);
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = hotBar.spriteNN[i] == generator ? new Color(1, 1, 1, 0.2f) : new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    private void ChangeSprite(int drillNumber)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BuildingsLevelUpMenu.LevelNow[drillNumber] == 1 ? BuildingsList.buildingsIcon[drillNumber] : BuildingsList.upgradeCostStat[drillNumber].levelSprite[BuildingsLevelUpMenu.LevelNow[drillNumber] - 2];
    }

    private TileBase GetChangedTile(int i, Sprite hotBarSprite)
    {
        TileBase changedTile;
        if (hotBarSprite == cannonHb) changedTile = null;
        else if (hotBarSprite == empty) changedTile = null;
        else if (hotBarSprite == pipes.pipesSprite[0]) changedTile = pipes.pipesArray[0];
        else changedTile = BuildingsList.buildings[Array.IndexOf(BuildingsList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)];
        return changedTile;
    }
}
