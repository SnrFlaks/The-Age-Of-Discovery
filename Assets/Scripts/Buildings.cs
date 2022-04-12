using UnityEngine;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    private Tilemap _ground;
    private Tilemap _objectInGround;
    public TileBase[] buildings;
    void Start() {
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
    }
    private void Update() 
    {
        var point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosition = _ground.WorldToCell(point);
        if (Input.GetMouseButtonDown(0) && HotBar.CreateLock == false) {
            if (HotBar.HotBarSelect[0]) {
                for (var i = 0; i < 54; i++)
                {
                    if (_ground.GetTile(cellPosition).name == "ironOre_" + i || _ground.GetTile(cellPosition).name == "goldOre_" + i)
                    {
                        _objectInGround.SetTile(cellPosition, buildings[0]);
                    }
                }
            }
            else if (HotBar.HotBarSelect[1])
            {
                if (_objectInGround.GetTile(cellPosition) == null) _objectInGround.SetTile(cellPosition, buildings[1]);
            }
        }
    }
}
