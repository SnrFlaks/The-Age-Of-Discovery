using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private GameObject hotBar;
    [SerializeField] private Text errorText;
    [SerializeField] private GameObject cannon;
    [SerializeField] private Sprite cannonHB;
    [SerializeField] private GameObject linePrefab;

    [SerializeField] private Text[] Ore;
    [SerializeField] private Text[] Ingot;
    public static float _ironDrillCount;
    private int _connectedIronDrillCount = 0;
    public static float _goldDrillCount;
    private int _connectedGoldDrillCount = 0;
    public static float _tinDrillCount;
    private int _connectedTinDrillCount = 0;
    public static float _copperDrillCount;
    private int _connectedCopperDrillCount = 0;

    public static float _tin;
    public static float _iron;
    public static float _copper;
    public static float _gold;

    private int _furnaceCount;
    public static int _connectedFurnace;

    public static int _tinIngot = 0;
    public static int _ironIngot;
    public static int _copperIngot = 0;
    public static int _goldIngot = 0;

    private Tilemap _ground;
    public static Tilemap _objectInGround;
    private TileBase[] _buildings;
    
    public Text ttokens;

    private void Start()
    {
        //ShopMenu.intTokens = PlayerPrefs.GetInt("tokens") == 0 ? 100000 : PlayerPrefs.GetInt("tokens");
        _buildings = ItemList.buildings;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
        StartCoroutine(OncePerSecond());
        ShopMenu.tokens = ttokens;
        ShopMenu.tokens.text = "Tokens: \n" + ShopMenu.intTokens;
    }

    private void Update()
    {
        var point = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosition = _ground.WorldToCell(point);
        ttokens.text = "Tokens: \n" + ShopMenu.intTokens;
        Ore[0].text = "Tin: \n" + Mathf.Round(_tin);
        Ore[1].text = "Iron: \n" + Mathf.Round(_iron);
        Ore[2].text = "Copper: \n" + Mathf.Round(_copper);
        Ore[3].text = "Gold: \n" + Mathf.Round(_gold);
        Ingot[0].text = "Tin ingot: \n" + _tinIngot;
        Ingot[1].text = "Iron ingot: \n" + _ironIngot;
        Ingot[2].text = "Copper ingot: \n" + _copperIngot;
        Ingot[3].text = "Gold ingot: \n" + _goldIngot;
        if (Input.GetMouseButton(0) && HotBar.CreateLock == false && Base.createLockHub == false && CannonShoot.createLockCannon == false && cellPosition.x >= 0 && cellPosition.y >= 0)
        {
            Debug.Log(_connectedFurnace);
            for (var i = 0; i < HotBar.HotBarSelect.Length; i++)
            {
                if (HotBar.HotBarSelect[i])
                {
                    TileBase changedTile = (hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite == cannonHB) ? null : ItemList.buildings[Array.IndexOf(ItemList.buildingsIcon, hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite)]!;
                    if (changedTile == _buildings[0] && _objectInGround.GetTile(cellPosition) == null)
                    {
                        if (_ground.GetTile(cellPosition).name == "ironRandomTile")
                        {
                            if (ShopMenu.intTokens >= 2500)
                            {
                                _objectInGround.SetTile(cellPosition, _buildings[0]);
                                ShopMenu.intTokens -= 2500;
                                PlayerPrefs.SetInt("tokens", ShopMenu.intTokens);
                                _ironDrillCount++;
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
                                _goldDrillCount++;
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
                                _tinDrillCount++;
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
                                _copperDrillCount++;
                                LineCreate();
                            }
                            else Error("You don't have enough tokens");
                        }
                    }
                    else if (changedTile == _buildings[4] && _objectInGround.GetTile(cellPosition) == null)
                    {
                        if (ShopMenu.intTokens >= 50)
                        {
                            _objectInGround.SetTile(cellPosition, _buildings[4]);
                            ShopMenu.intTokens -= 50;
                        }
                        else Error("You don't have enough tokens");
                    }
                    else if (changedTile == _buildings[5] && _objectInGround.GetTile(cellPosition) == null)
                    {
                        if (ShopMenu.intTokens >= 1500)
                        {
                            _objectInGround.SetTile(cellPosition, _buildings[5]);
                            ShopMenu.intTokens -= 1500;
                            _furnaceCount++;
                            LineCreate();
                        }
                        else Error("You don't have enough tokens");
                    }
                    else if (changedTile == _buildings[6] && _objectInGround.GetTile(cellPosition) == null)
                    {
                        if (ShopMenu.intTokens >= 1500)
                        {
                            _objectInGround.SetTile(cellPosition, _buildings[6]);
                            LineCheck();
                            ShopMenu.intTokens -= 1500;
                        }
                        else Error("You don't have enough tokens");
                    }
                    else if (hotBar.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite == cannonHB)
                    {
                        if (ShopMenu.intTokens >= 1000)
                        {
                            var can = Instantiate(cannon, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f), Quaternion.identity, gameObject.transform.GetChild(3));
                            can.name = cellPosition.ToString();
                            ShopMenu.intTokens -= 1000;
                            LineCreate();
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
            Transform gm = gameObject.transform.GetChild(2).Find($"{cellPosition}");
            if (gm != null) gm.GetComponent<Line>().LineDelete();
            _connectedIronDrillCount = ConnectedBuildingsCount(0);
            _connectedGoldDrillCount = ConnectedBuildingsCount(1);
            _connectedTinDrillCount = ConnectedBuildingsCount(2);
            _connectedCopperDrillCount = ConnectedBuildingsCount(3);
            _connectedFurnace = ConnectedBuildingsCount(5);
        }

        bool IsConnected(bool generatorCleanLock)
        {
            for (int x = cellPosition.x - 4; x < cellPosition.x + 5; x++)
            {
                for (int y = cellPosition.y - 4; y < cellPosition.y + 5; y++)
                {
                    Transform gm = gameObject.transform.GetChild(2).Find($"{new Vector3Int(x, y, 0)}");
                    if (gm == null) continue;
                    if (Vector3Int.FloorToInt(gm.GetComponent<LineRenderer>().GetPosition(1)) == cellPosition) generatorCleanLock = true;
                }
            }
            return generatorCleanLock;
        }

        void LineCreate()
        {
            var lp = Instantiate(linePrefab, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f), Quaternion.identity, gameObject.transform.GetChild(2));
            lp.name = cellPosition.ToString();
            lp.GetComponent<LineRenderer>().SetPosition(1, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f));
            lp.GetComponent<Line>().LineFilling();
            LineCheck();
        }

        void LineCheck()
        {
            for (int x = cellPosition.x - 4; x < cellPosition.x + 5; x++)
            {
                for (int y = cellPosition.y - 4; y < cellPosition.y + 5; y++)
                {
                    Transform gm = gameObject.transform.GetChild(2).Find($"{new Vector3Int(x, y, 0)}");
                    if (_objectInGround.GetTile(new Vector3Int(x, y, 0)) == null || _objectInGround.GetTile(new Vector3Int(x, y, 0)) == _buildings[6]) continue;
                    gm.GetComponent<Line>().LineSet();
                    // gm.GetComponent<Line>()._isPowered = _objectInGround.GetTile(new Vector3Int(x, y, 0)) == _buildings[6];
                    _connectedIronDrillCount = ConnectedBuildingsCount(0);
                    _connectedGoldDrillCount = ConnectedBuildingsCount(1);
                    _connectedTinDrillCount = ConnectedBuildingsCount(2);
                    _connectedCopperDrillCount = ConnectedBuildingsCount(3);
                    _connectedFurnace = ConnectedBuildingsCount(5);
                }
            }
        }
    }

    int ConnectedBuildingsCount(int build)
    {
        int connectedBuildCount = 0;
        for (int i = 0; i < gameObject.transform.GetChild(2).childCount; i++) {
            if (_objectInGround.GetTile(Vector3Int.FloorToInt(gameObject.transform.GetChild(2).GetChild(i).transform.position)) == _buildings[build] && gameObject.transform.GetChild(2).GetChild(i).GetComponent<Line>()._isPowered) connectedBuildCount++;
        }
        Debug.Log(connectedBuildCount);
        return connectedBuildCount;
    }

    void Error(string error)
    {
        errorText.text = error;
        StopCoroutine("showText");
        StartCoroutine("showText");
    }

    IEnumerator showText()
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

    IEnumerator OncePerSecond()
    {
        while (true)
        {
            _tin += 8 * _connectedTinDrillCount;
            _iron += 4 * _connectedIronDrillCount;
            _copper += 2 * _connectedCopperDrillCount;
            _gold += 1 * _connectedGoldDrillCount;
            yield return new WaitForSeconds(1);
        }
    }
}