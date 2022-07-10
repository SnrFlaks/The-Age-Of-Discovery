using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Line : MonoBehaviour
{
    private LineRenderer _line;
    private Vector3 _position;
    private readonly Tilemap _buildOig = Buildings._objectInGround;
    private TileBase _tile;
    private TileBase _getTile;
    public bool _isPowered;
    private Vector3Int _cellPosition;
    [SerializeField] private Transform _visibleGroup;
    [SerializeField] private Transform _invisibleGroup;

    public void LineDelete()
    {
        if (_isPowered)
        {
            if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[0]) Buildings.ConnectedIronDrillCount--;
            else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[1]) Buildings.ConnectedGoldDrillCount--;
            else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[2]) Buildings.ConnectedTinDrillCount--;
            else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[3]) Buildings.ConnectedCopperDrillCount--;
            else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[5]) Buildings.ConnectedFurnaceCount--;
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void LineFilling()
    {
        _line = GetComponent<LineRenderer>();
        _position = gameObject.transform.position;
        _cellPosition = _buildOig.WorldToCell(_position);
        _tile = ItemList.buildings[6];
        var parent = transform.parent;
        _visibleGroup = parent;
        _invisibleGroup = parent.GetChild(0);
        _line.SetPosition(0, _position);
    }

    public void LineSet()
    {
        for (int x = _cellPosition.x - 3; x < _cellPosition.x + 4; x++)
        {
            for (int y = _cellPosition.y - 3; y < _cellPosition.y + 4; y++)
            {
                _getTile = _buildOig.GetTile(new Vector3Int(x, y , (int) transform.position.z));
                if (_getTile == _tile)
                {
                    _line.SetPosition(1, new Vector2(x + 0.5f, y + 0.5f));
                    _isPowered = true;
                    if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[0]) Buildings.ConnectedIronDrillCount++;
                    else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[1]) Buildings.ConnectedGoldDrillCount++;
                    else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[2]) Buildings.ConnectedTinDrillCount++;
                    else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[3]) Buildings.ConnectedCopperDrillCount++;
                    else if (_buildOig.GetTile(_cellPosition) == ItemList.buildings[5]) Buildings.ConnectedFurnaceCount++;
                }
                else if (_getTile != _tile && _isPowered != true) {
                    _line.SetPosition(1, _position);
                    _isPowered = false;
                }
            }
        }
    }
    private void OnBecameVisible() => transform.SetParent(_visibleGroup, true);
    private void OnBecameInvisible() {
        if (gameObject.activeSelf) transform.SetParent(_invisibleGroup, true);
    }
}
