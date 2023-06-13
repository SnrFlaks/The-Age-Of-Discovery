using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Profiling;

public class WorldGeneration : MonoBehaviour
{
    private Tilemap _ground;
    [SerializeField] private TileBase[] tile;

    private float n, sid = 30f;
    public static Vector2Int coord;
    private float scale;

    [SerializeField] private bool generation;
    [SerializeField] private Vector2Int MapSize;

    private void Start()
    {
        coord = MapSize;
        _ground = transform.GetChild(0).GetComponent<Tilemap>();
        if (generation) GenerateWorld();
    }

    [ContextMenu("Generate World")]
    public void GenerateWorld()
    {
        TileBase[] tiles = new TileBase[coord.x * coord.y];
        Generate(1, 0.09f, 2, 0.88f, true, 20, ref tiles);
        Generate(3, 0.09f, 4, 0.88f, false, 20, ref tiles);
        Generate(5, 0.2f, 5, 2f, false, 13, ref tiles);
        _ground.SetTilesBlock(new BoundsInt(0, 0, 0, coord.x, coord.y, 1), tiles);
        for (int i = 243; i < 258; i++)
        {
            for (int j = 243; j < 258; j++)
            {
                _ground.SetTile(new Vector3Int(i, j), tile[0]);
            }
        }
    }

    private void Generate(int firstOre, float first, int secondOre, float second, bool stone, float scale, ref TileBase[] tiles)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        sid = Random.Range(0, 99999);
        int tileIndex = 0;
        for (int i = 0; i < coord.x; i++)
        {
            for (int j = 0; j < coord.y; j++)
            {
                n = Mathf.PerlinNoise(Convert.ToSingle(i + sid) / MapSize.x * scale, Convert.ToSingle(j + sid) / MapSize.y * scale);
                n -= (firstOre == 5) ? Random.Range(0, 0.035f) : 0;
                if (n < first) tiles[tileIndex] = tile[firstOre];
                else if (n > first && n < second && stone) tiles[tileIndex] = tile[0];
                else if (n > second) tiles[tileIndex] = tile[secondOre];
                tileIndex++;
            }
        }
        stopwatch.Stop();
        FPSView.wg1 = stopwatch.ElapsedMilliseconds;
    }
}