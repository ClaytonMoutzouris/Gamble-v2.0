using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {

    public bool dashTrigger;
    public Sightbox meleeDash;

    public override void EntityInit()
    {
        base.EntityInit();
        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(10.0f, 5.0f), new Vector2(0, 5.0f), new Vector3(1, 1, 1)));
        //Body.mAABB.ScaleX *= -1;

        body.mIsKinematic = false;
        body.mSpeed.x = mMovingSpeed;


        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, new Vector2(10.0f, 5.0f), Vector3.zero, new Vector3(1, 1, 1)));
        HurtBox.UpdatePosition();
        EnemyInit();


        meleeDash = new Sightbox(this, new CustomAABB(transform.position, new Vector2( 2 * MapManager.cTileSize / 2, MapManager.cTileSize / 2), new Vector3(-MapManager.cTileSize,11,0), new Vector3(2, 1, 1)));
        meleeDash.UpdatePosition();
    }

    public override void EntityUpdate()
    {
        EnemyUpdate();

        if (Body.mSpeed.x > 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed);
            Body.mAABB.ScaleX = 1;
        }
        else if (Body.mSpeed.x < 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
            Body.mAABB.ScaleX = -1;

        }

        if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
        {
            mMovingSpeed *= -1;

        }


        body.mSpeed.x = mMovingSpeed;


        base.EntityUpdate();

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(sight);
        sight.mEntitiesInSight.Clear();


        //HurtBox.mCollisions.Clear();
        //make sure the hitbox follows the object
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        /*
        if (meleeDash != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(meleeDash.mAABB.Center, meleeDash.mAABB.HalfSize * 2);
        }
        */
    }

    /*
    public override void SecondUpdate()
    {
        base.SecondUpdate();
        meleeDash.UpdatePosition();
    }
    */
}
