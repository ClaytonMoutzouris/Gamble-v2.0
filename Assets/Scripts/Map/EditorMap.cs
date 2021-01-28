using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorMap : MonoBehaviour
{
    public RoomData room;
    public TileMapRenderer mTileMap;
    public static EditorMap instance;
    public Vector3 mPosition;
    public int mWidth;
    public int mHeight;
    public List<Sprite> tileSprites;
    public List<Sprite> objectSprites;
    public Sprite NPCsprite;
    public SpriteRenderer mapBounds;
    public Transform objectsLayer;
    //public GameObject[,] objectIcons;
    public EditorIcon iconPrefab;
    public EditorIcon[,] objectIcons;


    [SerializeField]
    public const int cTileSize = 32;

    private void Awake()
    {
        instance = this;
        Init();

    }

    public void ClearIcons()
    {

    }

    public void Init()
    {
        //set the position
        mPosition = transform.position;

        room = new RoomData(RoomType.Ship, mWidth, mHeight);
        objectIcons = new EditorIcon[mWidth, mHeight];

        //room.roomType = RoomType.Hub;
        mTileMap.Init();
        mTileMap.DrawMap(room.tiles, mWidth, mHeight, tileSprites);
        mapBounds.size = new Vector2(mWidth * cTileSize, mHeight * cTileSize);
        mapBounds.transform.position = mapBounds.size / 2 - new Vector2(16, 16);
    }

    public void Draw()
    {
        for(int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                if (objectIcons[x, y] == null)
                    continue;
                Destroy(objectIcons[x, y].gameObject);
                objectIcons[x, y] = null;
            }
        }

        mWidth = room.mWidth;
        mHeight = room.mHeight;

        objectIcons = new EditorIcon[mWidth, mHeight];

        mTileMap.DrawMap(room.tiles, mWidth, mHeight, tileSprites);
        mapBounds.size = new Vector2(mWidth * cTileSize, mHeight * cTileSize);
        mapBounds.transform.position = mapBounds.size / 2 - new Vector2(16, 16);
        
       

        foreach (EntityData data in room.entityData)
        {
            if (data == null)
                continue;

            if(data is NPCData npc)
            {
                AddNPCEntity(npc);
            }

            if(data is ObjectData obj) {
                AddObjectEntity(obj);
            }
        }


    }

    public void AddEntity(EntityData entity)
    {
        if(objectIcons[entity.TilePosition.x, entity.TilePosition.y] != null)
        {

            Destroy(objectIcons[entity.TilePosition.x, entity.TilePosition.y].gameObject);
            objectIcons[entity.TilePosition.x, entity.TilePosition.y] = null;

        }

        EditorIcon icon = Instantiate(iconPrefab, objectsLayer);
        icon.transform.position = GetMapTilePosition(entity.TilePosition);
        Debug.Log("Placing a " + entity.EntityType.ToString());
        icon.SetIcon(objectSprites[(int)entity.EntityType]);

        room.entityData[entity.TilePosition.x, entity.TilePosition.y] = entity;
        objectIcons[entity.TilePosition.x, entity.TilePosition.y] = icon;

    }

    public void AddObjectEntity(ObjectData entity)
    {
        if (objectIcons[entity.TilePosition.x, entity.TilePosition.y] != null)
        {

            Destroy(objectIcons[entity.TilePosition.x, entity.TilePosition.y].gameObject);
            objectIcons[entity.TilePosition.x, entity.TilePosition.y] = null;

        }
        Debug.Log("Tile X: " + entity.TilePosition.x + "Tile Y: " + entity.TilePosition.y);


        EditorIcon icon = Instantiate(iconPrefab, objectsLayer);
        icon.transform.position = GetMapTilePosition(entity.TilePosition) + new Vector2(0, -16);
        Debug.Log("Placing a " + entity.EntityType.ToString());
        icon.SetIcon(objectSprites[(int)entity.type]);

        room.entityData[entity.TilePosition.x, entity.TilePosition.y] = entity;

        objectIcons[entity.TilePosition.x, entity.TilePosition.y] = icon;

    }

    public void AddNPCEntity(NPCData entity)
    {
        if (objectIcons[entity.TilePosition.x, entity.TilePosition.y] != null)
        {

            Destroy(objectIcons[entity.TilePosition.x, entity.TilePosition.y].gameObject);
            objectIcons[entity.TilePosition.x, entity.TilePosition.y] = null;

        }


        EditorIcon icon = Instantiate(iconPrefab, objectsLayer);
        icon.transform.position = GetMapTilePosition(entity.TilePosition) + new Vector2(0, -16);
        icon.SetIcon(NPCsprite);

        room.entityData[entity.TilePosition.x, entity.TilePosition.y] = entity;

        objectIcons[entity.TilePosition.x, entity.TilePosition.y] = icon;

    }

    public void ClearObjectEntity(int x, int y)
    {
        if (objectIcons[x, y] != null)
        {

            Destroy(objectIcons[x, y].gameObject);
            objectIcons[x, y] = null;

        }


        room.entityData[x, y] = null;
        
    }

    public void ClearNPCEntity(int x, int y)
    {
        if (objectIcons[x, y] != null)
        {

            Destroy(objectIcons[x, y].gameObject);
            objectIcons[x, y] = null;

        }


        room.entityData[x, y] = null;

    }

    public void SetTile(int x, int y, TileType tType)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return;

        room.tiles[x, y] = tType;
        mTileMap.DrawTile(x, y, tType, tileSprites[(int)tType]);
       // mTileMap.DrawTile(x, y, tType);
    }

    public void GetMapTileAtPoint(Vector2 point, out int tileIndexX, out int tileIndexY)
    {
        tileIndexY = (int)((point.y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize));
        tileIndexX = (int)((point.x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize));
    }

    public Vector2i GetMapTileAtPoint(Vector2 point)
    {
        //We should clamp all of these point getters
        Vector2i tilePoint = new Vector2i((int)((point.x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize)), (int)((point.y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize)));
        return tilePoint;
    }

    public Vector2 GetMapTilePosition(int tileIndexX, int tileIndexY)
    {
        return new Vector2(
                (float)(tileIndexX * cTileSize) + mPosition.x,
                (float)(tileIndexY * cTileSize) + mPosition.y
            );
    }

    public Vector2 GetMapTilePosition(Vector2i tileCoords)
    {
        return new Vector2(
            (float)(tileCoords.x * cTileSize) + mPosition.x,
            (float)(tileCoords.y * cTileSize) + mPosition.y
            );
    }

    

}
