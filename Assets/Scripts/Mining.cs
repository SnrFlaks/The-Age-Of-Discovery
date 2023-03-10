using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;

public class Mining : MonoBehaviour
{
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
    private bool _isPowered;
    public async void Awake()
    {
        _position = transform.position;
        _cellPosition = _buildOig.WorldToCell(_position);
        _objectInGround = transform.parent.parent.GetChild(1).GetComponent<Tilemap>();
        pipes = transform.parent.parent.GetChild(6).GetComponent<Pipes>();
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
                if (resourceQuantity >= 0)
                {
                    if (resourceQuantity < capacityLimit && gameObject.GetComponent<Line>()._isPowered) resourceQuantity++;
                    for (int i = 0; i < 4; i++)
                    {
                        if (resourceQuantity > 1 && pipesOutput[i] == false && GetDrillType() != -1)
                        {
                            if (CheckNeighbors(i) == false) pipesOutput[i] = true;
                            else if (CheckNeighbors(i) && _objectInGround.GetTile(GetPositionForInstantiate(i, false)) == pipes.DirectionTile[i])
                            {
                                Instantiate(resourcePrefabs[GetDrillType()], GetPositionForInstantiate(i), Quaternion.identity, resourcesGroup);
                                resourceQuantity -= 1;
                                pipesOutput[i] = true;
                                break;
                            }
                        }
                    }
                    int number = 0;
                    for (int i = 0; i < 4; i++) if (pipesOutput[i]) number++;
                    if (number == 4) for (int i = 0; i < 4; i++) pipesOutput[i] = false;
                }
            }
            await UniTask.Delay(1000, cancellationToken: cancellationToken);
        }
    }
    private Vector3 GetPositionForInstantiate(int side)
    {
        if (side == 0) return new Vector3(_cellPosition.x + .5f, _cellPosition.y + 1, _cellPosition.z);
        else if (side == 1) return new Vector3(_cellPosition.x + 1, _cellPosition.y + .5f, _cellPosition.z);
        else if (side == 2) return new Vector3(_cellPosition.x + .5f, _cellPosition.y, _cellPosition.z);
        else if (side == 3) return new Vector3(_cellPosition.x, _cellPosition.y + .5f, _cellPosition.z);
        return new Vector3();
    }
    private Vector3Int GetPositionForInstantiate(int side, bool pipe)
    {
        if (side == 0) return new Vector3Int(_cellPosition.x, _cellPosition.y + 1, _cellPosition.z);
        else if (side == 1) return new Vector3Int(_cellPosition.x + 1, _cellPosition.y, _cellPosition.z);
        else if (side == 2) return new Vector3Int(_cellPosition.x, _cellPosition.y - 1, _cellPosition.z);
        else if (side == 3) return new Vector3Int(_cellPosition.x - 1, _cellPosition.y, _cellPosition.z);
        return new Vector3Int();
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
    private bool CheckNeighbors(int side)
    {
        bool[] neighbors = new bool[4];
        for (int i = 0; i < neighbors.Length; i++) if (GetNeighborTileBySide(side)) neighbors[i] = GetNeighborTileBySide(side).name.StartsWith("TA_Pipes");
        return neighbors[side];
    }
    private TileBase GetNeighborTileBySide(int side)
    {
        if (side == 0) return _buildOig.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y + 1, _cellPosition.z));
        else if (side == 1) return _buildOig.GetTile(new Vector3Int(_cellPosition.x + 1, _cellPosition.y, _cellPosition.z));
        else if (side == 2) return _buildOig.GetTile(new Vector3Int(_cellPosition.x, _cellPosition.y - 1, _cellPosition.z));
        else if (side == 3) return _buildOig.GetTile(new Vector3Int(_cellPosition.x - 1, _cellPosition.y, _cellPosition.z));
        return null;
    }
}
