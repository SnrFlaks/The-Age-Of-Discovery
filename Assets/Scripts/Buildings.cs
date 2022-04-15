using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSizeIns;
    private static Vector2Int _mapSize;
    private string[][] _cellName;
    private Tilemap _ground;
    private Tilemap _objectInGround;
    public TileBase[] buildings;

    void Start()
    {
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
        _mapSize = mapSizeIns;
        _cellName = new string[_mapSize.x][];
        for (var i = 0; i < _mapSize.y; i++)
        {
            _cellName[i] = new string[_mapSize.y];
        }
    }

    private void Update()
    {
        var point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosition = _ground.WorldToCell(point);
        if (!Input.GetMouseButtonDown(0) || HotBar.CreateLock) return;
        if (HotBar.HotBarSelect[0])
        {
            if (_ground.GetTile(cellPosition).name == "ironRandomTile" || _ground.GetTile(cellPosition).name == "goldRandomTile")
            {
                _objectInGround.SetTile(cellPosition, buildings[0]);
                _cellName[cellPosition.x][cellPosition.y] = buildings[0].name;
                Debug.Log(_cellName[cellPosition.x][cellPosition.y]);
            }
        }
        else if (HotBar.HotBarSelect[1])
        {
            if (_objectInGround.GetTile(cellPosition) == null) _objectInGround.SetTile(cellPosition, buildings[1]);
        }
    }
}