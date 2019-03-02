using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{

    public static MiniMap instance;
    public MapManager mapManager;
    public Texture2D mapTexture;
    int mapSizeX = 100;
    int mapSizeY = 100;

    public RawImage image;
    TilePalette tilePalette;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }
        tilePalette = new TilePalette(mapTexture, 1);
        BuildInitialTexture();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMapTexture()
    {
      
    }

    void BuildInitialTexture()
    {

        mapTexture = new Texture2D(mapSizeX, mapSizeY);



        Color[] p = new Color[mapSizeX * mapSizeY];
        
        for(int i = 0; i < mapSizeX*mapSizeY; i++)
        {
            p[i] = Color.red;
        }

        mapTexture.SetPixels(0, 0, mapSizeX, mapSizeX, p);



        mapTexture.filterMode = FilterMode.Point;
        mapTexture.wrapMode = TextureWrapMode.Clamp;
        mapTexture.Apply();

        image.texture = mapTexture;
    }

    public void SetMap(TileType[,] tiles, int sizeX, int sizeY)
    {
        Color c = Color.gray;

        for (int y = 0; y < mapSizeX; y++)
        {
            for (int x = 0; x < mapSizeY; x++)
            {
                if (x < sizeX && y < sizeY)
                {
                    switch (tiles[x, y])
                    {
                        case TileType.Block:
                            c = Color.black;
                            break;
                        case TileType.Door:
                            c = Color.red;
                            break;
                        default:
                            c = Color.gray;
                            break;
                    }
                }
                
                mapTexture.SetPixel(x, y, c);
            }
        }

        mapTexture.Apply();
    }
}
