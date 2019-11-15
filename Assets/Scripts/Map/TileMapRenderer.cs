using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapRenderer : MonoBehaviour
{

    const int MAXSIZEX = 100, MAXSIZEY = 100;
    public float tileSize = 1.0f;
    private Vector3 tileOffset;
    public Vector2 MapSize;

    MapManager map;
    public WorldType CurrentMapType;
    Tile[,] spriteMap;
    public Tile tilePrefab;

    public void Awake()
    {
        spriteMap = new Tile[(int)MapSize.x, (int)MapSize.y];

        Debug.Log(MapSize.x + ", " + MapSize.y);


        for (int x = 0; x < MapSize.x; x++) {
            for (int y = 0; y < MapSize.y; y++)
            {
                spriteMap[x, y] = Instantiate(tilePrefab, transform);
                spriteMap[x, y].transform.localPosition = new Vector2(x * tileSize, y * tileSize);
            }
        }


    }

    public void DrawMap(TileType[,] tiles, MapData mapData)
    {
        //MapSize = new Vector2(MAXSIZEY, MAXSIZEY);
        for (int y = 0; y < MAXSIZEY; y++)
        {
            for (int x = 0; x < MAXSIZEY; x++)
            {
                if (x < mapData.sizeX * mapData.roomSizeX && y < mapData.sizeY * mapData.roomSizeY)
                {

                    spriteMap[x, y].SetSprite(mapData.tileSprites[(int)tiles[x, y]]);
                }
                else
                {
                    spriteMap[x, y].ClearSprite();

                }

            }
        }
    }


    public void DrawMap(TileType[,] tiles, int sizeX, int sizeY, List<Sprite> sprites)
    {
        //MapSize = new Vector2(MAXSIZEY, MAXSIZEY);
        for (int y = 0; y < MAXSIZEY; y++)
        {
            for (int x = 0; x < MAXSIZEY; x++)
            {
                if (x < sizeX && y < sizeY)
                {

                    spriteMap[x, y].SetSprite(sprites[(int)tiles[x, y]]);
                }
                else
                {
                    spriteMap[x, y].ClearSprite();

                }

            }
        }
    }

    //If we need to update 1 tile
    public void DrawTile(int x, int y, TileType tile, MapData data)
    {
        spriteMap[x, y].SetSprite(data.tileSprites[(int)tile]);
    }


}
