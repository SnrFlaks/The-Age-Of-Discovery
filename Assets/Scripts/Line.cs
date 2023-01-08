using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Profiling;

public class Line : MonoBehaviour
{
    [SerializeField] private Buildings buildings;
    private LineRenderer _line;
    private Vector3 _position;
    private readonly Tilemap _buildOig = Buildings._objectInGround;
    private TileBase _tile;
    private TileBase _getTile;
    public bool _isPowered;
    private Vector3Int _cellPosition;
    private TileBase[] _buildings;
    private Transform _visibleGroup;
    private Transform _invisibleGroup;

    public void LineDelete()
    {
        if (_isPowered)
        {
            TileBase buOig = _buildOig.GetTile(_cellPosition);
            if (buOig.name == $"drillIronTile{buOig.name[^1]}") buildings.ConnectedDrillCount[0][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]--;
            else if (buOig.name == $"drillGoldTile{buOig.name[^1]}") buildings.ConnectedDrillCount[1][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]--;
            else if (buOig.name == $"drillTinTile{buOig.name[^1]}") buildings.ConnectedDrillCount[2][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]--;
            else if (buOig.name == $"drillCopperTile{buOig.name[^1]}") buildings.ConnectedDrillCount[3][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]--;
            else if (buOig == BuildingsList.buildings[5]) buildings.ConnectedFurnaceCount--;
        }
        Destroy(gameObject);
    }

    public void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _position = transform.position;
        _cellPosition = _buildOig.WorldToCell(_position);
        _tile = BuildingsList.buildings[6];
        _buildings = BuildingsList.buildings;
        var parent = transform.parent;
        _visibleGroup = parent;
        buildings = parent.parent.GetComponent<Buildings>();
        _invisibleGroup = parent.GetChild(0);
        _line.SetPosition(0, _position);
        _line.SetPosition(1, new Vector2(_cellPosition.x + 0.5f, _cellPosition.y + 0.5f));
        LineSet();
    }

    public void LineSet()
    {
        TileBase buOig = _buildOig.GetTile(_cellPosition);
        for (int x = _cellPosition.x - 3; x < _cellPosition.x + 4; x++)
        {
            for (int y = _cellPosition.y - 3; y < _cellPosition.y + 4; y++)
            {
                _getTile = _buildOig.GetTile(new Vector3Int(x, y, 0));
                if (_getTile == _tile && _isPowered != true)
                {
                    _line.SetPosition(1, new Vector2(x + 0.5f, y + 0.5f));
                    _isPowered = true;
                    if (buOig.name == $"drillTinTile{buOig.name[^1]}") buildings.ConnectedDrillCount[0][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]++;
                    else if (buOig.name == $"drillIronTile{buOig.name[^1]}") buildings.ConnectedDrillCount[1][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]++;
                    else if (buOig.name == $"drillCopperTile{buOig.name[^1]}") buildings.ConnectedDrillCount[2][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]++;
                    else if (buOig.name == $"drillGoldTile{buOig.name[^1]}") buildings.ConnectedDrillCount[3][(int)char.GetNumericValue((char)(buOig.name[^1] - 1))]++;
                    else if (buOig == BuildingsList.buildings[5]) buildings.ConnectedFurnaceCount++;
                }
                else if (_getTile != _tile && _isPowered != true)
                {
                    _line.SetPosition(1, _position);
                    _isPowered = false;
                }
            }
        }
    }
    private void OnBecameVisible() => transform.SetParent(_visibleGroup, true);
    private void OnBecameInvisible() => Invoke(nameof(ReAttach), .1f);
    private void ReAttach()
    {
        if (gameObject.activeSelf) transform.SetParent(_invisibleGroup, true);
    }
}