using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pipe : MonoBehaviour
{
    [SerializeField] private bool[] isNeighbors = new bool[4];
    [SerializeField] private int[] neighborsData = new int[4];
    private Tilemap _objectInGround;
    private Vector3 _position;
    private Vector3Int _cellPosition;
    void Start()
    {
        _position = transform.position;
        _objectInGround = Buildings._objectInGround;
        _cellPosition = _objectInGround.WorldToCell(_position);
    }

    void Update()
    {
        for (int i = 0; i < isNeighbors.Length; i++) isNeighbors[i] = CheckNeighbors(i);
        for (int i = 0; i < neighborsData.Length; i++) neighborsData[i] = isNeighbors[i] ? Convert.ToInt32($"{Convert.ToInt32(isNeighbors[i])}{GetNeighborType(i)}") : 00;
    }
    private void ChangeTile()
    {
        for (int i = 0; i < isNeighbors.Length; i++)
        {
            if (isNeighbors[i] && i == 0);
        }
    }
    private void GetRule()
    {
        
    }
    private bool CheckNeighbors(int side)
    {
        bool[] neighbors = new bool[4];
        for (int i = 0; i < neighbors.Length; i++) neighbors[i] = GetNeighborTileBySide(side);
        return neighbors[side];
    }
    private int GetNeighborType(int side)
    {
        int type = -1;
        string name = GetNeighborTileBySide(side).name.Substring(0, GetNeighborTileBySide(side).name.Length - 9);
        if (name == "iron") type = 0;
        else if (name == "lead") type = 1;
        else if (name == "platinum") type = 2;
        else if (name == "wolfram") type = 3;
        else if (name == "titanium") type = 4;
        return type;
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
}
