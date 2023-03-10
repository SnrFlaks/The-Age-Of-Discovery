using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;

public class Line : MonoBehaviour
{
    [SerializeField] private Buildings buildings;
    private LineRenderer _line;
    private Vector3 _position;
    private readonly Tilemap _buildOig = Buildings._objectInGround;
    private TileBase _tile;
    public bool _isPowered;
    private Vector3Int _cellPosition;
    private Transform _visibleGroup;
    private Transform _invisibleGroup;

    public void LineDelete()
    {
        if (_isPowered)
        {
            TileBase tile = _buildOig.GetTile(_cellPosition);
            if (tile.name == $"drillIronTile{tile.name[^1]}") buildings.ConnectedDrillCount[0][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]--;
            else if (tile.name == $"drillGoldTile{tile.name[^1]}") buildings.ConnectedDrillCount[1][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]--;
            else if (tile.name == $"drillTinTile{tile.name[^1]}") buildings.ConnectedDrillCount[2][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]--;
            else if (tile.name == $"drillCopperTile{tile.name[^1]}") buildings.ConnectedDrillCount[3][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]--;
            else if (tile == BuildingsList.buildings[5]) buildings.ConnectedFurnaceCount--;
        }
        Destroy(gameObject);
    }

    public void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _position = transform.position;
        _cellPosition = _buildOig.WorldToCell(_position);
        _tile = BuildingsList.buildings[6];
        var parent = transform.parent;
        _visibleGroup = parent;
        buildings = parent.parent.GetComponent<Buildings>();
        _invisibleGroup = parent.GetChild(0);
        _line.SetPosition(0, _position);
        _line.SetPosition(1, new Vector2(_cellPosition.x + 0.5f, _cellPosition.y + 0.5f));
        _line.enabled = false;
        LineSet();
    }

    public void LineSet()
    {
        TileBase _getTile;
        TileBase tile = _buildOig.GetTile(_cellPosition);
        bool wasPowered = _isPowered;
        _isPowered = false;
        for (int x = _cellPosition.x - 3; x < _cellPosition.x + 4; x++)
        {
            for (int y = _cellPosition.y - 3; y < _cellPosition.y + 4; y++)
            {
                _getTile = _buildOig.GetTile(new Vector3Int(x, y, 0));
                if (_getTile == _tile)
                {
                    _line.enabled = true;
                    _line.SetPosition(1, new Vector2(x + 0.5f, y + 0.5f));
                    _isPowered = true;
                    if (!wasPowered)
                    {
                        if (tile.name.StartsWith("drillTinTile")) buildings.ConnectedDrillCount[0][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]++;
                        else if (tile.name.StartsWith("drillIronTile")) buildings.ConnectedDrillCount[1][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]++;
                        else if (tile.name.StartsWith("drillCopperTile")) buildings.ConnectedDrillCount[2][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]++;
                        else if (tile.name.StartsWith("drillGoldTile")) buildings.ConnectedDrillCount[3][(int)char.GetNumericValue((char)(tile.name[^1] - 1))]++;
                        else if (tile == BuildingsList.buildings[5]) buildings.ConnectedFurnaceCount++;
                    }
                    break;
                }
            }
            if (_isPowered) break;
        }
    }
}
