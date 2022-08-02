using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    private Camera mainCam;
    private Tilemap _ground;
    [SerializeField] private GameObject hotBar;
    [SerializeField] private Sprite cannonHb;
    [SerializeField] private Sprite empty;

    private void Start()
    {
        mainCam = Camera.main;
        _ground = transform.parent.GetChild(0).GetComponent<Tilemap>();
    }

    private void Update()
    {
        var cellPosition = _ground.WorldToCell(mainCam.ScreenToWorldPoint(Input.mousePosition));
        gameObject.transform.position = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
        for (var i = 0; i < 9; i++)
        {
            if (!HotBar.HotBarSelect[i]) continue;
            Sprite hBSprite = hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite;
            TileBase changedTile = hBSprite == cannonHb ? null : hBSprite == empty ? null : ItemList.buildings[Array.IndexOf(ItemList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)];
            if (changedTile == ItemList.buildings[0])
            {
                if (_ground.GetTile(cellPosition).name == "ironRandomTile")
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BuildingsLevelUpMenu.LevelNow[1] == 1 ? ItemList.buildingsIcon[0] :
                        ItemList.upgradeCostStat[1].levelSprite[BuildingsLevelUpMenu.LevelNow[1] - 2];
                }
                else if (_ground.GetTile(cellPosition).name == "goldRandomTile")
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BuildingsLevelUpMenu.LevelNow[3] == 1 ? ItemList.buildingsIcon[1] :
                        ItemList.upgradeCostStat[3].levelSprite[BuildingsLevelUpMenu.LevelNow[3] - 2];
                }
                else if (_ground.GetTile(cellPosition).name == "tinRandomTile")
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BuildingsLevelUpMenu.LevelNow[0] == 1 ? ItemList.buildingsIcon[2] :
                        ItemList.upgradeCostStat[0].levelSprite[BuildingsLevelUpMenu.LevelNow[0] - 2];
                }
                else if (_ground.GetTile(cellPosition).name == "copperRandomTile")
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BuildingsLevelUpMenu.LevelNow[2] == 1 ? ItemList.buildingsIcon[3] :
                        ItemList.upgradeCostStat[2].levelSprite[BuildingsLevelUpMenu.LevelNow[2] - 2];
                }
            }
            else transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = hotBar.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = HotBar.spriteNN[i] == cannonHb ? new Color(1,1,1,0.2f) : new Color(1f, 1f, 1f, 0f);
        }
    }
}