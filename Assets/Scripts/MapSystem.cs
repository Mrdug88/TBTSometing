using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEditor;
using System.Linq;

public class MapSystem : MonoBehaviour
{
    #region Declarations
    public Tilemap tileMap, detailsTileMap;
    public RuleTile tileToDraw;
    public RandomTile grassTile, rocks, bushes;
    public TreeClass[] trees;
    public int mapHeight;
    public int mapWidth;
    public LayerMask TargetLayer;
    int[,] map, inslandMap, StartMap, detailsMap;
    #endregion
    private void Awake()
    {
        tileMap.ClearAllTiles();
        detailsTileMap.ClearAllTiles();

        StartMap = new int[15, 4];
        map = new int[15, 4];

        #region jumpStartMap
        //its suck for now // redo later // maybe an Map editor// or create map from editor;
        StartMap[0, 0] = 0; StartMap[0, 1] = 1; StartMap[0, 2] = 1; StartMap[0, 3] = 1;
        StartMap[1, 0] = 0; StartMap[1, 1] = 1; StartMap[1, 2] = 1; StartMap[1, 3] = 1;
        StartMap[2, 0] = 0; StartMap[2, 1] = 1; StartMap[2, 2] = 1; StartMap[2, 3] = 1;
        StartMap[3, 0] = 0; StartMap[3, 1] = 1; StartMap[3, 2] = 1; StartMap[3, 3] = 1;
        StartMap[4, 0] = 1; StartMap[4, 1] = 1; StartMap[4, 2] = 1; StartMap[4, 3] = 1;
        StartMap[5, 0] = 1; StartMap[5, 1] = 1; StartMap[5, 2] = 1; StartMap[5, 3] = 1;
        StartMap[6, 0] = 1; StartMap[6, 1] = 1; StartMap[6, 2] = 1; StartMap[6, 3] = 1;
        StartMap[7, 0] = 1; StartMap[7, 1] = 1; StartMap[7, 2] = 1; StartMap[7, 3] = 1;
        StartMap[8, 0] = 1; StartMap[8, 1] = 1; StartMap[8, 2] = 1; StartMap[8, 3] = 1;
        StartMap[9, 0] = 1; StartMap[9, 1] = 1; StartMap[9, 2] = 1; StartMap[9, 3] = 1;
        StartMap[10, 0] = 1; StartMap[10, 1] = 1; StartMap[10, 2] = 1; StartMap[10, 3] = 1;
        StartMap[11, 0] = 1; StartMap[11, 1] = 1; StartMap[11, 2] = 1; StartMap[11, 3] = 1;
        StartMap[12, 0] = 1; StartMap[12, 1] = 1; StartMap[12, 2] = 1; StartMap[12, 3] = 1;
        StartMap[13, 0] = 0; StartMap[13, 1] = 0; StartMap[13, 2] = 1; StartMap[13, 3] = 1;
        StartMap[14, 0] = 0; StartMap[14, 1] = 0; StartMap[14, 2] = 1; StartMap[14, 3] = 1;
        #endregion

        //map = StartMap;
        map = perlinNoiseGenerator();
        drawMap(map, tileMap, tileToDraw, new Vector2Int(0, 0));
        GenerateDetails(map, new Vector2Int(0, 0));
    }
    private void Update()
    {

        CompositeCollider2D cc = tileMap.GetComponent<CompositeCollider2D>();
        Vector2 tilesCenter = new Vector2(cc.bounds.center.x, cc.bounds.center.y);

        //raycast
        RaycastHit2D playerHit = Physics2D.Raycast(tilesCenter, Vector2.up, Mathf.Infinity,
                                                   TargetLayer, Mathf.Infinity, Mathf.Infinity);

        if (playerHit.collider != null)
        {

            //Debug.Log("hit");
            int offSetX = UnityEngine.Random.Range(6, 12);
            int offSetY = UnityEngine.Random.Range(-1, 1);
            int[,] lastMap = new int[map.GetUpperBound(0), map.GetUpperBound(1)];
            lastMap = map;
            map = perlinNoiseGenerator();

            DrawIsland(offSetX, offSetY, lastMap, tilesCenter);

            drawMap(map, tileMap, tileToDraw, new Vector2Int(Mathf.RoundToInt(tilesCenter.x * 2)
                                                             + offSetX, offSetY));

            GenerateDetails(map, new Vector2Int(Mathf.RoundToInt(tilesCenter.x * 2)
                                                             + offSetX, offSetY));


        }
    }
    public void DrawIsland(int offSetX, int offSetY, int[,] oldMap, Vector2 tilesCenter)
    {

        inslandMap = walkOnTop(2, 2, (offSetX / 2) + 1, offSetX * 2);
        int posYOld = 0;
        int posYNew = 0;
        int positionX = Mathf.RoundToInt(tilesCenter.x * 2) + (offSetX / 3) + UnityEngine.Random.Range(-1, 1);


        //calcular posicao
        //LastMap
        for (int x = oldMap.GetLowerBound(0); x <= oldMap.GetUpperBound(0) - 1; x++)
        {
            for (int y = oldMap.GetLowerBound(1); y <= oldMap.GetUpperBound(1); y++)
            {
                if (oldMap[x, y] == 1)
                {
                    posYOld = y;
                }
            }
        }

        //NewMap
        for (int x = map.GetUpperBound(0); x >= map.GetLowerBound(0); x--)
        {
            for (int y = map.GetLowerBound(1); y <= map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    posYNew = y;
                }

            }
        }

        Vector2Int newPos;

        //the Y change (switch)
        if (posYNew + offSetY < posYOld)
        {
            newPos = new Vector2Int(positionX, posYNew - 1);
            drawMap(inslandMap, tileMap, tileToDraw, newPos);

        }
        else if (posYNew + offSetY > posYOld)
        {
            if ((posYNew + offSetY) - (posYOld + offSetY) >= 2)
            {
                newPos = new Vector2Int(positionX + 1, posYOld - 1);
                inslandMap = walkOnTop(2, 3, (offSetX / 2) + 1, Mathf.CeilToInt(offSetX * 2.5f));
                drawMap(inslandMap, tileMap, tileToDraw, newPos);
            }
            else
            {
                newPos = new Vector2Int(positionX, posYOld - 1);
                drawMap(inslandMap, tileMap, tileToDraw, newPos);
            }
        }
        else
        {
            newPos = new Vector2Int(positionX, posYOld - 2);
            drawMap(inslandMap, tileMap, tileToDraw, newPos);
        }

        GenerateDetails(inslandMap, newPos);
    }
    public int[,] perlinNoiseGenerator()
    {
        int[,] map = new int[UnityEngine.Random.Range(mapWidth / 3, mapWidth), mapHeight]; //MinWidth//MaxWidth//MaxHeight
        int initialPoint;
        float perlinOffset = UnityEngine.Random.Range(0.1f, 0.17f); //0.5f; //seed//
        float perlinSeed = UnityEngine.Random.Range(0.03f, 0.27f);

        //Debug.Log("Seed: " + perlinOffset + ";" + perlinSeed);

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            initialPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x * perlinSeed, map.GetUpperBound(0) * perlinSeed) - perlinOffset) * map.GetUpperBound(1));
            initialPoint += ((map.GetUpperBound(1) / 2)) - 1;
            for (int y = initialPoint; y >= 0; y--)
            {
                //Debug.Log(x + ";" + y );
                map[x, y] = 1;
            }
        }
        return map;
    }
    //WalkOnTopGenerator
    public int[,] walkOnTop(int minSection, int initialHeight, int mapWidth, int mapHeight)
    {
        int[,] walkOnTopMap = new int[mapWidth, mapHeight];
        System.Random random = new System.Random(0.5f.GetHashCode());
        int moves = 0;
        int section = 0;

        for (int x = 0; x < walkOnTopMap.GetUpperBound(0); x++)
        {
            moves = random.Next(2);
            if (moves == 0 && initialHeight > 0 && section > minSection)
            {
                initialHeight--;
                section = 0;
            }

            else if (moves == 1 && initialHeight < walkOnTopMap.GetUpperBound(1) && section > minSection)
            {
                initialHeight++;
                section = 0;
            }
            section++;
            for (int y = initialHeight; y >= 0; y--)
            {
                walkOnTopMap[x, y] = 1;
            }
        }

        return walkOnTopMap;
    }
    //Draw a map on A tileMap
    public void drawMap(int[,] map, Tilemap tilemap, RuleTile tile, Vector2Int Sposition)
    {

        for (int x = map.GetLowerBound(0); x <= map.GetUpperBound(0); x++, Sposition.x++)
        {
            for (int y = map.GetLowerBound(1); y <= map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(Sposition.x, y + Sposition.y, 0), tile);
                }

            }
        }
    }
    //DrawDetails 
    public void GenerateDetails(int[,] baseMap, Vector2Int sPosition)
    {
        int newHeight = 0;
        int oldHeight = 0;
        int countTiles = 1;
        //iterate basemap
        for (int x = 0; x < baseMap.GetUpperBound(0); x++) //width
        {
            for (int y = baseMap.GetUpperBound(1); y > baseMap.GetLowerBound(1); y--)
            {
                if (baseMap[x, y] == 1)
                {
                    newHeight = y;
                    y = 0;
                }
            }

            if (oldHeight != newHeight)
            {
                //Here is the point where map goes Up or Down
                //Debug.Log(countTiles);
                DrawDetails(new Vector2Int((sPosition.x + x) - countTiles, (oldHeight + sPosition.y)), countTiles);
                //need to check if there`s nothing on the next one
                oldHeight = newHeight;
                countTiles = 1;
            }

            else if (oldHeight == newHeight)
            {
                countTiles++;
            }

            if (x == baseMap.GetUpperBound(0) - 1)
            {
                Debug.Log("lastOne");
                Debug.Log(countTiles);
                Debug.Log(x);
                DrawDetails(new Vector2Int((sPosition.x + x) - countTiles + 1,  (oldHeight + sPosition.y)), countTiles);
            }
        }
    }
    public void DrawDetails(Vector2Int pos, int width)
    {
        if (width == 2)
        {
            Debug.Log("entrou 2");
            for (int x = 0; x < width; x++)
            {
                //60% each tile
                if (UnityEngine.Random.Range(0, 10) < 8)
                {
                    detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                }

            }
        }
        if (width == 3)
        {
            int check = UnityEngine.Random.Range(0, 10);
            //check if rock will be draw
            if (check < 5)
            {
                //Random Grass 40% each tile
                for (int x = 0; x < width; x++)
                {
                    if (UnityEngine.Random.Range(0, 10) < 7)
                    {
                        detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }
                //drawRock
                int drawPositionX = UnityEngine.Random.Range(0, 3);
                detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                      rocks);

                //random Grass // 40% each tile
            }
            else if (check >= 5 && check < 8)
            {

                for (int x = 0; x < width; x++)
                {
                    if (UnityEngine.Random.Range(0, 10) < 4)
                    {
                        detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }
                //drawBush
                int drawPositionX = UnityEngine.Random.Range(0, 3);
                detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                      bushes);

                //random Grass // 40% each tile
            }
            else
            {
                //only grass// 60% each tile
                for (int x = 0; x < width; x++)
                {
                    if (UnityEngine.Random.Range(0, 10) < 8)
                    {
                        detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }
                //Debug.Log("drew grass only");
            }
        }
        if (width > 3 && width < 6)
        {

            int check = UnityEngine.Random.Range(0, 10);
            int xPosition = UnityEngine.Random.Range(0, width - 1);
            //spawnTree
            if (check < 8)
            {
                //wich tree
                //big
                if (check < 2)
                {
                    //grass
                    for (int x = 0; x < width; x++)
                    {
                        if (UnityEngine.Random.Range(0, 10) < 8)
                        {
                            detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                        }
                    }

                    //rock
                    if (UnityEngine.Random.Range(0, 10) < 4)
                    {
                        int drawPositionX = UnityEngine.Random.Range(0, width);
                        detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                          rocks);
                    }

                    //bush
                    if (UnityEngine.Random.Range(0, 10) < 5)
                    {
                        int drawPositionX = UnityEngine.Random.Range(0, 3);
                        detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                          bushes);
                    }

                    //draw tree Big
                    for (int x = 0; x < trees[2].treeBase.Length; x++)
                    {
                        detailsTileMap.SetTile(new Vector3Int(xPosition + pos.x
                        + x, pos.y + 1, 0), trees[2].treeBase[x]);
                        detailsTileMap.SetTile(new Vector3Int(xPosition
                        + pos.x + x, pos.y + 2, 0), trees[2].treeHeight[x]);
                    }
                }
                //medium
                else if (check >= 2 && check <= 6)
                {
                    //grass
                    //grass
                    for (int x = 0; x < width; x++)
                    {
                        if (UnityEngine.Random.Range(0, 10) < 8)
                        {
                            detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                        }
                    }

                    //rock
                    if (UnityEngine.Random.Range(0, 10) < 4)
                    {
                        int drawPositionX = UnityEngine.Random.Range(0, width);
                        detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                          rocks);
                    }

                    //bush
                    if (UnityEngine.Random.Range(0, 10) < 5)
                    {
                        int drawPositionX = UnityEngine.Random.Range(0, 3);
                        detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                          bushes);
                    }

                    //medium Tree
                    for (int x = 0; x < trees[1].treeBase.Length; x++)
                    {
                        detailsTileMap.SetTile(new Vector3Int(xPosition + pos.x
                        + x, pos.y + 1, 0), trees[1].treeBase[x]);
                        detailsTileMap.SetTile(new Vector3Int(xPosition + pos.x
                        + x, pos.y + 2, 0), trees[1].treeHeight[x]);
                    }
                }
                //small
                else
                {
                    //grass
                    for (int x = 0; x < width; x++)
                    {
                        if (UnityEngine.Random.Range(0, 10) < 8)
                        {
                            detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                        }
                    }

                    //rock
                    if (UnityEngine.Random.Range(0, 10) < 4)
                    {
                        int drawPositionX = UnityEngine.Random.Range(0, width);
                        detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                          rocks);
                    }

                    //bush
                    if (UnityEngine.Random.Range(0, 10) < 5)
                    {
                        int drawPositionX = UnityEngine.Random.Range(0, 3);
                        detailsTileMap.SetTile(new Vector3Int(pos.x + drawPositionX, pos.y + 1, 0),
                                                          bushes);
                    }

                    //small Tree
                    for (int x = 0; x < trees[0].treeBase.Length; x++)
                    {
                        detailsTileMap.SetTile(new Vector3Int(xPosition + pos.x, pos.y + 1, 0),
                        trees[0].treeBase[x]);
                        detailsTileMap.SetTile(new Vector3Int(xPosition + pos.x, pos.y + 2, 0),
                        trees[0].treeHeight[x]);
                    }
                }
            }

            else
            {

                for (int x = 0; x < width; x++)
                {
                    if (UnityEngine.Random.Range(0, 10) < 8)
                    {
                        detailsTileMap.SetTile(new Vector3Int(pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }

            }


        }
        if (width >= 6)
        {
            //quebrar na metade e chamar draw Details denovo
            //exemplo width = 8, chamar para 4 , depois para 4 novamente.
        }
    }
}
