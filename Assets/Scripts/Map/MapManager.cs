﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class MapManager : MonoBehaviour
{
    [HideInInspector]
    protected TileType[,] mTileData;

    public Map HubMap;
    public Map mCurrentMap;
    public TileMapRenderer mTileMap;
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
        //mTileMap.DrawTile(x, y, tType);
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

        return (mTileData[x, y] == TileType.OneWay || mTileData[x, y] == TileType.Block || mTileData[x, y] == TileType.LadderTop);
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

        return (mTileData[x, y] == TileType.Block);
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

        if (emptySpaces > 10)
        {
            emptySpaces = 0;
        }

        return emptySpaces;

    }


    public Vector2i GetRoofTile(Vector2i start)
    {
        Vector2i currTile = start;

        while (currTile.y < mCurrentMap.getMapSize().y && GetTile(currTile.x, currTile.y + 1) != TileType.Block)
        {
            currTile = GetMapTileAtPoint(GetMapTilePosition(new Vector2i(currTile.x, currTile.y + 1)));
        }

        return currTile;

    }
    public float GetGravity()
    {
        return mCurrentMap.gravity;
    }


    public void InitMapObject()
    {

    }

    public void GoToHub()
    {

    }

    public void NewMap(MapData data)
    {


        //set the position

        mUpdatedAreas = new HashSet<Vector2i>();


        mCurrentMap = MapGenerator.GenerateMap(data);


        mWidth = mCurrentMap.getMapSize().x;
        mHeight = mCurrentMap.getMapSize().y;

        mTileData = mCurrentMap.GetMap();
        AddEntities(mCurrentMap);

        mTileMap.DrawMap(mTileData, data);


    }

    public void NewBossMap(MapData data, WorldType type)
    {

        //set the position

        mUpdatedAreas = new HashSet<Vector2i>();


        mCurrentMap = MapGenerator.GenerateBossMap(data, type);
        mCurrentMap.worldType = type;

        mWidth = mCurrentMap.getMapSize().x;
        mHeight = mCurrentMap.getMapSize().y;

        mTileData = mCurrentMap.GetMap();
        AddEntities(mCurrentMap);

        mTileMap.DrawMap(mTileData, data);


    }
    /*
    public void CreationMap()
    {
        mWidth = Constants.cDefaultMapWidth;
        mHeight = Constants.cDefaultMapHeight;


        //set the position

        mUpdatedAreas = new HashSet<Vector2i>();


        mCurrentMap = MapGenerator.CreationMap();


        mWidth = mCurrentMap.sizeX;
        mHeight = mCurrentMap.sizeY;

        mTileData = mCurrentMap.GetMap();

        mTileMap.DrawMap(mTileData, mCurrentMap.sizeX, mCurrentMap.sizeY, mCurrentMap.type);

    }
    */


    public virtual void Init()
    {
        mPosition = transform.position;

        //NewMap(MapType.Hub);
    }

    public void AddEntities(Map data)
    {
        foreach(EntityData eD in data.objects)
        { 
            switch (eD.EntityType)
            {
                case EntityType.Object:
                    AddObjectEntity((ObjectData)eD);
                    break;

                case EntityType.Enemy:
                    AddEnemyEntity((EnemyData)eD);
                    break;
                    
                case EntityType.Boss:
                    AddBossEntity((BossData)eD);
                    break;
                case EntityType.NPC:
                    AddNPCEntity((NPCData)eD);
                    break;

            }
            
        }
    }

    public NPC AddNPCEntity(NPCData data)
    {
        NPCPrototype proto = NPCDatabase.GetNPCPrototype(data.type);
        NPC temp = null;
        switch (data.type)
        {
            case NPCType.Shopkeeper:
                temp = new NPC(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
        }
    

        return temp;
    }

    public void AddObjectEntity(ObjectData data)
    {
        switch (data.type)
        {
            case ObjectType.Chest:
                Chest temp = new Chest(Resources.Load("Prototypes/Entity/Objects/Chest") as EntityPrototype);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case ObjectType.FallingRock:
                FallingRock temp2 = new FallingRock(Resources.Load("Prototypes/Entity/Objects/FallingRock") as EntityPrototype);
                temp2.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case ObjectType.FlowerBed:
                FlowerBed temp3 = new FlowerBed(Resources.Load("Prototypes/Entity/Objects/FlowerBed") as EntityPrototype);
                temp3.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case ObjectType.Tree:
                Tree temp4 = new Tree(Resources.Load("Prototypes/Entity/Objects/Tree") as EntityPrototype);
                temp4.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case ObjectType.Medbay:
                MedicalBay temp5 = new MedicalBay(Resources.Load("Prototypes/Entity/Objects/Medbay") as EntityPrototype);
                temp5.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case ObjectType.Door:
                Door temp6 = new Door(Resources.Load("Prototypes/Entity/Objects/Door") as EntityPrototype);
                if(mCurrentMap.mapType == MapType.Hub || mCurrentMap.mapType == MapType.BossMap)
                {
                    temp6.locked = false;
                }
                temp6.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case ObjectType.NavSystem:
                NavSystem temp7 = new NavSystem(Resources.Load("Prototypes/Entity/Objects/NavSystem") as EntityPrototype);
                temp7.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case ObjectType.BouncePad:
                BouncePad temp8 = new BouncePad(Resources.Load("Prototypes/Entity/Objects/BouncePad") as EntityPrototype);
                temp8.Spawn(GetMapTilePosition(data.TilePosition));
                break;
        }
    }
        
    public Enemy AddEnemyEntity(EnemyData data)
    {
        EnemyPrototype proto = EnemyDatabase.GetEnemyPrototype(data.type);
        Enemy temp = null;

        switch (proto.enemyType)
        {
            case EnemyType.Slime:
                temp = new Slime(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Eye:
                temp = new Eye(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.WurmAlien:
                temp = new WurmAlien(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Snek:
                temp = new Snek(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Stag:
                temp = new Stag(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Snowball:
                temp = new Snowball(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Snowdrift:
                temp = new Snowdrift(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Treedude:
                temp = new Treedude(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.FrogLegs:
                temp = new FrogLegs(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Hedgehog:
                temp = new Hedgehog(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case EnemyType.Nest:
                temp = new Nest(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
        }

        return temp;
    }

    public BossEnemy AddBossEntity(BossData data)
    {
        BossPrototype proto = BossDatabase.GetBossPrototype(data.type);
        BossEnemy temp = null;

        switch (proto.bossType)
        {
            case BossType.CatBoss:
                temp = new CatBoss(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case BossType.LavaBoss:
                temp = new LavaBoss(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case BossType.SharkBoss:
                temp = new SharkBoss(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case BossType.HedgehogBoss:
                temp = new HedgehogBoss(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
            case BossType.TentacleBoss:
                temp = new TentacleBoss(proto);
                temp.Spawn(GetMapTilePosition(data.TilePosition));
                break;
        }

        return temp;
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

        //mTileMap.DrawMap(mTileData, mWidth, mHeight);
    }

  
}
