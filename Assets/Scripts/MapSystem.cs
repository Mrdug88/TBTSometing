using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEditor;
using System.Linq;


public class MapSystem : MonoBehaviour
{

    public Tilemap tileMap;
    public RuleTile tileToDraw;
    public int mapHeight;
    public int mapWidth;
    public LayerMask TargetLayer;
    int[,] map;
    public GameObject player = null;


    private void Awake()
    {

        tileMap.ClearAllTiles();
        map = walkOnTop(UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(3, mapHeight-1));
        drawMap(map, tileMap, tileToDraw, new Vector2Int(0, 0));

    }

    private void Update()
    {

        CompositeCollider2D cc = tileMap.GetComponent<CompositeCollider2D>();
        Vector2 tilesCenter = new Vector2(cc.bounds.center.x, cc.bounds.center.y);

        //raycast
        RaycastHit2D playerHit = Physics2D.Raycast(tilesCenter, Vector2.up, Mathf.Infinity, TargetLayer, Mathf.Infinity, Mathf.Infinity);

        if (playerHit.collider != null)
        {
            //calcule sPosition
            Debug.Log("hit");
            map = walkOnTop(UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(3, mapHeight-1));
            drawMap(map, tileMap, tileToDraw, new Vector2Int(Mathf.RoundToInt(tilesCenter.x * 2) + 3, 0));

        }

		//<---->//       
		//TODO
		//Draw Holes (5-10)
		//Small Islands (3-5) (height?)
 		//Conect  To an UpperLane
		//Enemies
		//Colectables
		//<---->//
    }

    //WalkOnTopGenerator
    public int[,] walkOnTop(int minSection, int initialHeight)
    {
        int[,] walkOnTopMap = new int[mapWidth, mapHeight];
        System.Random random = new System.Random(0.3f.GetHashCode());
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
            for (int y = map.GetLowerBound(1); y <= map.GetUpperBound(1); y++, Sposition.y++)
            {
                if (map[x, y] == 1)
                {

                    //Debug.Log(position);
                    //Debug.Log("("+x+","+y+")");
                    //tilemap.SetTile(new Vector3Int(x,y,0),tile);
                    tilemap.SetTile(new Vector3Int(Sposition.x, Sposition.y, 0), tile);
                }
            }
            Sposition.y = 0; //resetWhenDone
        }
    }

}
