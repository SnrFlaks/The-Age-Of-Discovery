using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private PipeManager pipeManager;
    [SerializeField] private Buildings buildings;
    public Vector3Int[] keyArray;
    public int drillDictCount;
    private Tilemap _objectInGround;
    public Dictionary<Vector3Int, Drill> _drillDict = new Dictionary<Vector3Int, Drill>();
    private HashSet<Vector3Int> _hasObjectInGround = new HashSet<Vector3Int>();
    private Vector3Int[] offsets = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left };

    private void Start() => _objectInGround = Buildings._objectInGround;

    public class Drill
    {
        public bool isPowered;
        public int capacityLimit;
        public int resourceQuantity;
        public int drillType;
        public Vector3Int cellPosition;
        public bool[] pipesOutput;
        public Drill(bool iP, int dT, Vector3Int cP)
        {
            isPowered = iP;
            drillType = dT;
            cellPosition = cP;
            capacityLimit = 10;
            pipesOutput = new bool[4];
        }
    }

    private void Update()
    {
        for (int k = 0; k < drillDictCount; k++)
        {
            if (_drillDict.TryGetValue(keyArray[k], out Drill d))
            {
                if (d.resourceQuantity >= 0 && pipeManager.resourceNumber < 10000)
                {
                    int number = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (d.pipesOutput[i] || (pipeManager._pipeConnectionsDict.TryGetValue(d.cellPosition + offsets[i], out PipeManager.PipeConnection pipeByPosCon) && pipeByPosCon.IsBusy)) number++;
                        else if (pipeManager._pipeConnectionsDict.TryGetValue(d.cellPosition + offsets[i], out PipeManager.PipeConnection pipe) == false) number++;
                    }
                    if (number == 4) for (int i = 0; i < 4; i++) d.pipesOutput[i] = false;
                    for (int i = 0; i < 4; i++)
                    {
                        if (d.resourceQuantity > 1 && d.pipesOutput[i] == false && d.drillType != -1)
                        {
                            if (pipeManager._pipeConnectionsDict.TryGetValue(d.cellPosition + offsets[i], out PipeManager.PipeConnection pipeByPosCon))
                            {
                                if (pipeByPosCon.IsBusy == false && _objectInGround.GetTile(d.cellPosition + offsets[i]) != pipeManager.DirectionTile[(i + 2) % 4])
                                {
                                    pipeByPosCon.SetValue(true);
                                    pipeManager._pipeConnectionsDict[d.cellPosition + offsets[i]] = pipeByPosCon;
                                    int key = pipeManager.resourceNumber;
                                    if (resourceManager._deadIndices.Count > 0)
                                    {
                                        key = resourceManager._deadIndices.First();
                                        resourceManager._deadIndices.Remove(key);
                                        resourceManager._deadIndices.Add(pipeManager.resourceNumber);
                                    }
                                    ResourceManager.Resource resource = new ResourceManager.Resource();
                                    resource.type = d.drillType;
                                    resource.position = GetPositionForInstantiate(i, d.cellPosition);
                                    resource.lastDirection = i;
                                    resource.targetPosition = d.cellPosition + offsets[i] + new Vector3(0.5f, 0.5f);
                                    resourceManager._resourcesDict.Add(key, resource);
                                    resourceManager.keyArray = resourceManager._resourcesDict.Keys.ToArray();
                                    resourceManager.resourceDictCount = resourceManager._resourcesDict.Count;
                                    pipeManager.resourceNumber++;
                                    d.resourceQuantity--;
                                    d.pipesOutput[i] = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (d.resourceQuantity < d.capacityLimit && d.isPowered) d.resourceQuantity++;
                }
            }
        }
    }

    private Vector3 GetPositionForInstantiate(int side, Vector3Int cellPosition)
    {
        if (side == 0) return new Vector3(cellPosition.x + .5f, cellPosition.y + 1.1f, cellPosition.z);
        else if (side == 1) return new Vector3(cellPosition.x + 1.1f, cellPosition.y + .5f, cellPosition.z);
        else if (side == 2) return new Vector3(cellPosition.x + .5f, cellPosition.y - 0.1f, cellPosition.z);
        else if (side == 3) return new Vector3(cellPosition.x - 0.1f, cellPosition.y + .5f, cellPosition.z);
        return new Vector3();
    }
}
