using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : MonoBehaviour {
    #region Declarations
    public Tilemap tileMap, detailsTileMap, WaterfallsTileMap, trapTilemap,
    trapTilemap2, toptileMap, topTrapTileMap;
    public RuleTile tileToDraw;
    public RandomTile grassTile, rocks, bushes;
    public RuleTile waterfallTile;
    public Tile trapTile;
    public enum mapType { platform = 1, island = 2 }
    public float waterfallMinTimer = 500f;
    public float waterfallMaxTimer = 1000f;
    float waterFalltime;
    public TreeClass[] trees;
    public int bottomMapHeight = 8;
    public int bottomMapWidth = 20;
    public int topMapHeight = 8;
    public int topMapWidth = 40;
    public LayerMask TargetLayer;
    int[, ] map, inslandMap, StartMap, detailsMap, topMap;
    public static Vector2Int lastCheck;
    int lastTopXposition;
    private PiranhaPool piranhaPool;

    #endregion
    private void Awake () {
        //Pools Initialization
        piranhaPool = GetComponent<PiranhaPool> ();

        tileMap.ClearAllTiles ();
        detailsTileMap.ClearAllTiles ();
        WaterfallsTileMap.ClearAllTiles ();
        StartMap = new int[15, 4];
        map = new int[15, 4];
        topMap = new int[15, 4];

        #region jumpStartMap
        //its suck for now // redo later // maybe an Map editor// or create map from editor;
        StartMap[0, 0] = 0;
        StartMap[0, 1] = 1;
        StartMap[0, 2] = 1;
        StartMap[0, 3] = 1;
        StartMap[1, 0] = 0;
        StartMap[1, 1] = 1;
        StartMap[1, 2] = 1;
        StartMap[1, 3] = 1;
        StartMap[2, 0] = 0;
        StartMap[2, 1] = 1;
        StartMap[2, 2] = 1;
        StartMap[2, 3] = 1;
        StartMap[3, 0] = 0;
        StartMap[3, 1] = 1;
        StartMap[3, 2] = 1;
        StartMap[3, 3] = 1;
        StartMap[4, 0] = 1;
        StartMap[4, 1] = 1;
        StartMap[4, 2] = 1;
        StartMap[4, 3] = 1;
        StartMap[5, 0] = 1;
        StartMap[5, 1] = 1;
        StartMap[5, 2] = 1;
        StartMap[5, 3] = 1;
        StartMap[6, 0] = 1;
        StartMap[6, 1] = 1;
        StartMap[6, 2] = 1;
        StartMap[6, 3] = 1;
        StartMap[7, 0] = 1;
        StartMap[7, 1] = 1;
        StartMap[7, 2] = 1;
        StartMap[7, 3] = 1;
        StartMap[8, 0] = 1;
        StartMap[8, 1] = 1;
        StartMap[8, 2] = 1;
        StartMap[8, 3] = 1;
        StartMap[9, 0] = 1;
        StartMap[9, 1] = 1;
        StartMap[9, 2] = 1;
        StartMap[9, 3] = 1;
        StartMap[10, 0] = 1;
        StartMap[10, 1] = 1;
        StartMap[10, 2] = 1;
        StartMap[10, 3] = 1;
        StartMap[11, 0] = 1;
        StartMap[11, 1] = 1;
        StartMap[11, 2] = 1;
        StartMap[11, 3] = 1;
        StartMap[12, 0] = 1;
        StartMap[12, 1] = 1;
        StartMap[12, 2] = 1;
        StartMap[12, 3] = 1;
        StartMap[13, 0] = 0;
        StartMap[13, 1] = 0;
        StartMap[13, 2] = 1;
        StartMap[13, 3] = 1;
        StartMap[14, 0] = 0;
        StartMap[14, 1] = 0;
        StartMap[14, 2] = 1;
        StartMap[14, 3] = 1;
        #endregion

        //map = StartMap;
        map = PerlinNoiseGenerator (bottomMapWidth, bottomMapHeight);
        topMap = PerlinNoiseGenerator (topMapWidth, topMapHeight);

        DrawMap (topMap, toptileMap, tileToDraw, new Vector2Int (0, 0));
        lastTopXposition = topMap.GetUpperBound (0);

        DrawMap (map, tileMap, tileToDraw, new Vector2Int (0, 0));

        DrawWaterFall (map, 0);
        waterFalltime = UnityEngine.Random.Range (waterfallMinTimer, waterfallMaxTimer);
        GenerateDetails (map, new Vector2Int (0, 0));

        int offSetX = UnityEngine.Random.Range (6, 12);
        int offSetY = UnityEngine.Random.Range (-1, 1);

        int[, ] lastMap = new int[map.GetUpperBound (0), map.GetUpperBound (1)];
        lastMap = map;

        //CompositeCollider2D cc = tileMap.GetComponent<CompositeCollider2D>();

        Vector2 tilesCenter = new Vector2 (((float) tileMap.size.x / 2), 0);
        map = PerlinNoiseGenerator (bottomMapWidth, bottomMapHeight);
        lastCheck.x = DrawIsland (offSetX, offSetY, lastMap, tilesCenter);
        lastCheck.y = 0;
        DrawMap (map, tileMap, tileToDraw, new Vector2Int (Mathf.RoundToInt (tilesCenter.x * 2) +
            offSetX, offSetY));
        GenerateSpikes (map, new Vector2Int (Mathf.RoundToInt (tilesCenter.x * 2) +
            offSetX, offSetY), mapType.platform);
        GenerateDetails (map, new Vector2Int (Mathf.RoundToInt (tilesCenter.x * 2) +
            offSetX, offSetY));

        //drawTop

    }
    private void Update () {
        Vector2 tilesCenter = new Vector2 (((float) tileMap.size.x / 2), 0);

        RaycastHit2D playerHit = Physics2D.Raycast (lastCheck, Vector2.up, Mathf.Infinity,
            TargetLayer, Mathf.Infinity, Mathf.Infinity);

        if (playerHit.collider != null)
        {
            int offSetX = UnityEngine.Random.Range(6, 12);
            int offSetY = UnityEngine.Random.Range(-1, 1);
            int[,] lastMap = new int[map.GetUpperBound(0), map.GetUpperBound(1)];
            lastMap = map;

            lastCheck.x = DrawIsland(offSetX, offSetY, lastMap, tilesCenter);

            //Call EnemiePool SpawnPiranha //recalculate position
            //CoinFlip
            if (UnityEngine.Random.Range(0,10) <= 3)
             CallPeranha(tilesCenter, offSetX);

            //Platform Bottom
            map = PerlinNoiseGenerator(bottomMapWidth, bottomMapHeight);
            DrawMap(map, tileMap, tileToDraw, new Vector2Int(Mathf.RoundToInt(tilesCenter.x * 2) +
                offSetX, offSetY));

            //TopPlatForm
            topMap = PerlinNoiseGenerator(topMapWidth, topMapHeight);
            DrawMap(topMap, toptileMap, tileToDraw, new Vector2Int(lastTopXposition, 0));
            lastTopXposition += topMap.GetUpperBound(0);

            GenerateSpikes(map, new Vector2Int(Mathf.RoundToInt(tilesCenter.x * 2) +
                offSetX, offSetY), mapType.platform);

            GenerateDetails(map, new Vector2Int(Mathf.RoundToInt(tilesCenter.x * 2) +
                offSetX, offSetY));

            if (waterFalltime <= 0)
            {
                DrawWaterFall(map, Mathf.RoundToInt(tilesCenter.x * 2) + offSetX);
                waterFalltime = UnityEngine.Random.Range(waterfallMinTimer, waterfallMaxTimer);
            }
        }

        if (waterFalltime >= 0) {
            waterFalltime = waterFalltime - Time.time;
        }
    }

    private void CallPeranha(Vector2 tilesCenter, int offSetX)
    {
        Vector2 peranhaPosition = new Vector2(UnityEngine.Random.Range((lastCheck.x + (offSetX / 2) + 0.5f),
        Mathf.RoundToInt((tilesCenter.x * 2) + offSetX) - 0.5f), -2);
        piranhaPool.updatePiranhaPos(peranhaPosition);
    }

    public int DrawIsland (int offSetX, int offSetY, int[, ] oldMap, Vector2 tilesCenter) {

        inslandMap = WalkOnTop (2, 2, (offSetX / 2) + 1, offSetX * 2);
        int posYOld = 0;
        int posYNew = 0;
        int positionX = Mathf.RoundToInt (tilesCenter.x * 2) + (offSetX / 3) + UnityEngine.Random.Range (-1, 1);

        //calcular posicao
        //LastMap
        for (int x = oldMap.GetLowerBound (0); x <= oldMap.GetUpperBound (0) - 1; x++) {
            for (int y = oldMap.GetLowerBound (1); y <= oldMap.GetUpperBound (1); y++) {
                if (oldMap[x, y] == 1) {
                    posYOld = y;
                }
            }
        }

        //NewMap
        for (int x = map.GetUpperBound (0); x >= map.GetLowerBound (0); x--) {
            for (int y = map.GetLowerBound (1); y <= map.GetUpperBound (1); y++) {
                if (map[x, y] == 1) {
                    posYNew = y;
                }

            }
        }

        Vector2Int newPos;

        //the Y change (switch)
        if (posYNew + offSetY < posYOld) {
            newPos = new Vector2Int (positionX, posYNew - 1);
            DrawMap (inslandMap, tileMap, tileToDraw, newPos);

        } else if (posYNew + offSetY > posYOld) {
            if ((posYNew + offSetY) - (posYOld + offSetY) >= 2) {
                newPos = new Vector2Int (positionX + 1, posYOld - 1);
                inslandMap = WalkOnTop (2, 3, (offSetX / 2) + 1, Mathf.CeilToInt (offSetX * 2.5f));
                DrawMap (inslandMap, tileMap, tileToDraw, newPos);
            } else {
                newPos = new Vector2Int (positionX, posYOld - 1);
                DrawMap (inslandMap, tileMap, tileToDraw, newPos);
            }
        } else {
            newPos = new Vector2Int (positionX, posYOld - 2);
            DrawMap (inslandMap, tileMap, tileToDraw, newPos);
        }

        GenerateDetails (inslandMap, newPos);
        GenerateSpikes (inslandMap, newPos, mapType.island);
        return newPos.x;
    }
    public int[, ] PerlinNoiseGenerator (int mapWidht, int mapHeight) {
        int[, ] map = new int[UnityEngine.Random.Range (mapWidht / 3, mapWidht), mapHeight]; //MinWidth//MaxWidth//MaxHeight
        int initialPoint;
        float perlinOffset = 0.1f; // UnityEngine.Random.Range(0.1f, 0.15f); //0.5f; //seed//
        //float perlinSeed = UnityEngine.Random.Range(0.1f, 0.2f);
        float perlinSeed = 0.05f;

        for (int x = 0; x < map.GetUpperBound (0); x++) {
            initialPoint = Mathf.FloorToInt ((Mathf.PerlinNoise (x * perlinSeed, map.GetUpperBound (0) * perlinSeed) - perlinOffset) * map.GetUpperBound (1));
            initialPoint += ((map.GetUpperBound (1) / 2)) - 1;
            for (int y = initialPoint; y >= 0; y--) {
                map[x, y] = 1;
            }
        }
        return map;
    }
    public int[, ] WalkOnTop (int minSection, int initialHeight, int mapWidth, int mapHeight) {
        int[, ] walkOnTopMap = new int[mapWidth, mapHeight];
        System.Random random = new System.Random (0.5f.GetHashCode ());
        int moves = 0;
        int section = 0;

        for (int x = 0; x < walkOnTopMap.GetUpperBound (0); x++) {
            moves = random.Next (2);
            if (moves == 0 && initialHeight > 0 && section > minSection) {
                initialHeight--;
                section = 0;
            } else if (moves == 1 && initialHeight < walkOnTopMap.GetUpperBound (1) && section > minSection) {
                initialHeight++;
                section = 0;
            }
            section++;
            for (int y = initialHeight; y >= 0; y--) {
                walkOnTopMap[x, y] = 1;
            }
        }

        return walkOnTopMap;
    }
    public void DrawMap (int[, ] map, Tilemap tilemap, RuleTile tile, Vector2Int Sposition) {

        for (int x = map.GetLowerBound (0); x <= map.GetUpperBound (0); x++, Sposition.x++) {
            for (int y = map.GetLowerBound (1); y <= map.GetUpperBound (1); y++) {
                if (map[x, y] == 1) {
                    tilemap.SetTile (new Vector3Int (Sposition.x, y + Sposition.y, 0), tile);
                }

            }
        }
    }
    public void GenerateDetails (int[, ] baseMap, Vector2Int sPosition) {
        int newHeight = 0;
        int oldHeight = 0;
        int countTiles = 1;
        //iterate basemap
        for (int x = 0; x < baseMap.GetUpperBound (0); x++) //width
        {
            for (int y = baseMap.GetUpperBound (1); y > baseMap.GetLowerBound (1); y--) {
                if (baseMap[x, y] == 1) {
                    newHeight = y;
                    y = 0;
                }
            }

            if (oldHeight != newHeight) {
                DrawDetails (new Vector2Int ((sPosition.x + x) - countTiles, (oldHeight + sPosition.y)), countTiles);
                oldHeight = newHeight;
                countTiles = 1;
            } else if (oldHeight == newHeight) {
                countTiles++;
            }

            if (x == baseMap.GetUpperBound (0) - 1) {
                DrawDetails (new Vector2Int ((sPosition.x + x) - countTiles + 1, (oldHeight + sPosition.y)), countTiles);
            }
        }
    }
    public void DrawDetails (Vector2Int pos, int width) {
        if (width == 2) {
            for (int x = 0; x < width; x++) {
                //60% each tile
                if (UnityEngine.Random.Range (0, 10) < 8) {
                    detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                }

            }
        }
        if (width == 3) {
            int check = UnityEngine.Random.Range (0, 10);
            //check if rock will be draw
            if (check < 6) {
                for (int x = 0; x < width; x++) {
                    if (UnityEngine.Random.Range (0, 10) < 8) {
                        detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }
                //drawRock
                int drawPositionX = UnityEngine.Random.Range (0, 3);
                detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                    rocks);

                //random Grass // 40% each tile
            } else if (check > 5 && check < 10) {

                for (int x = 0; x < width; x++) {
                    if (UnityEngine.Random.Range (0, 10) < 4) {
                        detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }
                //drawBush
                int drawPositionX = UnityEngine.Random.Range (0, 3);
                detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                    bushes);

                //random Grass // 40% each tile
            } else {
                //only grass// 60% each tile
                for (int x = 0; x < width; x++) {
                    if (UnityEngine.Random.Range (0, 10) < 8) {
                        detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }
            }
        }
        if (width > 3 && width < 6) {

            int check = UnityEngine.Random.Range (0, 10);
            int xPosition = UnityEngine.Random.Range (0, width - 1);
            //spawnTree
            if (check < 8) {
                //wich tree
                //big
                if (check < 2) {
                    //grass
                    for (int x = 0; x < width; x++) {
                        if (UnityEngine.Random.Range (0, 10) < 8) {
                            detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                        }
                    }

                    //rock
                    if (UnityEngine.Random.Range (0, 10) < 7) {
                        int drawPositionX = UnityEngine.Random.Range (0, width);
                        detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                            rocks);
                    }

                    //bush
                    if (UnityEngine.Random.Range (0, 10) < 7) {
                        int drawPositionX = UnityEngine.Random.Range (0, 3);
                        detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                            bushes);
                    }

                    //draw tree Big
                    for (int x = 0; x < trees[2].treeBase.Length; x++) {
                        detailsTileMap.SetTile (new Vector3Int (xPosition + pos.x +
                            x, pos.y + 1, 0), trees[2].treeBase[x]);
                        detailsTileMap.SetTile (new Vector3Int (xPosition +
                            pos.x + x, pos.y + 2, 0), trees[2].treeHeight[x]);
                    }
                }
                //medium
                else if (check >= 2 && check <= 6) {
                    //grass
                    //grass
                    for (int x = 0; x < width; x++) {
                        if (UnityEngine.Random.Range (0, 10) < 8) {
                            detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                        }
                    }

                    //rock
                    if (UnityEngine.Random.Range (0, 10) < 4) {
                        int drawPositionX = UnityEngine.Random.Range (0, width);
                        detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                            rocks);
                    }

                    //bush
                    if (UnityEngine.Random.Range (0, 10) < 5) {
                        int drawPositionX = UnityEngine.Random.Range (0, 3);
                        detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                            bushes);
                    }

                    //medium Tree
                    for (int x = 0; x < trees[1].treeBase.Length; x++) {
                        detailsTileMap.SetTile (new Vector3Int (xPosition + pos.x +
                            x, pos.y + 1, 0), trees[1].treeBase[x]);
                        detailsTileMap.SetTile (new Vector3Int (xPosition + pos.x +
                            x, pos.y + 2, 0), trees[1].treeHeight[x]);
                    }
                }
                //small
                else {
                    //grass
                    for (int x = 0; x < width; x++) {
                        if (UnityEngine.Random.Range (0, 10) < 8) {
                            detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                        }
                    }

                    //rock
                    if (UnityEngine.Random.Range (0, 10) < 7) {
                        int drawPositionX = UnityEngine.Random.Range (0, width);
                        detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                            rocks);
                    }

                    //bush
                    if (UnityEngine.Random.Range (0, 10) < 7) {
                        int drawPositionX = UnityEngine.Random.Range (0, 3);
                        detailsTileMap.SetTile (new Vector3Int (pos.x + drawPositionX, pos.y + 1, 0),
                            bushes);
                    }

                    //small Tree
                    for (int x = 0; x < trees[0].treeBase.Length; x++) {
                        detailsTileMap.SetTile (new Vector3Int (xPosition + pos.x, pos.y + 1, 0),
                            trees[0].treeBase[x]);
                        detailsTileMap.SetTile (new Vector3Int (xPosition + pos.x, pos.y + 2, 0),
                            trees[0].treeHeight[x]);
                    }
                }
            } else {

                for (int x = 0; x < width; x++) {
                    if (UnityEngine.Random.Range (0, 10) < 8) {
                        detailsTileMap.SetTile (new Vector3Int (pos.x + x, pos.y + 1, 0), grassTile);
                    }
                }

            }

        }
        if (width >= 6) {
            int resto = width % 2;
            DrawDetails (pos, width / 2);
            DrawDetails (new Vector2Int (pos.x + (width / 2), pos.y), (width / 2) + resto); // working but the position      
            //exemplo width = 8, chamar para 4 , depois para 4 novamente.
        }
    }
    public void GenerateSpikes (int[, ] map, Vector2Int sPosition, mapType type) {

        if (type == mapType.platform) {
            for (int x = 0; x < map.GetUpperBound (0); x++) {
                for (int y = 0; y < map.GetUpperBound (1); y++) {
                    if (!(y == 0 || x == 0 || x == map.GetUpperBound (0) - 1 || y == map.GetUpperBound (1))) {
                        if (map[x, y] == 0 && map[x, y - 1] == 1 //Bottom
                            &&
                            map[x - 1, y] == 0 //Left
                            &&
                            map[x, y + 1] == 0 //Top
                            &&
                            map[x + 1, y] == 1) //Right
                        {
                            trapTilemap.SetTile (new Vector3Int (sPosition.x + x, sPosition.y + y, 0),
                                trapTile);
                        }

                    }
                }
            }
        } else if (type == mapType.island) {
            for (int x = 0; x < map.GetUpperBound (0); x++) {
                for (int y = 0; y < map.GetUpperBound (1); y++) {
                    //Corner One
                    if (x == 0 && y > 0 &&
                        map[x, y] == 1 &&
                        map[x + 1, y] == 1 //Right
                        &&
                        map[x, y - 1] == 1 //Bottom
                        &&
                        map[x, y + 1] == 0) //Top
                    {
                        trapTilemap.SetTile (new Vector3Int (sPosition.x + x - 1, sPosition.y + y, 0),
                            trapTile);
                    }

                    if (!(y == 0 || x == 0 || y == map.GetUpperBound (1))) {
                        if (map[x, y] == 0 && map[x - 1, y] == 1 &&
                            map[x + 1, y] == 0 &&
                            map[x, y - 1] == 1 &&
                            map[x, y + 1] == 0) {
                            trapTilemap2.SetTile (new Vector3Int (sPosition.x + x, sPosition.y + y, 0),
                                trapTile);
                            Matrix4x4 matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.Euler (0f, 0f, -90f),
                                Vector3.one);
                            trapTilemap2.SetTransformMatrix (new Vector3Int (sPosition.x + x, sPosition.y + y, 0), matrix);
                        }
                    }

                }
            }
        }

    }
    public void DrawWaterFall (int[, ] mapReference, int Sposition) {
        float width = UnityEngine.Random.Range (0, 3);
        int startPosition = UnityEngine.Random.Range (0, map.GetUpperBound (0));
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= mapReference.GetUpperBound (1) * 3; y++) {
                WaterfallsTileMap.SetTile (new Vector3Int (Sposition + startPosition + x, -y, 0), waterfallTile);
            }
        }
    }
}