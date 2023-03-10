using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Pipes pipes;
    [SerializeField] private Buildings buildings;
    [SerializeField] private bool[] isNeighbors = new bool[4];
    public bool[] IsNeighbor
    {
        get { return isNeighbors; }
    }
    [SerializeField] private bool[] pipesOutput = new bool[4];
    public bool[] PipesOutput
    {
        get { return pipesOutput; }
        set { pipesOutput = value; }
    }
    private Tilemap _objectInGround;
    private Vector3 _position;
    private Vector3Int _cellPosition;
    public TileBase currentTile;
    void Awake()
    {
        pipes = transform.parent.GetComponent<Pipes>();
        _position = transform.position;
        _objectInGround = Buildings._objectInGround;
        buildings = transform.parent.parent.GetComponent<Buildings>();
        _cellPosition = _objectInGround.WorldToCell(_position);
    }
    public void PipeDelete()
    {
        Destroy(gameObject);
    }
    public void ChangeCurrentTile()
    {
        for (int i = 0; i < isNeighbors.Length; i++) isNeighbors[i] = CheckNeighbors(i);
        int fromBinaryIndex = Convert.ToInt32($"{Convert.ToInt32(isNeighbors[0])}{Convert.ToInt32(isNeighbors[1])}{Convert.ToInt32(isNeighbors[2])}{Convert.ToInt32(isNeighbors[3])}", 2);
        int index = fromBinaryIndex + (GetCurrentTileType() * 16);
        if (_objectInGround.GetTile(_cellPosition) == pipes.pipesArray[index]) return;
        _objectInGround.SetTile(_cellPosition, pipes.pipesArray[index]);
        currentTile = pipes.pipesArray[index];
    }
    private bool CheckNeighbors(int side)
    {
        bool[] neighbors = new bool[4];
        for (int i = 0; i < neighbors.Length; i++) neighbors[i] = GetNeighborTileBySide(side);
        return neighbors[side];
    }
    private TileBase CheckTile(TileBase tile)
    {
        if (tile != null)
        {
            if (tile.name.StartsWith("TA_Pipes_")) return tile;
            else if (tile.name.StartsWith("drill")) return tile;
            else for (int i = 0; i < pipes.allowedBuildings.Length; i++) if (tile == pipes.allowedBuildings[i]) return tile;
        }
        return null;
    }
    private TileBase GetNeighborTileBySide(int side)
    {
        TileBase tile = null;
        if (side == 0)
        {
            tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y + 1, _cellPosition.z));
            return CheckTile(tile);
        }
        else if (side == 1)
        {
            tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x + 1, _cellPosition.y, _cellPosition.z));
            return CheckTile(tile);
        }
        else if (side == 2)
        {
            tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y - 1, _cellPosition.z));
            return CheckTile(tile);
        }
        else if (side == 3)
        {

            tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x - 1, _cellPosition.y, _cellPosition.z));
            return CheckTile(tile);
        }
        return tile;
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
    public TileBase[] GetNeighborTiles(int shiftF, int shiftS)
    {
        TileBase[] tiles = new TileBase[4];
        for (int i = 0; i < 4; i++)
        {
            if (CheckNeighbors(i))
            {
                if (i == 0) tiles[i] = _objectInGround.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y + 1 + shiftF, _cellPosition.z));
                else if (i == 1) tiles[i] = _objectInGround.GetTile(new Vector3Int(_cellPosition.x + 1 + shiftF, _cellPosition.y, _cellPosition.z));
                else if (i == 2) tiles[i] = _objectInGround.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y - (1 + shiftS), _cellPosition.z));
                else if (i == 3) tiles[i] = _objectInGround.GetTile(new Vector3Int(_cellPosition.x - (1 + shiftS), _cellPosition.y, _cellPosition.z));
            }
        }
        return tiles;
    }
    public Transform[] GetNeighborPipes(int shiftF, int shiftS)
    {
        Transform[] transforms = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            if (CheckNeighbors(i))
            {
                if (i == 0 && buildings._pipeGroupDict.TryGetValue(new Vector3Int(_cellPosition.x, _cellPosition.y + 1 + shiftF, 0), out Transform pipe)) transforms[i] = pipe;
                else if (i == 1 && buildings._pipeGroupDict.TryGetValue(new Vector3Int(_cellPosition.x + 1 + shiftF, _cellPosition.y, 0), out Transform pipe1)) transforms[i] = pipe1;
                else if (i == 2 && buildings._pipeGroupDict.TryGetValue(new Vector3Int(_cellPosition.x, _cellPosition.y - (1 + shiftS), 0), out Transform pipe2)) transforms[i] = pipe2;
                else if (i == 3 && buildings._pipeGroupDict.TryGetValue(new Vector3Int(_cellPosition.x - (1 + shiftS), _cellPosition.y, 0), out Transform pipe3)) transforms[i] = pipe3;
            }
        }
        return transforms;
    }
}
