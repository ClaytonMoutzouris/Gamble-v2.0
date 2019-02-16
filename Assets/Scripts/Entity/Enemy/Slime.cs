using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slime : Enemy {

    [HideInInspector]
    public bool dashTrigger;
    public Hitbox hitbox;

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
        mBehaviour.cooldownDuration = 0.5f;
        mBehaviour.jumpDuration = 3.0f;

        mBehaviour.cooldownTimer = 0f;
        mBehaviour.moveTimer = 0f;
        mBehaviour.jumpTimer = 0f;

        mBehaviour.jumpSpeed = 400f;
        mBehaviour.direction = 1;

    }

    public override void EntityUpdate()
    {

        EnemyUpdate();
        base.EntityUpdate();

        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

}
