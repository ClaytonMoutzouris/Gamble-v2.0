
using UnityEngine;

public class FallingRock : Entity
{
    public bool isTriggered = false;
    public float mTriggerTime = 5.0f;
    public float mTimeToTrigger = 0.0f;

    public Sightbox trigger;

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(trigger.mAABB.Center, trigger.mAABB.HalfSize * 2);
    }

    public override void EntityInit()
    {
        base.EntityInit();
        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(15.0f, 15.0f), new Vector2(0, 15f), new Vector3(1, 1, 1)));

        Body.mSpeed = Vector2.zero;
        Body.mIsKinematic = true;
        Body.mIgnoresGravity = true;
        /*
        mMap.GetMapTilePosition(Body.til)
        int sizeDown = 0;
        Debug.Log(mMap.GetTile(tile.x, tile.y - 1));
        while (mMap.GetTile(tile.x, tile.y - 1) == TileType.Empty)
        {
            
            sizeDown += 1;
            tile = mMap.GetMapTileAtPoint(new Vector3(tile.x, tile.y - 1));
        }
        */

        trigger = new Sightbox(this, new CustomAABB(transform.position, new Vector2(MapManager.cTileSize/2, 10 * MapManager.cTileSize/2), new Vector2(0, -10 * MapManager.cTileSize/2), new Vector3(1, 1, 1)));
        trigger.UpdatePosition();

        isTriggered = false;
    }

    int GetSizeDown(int sizeDown, int x, int y)
    {
        

        if (mMap.GetTile(x, y-1) == TileType.Empty)
        {
          sizeDown += GetSizeDown(sizeDown, x, y - 1);
        } else
        {
            sizeDown = 1;
        }

        return sizeDown;
    }

    public override void EntityUpdate()
    {
        foreach(Entity body in trigger.mEntitiesInSight)
        {
            if(body is Player)
            {
                isTriggered = true;
            }
        }

        if(isTriggered)
            Body.mIgnoresGravity = false;

        base.EntityUpdate();

        CollisionManager.UpdateAreas(trigger);

    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        trigger.UpdatePosition();

    }
}
