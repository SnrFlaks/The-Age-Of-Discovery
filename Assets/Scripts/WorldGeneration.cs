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
    private static  Vector2Int coord;
   [SerializeField] private   Vector2Int coord2;
    void Start()
    {
        coord = coord2;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        sid = Random.Range(0f, 10000000f);
        
        
            
        for (int i = -coord.x; i < coord.x; i++)
        {
            for (int j = -coord.y; j < coord.y; j++)
            {
                n = Mathf.PerlinNoise(Convert.ToSingle(i ) + sid  , Convert.ToSingle(j) + sid );
                
                Debug.Log(n);
                if (n < 0.01)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[0]);
                }
                else if (n > 0.01 && n < 0.99)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[1]);
                }
                else if (n > 0.99)
                {
                    _ground.SetTile(new Vector3Int(i,j,0),tile[2]);
                }
            }
        }
    }

}
