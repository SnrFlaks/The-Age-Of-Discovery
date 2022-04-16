using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    [SerializeField] private Vector2Int _mapSize;
    private string[][] _cellName;
    public static int[][] _cellCountArr;
    private int _cellCount = 0;
    private int[][] _drillMining;
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
        _cellCountArr = new int[_mapSize.x][];
        for (var i = 0; i < _mapSize.y; i++)
        {
            _cellName[i] = new string[_mapSize.y];
            _drillMining[i] = new int[_mapSize.y];
            _cellCountArr[i] = new int[_mapSize.y];
        }
    }

    private void Update()
    {
        point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        cellPosition = _ground.WorldToCell(point);
        if (!Input.GetMouseButtonDown(0) || HotBar.CreateLock) return;
        if (HotBar.HotBarSelect[0])
        {
            if (_objectInGround.GetTile(cellPosition) != _buildings[0] && (_ground.GetTile(cellPosition).name == "ironRandomTile" || _ground.GetTile(cellPosition).name == "goldRandomTile"))
            {
                _objectInGround.SetTile(cellPosition, _buildings[0]);
                Debug.Log(cellPosition);
                _cellName[cellPosition.x][cellPosition.y] = _buildings[0].name;
                _cellCountArr[cellPosition.x][cellPosition.y] = _cellCount++;
            }
        }
        else if (HotBar.HotBarSelect[1])
        {
            if (_objectInGround.GetTile(cellPosition) == null) _objectInGround.SetTile(cellPosition, _buildings[1]);
        }
    }
}