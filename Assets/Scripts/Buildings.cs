using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private GameObject hotBar;
    private string[][] _cellName;
    private int _drillCount = 1;
    private int[][] _drillCountArr;
    private int[][] _drillMining;
    private Vector2Int[][] _drillCord;
    private Tilemap _ground;
    private Tilemap _objectInGround;
    private TileBase[] _buildings;
    private Vector3 point;
    private Vector3Int cellPosition;

    void Start()
    {
        _buildings = ItemList.buildings;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
        _cellName = new string[_mapSize.x][];
        _drillMining = new int[_mapSize.x][];
        _drillCord = new Vector2Int[_mapSize.x][];
        for (var i = 0; i < _mapSize.y; i++)
        {
            _cellName[i] = new string[_mapSize.y];
            _drillMining[i] = new int[_mapSize.y];
            _drillCord[i] = new Vector2Int[_mapSize.y];
        }
        StartCoroutine(Mining());
    }

    private void Update()
    {
        point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        cellPosition = _ground.WorldToCell(point);
        if (!Input.GetMouseButtonDown(0) || HotBar.CreateLock) return;
        for (int i = 0; i < HotBar.HotBarSelect.Length; i++)
        {
            if (HotBar.HotBarSelect[i])
            {
                TileBase changedTile = ItemList.buildings[Array.IndexOf(ItemList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)];
                if (changedTile == _buildings[0] && _objectInGround.GetTile(cellPosition) == null)
                {
                    if (_ground.GetTile(cellPosition).name == "ironRandomTile")
                    {
                        _objectInGround.SetTile(cellPosition, _buildings[0]);
                        _cellName[cellPosition.x][cellPosition.y] = _buildings[0].name;
                        _drillCord[cellPosition.x][cellPosition.y] = new Vector2Int(cellPosition.x, cellPosition.y);
                    }
                    else if (_ground.GetTile(cellPosition).name == "goldRandomTile")
                    {
                        _objectInGround.SetTile(cellPosition, _buildings[2]);
                        _cellName[cellPosition.x][cellPosition.y] = _buildings[2].name;
                        _drillCord[cellPosition.x][cellPosition.y] = new Vector2Int(cellPosition.x, cellPosition.y);
                    }
                }
                if (changedTile == _buildings[1] && _objectInGround.GetTile(cellPosition) == null)
                {
                    _objectInGround.SetTile(cellPosition, _buildings[1]);
                }
            }
        }
    }

    IEnumerator Mining() {
        while (true) {

            yield return new WaitForSeconds(1);
        }
    }
}