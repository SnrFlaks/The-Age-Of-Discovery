using UnityEngine;
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
    private TileBase[] _buildings;
    private string[] _buildingsName;
    [SerializeField] private Transform _visibleGroup;
    [SerializeField] private Transform _invisibleGroup;

    public void LineDelete()
    {
        if (_isPowered)
        {
            TileBase buOig = _buildOig.GetTile(_cellPosition);
            if (buOig == _buildings[0]) Buildings.ConnectedIronDrillCount[0]--;
            else if (buOig.name == $"drillIronTile{buOig.name[^1]}") Buildings.ConnectedIronDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]--;
            if (buOig == _buildings[1]) Buildings.ConnectedGoldDrillCount[0]--;
            else if (buOig.name == $"drillGoldTile{buOig.name[^1]}") Buildings.ConnectedGoldDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]--;
            if (buOig == _buildings[2]) Buildings.ConnectedTinDrillCount[0]--;
            else if (buOig.name == $"drillTinTile{buOig.name[^1]}") Buildings.ConnectedTinDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]--;
            if (buOig == _buildings[3]) Buildings.ConnectedCopperDrillCount[0]--;
            else if (buOig.name == $"drillCopperTile{buOig.name[^1]}") Buildings.ConnectedCopperDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]--;
            else if (buOig == ItemList.buildings[5]) Buildings.ConnectedFurnaceCount--;
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
        _buildings = ItemList.buildings;
        _buildingsName = ItemList.buildingsName;
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
                if (_getTile == _tile && _isPowered != true)
                {
                    _line.SetPosition(1, new Vector2(x + 0.5f, y + 0.5f));
                    _isPowered = true;
                    TileBase buOig = _buildOig.GetTile(_cellPosition);
                    if (buOig == _buildings[0]) Buildings.ConnectedIronDrillCount[0]++;
                    else if (buOig.name == $"drillIronTile{buOig.name[^1]}") Buildings.ConnectedIronDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]++;
                    if (buOig == _buildings[1]) Buildings.ConnectedGoldDrillCount[0]++;
                    else if (buOig.name == $"drillGoldTile{buOig.name[^1]}") Buildings.ConnectedGoldDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]++;
                    if (buOig == _buildings[2]) Buildings.ConnectedTinDrillCount[0]++;
                    else if (buOig.name == $"drillTinTile{buOig.name[^1]}") Buildings.ConnectedTinDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]++;
                    if (buOig == _buildings[3]) Buildings.ConnectedCopperDrillCount[0]++;
                    else if (buOig.name == $"drillCopperTile{buOig.name[^1]}") Buildings.ConnectedCopperDrillCount[(int)char.GetNumericValue((char) (buOig.name[^1]- 1))]++;
                    else if (buOig == ItemList.buildings[5]) Buildings.ConnectedFurnaceCount++;
                }
                else if (_getTile != _tile && _isPowered != true) {
                    _line.SetPosition(1, _position);
                    _isPowered = false;
                }
            }
        }
    }
    private void OnBecameVisible() => transform.SetParent(_visibleGroup, true);
    private void OnBecameInvisible() => Invoke (nameof(ReAttach),.1f);
    private void ReAttach() {
        if (gameObject.activeSelf) transform.SetParent(_invisibleGroup, true);
    }
}