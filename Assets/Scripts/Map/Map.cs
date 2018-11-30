using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class Map : MonoBehaviour
{
    [HideInInspector]
    protected TileType[,] mTileData;
    public MapData mMapData;
    public TileMapObject mTileMap;
    public static Map instance;
    public Vector3 mPosition;
    public int mWidth;
    public int mHeight;

    [SerializeField]
    public const int cTileSize = 32;

    private static List<Vector2i> mOverlappingAreas = new List<Vector2i>(4);

    /// <summary>
    /// A set of areas in which at least one tile has been destroyed
    /// </summary>
    private HashSet<Vector2i> mUpdatedAreas;

    /// <summary>
    /// How many collision areas are there on the map horizontally.
    /// </summary>
    [HideInInspector]
    public int mHorizontalAreasCount;

    /// <summary>
    /// How many collision areas are there on the map vertically.
    /// </summary>
    [HideInInspector]
    public int mVerticalAreasCount;

    [SerializeField]
    public int mGridAreaWidth = 25;
    [SerializeField]
    public int mGridAreaHeight = 25;
    [HideInInspector]
    public List<PhysicsObject>[,] mObjectsInArea;

    public static List<PhysicsObject> mCollisionRequestObjects = new List<PhysicsObject>();

    private void Awake()
    {
        instance = this;
    }

    public void RemoveObjectFromAreas(PhysicsObject obj)
    {
        for (int i = 0; i < obj.mAreas.Count; ++i)
            RemoveObjectFromArea(obj.mAreas[i], obj.mIdsInAreas[i], obj);
        obj.mAreas.Clear();
        obj.mIdsInAreas.Clear();
    }

    public void DebugPrintAreas()
    {
        Debug.Log("Objects in areas:");

        for (int y = 0; y < mVerticalAreasCount; ++y)
        {
            for (int x = 0; x < mHorizontalAreasCount; ++x)
            {
                var objs = mObjectsInArea[x, y];
                if (objs.Count > 0)
                {
                    Debug.Log("Area X: " + x + " Y: " + y + " obj count: " + objs.Count);
                    for (int i = 0; i < objs.Count; ++i)
                    {
                        Debug.Log(objs[i].name);
                    }
                }
            }
        }

        Debug.Log("Object in areas End");
    }

    public void RemoveObjectFromArea(Vector2i areaIndex, int objIndexInArea, PhysicsObject obj)
    {
        var area = mObjectsInArea[areaIndex.x, areaIndex.y];

        if (obj.mToRemove)
        {
            Debug.Log("This object is flagged for removal");
        }

        if (area.Count == 0)
        {
            Debug.Log("Removing object from an area that doesn't contain any objects, areaIndex: " + areaIndex.x + ", " + areaIndex.y + ", objIndexInArea: " + objIndexInArea);
            //Debug.Break();
        }

        //swap the last item with the one we are removing
        var tmp = area[area.Count - 1];
        area[area.Count - 1] = obj;
        area[objIndexInArea] = tmp;

        var tmpIds = tmp.mIdsInAreas;
        var tmpAreas = tmp.mAreas;
        for (int i = 0; i < tmpAreas.Count; ++i)
        {
            if (tmpAreas[i] == areaIndex)
            {
                tmpIds[i] = objIndexInArea;
                break;
            }
        }

        //remove the last item
        area.RemoveAt(area.Count - 1);
    }

    public void AddObjectToArea(Vector2i areaIndex, PhysicsObject obj)
    {
        var area = mObjectsInArea[areaIndex.x, areaIndex.y];

        //save the index of  the object in the area
        obj.mAreas.Add(areaIndex);
        obj.mIdsInAreas.Add(area.Count);

        //add the object to the area
        area.Add(obj);
    }

    public void UpdateAreas(PhysicsObject obj)
    {
        //get the areas at the aabb's corners
        var topLeft = GetMapTileAtPoint((Vector2)obj.mAABB.Center + new Vector2(-obj.mAABB.HalfSize.x, obj.mAABB.HalfSizeY));
        var topRight = GetMapTileAtPoint(obj.mAABB.Center + obj.mAABB.HalfSize);
        var bottomLeft = GetMapTileAtPoint(obj.mAABB.Center - obj.mAABB.HalfSize);
        var bottomRight = new Vector2i();

        topLeft.x /= mGridAreaWidth;
        topLeft.y /= mGridAreaHeight;

        topRight.x /= mGridAreaWidth;
        topRight.y /= mGridAreaHeight;

        bottomLeft.x /= mGridAreaWidth;
        bottomLeft.y /= mGridAreaHeight;

        /*topLeft.x = Mathf.Clamp(topLeft.x, 0, mHorizontalAreasCount - 1);
        topLeft.y = Mathf.Clamp(topLeft.y, 0, mVerticalAreasCount - 1);

        topRight.x = Mathf.Clamp(topRight.x, 0, mHorizontalAreasCount - 1);
        topRight.y = Mathf.Clamp(topRight.y, 0, mVerticalAreasCount - 1);

        bottomLeft.x = Mathf.Clamp(bottomLeft.x, 0, mHorizontalAreasCount - 1);
        bottomLeft.y = Mathf.Clamp(bottomLeft.y, 0, mVerticalAreasCount - 1);*/

        bottomRight.x = topRight.x;
        bottomRight.y = bottomLeft.y;

        //see how many different areas we have
        if (topLeft.x == topRight.x && topLeft.y == bottomLeft.y)
        {
            mOverlappingAreas.Add(topLeft);
        }
        else if (topLeft.x == topRight.x)
        {
            mOverlappingAreas.Add(topLeft);
            mOverlappingAreas.Add(bottomLeft);
        }
        else if (topLeft.y == bottomLeft.y)
        {
            mOverlappingAreas.Add(topLeft);
            mOverlappingAreas.Add(topRight);
        }
        else
        {
            mOverlappingAreas.Add(topLeft);
            mOverlappingAreas.Add(bottomLeft);
            mOverlappingAreas.Add(topRight);
            mOverlappingAreas.Add(bottomRight);
        }

        var areas = obj.mAreas;
        var ids = obj.mIdsInAreas;

        for (int i = 0; i < areas.Count; ++i)
        {
            if (!mOverlappingAreas.Contains(areas[i]))
            {
                RemoveObjectFromArea(areas[i], ids[i], obj);
                //object no longer has an index in the area
                areas.RemoveAt(i);
                ids.RemoveAt(i);
                --i;
            }
        }

        for (var i = 0; i < mOverlappingAreas.Count; ++i)
        {
            var area = mOverlappingAreas[i];
            if (!areas.Contains(area))
                AddObjectToArea(area, obj);
        }

        mOverlappingAreas.Clear();
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
        return new Vector2i((int)((point.x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize)),
                    (int)((point.y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize)));
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


    public void Init()
    {
        mWidth = Constants.cMapWidth;
        mHeight = Constants.cMapHeight;

        //set the position
        mPosition = transform.position;

        mUpdatedAreas = new HashSet<Vector2i>();

        mHorizontalAreasCount = Mathf.CeilToInt((float)mWidth / (float)mGridAreaWidth);
        mVerticalAreasCount = Mathf.CeilToInt((float)mHeight / (float)mGridAreaHeight);

        mObjectsInArea = new List<PhysicsObject>[mHorizontalAreasCount, mVerticalAreasCount];

        for (var y = 0; y < mVerticalAreasCount; ++y)
        {
            for (var x = 0; x < mHorizontalAreasCount; ++x)
                mObjectsInArea[x, y] = new List<PhysicsObject>();
        }

        mMapData = MapGenerator.GenerateMap();

        mTileData = mMapData.GetMap();

        mTileMap.DrawMap(mTileData, mWidth, mHeight);
    }

    public void Save(BinaryWriter writer)
    {

        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                //For maximum efficiency, we store the tiletype as a byte,
                //since its an int within the 0-255 range for now
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

    public void CheckCollisions()
    {
        float overlapWidth, overlapHeight;

        for (int y = 0; y < mVerticalAreasCount; ++y)
        {
            for (int x = 0; x < mHorizontalAreasCount; ++x)
            {
                var objectsInArea = mObjectsInArea[x, y];
                for (int i = 0; i < objectsInArea.Count - 1; ++i)
                {
                    var obj1 = objectsInArea[i];
                    for (int j = i + 1; j < objectsInArea.Count; ++j)
                    {
                        var obj2 = objectsInArea[j];

                        if (obj1.mAABB.OverlapsSigned(obj2.mAABB, out overlapWidth, out overlapHeight) && !obj1.HasCollisionDataFor(obj2))
                        {
                            obj1.mAllCollidingObjects.Add(new CollisionData(obj2, new Vector2(overlapWidth, overlapHeight), obj1.mSpeed, obj2.mSpeed, obj1.mOldPosition, obj2.mOldPosition, obj1.mPosition, obj2.mPosition));
                            obj2.mAllCollidingObjects.Add(new CollisionData(obj1, -new Vector2(overlapWidth, overlapHeight), obj2.mSpeed, obj1.mSpeed, obj2.mOldPosition, obj1.mOldPosition, obj2.mPosition, obj1.mPosition));
                        }
                    }
                }
            }
        }
    }

  
}
