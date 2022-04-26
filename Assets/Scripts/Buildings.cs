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
    private float _ironDrillCount = 0;
    private float _goldDrillCount = 0;
    private float _tinDrillCount = 0;
    private float _copperDrillCount = 0;
    
    public static float _tin = 0;
    public static float _iron = 0;
    public static float _copper = 0;
    public static float _gold = 0;
    
    private int _tinFurnaceCount = 0;
    private int _ironFurnaceCount = 0;
    private int _copperFurnaceCount = 0;
    private int _goldFurnaceCount = 0;

    [SerializeField] private bool _tinFurnaceStatus;
    [SerializeField] private bool _ironFurnaceStatus;
    [SerializeField] private bool _copperFurnaceStatus;
    [SerializeField] private bool _goldFurnaceStatus;
    
    private int _tinIngot = 0;
    private int _ironIngot;
    private float _copperIngot = 0;
    private float _goldIngot = 0;
    
    private Tilemap _ground;
    private Tilemap _objectInGround;
    private TileBase[] _buildings;

    void Start()
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
                else if (changedTile == _buildings[1] && _objectInGround.GetTile(cellPosition) == null) _objectInGround.SetTile(cellPosition, _buildings[1]);
                else if (changedTile == _buildings[5] && _objectInGround.GetTile(cellPosition) == null)
                {
                    _objectInGround.SetTile(cellPosition, _buildings[5]);
                    _ironFurnaceCount++;
                }
                else if (changedTile == _buildings[5] && _objectInGround.GetTile(cellPosition) == null) _objectInGround.SetTile(cellPosition, _buildings[6]);
            }
        }
    }

    IEnumerator OncePerSecond()
    {
        while (true)
        {
            _tin += 8* _tinDrillCount;
            Ore[0].text = "Олово: \n" + Mathf.Round(_tin);

            _iron += 4*_ironDrillCount;
            Ore[1].text = "Железо: \n" + Mathf.Round(_iron);

            _copper += 2f * _copperDrillCount;
            Ore[2].text = "Медь: \n" + Mathf.Round(_copper);

            _gold += 1f * _goldDrillCount;
            Ore[3].text = "Золото: \n" + Mathf.Round(_gold);

         
            yield return new WaitForSeconds(1);
        }
    }
}