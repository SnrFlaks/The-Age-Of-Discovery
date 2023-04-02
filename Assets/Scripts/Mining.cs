using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;

public class Mining : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameObject[] resourcePrefabs;
    [SerializeField] private bool[] pipesOutput = new bool[4];
    [SerializeField] private int capacityLimit = 100;
    [SerializeField] private int resourceQuantity;
    private Transform resourcesGroup;
    private Tilemap _objectInGround;
    private Pipes pipes;
    private CancellationTokenSource cancellationTokenSource;
    private Vector3 _position;
    private Vector3Int _cellPosition;
    private readonly Tilemap _buildOig = Buildings._objectInGround;
    private Vector3Int[] offsets = new Vector3Int[] {
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0)
    };
    public async void Awake()
    {
        _position = transform.position;
        _cellPosition = _buildOig.WorldToCell(_position);
        _objectInGround = transform.parent.parent.GetChild(1).GetComponent<Tilemap>();
        pipes = transform.parent.parent.GetChild(6).GetComponent<Pipes>();
        resourceManager = transform.parent.parent.GetChild(7).GetComponent<ResourceManager>();
        resourcesGroup = transform.parent.parent.GetChild(7);
        cancellationTokenSource = new CancellationTokenSource();
        if (_buildOig.GetTile(_cellPosition).name.StartsWith("drill")) await MiningOre(cancellationTokenSource.Token);
    }

    private async UniTask MiningOre(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (this != null)
            {
                if (resourceQuantity >= 0 && pipes.resourceNumber < 5000)
                {
                    if (resourceQuantity < capacityLimit && gameObject.GetComponent<Line>()._isPowered) resourceQuantity++;
                    for (int i = 0; i < 4; i++)
                    {
                        if (resourceQuantity > 1 && pipesOutput[i] == false && GetDrillType() != -1)
                        {
                            if (pipes._pipeGroupDict.TryGetValue(_cellPosition + offsets[i], out Pipe pipeByPos) && pipeByPos.isBusy == false)
                            {
                                if (_objectInGround.GetTile(_cellPosition + offsets[i]) != pipes.DirectionTile[(i + 2) % 4])
                                {
                                    // GameObject res = Instantiate(resourcePrefabs[GetDrillType()], GetPositionForInstantiate(i), Quaternion.identity, resourcesGroup);
                                    // res.GetComponent<Resource>().lastDirection = i;
                                    pipeByPos.isBusy = true;
                                    int key = pipes.resourceNumber;
                                    if (resourceManager._deadIndices.Count > 0)
                                    {
                                        key = resourceManager._deadIndices.First();
                                        resourceManager._deadIndices.Remove(key);
                                        resourceManager._deadIndices.Add(pipes.resourceNumber);
                                    }
                                    ResourceManager.Resource resource = new ResourceManager.Resource();
                                    resource.position = GetPositionForInstantiate(i);
                                    resource.lastDirection = i;
                                    resource.targetPosition = _cellPosition + offsets[i] + new Vector3(0.5f, 0.5f);
                                    resource.oreType = GetDrillType();
                                    resourceManager._resourcesDict.Add(key, resource);
                                    resourceManager.keyArray = resourceManager._resourcesDict.Keys.ToArray();
                                    resourceManager.resourceDictCount = resourceManager._resourcesDict.Count;
                                    pipes.resourceNumber++;
                                    resourceQuantity -= 1;
                                    pipesOutput[i] = true;
                                    break;
                                }
                            }
                            else if (pipes._pipeGroupDict.TryGetValue(_cellPosition + offsets[i], out Pipe pipe) == false || pipe.isBusy) pipesOutput[i] = true;
                        }
                    }
                    int number = 0;
                    for (int i = 0; i < 4; i++) if (pipesOutput[i]) number++;
                    if (number == 4) for (int i = 0; i < 4; i++) pipesOutput[i] = false;
                }
            }
            await UniTask.Delay(1, cancellationToken: cancellationToken);
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
