using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WorldGeneration : MonoBehaviour
{
    private Tilemap _ground;
    [SerializeField] private TileBase[] tile;

    private float n, sid = 30f;
    private static Vector2Int coord;
    private float scale;

    [SerializeField] private bool generation;
    [SerializeField] private Vector2Int MapSize;

    private void Start()
    {
        coord = MapSize;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        if (generation)
        {
            Generate(1,0.09f,2,0.88f,true,20);  
            Generate(3,0.09f,4,0.88f,false,20);  
            Generate(5,0.2f,5,2f,false,13);  
        }
    }

    private void Generate(int firstOre,float first,int secondOre,float second,bool stone,float scale)
    {
        sid = Random.Range(0, 99999);
        for (int i = 0; i < coord.x; i++)
        {
            for (int j = 0; j < coord.y; j++)
            {
                n = Mathf.PerlinNoise(Convert.ToSingle(i + sid) / MapSize.x * scale, Convert.ToSingle(j + sid) / MapSize.y * scale);
                if (n < first)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[firstOre]);
                }
                else if (n >first  && n < second && stone)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[0]);
                }
                else if (n > second)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[secondOre]);
                }
              
            }
        }
    }

 

  
   
}