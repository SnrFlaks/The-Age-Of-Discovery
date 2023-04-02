using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Tilemaps;

public class ApplyVelocityParallelForSample : MonoBehaviour
{
    // [SerializeField] private Pipes pipes;
    // [SerializeField] private Buildings buildings;
    // private TileBase[] directionTile;
    // private TileBase[][] turnPipes = new TileBase[4][];
    // private TileBase[] teePipes = new TileBase[4];
    // private TileBase crossPipe;
    // private Transform[] neighborPipes = new Transform[4];
    // private Pipe[] neighborPipesComponent = new Pipe[4];
    // public Dictionary<int, Transform> _resourcesDict = new Dictionary<int, Transform>();
    // public Dictionary<int, Resource> _resourcesComponentDict = new Dictionary<int, Resource>();
    // public int resourceDictCount = 0;
    // private Vector3Int[] offsets = new Vector3Int[] {
    //     new Vector3Int(0, 1, 0),
    //     new Vector3Int(1, 0, 0),
    //     new Vector3Int(0, -1, 0),
    //     new Vector3Int(-1, 0, 0)
    // };
    // private void Awake()
    // {
    //     directionTile = pipes.DirectionTile;
    //     turnPipes[0] = pipes.TurnPipesUp;
    //     turnPipes[1] = pipes.TurnPipesRight;
    //     turnPipes[2] = pipes.TurnPipesDown;
    //     turnPipes[3] = pipes.TurnPipesLeft;
    //     teePipes = pipes.TeePipes;
    //     crossPipe = pipes.CrossPipe;
    // }
    // struct VelocityJob : IJobParallelFor
    // {
    //     public Pipes pipes;
    //     public Buildings buildings;
    //     public TileBase[] directionTile;
    //     public TileBase[][] turnPipes;
    //     public TileBase[] teePipes;
    //     public TileBase crossPipe;
    //     public Transform[] neighborPipes;
    //     public Pipe[] neighborPipesComponent;
    //     public int resourceDictCount;
    //     public Dictionary<int, Transform> _resourcesDict;
    //     public Dictionary<int, Resource> _resourcesComponentDict;
    //     public Vector3Int[] offsets;
    //     public void Execute(int k)
    //     {
    //         int key = _resourcesDict.Keys.ElementAt(k);
    //         if (_resourcesDict.TryGetValue(key, out Transform transform) && _resourcesComponentDict.TryGetValue(key, out Resource resource))
    //         {
    //             Vector3 position = resource.isVisible ? transform.localPosition : resource.dataPos;
    //             if (pipes._pipeGroupDict.TryGetValue(Vector3Int.FloorToInt(position), out Pipes.PipeObj pipeByPos))
    //             {
    //                 Vector3Int cellPosition = pipeByPos.pos;
    //                 var dict = pipes._pipeGroupDict;
    //                 for (int i = 0; i < 4; i++)
    //                 {
    //                     if (dict.TryGetValue(cellPosition + offsets[i], out Pipes.PipeObj neiPipe))
    //                     {
    //                         neighborPipes[i] = neiPipe.transform;
    //                         neighborPipesComponent[i] = neiPipe.pipe;
    //                     }
    //                     else neighborPipes[i] = null;
    //                 }
    //                 if (resource.lastPipe != pipeByPos.pipe)
    //                 {
    //                     if (resource.lastPipe != null) resource.lastPipe.isBusy = false;
    //                     resource.currentPipe = pipeByPos.pipe;
    //                     resource.neighboringPipes = neighborPipes;
    //                     resource.neighboringPipesComponent = neighborPipesComponent;
    //                 }
    //                 resource.lastPipe = pipeByPos.pipe;
    //                 if (resource.currentPipe != null)
    //                 {
    //                     if (resource.startPipe == null)
    //                     {
    //                         resource.startPipe = resource.currentPipe;
    //                         resource.isStart = resource.currentPipe == resource.startPipe && Vector3.Distance(position, resource.currentPipe.transform.localPosition) > 0.1f;
    //                     }
    //                     int neighbors = 0;
    //                     for (int i = 0; i < 4; i++)
    //                     {
    //                         if (resource.neighboringPipes[i] || ((i + 2) % 4 == resource.lastDirection)) neighbors++;
    //                         else if (buildings._lineGroupDict.ContainsKey(cellPosition + offsets[i])) neighbors++;
    //                     }
    //                     if (Vector3.Distance(position, resource.targetPosition) < 0.01f || resource.targetPosition == Vector3.zero)
    //                     {
    //                         if (Enumerable.ReferenceEquals(resource.neighboringPipes, neighborPipes) == false)
    //                         {
    //                             if (resource.lastPipe != null) resource.lastPipe.isBusy = false;
    //                             resource.currentPipe = pipeByPos.pipe;
    //                             resource.neighboringPipes = neighborPipes;
    //                             resource.neighboringPipesComponent = neighborPipesComponent;
    //                         }
    //                         if (resource.isStart)
    //                         {
    //                             resource.isStart = resource.currentPipe == resource.startPipe && Vector3.Distance(position, resource.currentPipe.transform.localPosition) > 0.01f;
    //                             resource.targetPosition = resource.currentPipe.transform.localPosition;
    //                         }
    //                         else if (neighbors == 2)
    //                         {
    //                             Resource r = resource;
    //                             for (int i = 0; i < directionTile.Length; i++)
    //                             {
    //                                 if (r.currentPipe.currentTile == directionTile[i] && r.neighboringPipes[i] && r.neighboringPipesComponent[i].isBusy == false)
    //                                 {
    //                                     r.targetPosition = r.neighboringPipes[i].localPosition;
    //                                     r.neighboringPipesComponent[i].isBusy = true;
    //                                     r.lastDirection = i;
    //                                     break;
    //                                 }
    //                                 else if (i == 3 && r.currentPipe.currentTile != directionTile[i])
    //                                 {
    //                                     for (int j = 0; j < turnPipes[r.lastDirection].Length; j++)
    //                                     {
    //                                         int directionIndex = r.lastDirection % 2 == 0 ? 3 : 2;
    //                                         int index = directionIndex / (directionIndex * (j % 2) + (j % 2 == 0 ? 1 : 0));
    //                                         int arrayIndex = directionIndex == 2 && index == 1 ? 0 : index;
    //                                         if (r.currentPipe.currentTile == turnPipes[r.lastDirection][j] && r.neighboringPipes[arrayIndex] && r.neighboringPipesComponent[arrayIndex].isBusy == false)
    //                                         {
    //                                             r.targetPosition = r.neighboringPipes[arrayIndex].localPosition;
    //                                             r.neighboringPipesComponent[arrayIndex].isBusy = true;
    //                                             r.lastDirection = arrayIndex;
    //                                         }
    //                                     }
    //                                 }
    //                             }
    //                         }
    //                         else if (neighbors == 3)
    //                         {
    //                             Resource r = resource;
    //                             int[] indices = new int[3];
    //                             var pipesOutput = r.currentPipe.PipesOutput;
    //                             for (int i = 0; i < 4; i++)
    //                             {
    //                                 if (r.currentPipe.currentTile == teePipes[i])
    //                                 {
    //                                     int index = 0;
    //                                     int direction = (r.lastDirection + 2) % 4;
    //                                     for (int j = 0; j < 4; j++) if (j != direction && r.currentPipe.IsNeighbor[j] && index < 2) indices[index++] = j;
    //                                     if (pipesOutput[indices[0]][indices[0]] || r.neighboringPipes[indices[0]] == false || r.neighboringPipesComponent[indices[0]].isBusy)
    //                                     {
    //                                         if (pipesOutput[indices[1]][indices[1]] || r.neighboringPipes[indices[1]] == false || r.neighboringPipesComponent[indices[1]].isBusy)
    //                                         {
    //                                             pipesOutput[indices[0]][indices[0]] = false;
    //                                             pipesOutput[indices[1]][indices[1]] = false;
    //                                         }
    //                                     }
    //                                     int indexCached = 0;
    //                                     for (int j = 0; j < 2; j++)
    //                                     {
    //                                         indexCached = indices[j];
    //                                         if (pipesOutput[indexCached][indexCached] == false && r.neighboringPipes[indexCached] && r.neighboringPipesComponent[indexCached].isBusy == false)
    //                                         {
    //                                             r.targetPosition = r.neighboringPipes[indexCached].localPosition;
    //                                             r.neighboringPipesComponent[indexCached].isBusy = true;
    //                                             r.lastDirection = indexCached;
    //                                             pipesOutput[indexCached][indexCached] = true;
    //                                             break;
    //                                         }
    //                                     }
    //                                 }
    //                             }
    //                         }
    //                         else if (neighbors == 4 && resource.currentPipe.currentTile == crossPipe)
    //                         {
    //                             Resource r = resource;
    //                             int number = 0;
    //                             int[] indices = new int[3];
    //                             int direction = (r.lastDirection + 2) % 4;
    //                             for (int j = 0; j < 4; j++) if (j != direction) indices[number++] = j;
    //                             var pipesOutput = r.currentPipe.PipesOutput;
    //                             int count = 0;
    //                             int index = 0;
    //                             for (int i = 0; i < 3; i++)
    //                             {
    //                                 index = indices[i];
    //                                 if (pipesOutput[r.lastDirection][index] || r.neighboringPipes[index] == false || r.neighboringPipesComponent[index].isBusy)
    //                                 {
    //                                     count++;
    //                                     if (count == 3) for (int j = 0; j < 4; j++) pipesOutput[r.lastDirection][j] = false;
    //                                 }
    //                             }
    //                             for (int i = 0; i < 3; i++)
    //                             {
    //                                 index = indices[i];
    //                                 if (pipesOutput[r.lastDirection][index] == false && r.neighboringPipes[index] && r.neighboringPipesComponent[index].isBusy == false)
    //                                 {
    //                                     r.targetPosition = r.neighboringPipes[index].localPosition;
    //                                     r.neighboringPipesComponent[index].isBusy = true;
    //                                     pipesOutput[r.lastDirection][index] = true;
    //                                     r.lastDirection = index;
    //                                     break;
    //                                 }
    //                             }
    //                         }
    //                     }
    //                     if (Vector3.Distance(position, resource.targetPosition) > 0.01f || neighbors != 1 && resource.neighboringPipes[resource.lastDirection])
    //                     {
    //                         if (resource.isVisible) transform.localPosition = Vector3.MoveTowards(transform.localPosition, resource.targetPosition, 5 * Time.deltaTime);
    //                         //if (r.isVisible) transform.localPosition = targetPosition;
    //                         else resource.dataPos = Vector3.MoveTowards(resource.dataPos, resource.targetPosition, 5 * Time.deltaTime);
    //                     }
    //                 }
    //             }
    //             else
    //             {
    //                 _resourcesDict.Remove(key);
    //                 _resourcesComponentDict[key].Destroy();
    //                 _resourcesComponentDict.Remove(key);
    //                 resourceDictCount = _resourcesDict.Count;
    //             }
    //         }
    //     }
    // }

    // public void Update()
    // {
    //     var job = new VelocityJob()
    //     {
    //         pipes = pipes,
    //         buildings = buildings,
    //         directionTile = directionTile,
    //         turnPipes = turnPipes,
    //         teePipes = teePipes,
    //         crossPipe =crossPipe,
    //         neighborPipes = neighborPipes,
    //         neighborPipesComponent = neighborPipesComponent,
    //         resourceDictCount = resourceDictCount,
    //         _resourcesDict = _resourcesDict,
    //         _resourcesComponentDict = _resourcesComponentDict,
    //         offsets = offsets
    //     };
    //     JobHandle jobHandle = job.Schedule(resourceDictCount, 64);
    //     jobHandle.Complete();
    // }
}
