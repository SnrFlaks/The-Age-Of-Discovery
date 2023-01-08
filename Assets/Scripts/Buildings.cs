using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Buildings : MonoBehaviour
{
    [SerializeField] private HotBar hotBar;
    [SerializeField] private Text errorText;
    [SerializeField] private GameObject cannon;
    [SerializeField] private Sprite cannonHb;
    [SerializeField] private Sprite empty;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private Text[] ore;
    [SerializeField] private Text[] ingot;
    public int[][] ConnectedDrillCount = new int[4][];
    public int ConnectedFurnaceCount;
    private int _coalDrillCount;
    private int _ironDrillCount;
    private int _goldDrillCount;
    private int _tinDrillCount;
    private int _copperDrillCount;

    private Tilemap _ground;
    public static Tilemap _objectInGround;
    private TileBase[] _buildings;
    private string[] _buildingsName;
    public Tile emptyTile;

    public static readonly bool[][] cannonBoolArr = new bool[500][];
    public Text tokensText;
    private Vector3 point;
    public Vector3Int cellPosition;
    private Vector3Int firstPosition;
    private Vector3Int secondPosition;

    private bool mouseLock = false;

    private Transform _grid;
    private Transform _lineGroup;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        _buildings = BuildingsList.buildings;
        _buildingsName = BuildingsList.buildingsName;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
        _grid = transform;
        _lineGroup = _grid.GetChild(2);
        for (int i = 0; i < ConnectedDrillCount.Length; i++) ConnectedDrillCount[i] = new int[7];
        for (int i = 0; i < cannonBoolArr.Length; i++) cannonBoolArr[i] = new bool[500];
        ShopMenu.tokens = tokensText;
        ShopMenu.tokens.text = "Tokens: \n" + ShopMenu.intTokens;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPosition = new Vector3Int(-1, -1, -1);
                point = mainCam.ScreenToWorldPoint(Input.mousePosition);
                cellPosition = _ground.WorldToCell(point);
                firstPosition = cellPosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                secondPosition = new Vector3Int(-1, -1, -1);
                point = mainCam.ScreenToWorldPoint(Input.mousePosition);
                cellPosition = _ground.WorldToCell(point);
                secondPosition = cellPosition;
                if (hotBar.CreateLock == false && Base.createLockHub == false && cellPosition.x >= 0 && cellPosition.y >= 0)
                {
                    if (hotBar.HotBarSelect[hotBar.hotBarButtonSelect])
                    {
                        Sprite hotBarSprite = hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite;
                        TileBase changedTile = hotBarSprite == cannonHb ? null : hotBarSprite == empty ? null : _buildings[Array.IndexOf(BuildingsList.buildingsIcon, hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite)]!;
                        if (changedTile != _buildings[6])
                        {
                            mouseLock = true;
                            if (secondPosition.x > firstPosition.x && firstPosition.y > secondPosition.y)
                            {
                                for (int x = firstPosition.x; x < secondPosition.x + 1; x++)
                                {
                                    for (int y = secondPosition.y; y < firstPosition.y + 1; y++)
                                    {
                                        cellPosition = new Vector3Int(x, y, cellPosition.z);
                                        if (cannonBoolArr[cellPosition.x][cellPosition.y] != true && _objectInGround.GetTile(cellPosition) == null && _ground.GetTile(cellPosition) != waterTile) SetBuildings(changedTile);
                                    }
                                }
                                mouseLock = false;
                            }
                            else if (firstPosition.x > secondPosition.x && secondPosition.y > firstPosition.y)
                            {
                                for (int x = secondPosition.x; x < firstPosition.x + 1; x++)
                                {
                                    for (int y = firstPosition.y; y < secondPosition.y + 1; y++)
                                    {
                                        cellPosition = new Vector3Int(x, y, cellPosition.z);
                                        if (cannonBoolArr[cellPosition.x][cellPosition.y] != true && _objectInGround.GetTile(cellPosition) == null && _ground.GetTile(cellPosition) != waterTile) SetBuildings(changedTile);
                                    }
                                }
                                mouseLock = false;
                            }
                            else if (secondPosition.x > firstPosition.x && secondPosition.y > firstPosition.y)
                            {
                                for (int x = firstPosition.x; x < secondPosition.x + 1; x++)
                                {
                                    for (int y = firstPosition.y; y < secondPosition.y + 1; y++)
                                    {
                                        cellPosition = new Vector3Int(x, y, cellPosition.z);
                                        if (cannonBoolArr[cellPosition.x][cellPosition.y] != true && _objectInGround.GetTile(cellPosition) == null && _ground.GetTile(cellPosition) != waterTile) SetBuildings(changedTile);
                                    }
                                }
                                mouseLock = false;
                            }
                            else if (firstPosition.x > secondPosition.x && firstPosition.y > secondPosition.y)
                            {
                                for (int x = secondPosition.x; x < firstPosition.x + 1; x++)
                                {
                                    for (int y = secondPosition.y; y < firstPosition.y + 1; y++)
                                    {
                                        cellPosition = new Vector3Int(x, y, cellPosition.z);
                                        SetBuildings(changedTile);
                                    }
                                }
                                mouseLock = false;
                            }
                        }
                        else Error("Connectors cannot be placed by area");
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.LeftShift) && mouseLock == false)
        {
            point = mainCam.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = _ground.WorldToCell(point);
            if (hotBar.CreateLock == false && Base.createLockHub == false && cannonBoolArr[cellPosition.x][cellPosition.y] != true && _objectInGround.GetTile(cellPosition) == null && _ground.GetTile(cellPosition) != waterTile && cellPosition.x >= 0 && cellPosition.y >= 0)
            {
                if (hotBar.HotBarSelect[hotBar.hotBarButtonSelect])
                {
                    Sprite hotBarSprite = hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite;
                    TileBase changedTile = hotBarSprite == cannonHb ? null : hotBarSprite == empty ? null : _buildings[Array.IndexOf(BuildingsList.buildingsIcon, hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite)]!;
                    SetBuildings(changedTile);
                }
            }
        }
        else if (Input.GetMouseButton(1) && mouseLock == false)
        {
            point = mainCam.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = _ground.WorldToCell(point);
            Transform cannonForDelete = _grid.GetChild(3).Find($"{cellPosition}");
            if (_objectInGround.GetTile(cellPosition) == null && cannonForDelete == null) return;
            else if (_objectInGround.GetTile(cellPosition) == _buildings[6] && IsConnected(false))
            {
                Error("You cannot remove a generator while it is connected");
                return;
            }
            TileBase buOig = _objectInGround.GetTile(cellPosition);
            if (buOig != null)
            {
                if (buOig.name == $"drillIronTile{buOig.name[^1]}") _ironDrillCount--;
                else if (buOig.name == $"drillGoldTile{buOig.name[^1]}") _goldDrillCount--;
                else if (buOig.name == $"drillTinTile{buOig.name[^1]}") _tinDrillCount--;
                else if (buOig.name == $"drillCopperTile{buOig.name[^1]}") _copperDrillCount--;
            }
            else
            {
                if (cannonForDelete != null)
                {
                    Destroy(cannonForDelete.gameObject);
                    cannonBoolArr[cellPosition.x][cellPosition.y] = false;
                }
            }
            Transform gameObjWithLine = _lineGroup.Find($"{cellPosition}");
            if (gameObjWithLine != null) gameObjWithLine.GetComponent<Line>().LineDelete();
            if (_objectInGround.GetTile(cellPosition) != emptyTile) _objectInGround.SetTile(cellPosition, null);
        }
    }

    private void SetBuildings(TileBase changedTile)
    {
        if (changedTile == _buildings[0]) SetDrill(_ground.GetTile(cellPosition).name);
        else if (changedTile == _buildings[4]) PutBuilding(50, false, false, changedTile);
        else if (changedTile == _buildings[5]) PutBuilding(1500, true, false, changedTile);
        else if (changedTile == _buildings[6]) PutBuilding(1500, false, true, changedTile);
        else if (hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite == cannonHb)
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

    private void PutBuilding(int buildingCost, bool lineCreate, bool lineCheck, TileBase changedTile)
    {
        if (ShopMenu.intTokens >= buildingCost)
        {
            _objectInGround.SetTile(cellPosition, changedTile);
            ShopMenu.intTokens -= buildingCost;
            if (lineCreate) LineCreate();
            if (lineCheck) LineCheck();
        }
        else Error("You don't have enough tokens");
    }

    private void SetDrill(string tileName)
    {
        if (tileName == "tinRandomTile" && ShopMenu.intTokens >= (_tinDrillCount == 0 ? 1080 : GetCostForDrill(6, _tinDrillCount <= 3 ? 4 : _tinDrillCount, BuildingsLevelUpMenu.LevelNow[0])))
        {
            _tinDrillCount = CutDrillCost(_ground.GetTile(cellPosition).name, _tinDrillCount, 0);
        }
        else if (tileName == "ironRandomTile" && ShopMenu.intTokens >= (_ironDrillCount == 0 ? 2160 : GetCostForDrill(8, _ironDrillCount <= 3 ? 4 : _ironDrillCount, BuildingsLevelUpMenu.LevelNow[1])))
        {
            _ironDrillCount = CutDrillCost(_ground.GetTile(cellPosition).name, _ironDrillCount, 1);
        }
        else if (tileName == "copperRandomTile" && ShopMenu.intTokens >= (_copperDrillCount == 0 ? 4320 : GetCostForDrill(12, _copperDrillCount <= 3 ? 4 : _copperDrillCount, BuildingsLevelUpMenu.LevelNow[2])))
        {
            _copperDrillCount = CutDrillCost(_ground.GetTile(cellPosition).name, _copperDrillCount, 2);
        }
        else if (tileName == "goldRandomTile" && ShopMenu.intTokens >= (_goldDrillCount == 0 ? 8640 : GetCostForDrill(14, _goldDrillCount <= 3 ? 4 : _goldDrillCount, BuildingsLevelUpMenu.LevelNow[3])))
        {
            _goldDrillCount = CutDrillCost(_ground.GetTile(cellPosition).name, _goldDrillCount, 3);
        }
    }

    private int CutDrillCost(string tileName, int drillCount, int drillNumber)
    {
        if (ShopMenu.intTokens >= (drillCount == 0 ? 1080 + (1080 * drillNumber) : GetCostForDrill(6 + (2 * drillNumber), drillCount <= 3 ? 4 : drillCount, BuildingsLevelUpMenu.LevelNow[drillNumber])))
        {
            PutDrill(drillNumber);
            ShopMenu.intTokens -= drillCount == 0 ? 1080 + (1080 * drillNumber) : GetCostForDrill(6 + (2 * drillNumber), drillCount <= 3 ? 4 : drillCount, BuildingsLevelUpMenu.LevelNow[drillNumber]);
            drillCount++;
            LineCreate();
        }
        else Error("You don't have enough tokens");
        return drillCount;
    }

    private void PutDrill(int drillNumber)
    {
        string drillName = drillNumber == 0 ? "drillTinTile" : drillNumber == 1 ? "drillIronTile" : drillNumber == 2 ? "drillCopperTile" : "drillGoldTile";
        _objectInGround.SetTile(cellPosition, BuildingsLevelUpMenu.LevelNow[drillNumber] == 1 ? _buildings[drillNumber] : _buildings[Array.IndexOf(_buildingsName, $"{drillName}{BuildingsLevelUpMenu.LevelNow[drillNumber]}")]);
    }

    private int GetCostForDrill(int mining, int drillCount, int drillLevel) => 60 * mining * mining * (drillCount / 8 < 1 ? 1 : drillCount / 8) * drillLevel == 1 ? 1 : drillLevel / 2;

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
        GameObject lineP = Instantiate(linePrefab, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f), Quaternion.identity, _lineGroup);
        lineP.name = cellPosition.ToString();
    }

    public void LineCheck()
    {
        for (int x = cellPosition.x - 3; x < cellPosition.x + 4; x++)
        {
            for (int y = cellPosition.y - 3; y < cellPosition.y + 4; y++)
            {
                Transform gm = _lineGroup.Find($"{new Vector3Int(x, y, 0)}");
                if (gm) gm.GetComponent<Line>().LineSet();
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
}