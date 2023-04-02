using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using UnityEngine.Profiling;
using Cysharp.Threading.Tasks;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private Pipes pipes;
    [SerializeField] private Buildings buildings;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material[] material;
    [SerializeField] private Matrix4x4[][] matrixPacks;
    private int numRows;
    private int numColumns;
    private TileBase[] directionTile;
    private TileBase[][] turnPipes = new TileBase[4][];
    private TileBase[] teePipes = new TileBase[4];
    private TileBase crossPipe;
    private int numOfNeighbors;
    //public Dictionary<int, Transform> _resourcesDict = new Dictionary<int, Transform>();
    //public Dictionary<int, Resource> _resourcesComponentDict = new Dictionary<int, Resource>();
    public Dictionary<int, Resource> _resourcesDict = new Dictionary<int, Resource>();
    public HashSet<int> _deadIndices = new HashSet<int>();
    private int[] indices = new int[3];
    public int[] keyArray;
    public int resourceDictCount = 0;
    private Camera mainCam;
    public static float camSize;

    private Vector3Int[] offsets = new Vector3Int[] {
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0)
    };
    private Quaternion resourceRotation = Quaternion.identity;
    private Vector3 resourceScale = new Vector3(0.85f, 0.85f, 0.85f);
    private void Awake()
    {
        directionTile = pipes.DirectionTile;
        turnPipes[0] = pipes.TurnPipesUp;
        turnPipes[1] = pipes.TurnPipesRight;
        turnPipes[2] = pipes.TurnPipesDown;
        turnPipes[3] = pipes.TurnPipesLeft;
        teePipes = pipes.TeePipes;
        crossPipe = pipes.CrossPipe;
        mainCam = Camera.main;
        matrixPacks = new Matrix4x4[100][];
        for (int i = 0; i < 100; i++)
        {
            matrixPacks[i] = new Matrix4x4[1023];
            for (int j = 0; j < 1023; j++)
            {
                Matrix4x4 matrix = matrixPacks[i][j];
                matrix = Matrix4x4.Scale(resourceScale) * matrix;
                matrixPacks[i][j] = matrix;
            }
        }
    }

    public class Resource
    {
        public Vector3 position { get; set; }
        public Vector3 targetPosition { get; set; }
        public int lastDirection { get; set; }
        private int[] inpDir;
        public int[] inputDirection
        {
            get
            {
                if (inpDir == null) inpDir = new int[4] { 2, 3, 1, 0 };
                return inpDir;
            }
        }
        public Pipe startPipe { get; set; }
        public Pipe lastPipe { get; set; }
        public bool isStart { get; set; }
        public int oreType { get; set; }
    }

    [BurstCompile]
    private void Update()
    {
        Profiler.BeginSample("Pipe.Calculating");
        float deltaTime = Time.deltaTime;
        for (int s = 0; s < 100000; s++)
        {
            for (int k = 0; k < resourceDictCount; k++)
            {
                if (_resourcesDict.TryGetValue(keyArray[k], out Resource r))
                {
                    numRows = k / 1023;
                    numColumns = k % 1023;
                    Vector3 position = r.position;
                    if (pipes._pipeGroupDict.TryGetValue(Vector3Int.FloorToInt(position), out Pipe pipeByPos))
                    {
                        if (r.lastPipe != pipeByPos) if (Enumerable.ReferenceEquals(r.lastPipe, null) == false) r.lastPipe.isBusy = false;
                        r.lastPipe = pipeByPos;
                        numOfNeighbors = pipeByPos.numOfNeighbors;
                        //Profiler.BeginSample("Pipe.ChangeTargetPosition");
                        if ((position - r.targetPosition).sqrMagnitude < 0.01f)
                        {
                            if (numOfNeighbors == 2)
                            {
                                int index = TGP_DirectionAndTurn(pipeByPos.isNeighboringPipes, r.lastDirection, r.inputDirection[r.lastDirection], pipeByPos.neighboringPipesComponent, pipeByPos.currentTile);
                                if (index >= 0)
                                {
                                    r.targetPosition = pipeByPos.neighboringPipesPosition[index];
                                    pipeByPos.neighboringPipesComponent[index].isBusy = true;
                                    r.lastDirection = index;
                                }
                            }
                            else if (numOfNeighbors > 2)
                            {
                                int index = TGP_TeeAndCross(pipeByPos.isNeighboringPipes, r.inputDirection[r.lastDirection], pipeByPos.neighboringPipesComponent, numOfNeighbors);
                                if (index >= 0)
                                {
                                    r.targetPosition = pipeByPos.neighboringPipesPosition[index];
                                    pipeByPos.neighboringPipesComponent[index].isBusy = true;
                                    pipeByPos.neighboringPipesComponent[index].isTimelyBlocked = true;
                                    r.lastDirection = index;
                                    int num = 0;
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (pipeByPos.isNeighboringPipes[i] || i == r.inputDirection[r.lastDirection] || (pipeByPos.isNeighboringPipes[i] && pipeByPos.neighboringPipesComponent[i].isTimelyBlocked))
                                        {
                                            if (++num == 4) for (int j = 0; j < 4; j++) if (pipeByPos.isNeighboringPipes[j]) pipeByPos.neighboringPipesComponent[j].isTimelyBlocked = false;
                                        }
                                    }
                                    //  || r.neighboringPipesComponent[i].isBusy
                                }
                            }
                        }
                        if ((position - r.targetPosition).sqrMagnitude > 0.01f || numOfNeighbors != 1 && pipeByPos.isNeighboringPipes[r.lastDirection])
                        {
                            matrixPacks[numRows][numColumns] = Matrix4x4.TRS(Vector3.MoveTowards(position, r.targetPosition, 10 * deltaTime), resourceRotation, resourceScale);
                            r.position = matrixPacks[numRows][numColumns].GetPosition();
                        }
                        //Profiler.EndSample();
                    }
                    else
                    {
                        _deadIndices.Add(keyArray[k]);
                        _resourcesDict.Remove(keyArray[k]);
                        resourceDictCount = _resourcesDict.Count;
                        keyArray = _resourcesDict.Keys.ToArray();
                        if (r.lastPipe) r.lastPipe.isBusy = false;
                    }
                }
            }
        }
        Profiler.BeginSample("Pipe.Draw");
        if (camSize < 55 && resourceDictCount > 0)
        {
            int limit = Mathf.CeilToInt(resourceDictCount / 1023) + 1;
            for (int i = 0; i < limit; i++)
            {
                Graphics.DrawMeshInstanced(mesh, 0, material[0], matrixPacks[i]);
            }
        }
        Profiler.EndSample();
        Profiler.EndSample();
    }

    [BurstCompile]
    private int TGP_DirectionAndTurn(bool[] isPipe, int lastDirection, int inputSide, Pipe[] pipeArrayComp, TileBase currentTile)
    {
        // Profiler.BeginSample("Pipe.DirOrTurn");
        if (isPipe[lastDirection])
        {
            if (pipeArrayComp[lastDirection].isBusy == false && currentTile == directionTile[lastDirection]) return lastDirection;
            else if (isPipe[inputSide] && currentTile == directionTile[inputSide] && pipeArrayComp[inputSide].isBusy == false) return inputSide;
        }
        else
        {
            for (int j = 0; j < 4; j++)
            {
                if (isPipe[j])
                {
                    if (j != inputSide && pipeArrayComp[j].isBusy == false) return j;
                    else if (j == inputSide && isPipe[inputSide] && pipeArrayComp[j].isBusy && pipeArrayComp[inputSide].isBusy == false) return inputSide;
                }
            }
        }
        // Profiler.EndSample();
        return -1;
    }

    [BurstCompile]
    private int TGP_TeeAndCross(bool[] isPipe, int inputSide, Pipe[] pipeArrayComp, int numOfNeighbors)
    {
        // Profiler.BeginSample("Pipe.CalculateSideForMoving");
        int numOfBlockedSides = 0;
        for (int j = 0; j < 4; j++)
        {
            if (j != inputSide && isPipe[j])
            {
                if (pipeArrayComp[j].isBusy == false && pipeArrayComp[j].isTimelyBlocked == false) return j;
                else if (pipeArrayComp[j].isBusy || pipeArrayComp[j].isTimelyBlocked) numOfBlockedSides++;
            }
        }
        for (int i = 0; i < 4; i++) if (numOfBlockedSides >= (numOfNeighbors - 2) && isPipe[i] && pipeArrayComp[i].isBusy == false) return i;
        if (isPipe[inputSide] && numOfBlockedSides == (numOfNeighbors - 1) && pipeArrayComp[inputSide].isBusy == false) return inputSide;
        // Profiler.EndSample();
        return -1;
    }
}