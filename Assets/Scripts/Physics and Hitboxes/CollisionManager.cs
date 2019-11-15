using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionManager {

    public static MapManager mapManager;

    private static List<Vector2i> mOverlappingAreas = new List<Vector2i>(4);
    public static int mHorizontalAreasCount = 0;
    public static int mVerticalAreasCount = 0;
    public static int mGridAreaWidth = 25;
    public static int mGridAreaHeight = 25;

    public static List<CustomCollider2D>[,] mCollidersInArea;

    //public static List<Entity> mCollisionRequestObjects = new List<Entity>();


    public static void InitializeCollisionManager()
    {
        mapManager = LevelManager.instance.mMap;
        mHorizontalAreasCount = Mathf.CeilToInt((float)mapManager.mWidth / (float)mGridAreaWidth);
        mVerticalAreasCount = Mathf.CeilToInt((float)mapManager.mHeight / (float)mGridAreaHeight);

        mCollidersInArea = new List<CustomCollider2D>[mHorizontalAreasCount, mVerticalAreasCount];

        for (var y = 0; y < mVerticalAreasCount; ++y)
        {
            for (var x = 0; x < mHorizontalAreasCount; ++x)
                mCollidersInArea[x, y] = new List<CustomCollider2D>();
        }

        
    }

    public static void RemoveObjectFromAreas(CustomCollider2D obj)
    {
        for (int i = 0; i < obj.mAreas.Count; ++i)
            RemoveObjectFromArea(obj.mAreas[i], obj.mIdsInAreas[i], obj);
        obj.mAreas.Clear();
        obj.mIdsInAreas.Clear();
    }

    static void RemoveObjectFromArea(Vector2i areaIndex, int objIndexInArea, CustomCollider2D obj)
    {
        var area = mCollidersInArea[areaIndex.x, areaIndex.y];

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


    static void AddObjectToArea(Vector2i areaIndex, CustomCollider2D obj)
    {
        List<CustomCollider2D> area = new List<CustomCollider2D>();
        try
        {
            area = mCollidersInArea[areaIndex.x, areaIndex.y];

        }
        catch (System.IndexOutOfRangeException e)
        {
            Debug.LogError("Index out of range " + areaIndex.x + " , " + areaIndex.y);
        }

        //save the index of  the object in the area
        obj.mAreas.Add(areaIndex);
        obj.mIdsInAreas.Add(area.Count);

        //add the object to the area
        area.Add(obj);
    }

   
    public static void UpdateAreas(CustomCollider2D obj)
    {
        //get the areas at the aabb's corners
        var topLeft = mapManager.GetMapTileAtPoint((Vector2)obj.mAABB.Center + new Vector2(-obj.mAABB.HalfSize.x, obj.mAABB.HalfSizeY));
        var topRight = mapManager.GetMapTileAtPoint(obj.mAABB.Center + obj.mAABB.HalfSize);
        var bottomLeft = mapManager.GetMapTileAtPoint(obj.mAABB.Center - obj.mAABB.HalfSize);
        var bottomRight = new Vector2i();

        if(topLeft.x > MapManager.instance.mWidth || topLeft.y > MapManager.instance.mHeight)
        {
            //Debug.Log("big X: " + topLeft.x + "big Y: " + topRight.y);
        }

        topLeft.x /= mGridAreaWidth;
        topLeft.y /= mGridAreaHeight;

        topRight.x /= mGridAreaWidth;
        topRight.y /= mGridAreaHeight;

        bottomLeft.x /= mGridAreaWidth;
        bottomLeft.y /= mGridAreaHeight;

        topLeft.x = Mathf.Clamp(topLeft.x, 0, mHorizontalAreasCount - 1);
        topLeft.y = Mathf.Clamp(topLeft.y, 0, mVerticalAreasCount - 1);

        topRight.x = Mathf.Clamp(topRight.x, 0, mHorizontalAreasCount - 1);
        topRight.y = Mathf.Clamp(topRight.y, 0, mVerticalAreasCount - 1);

        bottomLeft.x = Mathf.Clamp(bottomLeft.x, 0, mHorizontalAreasCount - 1);
        bottomLeft.y = Mathf.Clamp(bottomLeft.y, 0, mVerticalAreasCount - 1);

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
    /*
    public static bool LegalCollision(CustomCollider2D obj1, CustomCollider2D obj2)
    {
        //It's not a legal collision if an object is colliding with itself, or either of the colliders are closed
        if (obj1.mState == ColliderState.Closed || obj2.mState == ColliderState.Closed || obj1.mEntity == obj2.mEntity)
            return false;

        if(obj1.colliderType == ColliderType.Hitbox && obj2.colliderType == ColliderType.Hurtbox
            || obj2.colliderType == ColliderType.Hitbox && obj1.colliderType == ColliderType.Hurtbox
            || obj2.colliderType == ColliderType.Pushbox && obj1.colliderType == ColliderType.Pushbox)
        {
            return true;
        }

        return false;
    }
    */
    public static void PhysicsCollision(PhysicsBody obj1, PhysicsBody obj2)
    {
        float overlapWidth, overlapHeight;

        if (obj1.mAABB.OverlapsSigned(obj2.mAABB, out overlapWidth, out overlapHeight) && !obj1.HasCollisionDataFor(obj2))
        {

            obj1.mCollisions.Add(new CollisionData(obj2, new Vector2(overlapWidth, overlapHeight), obj1.mEntity.Body.mSpeed, obj2.mEntity.Body.mSpeed, obj1.mEntity.Body.mOldPosition, obj2.mEntity.Body.mOldPosition, obj1.mEntity.Position, obj2.mEntity.Position));
            obj2.mCollisions.Add(new CollisionData(obj1, -new Vector2(overlapWidth, overlapHeight), obj2.mEntity.Body.mSpeed, obj1.mEntity.Body.mSpeed, obj2.mEntity.Body.mOldPosition, obj1.mEntity.Body.mOldPosition, obj2.mEntity.Position, obj1.mEntity.Position));
            //new CollisionData(hurtbox, new Vector2(overlapWidth, overlapHeight), hitbox.mSpeed, hurtbox.mSpeed, hitbox.mOldPosition, hurtbox.mOldPosition, hitbox.mPosition, hurtbox.mPosition));
            //hurtbox.mAllCollidingObjects.Add(new CollisionData(hitbox, -new Vector2(overlapWidth, overlapHeight), hurtbox.mSpeed, hitbox.mSpeed, hurtbox.mOldPosition, hitbox.mOldPosition, hurtbox.mPosition, hitbox.mPosition));
        }
    }

    public static void HandleCollision(CustomCollider2D obj1, CustomCollider2D obj2)
    {
        //We add in a clause that if both colliders belong to the same entity they ignore eachother
        if (obj1.mState == ColliderState.Closed || obj2.mState == ColliderState.Closed || obj1.mEntity == obj2.mEntity)
            return;

        //First we'll check to see if its a pushbox collision
        if (obj1 is PhysicsBody && obj2 is PhysicsBody)
        {
            PhysicsCollision((PhysicsBody)obj1, (PhysicsBody)obj2);
        }

        if (obj1 is Hitbox && obj2 is Hurtbox)
        {
            HitCollision((Hitbox)obj1, (Hurtbox)obj2);
        }

        if (obj2 is Hitbox && obj1 is Hurtbox)
        {
            HitCollision((Hitbox)obj2, (Hurtbox)obj1);
        }

        if (obj1 is Blockbox && obj2 is Hitbox) {
            BlockCollision((Blockbox)obj1, (Hitbox)obj2);
        }

        if(obj2 is Blockbox && obj1 is Hitbox)
        {
            BlockCollision((Blockbox)obj2, (Hitbox)obj1);
        }

        if (obj1 is Sightbox && obj2 is PhysicsBody)
        {
            SightCollision((Sightbox)obj1, (PhysicsBody)obj2);
        }

        if (obj2 is Sightbox && obj1 is PhysicsBody)
        {
            SightCollision((Sightbox)obj2, (PhysicsBody)obj1);
        }


    }

    public static void SightCollision(Sightbox obj1, PhysicsBody obj2)
    {

        if (obj1.mAABB.Overlaps(obj2.mAABB) && !obj1.mEntitiesInSight.Contains(obj2.mEntity))
        {
            obj1.mEntitiesInSight.Add(obj2.mEntity);
        }
    }
   

    public static void HitCollision(Hitbox obj1, Hurtbox obj2)
    {
        if (obj1.mEntity.mEntityType == obj2.mEntity.mEntityType)
        {
            return;
        }

        if (obj1.mEntity is AttackObject proj)
        {
            if (proj.owner.mEntityType == obj2.mEntity.mEntityType)
                return;
        }


        if (obj1.mAABB.Overlaps(obj2.mAABB) && !obj1.mCollisions.Contains((IHurtable)obj2.mEntity))
        {

            obj1.mCollisions.Add((IHurtable)obj2.mEntity);
            //new CollisionData(hurtbox, new Vector2(overlapWidth, overlapHeight), hitbox.mSpeed, hurtbox.mSpeed, hitbox.mOldPosition, hurtbox.mOldPosition, hitbox.mPosition, hurtbox.mPosition));
            //hurtbox.mAllCollidingObjects.Add(new CollisionData(hitbox, -new Vector2(overlapWidth, overlapHeight), hurtbox.mSpeed, hitbox.mSpeed, hurtbox.mOldPosition, hitbox.mOldPosition, hurtbox.mPosition, hitbox.mPosition));
        }
    }

    public static void BlockCollision(Blockbox obj1, Hitbox obj2)
    {
        if (obj1.mEntity.mEntityType == obj2.mEntity.mEntityType)
        {
            return;
        }

        if (obj2.mEntity is AttackObject proj)
        {
            if (proj.owner.mEntityType == obj1.mEntity.mEntityType)
                return;
        }


        if (obj1.mAABB.Overlaps(obj2.mAABB))
        {

            obj2.Blocked();
            //new CollisionData(hurtbox, new Vector2(overlapWidth, overlapHeight), hitbox.mSpeed, hurtbox.mSpeed, hitbox.mOldPosition, hurtbox.mOldPosition, hitbox.mPosition, hurtbox.mPosition));
            //hurtbox.mAllCollidingObjects.Add(new CollisionData(hitbox, -new Vector2(overlapWidth, overlapHeight), hurtbox.mSpeed, hitbox.mSpeed, hurtbox.mOldPosition, hitbox.mOldPosition, hurtbox.mPosition, hitbox.mPosition));
        }
    }

    public static void CheckCollisions()
    {

        //First check the pushboxes


        //Next check the hitboxes on the hurtboxes
        for (int y = 0; y < mVerticalAreasCount; ++y)
        {
            for (int x = 0; x < mHorizontalAreasCount; ++x)
            {
                var colliderInArea = mCollidersInArea[x, y];
                for (int i = 0; i < colliderInArea.Count - 1; ++i)
                {
                    var obj1 = colliderInArea[i];                 

                    for (int j = i + 1; j < colliderInArea.Count; ++j)
                    {
                        var obj2 = colliderInArea[j];


                        HandleCollision(obj1, obj2);
                        
                    }
                }
            }
        }



    }

}
