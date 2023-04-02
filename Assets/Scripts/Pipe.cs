using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Profiling;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Pipes pipes;
    [SerializeField] private Buildings buildings;
    [SerializeField] private bool[] isNeighbors = new bool[4];
    [SerializeField] private bool[][] pipesOutput = new bool[4][];
    public bool[][] PipesOutput
    {
        get { return pipesOutput; }
        set { pipesOutput = value; }
    }
    public int numOfNeighbors;
    public bool isTimelyBlocked = false;
    public bool isBusy;
    public Vector3Int pos;
    private Tilemap _objectInGround;
    private Vector3 _position;
    private Vector3Int _cellPosition;
    public TileBase currentTile;
    public bool[] isNeighboringPipes;
    public Vector3[] neighboringPipesPosition;
    public Pipe[] neighboringPipesComponent;
    private Vector3Int[] offsets = new Vector3Int[] {
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0)
    };
    void Awake()
    {
        pipes = transform.parent.GetComponent<Pipes>();
        _position = transform.position;
        _objectInGround = Buildings._objectInGround;
        buildings = transform.parent.parent.GetComponent<Buildings>();
        _cellPosition = _objectInGround.WorldToCell(_position);
        isNeighboringPipes = new bool[4];
        neighboringPipesPosition = new Vector3[4];
        neighboringPipesComponent = new Pipe[4];
        for (int i = 0; i < 4; i++) PipesOutput[i] = new bool[4];
    }
    public void PipeDelete()
    {
        Destroy(gameObject);
    }
    public void ChangeCurrentTile()
    {
        numOfNeighbors = 0;
        for (int i = 0; i < 4; i++)
        {
            isNeighbors[i] = CheckNeighbors(i);
            if (isNeighbors[i]) numOfNeighbors++;
            isNeighboringPipes[i] = pipes._pipeGroupDict.ContainsKey(_cellPosition + offsets[i]);
        }
        int fromBinaryIndex = Convert.ToInt32($"{Convert.ToInt32(isNeighbors[0])}{Convert.ToInt32(isNeighbors[1])}{Convert.ToInt32(isNeighbors[2])}{Convert.ToInt32(isNeighbors[3])}", 2);
        int index = fromBinaryIndex + (GetCurrentTileType() * 16);
        if (_objectInGround.GetTile(_cellPosition) == pipes.pipesArray[index]) return;
        _objectInGround.SetTile(_cellPosition, pipes.pipesArray[index]);
        currentTile = pipes.pipesArray[index];
        for (int i = 0; i < 4; i++) ChangeNeighborsData(i);
    }
    private bool CheckNeighbors(int side)
    {
        TileBase tile = null;
        Vector3Int adjustedPosition = _cellPosition + offsets[side];
        tile = _objectInGround.GetTile(adjustedPosition);
        if (tile != null && tile.name.StartsWith("TA_Pipes_") || buildings._lineGroupDict.TryGetValue(adjustedPosition, out Transform lipe))
        {
            ChangeNeighborsData(-1);
            return true;
        }
        return false;
    }
    public void ChangeNeighborsData(int side)
    {
        Vector3Int adjustedPosition = side == -1 ? _cellPosition : _cellPosition + offsets[side];
        if (pipes._pipeGroupDict.TryGetValue(adjustedPosition, out Pipe mainPipe))
        {
            for (int i = 0; i < 4; i++)
            {
                if (pipes._pipeGroupDict.TryGetValue(adjustedPosition + offsets[i], out Pipe neiPipe))
                {
                    mainPipe.neighboringPipesPosition[i] = neiPipe.transform.localPosition;
                    mainPipe.neighboringPipesComponent[i] = neiPipe;
                }
            }
        }
    }
    public int GetCurrentTileType()
    {
        int type = Mathf.FloorToInt(Convert.ToInt32(_objectInGround.GetTile(_cellPosition).name.Replace("TA_Pipes_", "")) / 16);
        int index = Convert.ToInt32(_objectInGround.GetTile(_cellPosition).name.Replace("TA_Pipes_", ""));
        if (index >= 80)
        {
            if (index % 2 == 0) type = Mathf.FloorToInt((90 - index) / 2) - 5;
            else if (index % 2 == 1) type = Mathf.FloorToInt((90 - index) / 2) - 5;
            type *= type;
            type -= type == 0 ? 0 : 1;
        }
        return type;
    }
    public TileBase[] GetNeighborTiles(int offsetF, int offsetS)
    {
        TileBase[] tiles = new TileBase[4];
        Vector3Int[] offsets = new Vector3Int[] {
            new Vector3Int(0, 1 + offsetF, 0),
            new Vector3Int(1 + offsetF, 0, 0),
            new Vector3Int(0, 0 - (1 + offsetS), 0),
            new Vector3Int(0 - (1 + offsetS), 0, 0)
        };
        for (int i = 0; i < 4; i++)
        {
            Vector3Int adjustedPosition = _cellPosition + offsets[i];
            if (CheckNeighbors(i)) tiles[i] = _objectInGround.GetTile(adjustedPosition);
        }
        return tiles;
    }
    public Transform[] GetNeighborPipes(int offsetF, int offsetS)
    {
        Vector3Int[] offsets = new Vector3Int[] {
            new Vector3Int(0, 1 + offsetF, 0),
            new Vector3Int(1 + offsetF, 0, 0),
            new Vector3Int(0, 0 - (1 + offsetS), 0),
            new Vector3Int(0 - (1 + offsetS), 0, 0)
        };
        Transform[] transforms = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            Vector3Int adjustedPosition = _cellPosition + offsets[i];
            if (CheckNeighbors(i) && pipes._pipeGroupDict.TryGetValue(adjustedPosition, out Pipe pipe)) transforms[i] = pipe.transform;
        }
        return transforms;
    }
}
