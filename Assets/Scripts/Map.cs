using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField]
    public TileType[,] mTiles;

    public TileMapObject mTileMap;

    public Vector3 mPosition;

    public int mWidth = 80;
    public int mHeight = 60;

    public const int cTileSize = 32;

    private static List<Vector2i> mOverlappingAreas = new List<Vector2i>(4);

    private HashSet<Vector2i> mUpdatedAreas;

    [HideInInspector]
    public int mHorizontalAreasCount;

    [HideInInspector]
    public int mVerticalAreasCount;

    public int mGridAreaWidth = 16;
    public int mGridAreaHeight = 16;
    public List<MovingObject>[,] mObjectsInArea;

    public static List<MovingObject> mCollisionRequestObjects = new List<MovingObject>();


    public void InitMap()
    {
        mPosition = transform.position;

        mUpdatedAreas = new HashSet<Vector2i>();

        mHorizontalAreasCount = Mathf.CeilToInt((float)mWidth / (float)mGridAreaWidth);
        mVerticalAreasCount = Mathf.CeilToInt((float)mHeight / (float)mGridAreaHeight);

        mObjectsInArea = new List<MovingObject>[mHorizontalAreasCount, mVerticalAreasCount];

        for (var y = 0; y < mVerticalAreasCount; ++y)
        {
            for (var x = 0; x < mHorizontalAreasCount; ++x)
                mObjectsInArea[x, y] = new List<MovingObject>();
        }

        mTiles = new TileType[mWidth, mHeight];

        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                if (x == 0 || x == mWidth - 1 || y == 0 || y == mHeight - 1)
                {
                    mTiles[x, y] = TileType.Block;
                }
                else
                {
                    mTiles[x, y] = TileType.Empty;
                }
            }
        }

        mTiles[0, 2] = TileType.OneWay;
        mTiles[1, 2] = TileType.OneWay;
        mTiles[2, 2] = TileType.OneWay;

        mTiles[3, 3] = TileType.Block;
        mTiles[4, 3] = TileType.Block;
        mTiles[5, 3] = TileType.Block;

        for(int y = 0; y < mHeight; y++)
        {
            if (y % 3 == 1)
            {
                mTiles[mWidth - 2, y] = TileType.Block;
            }
        }

        mTileMap.DrawMap(mTiles, mWidth, mHeight);

    }

    public void UpdateAreas(MovingObject obj)
    {
        //get the areas at the aabb's corners
        var topLeft = GetMapTileAtPoint(obj.mAABB.center + new Vector2(-obj.mAABB.HalfSize.x, obj.mAABB.HalfSizeY));
        var topRight = GetMapTileAtPoint(obj.mAABB.center + obj.mAABB.HalfSize);
        var bottomLeft = GetMapTileAtPoint(obj.mAABB.center - obj.mAABB.HalfSize);
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

    public void RemoveObjectFromAreas(MovingObject obj)
    {
        for (int i = 0; i < obj.mAreas.Count; ++i)
            RemoveObjectFromArea(obj.mAreas[i], obj.mIdsInAreas[i], obj);
        obj.mAreas.Clear();
        obj.mIdsInAreas.Clear();
    }

    public void AddObjectToArea(Vector2i areaIndex, MovingObject obj)
    {
        var area = mObjectsInArea[areaIndex.x, areaIndex.y];

        //save the index of  the object in the area
        obj.mAreas.Add(areaIndex);
        obj.mIdsInAreas.Add(area.Count);

        //add the object to the area
        area.Add(obj);
    }

    public void RemoveObjectFromArea(Vector2i areaIndex, int objIndexInArea, MovingObject obj)
    {
        var area = mObjectsInArea[areaIndex.x, areaIndex.y];

        if (Debug.isDebugBuild && area.Count == 0)
        {
            Debug.Log("Removing object from an area that doesn't contain any objects, areaIndex: " + areaIndex.x + ", " + areaIndex.y + ", objIndexInArea: " + objIndexInArea);
            Debug.Break();
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

    bool SweepTest(MovingObject obj1, MovingObject obj2, out float u0, out float u1)
    {
        AABB a = obj1.mAABB;
        AABB b = obj2.mAABB;

        Vector2 va = obj1.mSpeed * Time.deltaTime;
        Vector2 vb = obj2.mSpeed * Time.deltaTime;

        Vector2 v = vb - va;

        Vector2 u_0 = Vector2.zero;
        Vector2 u_1 = Vector2.one;

        if (a.Overlaps(b))
        {
            u0 = u1 = 0.0f;
            return true;
        }
        else if (v.x == 0.0f && v.y == 0.0f)
        {
            u0 = u1 = 0.0f;
            return false;
        }

        Vector2 maxA = a.Max();
        Vector2 minA = a.Min();
        Vector2 maxB = b.Max();
        Vector2 minB = b.Min();

        if (maxA.x < minB.x && v.x < 0.0f)
            u_0.x = (maxA.x - minB.x) / v.x;
        else if (maxB.x < minA.x && v.x > 0.0f)
            u_0.x = (minA.x - minB.x) / v.x;

        if (maxA.y < minB.y && v.y < 0.0f)
            u_0.y = (maxA.y - minB.y) / v.y;
        else if (maxB.y < minA.y && v.y > 0.0f)
            u_0.y = (minA.y - minB.y) / v.y;

        if (maxB.x > minA.x && v.x < 0.0f)
            u_1.x = (minA.x - maxB.x) / v.x;
        else if (maxA.x > minB.x && v.x > 0.0f)
            u_1.x = (maxA.x - minB.x) / v.x;

        if (maxB.y > minA.y && v.y < 0.0f)
            u_1.y = (minA.y - maxB.y) / v.y;
        else if (maxA.y > minB.y && v.y > 0.0f)
            u_1.y = (maxA.y - minB.y) / v.y;

        u0 = Mathf.Max(u_0.x, u_0.y);
        u1 = Mathf.Min(u_1.x, u_1.y);

        return u0 <= u1;
    }

    public void CheckCollisions()
    {
        Vector2 overlap;

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

                        if (obj1.mAABB.OverlapsSigned(obj2.mAABB, out overlap) && !obj1.HasCollisionDataFor(obj2))
                        {
                            obj1.mAllCollidingObjects.Add(new CollisionData(obj2, overlap, obj1.mSpeed, obj2.mSpeed, obj1.mOldPosition, obj2.mOldPosition, obj1.mPosition, obj2.mPosition));
                            obj2.mAllCollidingObjects.Add(new CollisionData(obj1, -overlap, obj2.mSpeed, obj1.mSpeed, obj2.mOldPosition, obj1.mOldPosition, obj2.mPosition, obj1.mPosition));
                        }
                    }
                }
            }
        }
    }

    public void UpdateCollisionState(MovingObject obj)
    {
        if (obj.mSpeed.x > 0.0f)
        {
            obj.mSpeed.x = 0.0f;
            obj.mPS.pushesRight = true;
        }
        else if (obj.mSpeed.x < 0.0f)
        {
            obj.mSpeed.x = 0.0f;
            obj.mPS.pushesLeft = true;
        }

        if (obj.mSpeed.y > 0.0f)
        {
            obj.mSpeed.y = 0.0f;
            obj.mPS.pushesTop = true;
        }
        else if (obj.mSpeed.y < 0.0f)
        {
            obj.mSpeed.y = 0.0f;
            obj.mPS.pushesBottom = true;
        }
    }

    

    public Vector2i GetMapTileAtPoint(Vector2 point)
    {
        return new Vector2i((int)((point.x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize)),
                    (int)((point.y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize)));
    }

    public int GetMapTileYAtPoint(float y)
    {
        return (int)((y - mPosition.y + cTileSize / 2.0f) / (float)(cTileSize));
    }

    public int GetMapTileXAtPoint(float x)
    {
        return (int)((x - mPosition.x + cTileSize / 2.0f) / (float)(cTileSize));
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

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return TileType.Block;

        return mTiles[x, y];
    }

    public bool IsObstacle(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return true;

        return (mTiles[x, y] == TileType.Block);
    }

    public bool IsGround(int x, int y)
    {
        if (x < 0 || x >= mWidth
           || y < 0 || y >= mHeight)
            return false;

        return (mTiles[x, y] == TileType.OneWay || mTiles[x, y] == TileType.Block);
    }

    public bool IsOneWayPlatform(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return false;

        return (mTiles[x, y] == TileType.OneWay);
    }

    public bool IsEmpty(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return false;

        return (mTiles[x, y] == TileType.Empty);
    }

    public bool IsNotEmpty(int x, int y)
    {
        if (x < 0 || x >= mWidth
            || y < 0 || y >= mHeight)
            return true;

        return (mTiles[x, y] != TileType.Empty);
    }

    public void SetTile(int x, int y, TileType type)
    {
        if (x <= 1 || x >= mWidth - 2 || y <= 1 || y >= mHeight - 2)
            return;

        mTiles[x, y] = type;

        if (type == TileType.Block)
        {
            
        }
        else if (type == TileType.OneWay)
        {
            
        }
        else
        {
            
        }


    }

}
