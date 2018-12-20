using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionManager {

    public static MapManager mapManager;

    private static List<Vector2i> mOverlappingAreas = new List<Vector2i>(4);
    public static int mHorizontalAreasCount = 0;
    public static int mVerticalAreasCount = 0;
    public static int mGridAreaWidth = 10;
    public static int mGridAreaHeight = 10;

    public static List<PhysicsObject>[,] mObjectsInArea;
    public static List<Hitbox>[,] hitboxes;
    public static List<Hurtbox>[,] hurtboxes;

    public static List<CustomAABB>[,] mAllAABBs;
    //public static List<Entity> mCollisionRequestObjects = new List<Entity>();


    public static void InitializeCollisionManager()
    {
        mapManager = Game.instance.mMap;
        mHorizontalAreasCount = Mathf.CeilToInt((float)mapManager.mWidth / (float)mGridAreaWidth);
        mVerticalAreasCount = Mathf.CeilToInt((float)mapManager.mHeight / (float)mGridAreaHeight);

        //How do we combine these?
        mObjectsInArea = new List<PhysicsObject>[mHorizontalAreasCount, mVerticalAreasCount];

        for (var y = 0; y < mVerticalAreasCount; ++y)
        {
            for (var x = 0; x < mHorizontalAreasCount; ++x)
                mObjectsInArea[x, y] = new List<PhysicsObject>();
        }

        hitboxes = new List<Hitbox>[mHorizontalAreasCount, mVerticalAreasCount];

        for (var y = 0; y < mVerticalAreasCount; ++y)
        {
            for (var x = 0; x < mHorizontalAreasCount; ++x)
                hitboxes[x, y] = new List<Hitbox>();
        }

        
            hurtboxes = new List<Hurtbox>[mHorizontalAreasCount, mVerticalAreasCount];

        for (var y = 0; y < mVerticalAreasCount; ++y)
        {
            for (var x = 0; x < mHorizontalAreasCount; ++x)
                hurtboxes[x, y] = new List<Hurtbox>();
        }

    }

    public static void RemoveObjectFromAreas(PhysicsObject obj)
    {
        for (int i = 0; i < obj.mAreas.Count; ++i)
            RemoveObjectFromArea(obj.mAreas[i], obj.mIdsInAreas[i], obj);
        obj.mAreas.Clear();
        obj.mIdsInAreas.Clear();
    }

    public static void RemoveObjectFromAreas(Hitbox obj)
    {
        for (int i = 0; i < obj.mAreas.Count; ++i)
            RemoveObjectFromArea(obj.mAreas[i], obj.mIdsInAreas[i], obj);
        obj.mAreas.Clear();
        obj.mIdsInAreas.Clear();
    }

    public static void RemoveObjectFromAreas(Hurtbox obj)
    {
        for (int i = 0; i < obj.mAreas.Count; ++i)
            RemoveObjectFromArea(obj.mAreas[i], obj.mIdsInAreas[i], obj);
        obj.mAreas.Clear();
        obj.mIdsInAreas.Clear();
    }


    static void RemoveObjectFromArea(Vector2i areaIndex, int objIndexInArea, PhysicsObject obj)
    {
        var area = mObjectsInArea[areaIndex.x, areaIndex.y];

        if (obj.mEntity.mToRemove)
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



    static void RemoveObjectFromArea(Vector2i areaIndex, int objIndexInArea, Hitbox obj)
    {
        var area = hitboxes[areaIndex.x, areaIndex.y];

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

    static void RemoveObjectFromArea(Vector2i areaIndex, int objIndexInArea, Hurtbox obj)
    {
        var area = hurtboxes[areaIndex.x, areaIndex.y];

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


    static void AddObjectToArea(Vector2i areaIndex, PhysicsObject obj)
    {
        var area = mObjectsInArea[areaIndex.x, areaIndex.y];

        //save the index of  the object in the area
        obj.mAreas.Add(areaIndex);
        obj.mIdsInAreas.Add(area.Count);

        //add the object to the area
        area.Add(obj);
    }

    static void AddObjectToArea(Vector2i areaIndex, Hitbox obj)
    {
        var area = hitboxes[areaIndex.x, areaIndex.y];

        //save the index of  the object in the area
        obj.mAreas.Add(areaIndex);
        obj.mIdsInAreas.Add(area.Count);

        //add the object to the area
        area.Add(obj);
    }

    static void AddObjectToArea(Vector2i areaIndex, Hurtbox obj)
    {
        var area = hurtboxes[areaIndex.x, areaIndex.y];

        //save the index of  the object in the area
        obj.mAreas.Add(areaIndex);
        obj.mIdsInAreas.Add(area.Count);

        //add the object to the area
        area.Add(obj);
    }

    public static void UpdateAreas(PhysicsObject obj)
    {
        //get the areas at the aabb's corners
        var topLeft = mapManager.GetMapTileAtPoint((Vector2)obj.mAABB.Center + new Vector2(-obj.mAABB.HalfSize.x, obj.mAABB.HalfSizeY));
        var topRight = mapManager.GetMapTileAtPoint(obj.mAABB.Center + obj.mAABB.HalfSize);
        var bottomLeft = mapManager.GetMapTileAtPoint(obj.mAABB.Center - obj.mAABB.HalfSize);
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

    public static void UpdateAreas(Hitbox obj)
    {
        //get the areas at the aabb's corners
        var topLeft = mapManager.GetMapTileAtPoint((Vector2)obj.collider.Center + new Vector2(-obj.collider.HalfSize.x, obj.collider.HalfSizeY));
        var topRight = mapManager.GetMapTileAtPoint(obj.collider.Center + obj.collider.HalfSize);
        var bottomLeft = mapManager.GetMapTileAtPoint(obj.collider.Center - obj.collider.HalfSize);
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

    public static void UpdateAreas(Hurtbox obj)
    {
        //get the areas at the aabb's corners
        var topLeft = mapManager.GetMapTileAtPoint((Vector2)obj.collider.Center + new Vector2(-obj.collider.HalfSize.x, obj.collider.HalfSizeY));
        var topRight = mapManager.GetMapTileAtPoint(obj.collider.Center + obj.collider.HalfSize);
        var bottomLeft = mapManager.GetMapTileAtPoint(obj.collider.Center - obj.collider.HalfSize);
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

    // Use this for initialization


    public static void CheckCollisions()
    {

        //First check the pushboxes
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

        //Next check the hitboxes on the hurtboxes
        for (int y = 0; y < mVerticalAreasCount; ++y)
        {
            for (int x = 0; x < mHorizontalAreasCount; ++x)
            {
                foreach(Hitbox hitbox in hitboxes[x, y])
                {
                    foreach(Hurtbox hurtbox in hurtboxes[x, y])
                    {
                        if (hitbox.collider.OverlapsSigned(hurtbox.collider, out overlapWidth, out overlapHeight) && !hitbox.colliders.Contains(hurtbox))
                        {
                            hitbox.colliders.Add(hurtbox);
                            //new CollisionData(hurtbox, new Vector2(overlapWidth, overlapHeight), hitbox.mSpeed, hurtbox.mSpeed, hitbox.mOldPosition, hurtbox.mOldPosition, hitbox.mPosition, hurtbox.mPosition));
                            //hurtbox.mAllCollidingObjects.Add(new CollisionData(hitbox, -new Vector2(overlapWidth, overlapHeight), hurtbox.mSpeed, hitbox.mSpeed, hurtbox.mOldPosition, hitbox.mOldPosition, hurtbox.mPosition, hitbox.mPosition));
                        }
                    }
                }
            }
        }



    }

}
