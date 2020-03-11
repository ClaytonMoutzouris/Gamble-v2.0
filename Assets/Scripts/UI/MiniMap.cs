using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{

    public static MiniMap instance;
    public MapManager mapManager;
    public Texture2D mapTexture;
    public static int mapSizeX = 100;
    public static int mapSizeY = 100;
    public GameObject playerIcon;
    public GameObject doorIcon;
    public MiniMapIcon prefab;
    public List<MiniMapIcon> icons;

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

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public MiniMapIcon AddStaticIcon(MinimapIconType type, Vector2i tilePos)
    {
        prefab.type = type;
        MiniMapIcon icon = Instantiate(prefab, transform);
        icon.UpdateIcon(tilePos);
        icons.Add(icon);
        return icon;
    }

    public void RemoveIcon(MiniMapIcon icon)
    {
        icons.Remove(icon);
        Destroy(icon.gameObject);
    }

    public void SetMap(Map map, Player[] players)
    {
        ClearMinimap();

        icons.Add(AddStaticIcon(MinimapIconType.Door, map.exitTile));
        icons.Add(AddStaticIcon(MinimapIconType.Boss, map.bossTile));

        foreach(Player player in players)
        {
            if (player != null)
            {
                player.MiniMapIcon = AddStaticIcon(MinimapIconType.Player, map.startTile);

                icons.Add(player.MiniMapIcon);
            }
        }

        SetMapTexture(map.GetMap(), map.getMapSize().x, map.getMapSize().y);
    }

    public static Vector2 TileToMinimap(Vector2i tilePos)
    {
        return ((Vector2)tilePos) * 2 - (Vector2.up * mapSizeX) * 2;
    }
    public void ClearMinimap()
    {
        foreach(MiniMapIcon icon in icons)
        {
            Destroy(icon.gameObject);
        }
        icons.Clear();
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

    public void SetMapTexture(TileType[,] tiles, int sizeX, int sizeY)
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
                        case TileType.Chest:
                            c = Color.yellow;
                            break;
                        /*
                        case TileType.Spikes:
                            c = Color.white;
                            break;    
                        case TileType.Ladder:
                        case TileType.LadderTop:
                            c = Color.yellow;
                            break;
                        */
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
