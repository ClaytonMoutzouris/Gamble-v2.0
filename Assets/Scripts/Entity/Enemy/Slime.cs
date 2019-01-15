using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {

    public bool dashTrigger;
    public Sightbox meleeDash;
    public float moveCooldown = 0.5f;
    public float strollTime = 0f;
    public float wait = 0.5f;

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

        wait += moveCooldown;

        meleeDash = new Sightbox(this, new CustomAABB(transform.position, new Vector2( 2 * MapManager.cTileSize / 2, MapManager.cTileSize / 2), new Vector3(-MapManager.cTileSize,11,0), new Vector3(2, 1, 1)));
        meleeDash.UpdatePosition();
    }
    /*1. Entities stroll around until their moveCooldown is reached.
     *2. If they have a target, they move in the direction of the target.
     *3. If the strollTime reaches moveCooldown, the entity will stop moving.
     *4. The entity will consider where to move next when it is done waiting.
     *5. When it is done waiting strollTime is reset to 0f.
     */
    public void Move(Entity target, int direction)
    {
        //1-2
        if (strollTime < moveCooldown)
        {
            //If we have a target move in it's direction.
            if (target != null)
            {
                body.mSpeed.x = mMovingSpeed * direction;
            }
           
            strollTime += Time.deltaTime;
            return;
        }
        //3-4
        else if(strollTime > moveCooldown && strollTime < wait)
        {
            Debug.Log("Resting");
            //Movement Cooldown reached .
            //reset strollTime.
            body.mSpeed.x = 0f;
            strollTime += Time.deltaTime;
            return;
        }
        //5
        else if(strollTime > wait)
        {
            Debug.Log("Done Resting.");
            strollTime = 0f;
            return;
        }
    }

   /*1. Entities stroll around until their moveCooldown is reached.
    *2. If the entity hits a wall, they will wiggle until they turn around.
    *Consider fixing the wiggle.
    *3. If the strollTime reaches moveCooldown, the entity will stop moving.
    *4. The entity will consider where to move next when it is done waiting.
    *5. When it is done waiting strollTime is reset to 0f.
   */
    public void Move()
    {
        //1
        body.mSpeed.x = mMovingSpeed;
        //2
        if (strollTime < moveCooldown)
        {
            if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
            {
                mMovingSpeed *= -1;

            }
            strollTime += Time.deltaTime;
            return;
        }
        //3-4
        else if (strollTime > moveCooldown && strollTime < wait)
        {
            //Movement Cooldown reached .
            //reset strollTime.
            body.mSpeed.x = 0f;
            strollTime += Time.deltaTime;
            return;
        }
        //5
        else if (strollTime > wait)
        {
            strollTime = 0f;
            return;
        }
    }

    public override void EntityUpdate()
    {
        EnemyUpdate();
        if (target != null)
        {
            if (target.Position.x > this.Body.mPosition.x)
            {
                Move(target, 1);
            }
            else
            {
                Move(target, -1);
            }
        }
        else
        {
            Move();
        }

        /*
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
        */


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
