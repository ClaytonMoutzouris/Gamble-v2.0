
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

    public FallingRock(EntityPrototype proto) :base(proto)
    {

        Body.mSpeed = Vector2.zero;
        Body.mIsKinematic = true;
        Body.mIgnoresGravity = true;



        isTriggered = false;
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);

        Renderer.SetSprite(Resources.Load<Sprite>("Sprites/FallingRock") as Sprite);
        //Vector2i tilePos = mMap.GetMapTileAtPoint(pos);
        sizeDown = Map.CheckEmptySpacesBelow(Map.GetMapTileAtPoint(spawnPoint));
        if (sizeDown == 0)
            mToRemove = true;
        trigger = new Sightbox(this, new CustomAABB(Position, new Vector2(MapManager.cTileSize / 2, (sizeDown * MapManager.cTileSize / 2)), new Vector2(0, -(sizeDown * MapManager.cTileSize / 2) - (MapManager.cTileSize / 2))));
        trigger.UpdatePosition();
        if (sizeDown <= 0)
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

        if (Body.mIsKinematic && Body.mPS.pushedBottom)
        {
            Body.mIsKinematic = false;

        }

        if (trigger != null)
        CollisionManager.UpdateAreas(trigger);

    }

    public void DropTrigger()
    {
        Body.mIgnoresGravity = false;
        Body.mIsKinematic = false;
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
