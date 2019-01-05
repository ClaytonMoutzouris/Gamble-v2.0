
using UnityEngine;

public class FallingRock : Entity
{
    public bool isTriggered = false;
    public float mTriggerTime = 5.0f;
    public float mTimeToTrigger = 0.0f;

    public Sightbox trigger;
    public Vector2i tilePos;
    public int sizeDown = 0;

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
        
        */



        isTriggered = false;
    }

    public void InitPosition(Vector2i pos)
    {
        //Vector2i tilePos = mMap.GetMapTileAtPoint(pos);
        Body.SetTilePosition(pos);
        tilePos = pos;

        sizeDown = mMap.CheckEmptySpacesBelow(pos);

        trigger = new Sightbox(this, new CustomAABB(Position, new Vector2(MapManager.cTileSize / 2, (sizeDown * MapManager.cTileSize / 2)), new Vector2(0,  -(sizeDown * MapManager.cTileSize / 2) - (MapManager.cTileSize / 2)), new Vector3(1, 1, 1)));
        trigger.UpdatePosition();
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

    public override void Die()
    {
        base.Die();
    }

    public override void ActuallyDie()
    {

        CollisionManager.RemoveObjectFromAreas(trigger);

        base.ActuallyDie();

    }
}
