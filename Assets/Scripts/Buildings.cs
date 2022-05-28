using System;
using System.Collections;
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
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private TileBase waterTile;

    [SerializeField] private Text[] ore;
    [SerializeField] private Text[] ingot;
    public static int ConnectedIronDrillCount;
    public static int ConnectedGoldDrillCount;
    public static int ConnectedTinDrillCount;
    public static int ConnectedCopperDrillCount; 
    public static int ConnectedFurnaceCount;

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

    public static readonly bool[][] cannonBoolArr = new bool[500][];
    public Text tokensText;
    private Vector3 point;
    private Vector3Int cellPosition;

    private Transform _grid;
    private Transform _lineGroup;

    private void Start()
    {
        //ShopMenu.intTokens = PlayerPrefs.GetInt("tokens") == 0 ? 100000 : PlayerPrefs.GetInt("tokens");
        _buildings = ItemList.buildings;
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
        point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
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
                    TileBase changedTile = hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite == cannonHb ? null : ItemList.buildings[Array.IndexOf(ItemList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)]!;
                    if (changedTile == _buildings[0])
                    {
                        if (_ground.GetTile(cellPosition).name == "ironRandomTile")
                        {
                            if (ShopMenu.intTokens >= 2500)
                            {
                                _objectInGround.SetTile(cellPosition, _buildings[0]);
                                ShopMenu.intTokens -= 2500;
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                        else if (_ground.GetTile(cellPosition).name == "goldRandomTile")
                        {
                            if (ShopMenu.intTokens >= 5000)
                            {
                                _objectInGround.SetTile(cellPosition, _buildings[1]);
                                ShopMenu.intTokens -= 5000;
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                        else if (_ground.GetTile(cellPosition).name == "tinRandomTile")
                        {
                            if (ShopMenu.intTokens >= 1000)
                            {
                                _objectInGround.SetTile(cellPosition, _buildings[2]);
                                ShopMenu.intTokens -= 1000;
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                        else if (_ground.GetTile(cellPosition).name == "copperRandomTile")
                        {
                            if (ShopMenu.intTokens >= 3500)
                            {
                                _objectInGround.SetTile(cellPosition, _buildings[3]);
                                ShopMenu.intTokens -= 3500;
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
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
            if (_objectInGround.GetTile(cellPosition) == _buildings[6] && IsConnected(false)) {
                Error("You cannot remove a generator while it is connected");
                return;
            }
            _objectInGround.SetTile(cellPosition, null);
            Transform cannonForDelete = _grid.GetChild(3).Find($"{cellPosition}");
            if (cannonForDelete != null) {
                Destroy(cannonForDelete.gameObject);
                cannonBoolArr[cellPosition.x][cellPosition.y] = false;
            }
            Transform gameObjWithLine = _lineGroup.Find($"{cellPosition}");
            if (gameObjWithLine != null) gameObjWithLine.GetComponent<Line>().LineDelete();
            ConnectedIronDrillCount = ConnectedBuildingsCount(0);
            ConnectedGoldDrillCount = ConnectedBuildingsCount(1);
            ConnectedTinDrillCount = ConnectedBuildingsCount(2);
            ConnectedCopperDrillCount = ConnectedBuildingsCount(3);
            ConnectedFurnaceCount = ConnectedBuildingsCount(5);
        }
    }

    private bool IsConnected(bool generatorCleanLock)
    {
        for (int x = cellPosition.x - 4; x < cellPosition.x + 5; x++)
        {
            for (int y = cellPosition.y - 4; y < cellPosition.y + 5; y++)
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
        for (int x = cellPosition.x - 4; x < cellPosition.x + 5; x++)
        {
            for (int y = cellPosition.y - 4; y < cellPosition.y + 5; y++)
            {
                Transform gm = _lineGroup.Find($"{new Vector3Int(x, y, 0)}");
                if (_objectInGround.GetTile(new Vector3Int(x, y, 0)) == null || _objectInGround.GetTile(new Vector3Int(x, y, 0)) == _buildings[6]) continue;
                gm.GetComponent<Line>().LineSet();
                ConnectedIronDrillCount = ConnectedBuildingsCount(0);
                ConnectedGoldDrillCount = ConnectedBuildingsCount(1);
                ConnectedTinDrillCount = ConnectedBuildingsCount(2);
                ConnectedCopperDrillCount = ConnectedBuildingsCount(3);
                ConnectedFurnaceCount = ConnectedBuildingsCount(5);
            }
        }
    }

    private int ConnectedBuildingsCount(int build)
    {
        int connectedBuildCount = 0;
        for (int i = 0; i < _lineGroup.childCount; i++)
        {
            if (_objectInGround.GetTile(Vector3Int.FloorToInt(_lineGroup.GetChild(i).transform.position)) != _buildings[build] || !_lineGroup.GetChild(i).GetComponent<Line>()._isPowered) continue;
            connectedBuildCount++;
        }
        return connectedBuildCount;
    }

    private void Error(string error)
    {
        errorText.text = error;
        StopCoroutine("ShowText");
        StartCoroutine("ShowText");
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
            _tin += 8 * ConnectedTinDrillCount;
            _iron += 4 * ConnectedIronDrillCount;
            _copper += 2 * ConnectedCopperDrillCount;
            _gold += 1 * ConnectedGoldDrillCount;
            yield return new WaitForSeconds(1);
        }
    }
}