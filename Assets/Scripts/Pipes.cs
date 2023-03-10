using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pipes : MonoBehaviour
{
    public TileBase[] allowedBuildings;
    public TileBase[] pipesArray;
    public Sprite[] pipesSprite;
    public TileBase[] DirectionTile;
    public TileBase[] TurnPipesUp;
    public TileBase[] TurnPipesRight;
    public TileBase[] TurnPipesDown;
    public TileBase[] TurnPipesLeft;
    public TileBase[] TeePipes;
    public TileBase CrossPipes;
}
