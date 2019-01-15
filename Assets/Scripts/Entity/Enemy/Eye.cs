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


    }

    public override void EntityUpdate()
    {
        EnemyUpdate();

        if(target != null)
        {
            HasTargetUpdate();
        } else
        {
            NoTargetUpdate();
        }
        
        

        //This is just a test, probably dont need to do it this way
        base.EntityUpdate();


        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(sight);
        sight.mEntitiesInSight.Clear();


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
        body.mSpeed = ((Vector2)target.Position - body.mPosition).normalized*mMovingSpeed;


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
