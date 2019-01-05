using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class MapManager : MonoBehaviour
{
    [HideInInspector]
    protected TileType[,] mTileData;
    public MapData mMapData;
    public TileMapObject mTileMap;
    public static MapManager instance;
    public Vector3 mPosition;
    public int mWidth;
    public int mHeight;

    [SerializeField]
    public const int cTileSize = 32;

    //private static List<Vector2i> mOverlappingAreas = new List<Vector2i>(4);

    /// <summary>
    /// A set of areas in which at least one tile has been destroyed
    /// </summary>
    private HashSet<Vector2i> mUpdatedAreas;


    private void Awake()
    {
        instance = this;
    }

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return TileType.Block;

        return mTileData[x, y];
    }

    public void SetTile(int x, int y, TileType tType)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return;

        mTileData[x, y] = tType;
        mTileMap.DrawTile(x, y, tType);
    }

    public TileType GetCollisionType(Vector2i pos)
    {
        if (pos.x <= -1 || pos.x >= mWidth
            || pos.y <= -1 || pos.y >= mHeight)
            return TileType.Block;

        return mTileData[pos.x, pos.y];
    }

    public bool IsOneWayPlatform(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return false;

        return (mTileData[x, y] == TileType.OneWay || mTileData[x,y] == TileType.LadderTop);
    }

    public bool IsGround(int x, int y)
    {
        if (x < 0 || x >= mWidth
           || y < 0 || y >= mHeight)
            return false;

        return (mTileData[x, y] == TileType.OneWay || mTileData[x, y] == TileType.Block || mTileData[x, y] == TileType.LadderTop || mTileData[x,y] == TileType.IceBlock || mTileData[x, y] == TileType.ConveyorLeft || mTileData[x, y] == TileType.ConveyorRight);
    }

    public bool IsGround(Vector2i pos)
    {
        return IsGround(pos.x, pos.y);
    }


    public bool IsObstacle(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return true;

        return (mTileData[x, y] == TileType.Block || mTileData[x,y] == TileType.IceBlock);
    }

    public bool IsEmpty(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return false;

        return (mTileData[x, y] == TileType.Empty);
    }

    public bool IsNotEmpty(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return true;

        return (mTileData[x, y] != TileType.Empty);
    }

    public void GetMapTileAtPoint(Vector2 point, out int tileIndexX, out int tileIndexY)
    {
        tileIndexY = (int)((point.y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize));
        tileIndexX = (int)((point.x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize));
    }

    public int GetMapTileYAtPoint(float y)
    {
        return (int)((y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize));
    }

    public int GetMapTileXAtPoint(float x)
    {
        return (int)((x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize));
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

    public int CheckEmptySpacesBelow(Vector2i start)
    {
        int emptySpaces = 0;
        Vector2i currTile = start;

        while (currTile.y != 0 && GetTile(currTile.x, currTile.y - 1) == TileType.Empty)
        {

            emptySpaces += 1;
            currTile = GetMapTileAtPoint(GetMapTilePosition(new Vector2i(currTile.x, currTile.y - 1)));
        }

        return emptySpaces;

    }


    public void InitMapObject()
    {

    }

    public void NewMap(MapType mapType)
    {
        mWidth = Constants.cDefaultMapWidth;
        mHeight = Constants.cDefaultMapHeight;


        //set the position

        mUpdatedAreas = new HashSet<Vector2i>();

        switch (mapType)
        {
            case MapType.Hub:
                mMapData = MapGenerator.GenerateHubMap();
                break;
            case MapType.World:
                mMapData = MapGenerator.GenerateMap();
                break;
            case MapType.BossMap:
                mMapData = MapGenerator.GenerateBossMap(mMapData.type);
                break;
        }

        mWidth = mMapData.sizeX;
        mHeight = mMapData.sizeY;

        mTileData = mMapData.GetMap();
        AddEntities();
        Debug.Log(mMapData.sizeX + ", " + mMapData.sizeY);
        mTileMap.DrawMap(mTileData, mMapData.sizeX, mMapData.sizeY, mMapData.type);
    }



    public virtual void Init()
    {
        mPosition = transform.position;

        NewMap(MapType.Hub);
    }

    public void AddEntities()
    {
        foreach(EntityData eD in mMapData.objects)
        { 
            switch (eD.EntityType)
            {
                case EntityType.Object:
                    AddObjectEntity((ObjectData)eD);
                    break;

                case EntityType.Enemy:
                    AddEnemyEntity((EnemyData)eD);
                    break;
            }
            
        }
    }

    public void AddObjectEntity(ObjectData data)
    {
        switch (data.type)
        {
            case ObjectType.Chest:
                Chest temp = Instantiate(Resources.Load<Chest>("Prefabs/Objects/Chest")) as Chest;
                temp.EntityInit();
                temp.Body.SetTilePosition(data.TilePosition);
                break;
            case ObjectType.FallingRock:
                FallingRock temp2 = Instantiate(Resources.Load<FallingRock>("Prefabs/Objects/FallingRock")) as FallingRock;
                temp2.EntityInit();
                temp2.InitPosition(data.TilePosition);
                break;
        }
    }

    public void AddEnemyEntity(EnemyData data)
    {
  
        Enemy temp = Instantiate(EnemyDatabase.GetEnemyPrefab(data.type));
        temp.EntityInit();
        temp.Body.SetTilePosition(data.TilePosition);
    }


    public void Save(BinaryWriter writer)
    {

        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                //For maximum efficiency, we store the tiletype as a byte,
                //since its an int within the 0-255 range for now
                //writer.Write((byte)mMapData.type);
                writer.Write((byte)mTileData[x, y]);
            }
        }

    }

    public void Load(BinaryReader reader)
    {
        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                
                mTileData[x,y] = (TileType)reader.ReadByte();
            }
        }

        mTileMap.DrawMap(mTileData, mWidth, mHeight);
    }

  
}
