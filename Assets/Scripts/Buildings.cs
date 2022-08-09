using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    [SerializeField] private GameObject hotBar;
    [SerializeField] private Text errorText;
    [SerializeField] private GameObject cannon;
    [SerializeField] private Sprite cannonHb;
    [SerializeField] private Sprite empty;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private TileBase waterTile;

    [SerializeField] private Text[] ore;
    [SerializeField] private Text[] ingot;
    public static readonly int[] ConnectedIronDrillCount = new int[7];
    public static readonly int[] ConnectedGoldDrillCount = new int[7];
    public static readonly int[] ConnectedTinDrillCount = new int[7];
    public static readonly int[] ConnectedCopperDrillCount = new int[7];
    public static int ConnectedFurnaceCount;
    private int _coalDrillCount;
    private int _ironDrillCount;
    private int _goldDrillCount;
    private int _tinDrillCount;
    private int _copperDrillCount;

    public static float _tin;
    public static float _iron;
    public static float _copper;
    public static float _gold;
    
    public static int _tinIngot;
    public static int _ironIngot;
    public static int _copperIngot;
    public static int _goldIngot;

    private Tilemap _ground;
    public static Tilemap _objectInGround;
    private TileBase[] _buildings;
    private string[] _buildingsName;
    public Tile emptyTile;

    public static readonly bool[][] cannonBoolArr = new bool[500][];
    public Text tokensText;
    private Vector3 point;
    private Vector3Int cellPosition;

    private Transform _grid;
    private Transform _lineGroup;

    private Camera mainCam;

    private void Awake()
    {
        //ShopMenu.intTokens = PlayerPrefs.GetInt("tokens") == 0 ? 100000 : PlayerPrefs.GetInt("tokens");
        mainCam = Camera.main;
        _buildings = ItemList.buildings;
        _buildingsName = ItemList.buildingsName;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
        _grid = transform;
        _lineGroup = _grid.GetChild(2);
        StartCoroutine(OncePerSecond());
        for (int i = 0; i < cannonBoolArr.Length; i++) cannonBoolArr[i] = new bool[500];
        ShopMenu.tokens = tokensText;
        ShopMenu.tokens.text = "Tokens: \n" + ShopMenu.intTokens;
    }

    private void Update()
    {
        point = mainCam.ScreenToWorldPoint(Input.mousePosition);
        cellPosition = _ground.WorldToCell(point);
        tokensText.text = "Tokens: \n" + ShopMenu.intTokens;
        ore[0].text = "Tin: \n" + Mathf.Round(_tin);
        ore[1].text = "Iron: \n" + Mathf.Round(_iron);
        ore[2].text = "Copper: \n" + Mathf.Round(_copper);
        ore[3].text = "Gold: \n" + Mathf.Round(_gold);
        ingot[0].text = "Tin ingot: \n" + _tinIngot;
        ingot[1].text = "Iron ingot: \n" + _ironIngot;
        ingot[2].text = "Copper ingot: \n" + _copperIngot;
        ingot[3].text = "Gold ingot: \n" + _goldIngot;
        if (Input.GetMouseButton(0) && HotBar.CreateLock == false && Base.createLockHub == false && cellPosition.x >= 0 && cellPosition.y >= 0 && cannonBoolArr[cellPosition.x][cellPosition.y] != true && _objectInGround.GetTile(cellPosition) == null && _ground.GetTile(cellPosition) != waterTile)
        {
            for (var i = 0; i < HotBar.HotBarSelect.Length; i++)
            {
                if (HotBar.HotBarSelect[i])
                {
                    Sprite hBSprite = hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite;
                    TileBase changedTile = hBSprite == cannonHb ? null : hBSprite == empty ? null : ItemList.buildings[Array.IndexOf(ItemList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)]!;
                    if (changedTile == _buildings[0])
                    {
                        if (_ground.GetTile(cellPosition).name == "ironRandomTile")
                        {
                            if (ShopMenu.intTokens >= (_ironDrillCount == 0 ? 1920 : GetCostForDrill(8, _ironDrillCount <= 3 ? 4 : _ironDrillCount, BuildingsLevelUpMenu.LevelNow[1])))
                            {
                                _objectInGround.SetTile(cellPosition, BuildingsLevelUpMenu.LevelNow[1] == 1 ? _buildings[0] : _buildings[Array.IndexOf(_buildingsName, $"drillIronTile{BuildingsLevelUpMenu.LevelNow[1]}")]);
                                ShopMenu.intTokens -= _ironDrillCount == 0 ? 1920 : GetCostForDrill(8, _ironDrillCount <= 3 ? 4 : _ironDrillCount, BuildingsLevelUpMenu.LevelNow[1]);
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                _ironDrillCount++;
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                        else if (_ground.GetTile(cellPosition).name == "goldRandomTile")
                        {
                            if (ShopMenu.intTokens >= (_goldDrillCount == 0 ? 5880 : GetCostForDrill(14, _goldDrillCount <= 3 ? 4 : _goldDrillCount, BuildingsLevelUpMenu.LevelNow[3])))
                            {
                                _objectInGround.SetTile(cellPosition, BuildingsLevelUpMenu.LevelNow[3] == 1 ? _buildings[1] : _buildings[Array.IndexOf(_buildingsName, $"drillGoldTile{BuildingsLevelUpMenu.LevelNow[3]}")]);
                                ShopMenu.intTokens -= GetCostForDrill(14, _goldDrillCount <= 3 ? 4 : _goldDrillCount, BuildingsLevelUpMenu.LevelNow[3]);
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                _goldDrillCount++;
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                        else if (_ground.GetTile(cellPosition).name == "tinRandomTile")
                        {
                            if (ShopMenu.intTokens >= (_tinDrillCount == 0 ? 1080 : GetCostForDrill(6, _tinDrillCount <= 3 ? 4 : _tinDrillCount, BuildingsLevelUpMenu.LevelNow[0])))
                            {
                                _objectInGround.SetTile(cellPosition, BuildingsLevelUpMenu.LevelNow[0] == 1 ? _buildings[2] : _buildings[Array.IndexOf(_buildingsName, $"drillTinTile{BuildingsLevelUpMenu.LevelNow[0]}")]);
                                ShopMenu.intTokens -= GetCostForDrill(6, _tinDrillCount <= 3 ? 4 : _tinDrillCount, BuildingsLevelUpMenu.LevelNow[0]);
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                _tinDrillCount++;
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                        else if (_ground.GetTile(cellPosition).name == "copperRandomTile")
                        {
                            if (ShopMenu.intTokens >= (_copperDrillCount == 0 ? 4320 : GetCostForDrill(12, _copperDrillCount <= 3 ? 4 : _copperDrillCount, BuildingsLevelUpMenu.LevelNow[2])))
                            {
                                _objectInGround.SetTile(cellPosition, BuildingsLevelUpMenu.LevelNow[2] == 1 ? _buildings[3] : _buildings[Array.IndexOf(_buildingsName, $"drillCopperTile{BuildingsLevelUpMenu.LevelNow[2]}")]);
                                ShopMenu.intTokens -= GetCostForDrill(12, _copperDrillCount <= 3 ? 4 : _copperDrillCount, BuildingsLevelUpMenu.LevelNow[2]);
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                _copperDrillCount++;
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                        else if (_ground.GetTile(cellPosition).name == "coalRandomTile")
                        {
                            if (ShopMenu.intTokens >= (_coalDrillCount == 0 ? 480 : GetCostForDrill(4, _coalDrillCount <= 3 ? 4 : _coalDrillCount, BuildingsLevelUpMenu.LevelNow[2])))
                            {
                                _objectInGround.SetTile(cellPosition, BuildingsLevelUpMenu.LevelNow[4] == 1 ? _buildings[7] : _buildings[Array.IndexOf(_buildingsName, $"drillCoalTile{BuildingsLevelUpMenu.LevelNow[4]}")]);
                                ShopMenu.intTokens -= GetCostForDrill(4, _coalDrillCount <= 3 ? 4 : _coalDrillCount, BuildingsLevelUpMenu.LevelNow[4]);
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                _coalDrillCount++;
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                    }
                    else if (changedTile == _buildings[4])
                    {
                        if (ShopMenu.intTokens >= 50)
                        {
                            _objectInGround.SetTile(cellPosition, _buildings[4]);
                            ShopMenu.intTokens -= 50;
                        }
                        else Error("You don't have enough tokens");
                    }
                    else if (changedTile == _buildings[5])
                    {
                        if (ShopMenu.intTokens >= 1500)
                        {
                            _objectInGround.SetTile(cellPosition, _buildings[5]);
                            ShopMenu.intTokens -= 1500;
                            LineCreate();
                        }
                        else Error("You don't have enough tokens");
                    }
                    else if (changedTile == _buildings[6])
                    {
                        if (ShopMenu.intTokens >= 1500)
                        {
                            _objectInGround.SetTile(cellPosition, _buildings[6]);
                            LineCheck();
                            ShopMenu.intTokens -= 1500;
                        }
                        else Error("You don't have enough tokens");
                    }
                    else if (hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite == cannonHb)
                    {
                        if (ShopMenu.intTokens >= 1000)
                        {
                            cannonBoolArr[cellPosition.x][cellPosition.y] = true;
                            var can = Instantiate(cannon, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f), Quaternion.identity, _grid.GetChild(3));
                            can.name = cellPosition.ToString();
                            ShopMenu.intTokens -= 1000;
                        }
                        else Error("You don't have enough tokens");
                    }
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (_objectInGround.GetTile(cellPosition) == null) return;
            if (_objectInGround.GetTile(cellPosition) == _buildings[6] && IsConnected(false)) {
                Error("You cannot remove a generator while it is connected");
                return;
            }
            TileBase buOig = _objectInGround.GetTile(cellPosition);
            if (buOig == _buildings[0]) _ironDrillCount--;
            else if (buOig.name == $"drillIronTile{buOig.name[^1]}") _ironDrillCount--;
            else if (buOig == _buildings[1]) _goldDrillCount--;
            else if (buOig.name == $"drillGoldTile{buOig.name[^1]}") _goldDrillCount--;
            else if (buOig == _buildings[2]) _tinDrillCount--;
            else if (buOig.name == $"drillTinTile{buOig.name[^1]}") _tinDrillCount--;
            else if (buOig == _buildings[3]) _copperDrillCount--;
            else if (buOig.name == $"drillCopperTile{buOig.name[^1]}") _copperDrillCount--;
            Transform cannonForDelete = _grid.GetChild(3).Find($"{cellPosition}");
            if (cannonForDelete != null) {
                Destroy(cannonForDelete.gameObject);
                cannonBoolArr[cellPosition.x][cellPosition.y] = false;
            }
            Transform gameObjWithLine = _lineGroup.Find($"{cellPosition}");
            if (gameObjWithLine != null) gameObjWithLine.GetComponent<Line>().LineDelete();
            if (_objectInGround.GetTile(cellPosition) != emptyTile) _objectInGround.SetTile(cellPosition, null);
        }
    }

    private static int GetCostForDrill(int mining, int drillCount, int drillLevel)
    {
        if (drillLevel == 1) return 60 * mining * mining * (drillCount / 8 < 1 ? 1 : drillCount / 8) / 2;
        return 60 * mining * mining * (drillCount / 8 < 1 ? 1 : drillCount / 8) * drillLevel / 2;
    }

    private bool IsConnected(bool generatorCleanLock)
    {
        for (int x = cellPosition.x - 3; x < cellPosition.x + 4; x++)
        {
            for (int y = cellPosition.y - 3; y < cellPosition.y + 4; y++)
            {
                Transform gm = _lineGroup.Find($"{new Vector3Int(x, y, 0)}");
                if (gm == null) continue;
                if (Vector3Int.FloorToInt(gm.GetComponent<LineRenderer>().GetPosition(1)) == cellPosition) generatorCleanLock = true;
            }
        }

        return generatorCleanLock;
    }

    private void LineCreate()
    {
        var lp = Instantiate(linePrefab, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f), Quaternion.identity, _lineGroup);
        lp.name = cellPosition.ToString();
        lp.GetComponent<LineRenderer>().SetPosition(1, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f));
        lp.GetComponent<Line>().LineFilling();
        LineCheck();
    }

    private void LineCheck()
    {
        for (int x = cellPosition.x - 3; x < cellPosition.x + 4; x++)
        {
            for (int y = cellPosition.y - 3; y < cellPosition.y + 4; y++)
            {
                Vector3Int coord = new Vector3Int(x, y, 0);
                Transform gm = _lineGroup.Find($"{coord}");
                TileBase tile = _objectInGround.GetTile(coord);
                if (tile == null || tile == _buildings[6]) continue;
                gm.GetComponent<Line>().LineSet();
            }
        }
    }

    private void Error(string error)
    {
        errorText.text = error;
        StopCoroutine(nameof(ShowText));
        StartCoroutine(nameof(ShowText));
    }

    private IEnumerator ShowText()
    {
        Color textColor = errorText.color;
        textColor.a = 1;
        errorText.color = textColor;
        float hideTime = 2f;
        float timer = hideTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            textColor.a = 1f / hideTime * timer;
            errorText.color = textColor;
            yield return null;
        }
    }

    private static IEnumerator OncePerSecond()
    {
        while (true)
        {
            for (int i = 0; i < ConnectedTinDrillCount.Length; i++) {
                _tin += 6 * (i + 1) * ConnectedTinDrillCount[i];
                _iron += 4 * (i + 1) * ConnectedIronDrillCount[i];
                _copper += 2 * (i + 1) * ConnectedCopperDrillCount[i];
                _gold += 1 * (i + 1) * ConnectedGoldDrillCount[i];
            }
            yield return new WaitForSeconds(1);
        }
    }
}