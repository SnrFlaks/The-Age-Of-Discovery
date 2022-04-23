using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private GameObject hotBar;

    [SerializeField] private Text[] Ore;
    private float _ironDrillCount = 1;
    private float _goldDrillCount = 1;
    private float _tinDrillCount = 1;
    private float _copperDrillCount = 1;
    
    private Tilemap _ground;
    private Tilemap _objectInGround;
    private TileBase[] _buildings;
    private Vector3 point;
    private Vector3Int cellPosition;

    void Start()
    {
        _buildings = ItemList.buildings;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();

        StartCoroutine(Mining());
    }

    private void Update()
    {
        point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        cellPosition = _ground.WorldToCell(point);
        if (!Input.GetMouseButtonDown(0) || HotBar.CreateLock) return;
        for (int i = 0; i < HotBar.HotBarSelect.Length; i++)
        {
            if (HotBar.HotBarSelect[i])
            {
                TileBase changedTile = ItemList.buildings[Array.IndexOf(ItemList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)];
                if (changedTile == _buildings[0] && _objectInGround.GetTile(cellPosition) == null)
                {
                    
                    if (_ground.GetTile(cellPosition).name == "ironRandomTile")
                    {
                        _objectInGround.SetTile(cellPosition, _buildings[0]);
                        _ironDrillCount++;
                    }
                    else if (_ground.GetTile(cellPosition).name == "goldRandomTile")
                    {
                        _objectInGround.SetTile(cellPosition, _buildings[2]);
                        _goldDrillCount++;
                    }
                    else if (_ground.GetTile(cellPosition).name == "tinRandomTile")
                    {
                        _objectInGround.SetTile(cellPosition, _buildings[3]);
                        _tinDrillCount++;
                    }
                    else if (_ground.GetTile(cellPosition).name == "copperRandomTile")
                    {
                        _objectInGround.SetTile(cellPosition, _buildings[4]);
                        _copperDrillCount++;
                    }
                }

                if (changedTile == _buildings[1] && _objectInGround.GetTile(cellPosition) == null)
                {
                    _objectInGround.SetTile(cellPosition, _buildings[1]);
                }
            }
        }
    }

    IEnumerator Mining()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(1);
        }
    }
}