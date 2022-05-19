using UnityEngine;
using UnityEngine.Tilemaps;

public class Line : MonoBehaviour
{
    private LineRenderer _line;
    private Vector3 _position;
    private Tilemap _buildOig;
    private TileBase _tile;
    private TileBase _getTile;
    public bool _isPowered;
    private Vector3Int _cellPosition;

    public void LineDelete() => Destroy(gameObject);

    public void LineFilling() {
        _tile = ItemList.buildings[6];
        _buildOig = Buildings._objectInGround;
        _position = gameObject.transform.position;
        _cellPosition = _buildOig.WorldToCell(_position);
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(0, _position);
    }

    public void LineSet()
    {
        for (int x = _cellPosition.x - 4; x < _cellPosition.x + 5; x++)
        {
            for (int y = _cellPosition.y - 4; y < _cellPosition.y + 5; y++)
            {
                _getTile = _buildOig.GetTile(new Vector3Int(x, y , (int) transform.position.z));
                if (_getTile == _tile) {
                    _line.SetPosition(1, new Vector2(x + 0.5f, y + 0.5f));
                    _isPowered = true;
                }
                else if (_getTile != _tile && _isPowered != true) {
                    _line.SetPosition(1, _position);
                    _isPowered = false;
                }
            }
        }
    }
}
