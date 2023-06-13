using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Profiling;

public class PipeManager : MonoBehaviour
{
    public int resourceNumber = 0;
    public TileBase[] allowedBuildings;
    public TileBase[] pipesArray;
    public Sprite[] pipesSprite;
    public TileBase[] DirectionTile;
    public Dictionary<Vector3Int, Pipe> _pipeGroupDict = new Dictionary<Vector3Int, Pipe>();
    public Dictionary<Vector3Int, PipeConnection> _pipeConnectionsDict = new Dictionary<Vector3Int, PipeConnection>();

    public struct PipeConnection
    {
        public Vector3 position { get; set; }
        public bool[] IsConnected { get; set; }
        public bool[] IsTemporarilyBlocked { get; set; }
        public bool IsBusy { get; set; }
        public int numOfNeighbors { get; set; }
        public int direction { get; set; }

        public PipeConnection(Vector3 pos, bool[] isCon, bool isBusy, int numNei, int dir)
        {
            position = pos;
            IsConnected = isCon;
            IsTemporarilyBlocked = new bool[4];
            IsBusy = isBusy;
            numOfNeighbors = numNei;
            direction = dir;
        }

        public void SetValue(bool isBusy) => IsBusy = isBusy;

        public void SetValues(bool isBusy, int indexOfBlockedSide, bool isTemporarilyBlocked)
        {
            IsBusy = isBusy;
            IsTemporarilyBlocked[indexOfBlockedSide] = isTemporarilyBlocked;
        }

        public bool Equals(PipeConnection other)
        {
            if (position == other.position) { return true; }
            else return false;
        }
    }
}
