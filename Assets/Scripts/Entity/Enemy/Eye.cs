using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{

    

    public override void EntityInit()
    {
        base.EntityInit();


        Body.mIsKinematic = false;
        Body.mIgnoresGravity = true;


        EnemyInit();

        mBehaviour.canJump = false;

        mBehaviour.moveDuration = 0.5f;
        mBehaviour.cooldownDuration = 0.5f;
        mBehaviour.jumpDuration = 3.0f;

        mBehaviour.cooldownTimer = 0f;
        mBehaviour.moveTimer = 0f;
        mBehaviour.jumpTimer = 0f;
        mBehaviour.direction = 1;
    }

    public override void EntityUpdate()
    {
        EnemyUpdate();

        
        if(this.Target != null)
        {
            HasTargetUpdate();
        } else
        {
            NoTargetUpdate();
        }
        
        

        //This is just a test, probably dont need to do it this way
        base.EntityUpdate();


        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

    }

    void HasTargetUpdate()
    {
        mAnimator.Play("Eye_Fly");
        //This works amazing!
        body.mSpeed = ((Vector2)this.Target.Position - body.mPosition).normalized*mMovingSpeed;
    }

    void NoTargetUpdate()
    {
        if (!body.mPS.pushesTop)
        {
            mAnimator.Play("Eye_Fly");
            body.mSpeed.y = mMovingSpeed;
        }
        else
        {
            mAnimator.Play("Eye_Sleep");
            body.mSpeed.y = 0;
        }

        body.mSpeed.x = 0;
    }
}
