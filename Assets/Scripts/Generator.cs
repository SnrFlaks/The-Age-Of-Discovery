using UnityEngine;
public class Generator : MonoBehaviour
{
    // public static readonly bool[][] _lineHaveOrNot = new bool[500][];
    // private Vector3 _position;
    // private Tilemap _buildOig;
    // private TileBase _tile;
    // private TileBase _getTile;
    // private float x, y;
    //
    // void Start()
    // {
    //     _buildOig = Buildings._objectInGround;
    //     _position = gameObject.transform.position;
    //     _tile = ItemList.buildings[6];
    //     for (int i = 0; i < _lineHaveOrNot.Length; i++)
    //     {
    //         _lineHaveOrNot[i] = new bool[500];
    //         for (int j = 0; j < _lineHaveOrNot[i].Length; j++) _lineHaveOrNot[i][j] = false;
    //     }
    // }
    //
    // public void Update()
    // {
    //     if (_buildOig.GetTile(Vector3Int.FloorToInt(_position)) != _tile) Destroy(gameObject);
    //     else {
    //         for (x = _position.x - 3; x < _position.x + 4; x++) {
    //             for (y = _position.y - 3; y < _position.y + 4; y++) {
    //                 _getTile = _buildOig.GetTile(new Vector3Int((int) (x - 0.5f), (int) (y - 0.5f), 0));
    //                 if (_getTile == null || _getTile == _tile) continue;
    //                 if (_lineHaveOrNot[(int) (x - 0.5f)][(int) (y - 0.5f)]) continue;
    //                 _lineHaveOrNot[(int) (x - 0.5f)][(int) (y - 0.5f)] = true;
    //                 var lp = Instantiate(linePrefab, new Vector2(x, y), Quaternion.identity, gameObject.transform);
    //                 lp.GetComponent<LineRenderer>().SetPosition(1, _position);
    //             }
    //         }
    //     }
    // }
}
