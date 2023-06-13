using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;

public class Mining : MonoBehaviour
{
    private ResourceManager resourceManager;
    private bool[] pipesOutput = new bool[4];
    private int capacityLimit = 10;
    private int resourceQuantity;
    private Tilemap _objectInGround;
    private PipeManager pipeManager;
    private CancellationTokenSource cancellationTokenSource;
    private Vector3Int _cellPosition;
    private Line line;
    private readonly Tilemap _buildOig = Buildings._objectInGround;
    private Vector3Int[] offsets = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left };
    public async void Awake()
    {
        _cellPosition = _buildOig.WorldToCell(transform.position);
        _objectInGround = _buildOig;
        pipeManager = transform.parent.parent.GetChild(6).GetComponent<PipeManager>();
        resourceManager = transform.parent.parent.GetChild(7).GetComponent<ResourceManager>();
        line = gameObject.GetComponent<Line>();
        cancellationTokenSource = new CancellationTokenSource();
        if (_buildOig.GetTile(_cellPosition).name.StartsWith("drill"))
        {
            gameObject.GetComponent<Smelting>().enabled = false;
            await MiningOre(cancellationTokenSource.Token);
        }
    }

    private async UniTask MiningOre(CancellationToken cancellationToken)
    {
        while (this != null)
        {
            if (resourceQuantity >= 0 && pipeManager.resourceNumber < 10000)
            {
                int number = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (pipesOutput[i] || (pipeManager._pipeConnectionsDict.TryGetValue(_cellPosition + offsets[i], out PipeManager.PipeConnection pipeByPosCon) && pipeByPosCon.IsBusy)) number++;
                    else if (pipeManager._pipeConnectionsDict.TryGetValue(_cellPosition + offsets[i], out PipeManager.PipeConnection pipe) == false) number++;
                }
                if (number == 4) for (int i = 0; i < 4; i++) pipesOutput[i] = false;
                for (int i = 0; i < 4; i++)
                {
                    if (resourceQuantity > 1 && pipesOutput[i] == false && GetDrillType() != -1)
                    {
                        if (pipeManager._pipeConnectionsDict.TryGetValue(_cellPosition + offsets[i], out PipeManager.PipeConnection pipeByPosCon))
                        {
                            if (pipeByPosCon.IsBusy == false && _objectInGround.GetTile(_cellPosition + offsets[i]) != pipeManager.DirectionTile[(i + 2) % 4])
                            {
                                pipeByPosCon.SetValue(true);
                                pipeManager._pipeConnectionsDict[_cellPosition + offsets[i]] = pipeByPosCon;
                                int key = pipeManager.resourceNumber;
                                if (resourceManager._deadIndices.Count > 0)
                                {
                                    key = resourceManager._deadIndices.First();
                                    resourceManager._deadIndices.Remove(key);
                                    resourceManager._deadIndices.Add(pipeManager.resourceNumber);
                                }
                                ResourceManager.Resource resource = new ResourceManager.Resource();
                                resource.type = GetDrillType();
                                resource.position = GetPositionForInstantiate(i);
                                resource.lastDirection = i;
                                resource.targetPosition = _cellPosition + offsets[i] + new Vector3(0.5f, 0.5f);
                                resourceManager._resourcesDict.Add(key, resource);
                                resourceManager.keyArray = resourceManager._resourcesDict.Keys.ToArray();
                                resourceManager.resourceDictCount = resourceManager._resourcesDict.Count;
                                pipeManager.resourceNumber++;
                                resourceQuantity--;
                                pipesOutput[i] = true;
                                break;
                            }
                        }
                    }
                }
                if (resourceQuantity < capacityLimit && line._isPowered) resourceQuantity++;
            }
            await UniTask.Delay(1000, cancellationToken: cancellationToken);
        }
    }
    private Vector3 GetPositionForInstantiate(int side)
    {
        if (side == 0) return new Vector3(_cellPosition.x + .5f, _cellPosition.y + 1.1f, _cellPosition.z);
        else if (side == 1) return new Vector3(_cellPosition.x + 1.1f, _cellPosition.y + .5f, _cellPosition.z);
        else if (side == 2) return new Vector3(_cellPosition.x + .5f, _cellPosition.y - 0.1f, _cellPosition.z);
        else if (side == 3) return new Vector3(_cellPosition.x - 0.1f, _cellPosition.y + .5f, _cellPosition.z);
        return new Vector3();
    }
    private int GetDrillType()
    {
        string tileName = _buildOig.GetTile(_cellPosition).name;
        if (tileName.StartsWith("drillTin")) return 0;
        else if (tileName.StartsWith("drillIron")) return 1;
        else if (tileName.StartsWith("drillCopper")) return 2;
        else if (tileName.StartsWith("drillGold")) return 3;
        return -1;
    }
}
