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
    private float time;
     
    private void Start()
    {
        coord = MapSize;
        scale = coord.x / 20;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        sid = Random.Range(0,99999);
        
        if(generation){Generate();}

        Debug.Log(time);
    }
    
    private void Generate()
    {
        for (int i = 0; i < coord.x; i++)
        {
            for (int j = 0; j < coord.y; j++)
            {
                n = Mathf.PerlinNoise(Convert.ToSingle(i + sid ) / MapSize.x * scale , Convert.ToSingle(j+ sid) / MapSize.y * scale   );
               
                
                if (n < 0.085)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[0]);
                }
                else if (n > 0.085 && n < 0.835)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[1]);
                }
                else if (n > 0.835)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[2]);
                }
                
            }

            
            
        }
     
    }

    
}