
using UnityEngine;

public class FallingRock : Entity
{
    //[HideInInspector]
    public bool isTriggered = false;
    [HideInInspector]
    public Sightbox trigger;
    [HideInInspector]
    public Vector2i tilePos;
    [HideInInspector]
    public int sizeDown = 0;

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        if(trigger != null)
        Gizmos.DrawCube(trigger.mAABB.Center, trigger.mAABB.HalfSize * 2);
    }

    public override void EntityInit()
    {
        base.EntityInit();

        Body.mSpeed = Vector2.zero;
        Body.mIsKinematic = false;
        Body.mIsHeavy = true;
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
        if (sizeDown == 0)
            mToRemove = true;
        trigger = new Sightbox(this, new CustomAABB(Position, new Vector2(MapManager.cTileSize / 2, (sizeDown * MapManager.cTileSize / 2)), new Vector2(0,  -(sizeDown * MapManager.cTileSize / 2) - (MapManager.cTileSize / 2)), new Vector3(1, 1, 1)));
        trigger.UpdatePosition();
        if(sizeDown <= 0)
        {
            isTriggered = true;
        }
    }


    public override void EntityUpdate()
    {
        if(trigger != null)
        {
            foreach (Entity body in trigger.mEntitiesInSight)
            {
                if (body is Player)
                {
                    isTriggered = true;
                }
            }
        }
        

        if (isTriggered && trigger != null)
        {
            DropTrigger();

        } else
        {

        }




        base.EntityUpdate();

        if(trigger != null)
        CollisionManager.UpdateAreas(trigger);

    }

    public void DropTrigger()
    {
        Body.mIgnoresGravity = false;
        CollisionManager.RemoveObjectFromAreas(trigger);
        trigger = null;
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        if (trigger != null)
            trigger.UpdatePosition();

    }

    public override void Die()
    {

        base.Die();
    }

    public override void ActuallyDie()
    {
        if(trigger != null)
        CollisionManager.RemoveObjectFromAreas(trigger);


        base.ActuallyDie();

    }
}
