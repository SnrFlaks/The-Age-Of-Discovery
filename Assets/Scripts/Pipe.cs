using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Profiling;

public class Pipe
{
    private PipeManager pipeManager;
    private Buildings buildings;
    private bool[] isNeighbors = new bool[4];
    public int numOfNeighbors;
    public bool isTimelyBlocked = false;
    public bool isBusy;
    public Vector3Int pos;
    private Tilemap _objectInGround;
    private Vector3 _position;
    private Vector3Int _cellPosition;
    public TileBase currentTile;
    public bool[] isNeighboringPipes;
    private Vector3Int[] offsets = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left };

    public Pipe(PipeManager pM, Buildings b, Vector3Int _cP, Tilemap _oIg)
    {
        pipeManager = pM;
        buildings = b;
        _cellPosition = _cP;
        _objectInGround = _oIg;
        isNeighboringPipes = new bool[4];
    }

    public void ChangeCurrentTile()
    {
        numOfNeighbors = 0;
        for (int i = 0; i < 4; i++)
        {
            isNeighbors[i] = CheckNeighbors(i);
            if (isNeighbors[i]) numOfNeighbors++;
            isNeighboringPipes[i] = pipeManager._pipeGroupDict.ContainsKey(_cellPosition + offsets[i]);
        }
        Profiler.BeginSample("1");
        int fromBinaryIndex = ((isNeighbors[0] ? 1 : 0) << 3) |
                          ((isNeighbors[1] ? 1 : 0) << 2) |
                          ((isNeighbors[2] ? 1 : 0) << 1) |
                          ((isNeighbors[3] ? 1 : 0) << 0);
        int index = fromBinaryIndex + (GetCurrentTileType() * 16);
        if (_objectInGround.GetTile(_cellPosition) == pipeManager.pipesArray[index]) return;
        _objectInGround.SetTile(_cellPosition, pipeManager.pipesArray[index]);
        currentTile = pipeManager.pipesArray[index];
        ChangeDirection(_objectInGround.GetTile(_cellPosition));
    }

    public void ChangeDirection(TileBase tile)
    {
        int direction = -1;
        for (int i = 0; i < 4; i++) if (tile == pipeManager.DirectionTile[i]) direction = i;
        if (pipeManager._pipeConnectionsDict.ContainsKey(_cellPosition))
        {
            pipeManager._pipeConnectionsDict[_cellPosition] = new PipeManager.PipeConnection(_cellPosition, isNeighboringPipes, pipeManager._pipeConnectionsDict[_cellPosition].IsBusy, numOfNeighbors, direction);
        }
        else pipeManager._pipeConnectionsDict.Add(_cellPosition, new PipeManager.PipeConnection(_cellPosition, isNeighboringPipes, false, numOfNeighbors, direction));
    }

    private bool CheckNeighbors(int side)
    {
        TileBase tile = null;
        Vector3Int adjustedPosition = _cellPosition + offsets[side];
        tile = _objectInGround.GetTile(adjustedPosition);
        if (tile != null && tile.name.StartsWith("TA_Pipes_") || buildings._lineGroupDict.TryGetValue(adjustedPosition, out Transform lipe)) return true;
        return false;
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
}
