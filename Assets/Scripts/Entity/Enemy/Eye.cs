using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{
    public PhysicsObject target = null;



    public override void EntityInit()
    {
        mAnimator = GetComponent<Animator>();

        Body.mCollider.mAABB.HalfSize = new Vector2(16.0f, 10.0f);
        Body.mIsKinematic = false;

        int r = Random.Range(0, 2);

        //mEnemyType = EnemyType.Slime;



        base.EntityInit();

    }

    public override void EntityUpdate()
    {
        if(target != null)
        {
            HasTargetUpdate();
        } else
        {
            NoTargetUpdate();
        }
        
        

        //This is just a test, probably dont need to do it this way
        base.EntityUpdate();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    void HasTargetUpdate()
    {
        mAnimator.Play("Eye_Fly");
        //This works amazing!
        body.mSpeed = (target.mPosition - body.mPosition).normalized*mMovingSpeed;


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
    }
}
