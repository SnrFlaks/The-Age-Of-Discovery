using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;

public class Buildings : MonoBehaviour
{
    [SerializeField] private PipeManager pipeManager;
    [SerializeField] private MiningManager miningManager;
    [SerializeField] private HotBar hotBar;
    [SerializeField] private Text errorText;
    [SerializeField] private GameObject cannon;
    [SerializeField] private Sprite cannonHb;
    [SerializeField] private Sprite empty;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private Text[] ore;
    [SerializeField] private Text[] ingot;
    [SerializeField] private TileBase[] allowedToRotatePipes;
    [SerializeField] private TileBase[] rotatedPipes;
    public bool pipeRotateMode = false;
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

    public Text tokensText;
    private Vector3 point;
    public Vector3Int cellPosition;
    private Vector3Int firstPosition;
    private Vector3Int secondPosition;

    private bool mouseLock = false;

    private Transform _grid;
    private Transform _lineGroup;
    private Transform _pipeGroup;

    private Camera mainCam;

    public HashSet<Vector3Int> _buildingsDict;
    public Dictionary<Vector3Int, Transform> _lineGroupDict = new Dictionary<Vector3Int, Transform>();


    private void Awake()
    {
        mainCam = Camera.main;
        _buildings = BuildingsList.buildings;
        _buildingsName = BuildingsList.buildingsName;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        _objectInGround = transform.GetChild(1).GetComponent<Tilemap>();
        _grid = transform;
        _lineGroup = _grid.GetChild(2);
        _buildingsDict = new HashSet<Vector3Int>();
        for (int i = 0; i < 500; i++)
        {
            for (int j = 0; j < 500; j++)
            {
                if (_objectInGround.GetTile(new Vector3Int(i, j))) _buildingsDict.Add(new Vector3Int(i, j));
            }
        }
        for (int i = 0; i < ConnectedDrillCount.Length; i++) ConnectedDrillCount[i] = new int[7];
        ShopMenu.tokens = tokensText;
        ShopMenu.tokens.text = "Tokens: \n" + ShopMenu.intTokens;
    }


    async private void Update()
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
                if (hotBar.CreateLock == false && Base.createLockHub == false)
                {
                    if (hotBar.HotBarSelect[hotBar.hotBarButtonSelect])
                    {
                        Sprite hotBarSprite = hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite;
                        TileBase changedTile;
                        if (hotBarSprite == cannonHb) changedTile = null;
                        else if (hotBarSprite == empty) changedTile = null;
                        else if (hotBarSprite == pipeManager.pipesSprite[0]) changedTile = pipeManager.pipesArray[0];
                        else changedTile = _buildings[Array.IndexOf(BuildingsList.buildingsIcon, hotBarSprite)]!;
                        if (changedTile != _buildings[6])
                        {
                            mouseLock = true;
                            int minX = Mathf.Clamp(Mathf.Min(firstPosition.x, secondPosition.x), 0, 499);
                            int maxX = Mathf.Clamp(Mathf.Max(firstPosition.x, secondPosition.x), 0, 499);
                            int minY = Mathf.Clamp(Mathf.Min(firstPosition.y, secondPosition.y), 0, 499);
                            int maxY = Mathf.Clamp(Mathf.Max(firstPosition.y, secondPosition.y), 0, 499);
                            for (int x = minX; x <= maxX; x++)
                            {
                                await UniTask.Yield();
                                for (int y = minY; y <= maxY; y++)
                                {
                                    cellPosition = new Vector3Int(x, y, 0);
                                    if (_objectInGround.GetTile(cellPosition)) continue;
                                    if (_buildingsDict.Contains(cellPosition)) continue;
                                    if (_ground.GetTile(cellPosition) == waterTile) continue;
                                    SetBuildings(changedTile);
                                }
                            }
                            mouseLock = false;
                        }
                        else Error("Connectors cannot be placed by area");
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0) && Input.GetKeyDown(KeyCode.LeftShift) == false && mouseLock == false && pipeRotateMode == false)
        {
            point = mainCam.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = _ground.WorldToCell(point);
            if ((cellPosition.x >= 0 && cellPosition.x < 500) && (cellPosition.y >= 0 && cellPosition.y < 500))
            {
                if (_buildingsDict.Contains(cellPosition) == false && _objectInGround.GetTile(cellPosition) == null && hotBar.CreateLock == false && Base.createLockHub == false && _ground.GetTile(cellPosition) != waterTile)
                {
                    if (hotBar.HotBarSelect[hotBar.hotBarButtonSelect])
                    {
                        Sprite hotBarSprite = hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite;
                        TileBase changedTile;
                        if (hotBarSprite == cannonHb) changedTile = null;
                        else if (hotBarSprite == empty) changedTile = null;
                        else if (hotBarSprite == pipeManager.pipesSprite[0]) changedTile = pipeManager.pipesArray[0];
                        else changedTile = _buildings[Array.IndexOf(BuildingsList.buildingsIcon, hotBarSprite)]!;
                        SetBuildings(changedTile);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && Input.GetKeyDown(KeyCode.LeftShift) == false && mouseLock == false && pipeRotateMode)
        {
            point = mainCam.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = _ground.WorldToCell(point);
            for (int i = 0; i < allowedToRotatePipes.Length; i++)
            {
                if (_objectInGround.GetTile(cellPosition) == allowedToRotatePipes[i] && hotBar.CreateLock == false && Base.createLockHub == false)
                {
                    if (pipeManager._pipeGroupDict.TryGetValue(cellPosition, out Pipe pipe))
                    {
                        int offsetY = 0;
                        int offsetX = 0;
                        int limit = 0;
                        _objectInGround.SetTile(cellPosition, rotatedPipes[i]);
                        pipe.currentTile = _objectInGround.GetTile(_objectInGround.WorldToCell(cellPosition));
                        pipe.ChangeDirection(rotatedPipes[i]);
                        TileBase[] tiles = pipe.GetNeighborTiles(offsetY, offsetX);
                        do
                        {
                            Vector3Int[] offsets = new Vector3Int[] {
                                new Vector3Int(0, 1 + offsetY, 0),
                                new Vector3Int(1 + offsetY, 0, 0),
                                new Vector3Int(0, 0 - (1 + offsetX), 0),
                                new Vector3Int(0 - (1 + offsetX), 0, 0)
                            };
                            for (int j = 0; j < 4; j++)
                            {
                                Vector3Int adjustedPosition = cellPosition + offsets[j];
                                if (tiles[j] == allowedToRotatePipes[i] && pipeManager._pipeGroupDict.TryGetValue(adjustedPosition, out Pipe pipeWithOffset))
                                {
                                    _objectInGround.SetTile(Vector3Int.FloorToInt(adjustedPosition), rotatedPipes[i]);
                                    pipeWithOffset.currentTile = _objectInGround.GetTile(Vector3Int.FloorToInt(adjustedPosition));
                                    pipeWithOffset.ChangeDirection(rotatedPipes[i]);
                                    if (j < 2) offsetY++;
                                    else offsetX++;
                                    tiles = pipe.GetNeighborTiles(offsetY, offsetX);
                                }
                            }
                            limit++;
                        } while ((tiles[0] == allowedToRotatePipes[i] || tiles[1] == allowedToRotatePipes[i] || tiles[2] == allowedToRotatePipes[i] || tiles[3] == allowedToRotatePipes[i]) && limit < 500);
                    }
                    break;
                }
            }
        }
        else if (Input.GetMouseButton(1) && mouseLock == false)
        {
            point = mainCam.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = _ground.WorldToCell(point);
            Transform cannonForDelete = _grid.GetChild(3).Find($"{cellPosition}");
            TileBase buildingsOnGround = _objectInGround.GetTile(cellPosition);
            if (buildingsOnGround == null && cannonForDelete == null) return;
            else if (buildingsOnGround == _buildings[6] && IsConnected(false))
            {
                Error("You cannot remove a generator while it is connected");
                return;
            }
            if (buildingsOnGround != null)
            {
                if (buildingsOnGround.name == $"drillIronTile{buildingsOnGround.name[^1]}") _ironDrillCount--;
                else if (buildingsOnGround.name == $"drillGoldTile{buildingsOnGround.name[^1]}") _goldDrillCount--;
                else if (buildingsOnGround.name == $"drillTinTile{buildingsOnGround.name[^1]}") _tinDrillCount--;
                else if (buildingsOnGround.name == $"drillCopperTile{buildingsOnGround.name[^1]}") _copperDrillCount--;
            }
            else if (cannonForDelete != null)
            {
                Destroy(cannonForDelete.gameObject);
            }
            if (_lineGroupDict.TryGetValue(cellPosition, out Transform gameObjWithLine))
            {
                gameObjWithLine.GetComponent<Line>().LineDelete();
                _lineGroupDict.Remove(cellPosition);
            }
            else if (pipeManager._pipeGroupDict.TryGetValue(cellPosition, out Pipe pipe))
            {
                pipeManager._pipeGroupDict.Remove(cellPosition);
                pipeManager._pipeConnectionsDict.Remove(cellPosition);
                RefreshNeighborsPipes();
            }
            else if (miningManager._drillDict.TryGetValue(cellPosition, out MiningManager.Drill drill))
            {
                miningManager._drillDict.Remove(cellPosition);
                RefreshNeighborsPipes();
            }
            if (buildingsOnGround != emptyTile)
            {
                _objectInGround.SetTile(cellPosition, null);
                _buildingsDict.Remove(cellPosition);
                RefreshNeighborsPipes();
            }
        }
    }

    private void SetBuildings(TileBase changedTile)
    {
        if (changedTile == _buildings[1]) SetDrill(_ground.GetTile(cellPosition).name);
        else if (changedTile == _buildings[4]) PutBuilding(50, false, false, changedTile);
        else if (changedTile == _buildings[5]) PutBuilding(1500, true, false, changedTile);
        else if (changedTile == _buildings[6]) PutBuilding(1500, false, true, changedTile);
        else if (changedTile == pipeManager.pipesArray[0]) PutPipe(100, changedTile);
        else if (hotBar.transform.GetChild(hotBar.hotBarButtonSelect).GetChild(0).GetComponentInChildren<Image>().sprite == cannonHb)
        {
            if (ShopMenu.intTokens >= 1000)
            {
                var can = Instantiate(cannon, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f), Quaternion.identity, _grid.GetChild(3));
                can.name = cellPosition.ToString();
                ShopMenu.intTokens -= 1000;
            }
            else Error("You don't have enough tokens");
        }
    }
    private void PutPipe(int buildingCost, TileBase changedTile)
    {
        if (ShopMenu.intTokens >= buildingCost)
        {
            _objectInGround.SetTile(cellPosition, changedTile);
            _buildingsDict.Add(cellPosition);
            ShopMenu.intTokens -= buildingCost;
            PipeCreate();
        }
        else Error("You don't have enough tokens");
    }
    private void PutBuilding(int buildingCost, bool lineCreate, bool lineCheck, TileBase changedTile)
    {
        if (ShopMenu.intTokens >= buildingCost)
        {
            _objectInGround.SetTile(cellPosition, changedTile);
            _buildingsDict.Add(cellPosition);
            RefreshNeighborsPipes();
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
            //miningManager._drillDict.Add(cellPosition, new MiningManager.Drill(_lineGroupDict[cellPosition].GetComponent<Line>()._isPowered, drillNumber, cellPosition));
        }
        else Error("You don't have enough tokens");
        return drillCount;
    }

    private void PutDrill(int drillNumber)
    {
        string drillName = drillNumber == 0 ? "drillTinTile" : drillNumber == 1 ? "drillIronTile" : drillNumber == 2 ? "drillCopperTile" : "drillGoldTile";
        _objectInGround.SetTile(cellPosition, BuildingsLevelUpMenu.LevelNow[drillNumber] == 1 ? _buildings[drillNumber] : _buildings[Array.IndexOf(_buildingsName, $"{drillName}{BuildingsLevelUpMenu.LevelNow[drillNumber]}")]);
        _buildingsDict.Add(cellPosition);
        RefreshNeighborsPipes();
    }

    private int GetCostForDrill(int mining, int drillCount, int drillLevel) => 60 * mining * mining * (drillCount / 8 < 1 ? 1 : drillCount / 8) * drillLevel == 1 ? 1 : drillLevel / 2;

    private bool IsConnected(bool generatorCleanLock)
    {
        for (int x = cellPosition.x - 3; x < cellPosition.x + 4; x++)
        {
            for (int y = cellPosition.y - 3; y < cellPosition.y + 4; y++)
            {
                if (_lineGroupDict.TryGetValue(new Vector3Int(x, y, 0), out Transform gm) == false) continue;
                if (Vector3Int.FloorToInt(gm.GetComponent<LineRenderer>().GetPosition(1)) == cellPosition) generatorCleanLock = true;
            }
        }
        return generatorCleanLock;
    }

    private void LineCreate()
    {
        GameObject lineP = Instantiate(linePrefab, new Vector2(cellPosition.x + 0.5f, cellPosition.y + 0.5f), Quaternion.identity, _lineGroup);
        _lineGroupDict[new Vector3Int(cellPosition.x, cellPosition.y, 0)] = lineP.transform;
        lineP.name = cellPosition.ToString();
    }

    public void LineCheck()
    {
        for (int x = cellPosition.x - 3; x < cellPosition.x + 4; x++)
        {
            for (int y = cellPosition.y - 3; y < cellPosition.y + 4; y++)
            {
                if (_lineGroupDict.TryGetValue(new Vector3Int(x, y, 0), out Transform gm)) gm.GetComponent<Line>().LineSet();
            }
        }
    }
    public void PipeCreate()
    {
        pipeManager._pipeGroupDict.Add(cellPosition, new Pipe(pipeManager, this, cellPosition, _objectInGround));
        pipeManager._pipeGroupDict[cellPosition].ChangeCurrentTile();
        RefreshNeighborsPipes();
    }
    public void RefreshNeighborsPipes()
    {
        Vector3Int[] offsets = new Vector3Int[] {
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, 0, 0)
        };
        for (int i = 0; i < 4; i++)
        {
            Vector3Int adjustedPosition = cellPosition + offsets[i];
            if (pipeManager._pipeGroupDict.TryGetValue(adjustedPosition, out Pipe pipe)) pipe.ChangeCurrentTile();
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