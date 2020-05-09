using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapRenderer : MonoBehaviour
{

    const int MAXSIZEX = 100, MAXSIZEY = 100;
    public float tileSize = 1.0f;
    private Vector3 tileOffset;
    public Vector2 MapSize;
    Dictionary<TileType, TilePrototype> tilePrototypes;
    MapManager map;
    public WorldType CurrentMapType;
    Tile[,] spriteMap;
    public Tile tilePrefab;

    public void Awake()
    {
        tilePrototypes = new Dictionary<TileType, TilePrototype>();
        TilePrototype[] prototypes = Resources.LoadAll<TilePrototype>("Tiles");
        
        foreach(TilePrototype proto in prototypes)
        {
            Debug.Log("Adding tile prototype for " + proto.name);
            tilePrototypes.Add(proto.type, proto);
        }


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

    public Sprite GetSprite(TileType tileType, WorldType world)
    {
        if(tilePrototypes.ContainsKey(tileType))
        {
            return tilePrototypes[tileType].sprites[(int)world];
        }

        return null;
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
                    if (tilePrototypes.ContainsKey(tiles[x, y]))
                    {
                        spriteMap[x, y].SetSprite(tilePrototypes[tiles[x,y]].sprites[(int)mapData.type]);
                        spriteMap[x, y].SetAnimator(tilePrototypes[tiles[x, y]].animationController);

                    }
                    else
                    {
                        spriteMap[x, y].Clear();
                    }
                    
                }
                else
                {
                    spriteMap[x, y].Clear();
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
                    if (tilePrototypes.ContainsKey(tiles[x, y]))
                    {
                        spriteMap[x, y].SetSprite(sprites[(int)tiles[x, y]]);
                        spriteMap[x, y].SetAnimator(tilePrototypes[tiles[x, y]].animationController);

                    }
                    else
                    {
                        spriteMap[x, y].Clear();
                    }
                }
                else
                {
                    spriteMap[x, y].Clear();

                }

            }
        }
    }

    //If we need to update 1 tile
    public void DrawTile(int x, int y, TileType tile, Sprite sprite)
    {
        spriteMap[x, y].SetSprite(sprite);
        if (tilePrototypes.ContainsKey(tile))
        {

            spriteMap[x, y].SetAnimator(tilePrototypes[tile].animationController);
        }
    }


}
