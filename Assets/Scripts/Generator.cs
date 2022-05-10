
using UnityEngine;

public class Generator : MonoBehaviour
{
    private readonly bool[][] _lineHaveOrNot = new bool[500][];
    [SerializeField] private GameObject linePrefab;
    void Start()
    {
        for (int i = 0; i < _lineHaveOrNot.Length; i++)
        {
            _lineHaveOrNot[i] = new bool[500];
            for (int j = 0; j < _lineHaveOrNot[i].Length; j++) _lineHaveOrNot[i][j] = false;
        }
    }
    void Update()
    {
        var position = gameObject.transform.position;
        Debug.Log(position);
        if (Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(position)) != ItemList.buildings[6]) Destroy(gameObject);
        else {
            for (float x = position.x - 5; x < position.x + 6; x++) {
                for (float y = position.y - 5; y < position.y + 6; y++) {
                    if (Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(new Vector2(x, y))) != null && Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(new Vector2(x, y))) != ItemList.buildings[6]) {
                        if (_lineHaveOrNot[(int) (x - 0.5f)][(int) (y - 0.5f)] == false) {
                            _lineHaveOrNot[(int) x][(int) y] = true;
                            Instantiate(linePrefab, new Vector2(x, y), Quaternion.identity, gameObject.transform);
                        }
                    }
                }
            }
        }
    }
}
