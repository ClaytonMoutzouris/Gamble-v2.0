using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slime : Enemy {

    [HideInInspector]
    public bool dashTrigger;


    public override void EntityInit()
    {
        base.EntityInit();
        //Body.mAABB.ScaleX *= -1;

        body.mIsKinematic = false;
        body.mSpeed.x = mMovingSpeed;


        EnemyInit();

        //Set Custom stats
        
        mBehaviour.canJump = true;
        mBehaviour.moveDuration = 0.5f;
        mBehaviour.moveTimer = 0f;
        mBehaviour.wait = 0.5f;
        mBehaviour.jumpDuration = 0f;
        mBehaviour.jumpTimer = 3.0f;
        mBehaviour.jumpSpeed = 400f;
        mBehaviour.wait += this.mBehaviour.moveDuration;
        
    }

    public override void EntityUpdate()
    {
        EnemyUpdate();
        base.EntityUpdate();

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();


        //HurtBox.mCollisions.Clear();
        //make sure the hitbox follows the object
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

}
