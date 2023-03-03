using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Pipes pipes;
    [SerializeField] private bool[] isNeighbors = new bool[4];
    private Tilemap _objectInGround;
    private Vector3 _position;
    private Vector3Int _cellPosition;
    void Awake()
    {
        pipes = transform.parent.GetComponent<Pipes>();
        _position = transform.position;
        _objectInGround = Buildings._objectInGround;
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
    }
    public void RefreshNeighbors()
    {
        Transform pipe = transform.parent.Find($"{_cellPosition}");
        pipe = transform.parent.Find($"{new Vector3Int(_cellPosition.x, _cellPosition.y + 1, 0)}");
        if (pipe != null) pipe.GetComponent<Pipe>().ChangeCurrentTile();
        pipe = transform.parent.Find($"{new Vector3Int(_cellPosition.x + 1, _cellPosition.y, 0)}");
        if (pipe != null) pipe.GetComponent<Pipe>().ChangeCurrentTile();
        pipe = transform.parent.Find($"{new Vector3Int(_cellPosition.x, _cellPosition.y - 1, 0)}");
        if (pipe != null) pipe.GetComponent<Pipe>().ChangeCurrentTile();
        pipe = transform.parent.Find($"{new Vector3Int(_cellPosition.x - 1, _cellPosition.y, 0)}");
        if (pipe != null) pipe.GetComponent<Pipe>().ChangeCurrentTile();
    }
    private bool CheckNeighbors(int side)
    {
        bool[] neighbors = new bool[4];
        for (int i = 0; i < neighbors.Length; i++) neighbors[i] = GetNeighborTileBySide(side);
        return neighbors[side];
    }
    private int GetNeighborNumber()
    {
        int number = 0;
        for (int i = 0; i < 4; i++) number += isNeighbors[i] ? 1 : 0;
        return number;
    }
    private TileBase GetNeighborTileBySide(int side)
    {
        TileBase tile = null;
        if (side == 0) tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y + 1, _cellPosition.z));
        else if (side == 1) tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x + 1, _cellPosition.y, _cellPosition.z));
        else if (side == 2) tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y - 1, _cellPosition.z));
        else if (side == 3) tile = _objectInGround.GetTile(new Vector3Int(_cellPosition.x - 1, _cellPosition.y, _cellPosition.z));
        return tile;
    }
    private int GetCurrentTileType()
    {
        int type = Mathf.FloorToInt(Convert.ToInt32(_objectInGround.GetTile(_cellPosition).name.Replace("TA_Pipes_", "")) / 16);
        return type;
    }
}
