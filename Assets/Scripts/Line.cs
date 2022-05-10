using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer _line;
    private bool _isPowered;
    private void Start() {
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(0, gameObject.transform.position);
    }
    private void Update() => _line.SetPosition(1, Buildings._objectInGround.GetTile(Vector3Int.FloorToInt(gameObject.transform.position)) != null ? gameObject.transform.parent.position : gameObject.transform.position);
}
