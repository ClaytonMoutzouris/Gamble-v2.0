using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorMap : MonoBehaviour
{
    public RoomData room;
    public TileMapRenderer mTileMap;
    public EditorMap instance;
    public Vector3 mPosition;
    public int mWidth;
    public int mHeight;
    public List<Sprite> sprites;


    [SerializeField]
    public const int cTileSize = 32;

    private void Awake()
    {
        instance = this;
        Init();

    }


    public void Init()
    {
        //set the position
        mPosition = transform.position;

        room = new RoomData(RoomType.Hub, mWidth, mHeight);
        //room.roomType = RoomType.Hub;
        mTileMap.Init();
        mTileMap.DrawMap(room.tiles, mWidth, mHeight, sprites);
    }

    public void Draw()
    {
        mWidth = room.mWidth;
        mHeight = room.mHeight;
        mTileMap.DrawMap(room.tiles, mWidth, mHeight, sprites);

    }

    public void SetTile(int x, int y, TileType tType)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return;

        room.tiles[x, y] = tType;
        mTileMap.DrawTile(x, y, tType, sprites[(int)tType]);
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
