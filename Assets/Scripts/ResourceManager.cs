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
    [SerializeField] private PipeManager pipeManager;
    [SerializeField] private Buildings buildings;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material[] material;
    private int numRows;
    private int numColumns;
    private int numOfNeighbors;
    private Camera mainCam;
    public Dictionary<int, Resource> _resourcesDict = new Dictionary<int, Resource>();
    public HashSet<int> _deadIndices = new HashSet<int>();
    private int[] indices = new int[3];
    public Matrix4x4[][][] matrixPacks;
    public int[] keyArray;
    public int resourceDictCount = 0;
    public static float camSize;

    private Vector3[] offsets = new Vector3[] {
        new Vector3(0.5f, 1.5f, 0),
        new Vector3(1.5f, 0.5f, 0),
        new Vector3(0.5f, -0.5f, 0),
        new Vector3(-0.5f, 0.5f, 0)
    };
    private Vector3Int[] offsetsInt = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left };
    private Quaternion resourceRotation = Quaternion.identity;
    private Vector3 resourceScale = new Vector3(0.6f, 0.6f, 0.6f);
    private void Awake()
    {
        mainCam = Camera.main;
        matrixPacks = new Matrix4x4[8][][];
        for (int i = 0; i < 8; i++)
        {
            matrixPacks[i] = new Matrix4x4[100][];
            for (int j = 0; j < 100; j++) matrixPacks[i][j] = new Matrix4x4[1023];
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
                if (inpDir == null) inpDir = new int[4] { 2, 3, 0, 1 };
                return inpDir;
            }
        }
        public int type { get; set; }
    }

    [BurstCompile]
    private void Update()
    {
        Profiler.BeginSample("Pipe.Calculating");
        float deltaTime = Time.deltaTime;
        for (int k = 0; k < resourceDictCount; k++)
        {
            //Debug.Log("R: " + k / 1023 + " - " + k % 1023 + "Count: " + resourceDictCount + " - " + matrixPacks[k / 1023][k % 1023]);
            if (_resourcesDict.TryGetValue(keyArray[k], out Resource r))
            {
                Vector3 position = r.position;
                Vector3Int positionInt = Vector3Int.FloorToInt(position);
                Vector3 previousTargetPosition = r.targetPosition;
                if (pipeManager._pipeConnectionsDict.TryGetValue(positionInt, out PipeManager.PipeConnection pipeByPos))
                {
                    numOfNeighbors = pipeByPos.numOfNeighbors;
                    if ((position - r.targetPosition).sqrMagnitude < 0.001f)
                    {
                        if (numOfNeighbors == 2)
                        {
                            int index = TGP_DirectionAndTurn(pipeByPos, positionInt, r.lastDirection, r.inputDirection[r.lastDirection], pipeByPos.direction);
                            if (index >= 0)
                            {
                                r.targetPosition = positionInt + offsets[index];
                                r.lastDirection = index;
                                PipeManager.PipeConnection targetPipe = pipeManager._pipeConnectionsDict[positionInt + offsetsInt[index]];
                                targetPipe.SetValue(true);
                                pipeByPos.SetValue(false);
                                pipeManager._pipeConnectionsDict[positionInt + offsetsInt[index]] = targetPipe;
                                pipeManager._pipeConnectionsDict[positionInt] = pipeByPos;
                            }
                        }
                        else if (numOfNeighbors > 2)
                        {
                            int index = TGP_TeeAndCross(pipeByPos, positionInt, r.inputDirection[r.lastDirection], numOfNeighbors);
                            if (index >= 0)
                            {
                                r.targetPosition = positionInt + offsets[index];
                                PipeManager.PipeConnection targetPipe = pipeManager._pipeConnectionsDict[positionInt + offsetsInt[index]];
                                targetPipe.SetValue(true);
                                pipeByPos.SetValues(false, index, true);
                                pipeManager._pipeConnectionsDict[positionInt + offsetsInt[index]] = targetPipe;
                                pipeManager._pipeConnectionsDict[positionInt] = pipeByPos;
                                int num = 0;
                                for (int i = 0; i < 4; i++)
                                {
                                    if (pipeByPos.IsConnected[i] == false || pipeByPos.IsTemporarilyBlocked[i] || i == r.inputDirection[r.lastDirection])
                                    {
                                        if (++num == 4)
                                        {
                                            pipeByPos.IsTemporarilyBlocked = new bool[4];
                                            pipeManager._pipeConnectionsDict[positionInt] = pipeByPos;
                                        }
                                    }
                                }
                                r.lastDirection = index;
                            }
                        }
                    }
                    if ((position - r.targetPosition).sqrMagnitude > 0.001f || numOfNeighbors != 1 && pipeByPos.IsConnected[r.lastDirection])
                    {
                        numRows = keyArray[k] / 1023;
                        numColumns = keyArray[k] % 1023;
                        matrixPacks[r.type][numRows][numColumns] = Matrix4x4.TRS(Vector3.MoveTowards(position, r.targetPosition, 3 * deltaTime), resourceRotation, resourceScale);
                        r.position = matrixPacks[r.type][numRows][numColumns].GetPosition();
                    }
                }
                else if (r.type < 4 && buildings._lineGroupDict.TryGetValue(positionInt, out Transform transform))
                {
                    Smelting smelting = transform.GetComponent<Smelting>();
                    if (smelting.enabled)
                    {
                        smelting._resourceDict.Add(keyArray[k], r);
                        smelting.keyArray = smelting._resourceDict.Keys.ToArray();
                        smelting.resourceDictCount = smelting._resourceDict.Count;
                        matrixPacks[r.type][keyArray[k] / 1023][keyArray[k] % 1023] = new Matrix4x4();
                        _resourcesDict.Remove(keyArray[k]);
                        resourceDictCount = _resourcesDict.Count;
                        keyArray = _resourcesDict.Keys.ToArray();
                    }
                    else ResourceDelete(k, r, positionInt, previousTargetPosition);
                }
                else ResourceDelete(k, r, positionInt, previousTargetPosition);
            }
        }
        Profiler.BeginSample("Pipe.Draw");
        if (camSize < 55 && resourceDictCount > 0)
        {
            int limit = Mathf.CeilToInt(resourceDictCount / 1023) + 1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < limit; j++)
                {
                    Graphics.DrawMeshInstanced(mesh, 0, material[i], matrixPacks[i][j]);
                }
            }
        }
        Profiler.EndSample();
        Profiler.EndSample();
    }

    [BurstCompile]
    private int TGP_DirectionAndTurn(PipeManager.PipeConnection currentPipe, Vector3Int pos, int lastDirection, int inputSide, int direction)
    {
        //if (direction == lastDirection) Debug.Log(direction + " - " + lastDirection + " - " + currentPipe.IsConnected[direction] + " - " + pipeManager._pipeConnectionsDict[pos + offsetsInt[direction]].IsBusy);
        if ((direction == lastDirection || direction == inputSide) && pipeManager._pipeConnectionsDict.TryGetValue(pos + offsetsInt[direction], out PipeManager.PipeConnection pipe) && currentPipe.IsConnected[direction] && pipe.IsBusy == false) return direction;
        else if (direction == -1)
        {
            for (int j = 0; j < 4; j++)
            {
                if (pipeManager._pipeConnectionsDict.TryGetValue(pos + offsetsInt[j], out PipeManager.PipeConnection pipeTurn) && currentPipe.IsConnected[j])
                {
                    if (j != inputSide && pipeTurn.IsBusy == false) return j;
                    else if (pipeManager._pipeConnectionsDict.TryGetValue(Vector3Int.FloorToInt(pos + offsets[inputSide]), out PipeManager.PipeConnection pipeTurnInp) && currentPipe.IsConnected[inputSide])
                    {
                        if (j == inputSide && pipeTurnInp.IsBusy && pipeTurnInp.IsBusy == false) return inputSide;
                    }
                }
            }
        }
        return -1;
    }

    [BurstCompile]
    private int TGP_TeeAndCross(PipeManager.PipeConnection currentPipe, Vector3Int pos, int inputSide, int numOfNeighbors)
    {
        int numOfBlockedSides = 0;
        for (int j = 0; j < 4; j++)
        {
            //Debug.Log("j:" + j);
            if (j != inputSide && currentPipe.IsConnected[j] && pipeManager._pipeConnectionsDict.TryGetValue(pos + offsetsInt[j], out PipeManager.PipeConnection pipeCon))
            {
                //Debug.Log("IsBusy: " + pipeCon.IsBusy + " - " + currentPipe.IsTemporarilyBlocked[j]);
                if (pipeCon.IsBusy == false && currentPipe.IsTemporarilyBlocked[j] == false) return j;
                else if (pipeCon.IsBusy || currentPipe.IsTemporarilyBlocked[j]) numOfBlockedSides++;
            }
            else if (currentPipe.IsConnected[j] == false) numOfBlockedSides++;
        }
        //Debug.Log("Num: " + numOfBlockedSides);
        for (int i = 0; i < 4; i++) if (numOfBlockedSides >= (numOfNeighbors - 2) && pipeManager._pipeConnectionsDict.TryGetValue(pos + offsetsInt[i], out PipeManager.PipeConnection pipeCon) && pipeCon.IsBusy == false) return i;
        if (pipeManager._pipeConnectionsDict.TryGetValue(pos + offsetsInt[inputSide], out PipeManager.PipeConnection pipeInput) && numOfBlockedSides == (numOfNeighbors - 1) && pipeInput.IsBusy == false) return inputSide;
        return -1;
    }

    private void ResourceDelete(int k, Resource r, Vector3Int positionInt, Vector3 previousTargetPosition)
    {
        if (pipeManager._pipeConnectionsDict.TryGetValue(positionInt + offsetsInt[r.lastDirection], out PipeManager.PipeConnection targetPipe) && previousTargetPosition == r.targetPosition && targetPipe.IsBusy)
        {
            targetPipe.SetValue(false);
            pipeManager._pipeConnectionsDict[positionInt + offsetsInt[r.lastDirection]] = targetPipe;
        }
        matrixPacks[r.type][keyArray[k] / 1023][keyArray[k] % 1023] = new Matrix4x4();
        _deadIndices.Add(keyArray[k]);
        _resourcesDict.Remove(keyArray[k]);
        resourceDictCount = _resourcesDict.Count;
        keyArray = _resourcesDict.Keys.ToArray();
    }
}