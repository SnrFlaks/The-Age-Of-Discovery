using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pipes : MonoBehaviour
{
    [SerializeField] HotBar hotBar;
    public TileBase[] pipesArray;
    public Sprite[] pipesSprite;
    private Vector3Int cellPosition;
    private Vector3Int firstPosition;
    private Vector3Int secondPosition;
    private Tilemap _ground;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        _ground = transform.parent.GetChild(0).GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPosition = new Vector3Int(-1, -1, -1);
            cellPosition = _ground.WorldToCell(mainCam.ScreenToWorldPoint(Input.mousePosition));
            firstPosition = cellPosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPosition = new Vector3Int(-1, -1, -1);
            cellPosition = _ground.WorldToCell(mainCam.ScreenToWorldPoint(Input.mousePosition));
            secondPosition = cellPosition;
            if (hotBar.CreateLock == false && Base.createLockHub == false && cellPosition.x >= 0 && cellPosition.y >= 0)
            {

            }
        }
    }
}
