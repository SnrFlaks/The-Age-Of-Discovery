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
    [SerializeField] private Text[] Ingot;
    public static float _ironDrillCount = 0;
    public static float _goldDrillCount = 0;
    public static float _tinDrillCount = 0;
    public static float _copperDrillCount = 0;

    public static float _tin = 0;
    public static float _iron = 0;
    public static float _copper = 0;
    public static float _gold = 0;

    public static int _furnaceCount = 0;

    public static int _tinIngot = 0;
    public static int _ironIngot;
    public static int _copperIngot = 0;
    public static int _goldIngot = 0;

    private Tilemap _ground;
    private Tilemap _objectInGround;
    private TileBase[] _buildings;

    private void Start()
    {
        _buildings = ItemList.buildings;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
        StartCoroutine(OncePerSecond());
    }

    private void Update()
    {
        var point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosition = _ground.WorldToCell(point);
        Ore[0].text = "Tin: \n" + Mathf.Round(_tin);
        Ore[1].text = "Iron: \n" + Mathf.Round(_iron);
        Ore[2].text = "Copper: \n" + Mathf.Round(_copper);
        Ore[3].text = "Gold: \n" + Mathf.Round(_gold);
        Ingot[0].text = "Tin ingot: \n" + _tinIngot;
        Ingot[1].text = "Iron ingot: \n" + _ironIngot;
        Ingot[2].text = "Copper ingot: \n" + _copperIngot;
        Ingot[3].text = "Gold ingot: \n" + _goldIngot;
        if (Input.GetMouseButton(0) && HotBar.CreateLock == false && cellPosition.x >= 0 && cellPosition.y >= 0)
        {
            for (var i = 0; i < HotBar.HotBarSelect.Length; i++)
            {
                if (HotBar.HotBarSelect[i])
                {
                    TileBase changedTile = ItemList.buildings[Array.IndexOf(ItemList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)]!;
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
                    else if (changedTile == _buildings[1] && _objectInGround.GetTile(cellPosition) == null) _objectInGround.SetTile(cellPosition, _buildings[1]);
                    else if (changedTile == _buildings[5] && _objectInGround.GetTile(cellPosition) == null)
                    {
                        _objectInGround.SetTile(cellPosition, _buildings[5]);
                        _furnaceCount++;
                    }
                    else if (changedTile == _buildings[6] && _objectInGround.GetTile(cellPosition) == null) _objectInGround.SetTile(cellPosition, _buildings[6]);
                }
            }
        }
    }

    IEnumerator OncePerSecond()
    {
        while (true)
        {
            _tin += 8 * _tinDrillCount;
            _iron += 4 * _ironDrillCount;
            _copper += 2 * _copperDrillCount;
            _gold += 1 * _goldDrillCount;
            yield return new WaitForSeconds(1);
        }
    }
}