using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEditor;
using System.Linq;

public class ProceduralMapGenarator : MonoBehaviour
{

    public RuleTile tileToDrow;
    public Tilemap tileMap;
    int[,] map;
    public int mapHeight;
    public int mapWidth;

    private void Start()
    {
        tileMap.ClearAllTiles();
        //map = generateRandomMap();
        //map = perlinNoiseGenerator();
        map = walkTop(0.3f, UnityEngine.Random.Range(2, 5), 5, mapWidth, mapHeight);
        drawMap(map);
    }

    //render Map
    public void drawMap(int[,] map)
    {
        for (int x = map.GetLowerBound(0); x < map.GetUpperBound(0); x++)
        {
            for (int y = map.GetLowerBound(1); y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                    tileMap.SetTile(new Vector3Int(x, y, 0), tileToDrow);
            }
        }
    }

    //topWalkByUnityBlog
    public int[,] walkTop(float seed, int minSectionWidth, int initialHeight, int mapWidth, int mapHeight)
    {
        int[,] map = new int[mapWidth, mapHeight];
        System.Random random = new System.Random(seed.GetHashCode());
        Debug.Log(initialHeight);
        int nextMove = 0;
        int sectionWidth = 0;

        for (int x = 0; x < mapWidth; x++)
        {
            nextMove = random.Next(2);
            if (nextMove == 0 && initialHeight > 0 && sectionWidth > minSectionWidth)
            {
                initialHeight--;
                sectionWidth = 0;
            }

            else if (nextMove == 1 && initialHeight < mapHeight && sectionWidth > minSectionWidth)
            {
                initialHeight++;
                sectionWidth = 0;
            }
            sectionWidth++;
            for (int y = initialHeight; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }
        return map;
    }

    //perlin NoiseMap
    public int[,] perlinNoiseGenerator()
    {
        int[,] map = new int[mapWidth, mapHeight];
        int initialPoint;
        float perlinOffset = 0.5f; //seed//

        for (int x = 0; x < mapWidth; x++)
        {
            initialPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x * 0.1f, map.GetUpperBound(0) * 0.1f) - perlinOffset) * map.GetUpperBound(1));
            initialPoint += (map.GetUpperBound(1) / 2); // begin somewhere half of the map
            for (int y = initialPoint; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }
        return map;
    }

    public int[,] generateRandomMap()
    {
        int[,] randomMap = new int[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            int maxHeight = UnityEngine.Random.Range((mapHeight / 2), mapHeight);
            for (int y = 0; y < maxHeight; y++)
            {
                randomMap[x, y] = 1;
            }
        }
        return randomMap;
    }
}
