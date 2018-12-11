using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralMapGenarator : MonoBehaviour
{

    public RuleTile tileToDrow;
    public Tilemap tileMap;

    int[,] map;

    //need to randomize the height between some values//
    //also randomize the lenght between soem values//

    private void Start()
    {
        //generate a multidimensional array. (width,height)
        map = AuxCommands.GenerateArray(30, 5, false);
        drawTiles(tileMap,map,tileToDrow);
        
        
    }



    //drawTileMapsOnGrid
    void drawTiles(Tilemap tilemap, int[,] map, RuleTile tile)
    {
        for (int i = 0; i < map.GetUpperBound(0); i++)
        {
            for (int j = 0; j < map.GetUpperBound(1); j++)
            {
                if (map[i, j] == 1)
                {
                    //position
                    tilemap.SetTile(new Vector3Int(i, j, 0), tile);
                }
            }
        }
    }


}
