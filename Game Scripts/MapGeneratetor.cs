using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class MapGeneratetor : MonoBehaviour
{
    public static MapGeneratetor instance;

    public HexTile hexTile;


    public GameObject hexMap;

    public int mapSize = 2;


    void Start()
    {
        instance = gameObject.GetComponent<MapGeneratetor>();
        generate();
    }



    public void generate()
    {
        for (int y = -mapSize; y <= mapSize; y++)
        {
            int startx;
            int endx;
            if (y > 0)
            {
                startx = -mapSize;
                endx = mapSize - y;
            }
            else
            {
                startx = -mapSize - y;
                endx = mapSize;
            }


            for (int x = startx; x <= endx; x++)
            {
                HexTile tile;
                tile = Instantiate(hexTile, new Vector3((0.3f * y) + (x * 0.6f), (y * 0.345f), 0.1f * y), Quaternion.identity);
                tile.x = x;
                tile.y = y;
                tile.name = "Hex_Grid(" + x + ","+ y + ")";
                tile.transform.SetParent(hexMap.transform);
                ArenaUIManager.instance.hexTileList.Add(tile);
            }
 
        }
        
    }
}
