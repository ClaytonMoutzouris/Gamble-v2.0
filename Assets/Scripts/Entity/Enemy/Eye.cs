using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : EnemyObject
{
    public PhysicsObject target = null;

    Animator mAnimator;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    public override void ObjectInit()
    {

        mAABB.HalfSize = new Vector2(16.0f, 10.0f);
        mIsKinematic = false;

        int r = Random.Range(0, 2);

        //mEnemyType = EnemyType.Slime;



        base.ObjectInit();

    }

    public override void CustomUpdate()
    {
        if(target != null)
        {
            HasTargetUpdate();
        } else
        {
            NoTargetUpdate();
        }
        
        

        //This is just a test, probably dont need to do it this way
        base.CustomUpdate();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    void HasTargetUpdate()
    {
        mAnimator.Play("Eye_Fly");
        //This works amazing!
        mSpeed = (target.mPosition - mPosition).normalized*mMovingSpeed;


    }

    void NoTargetUpdate()
    {
        if (!mPS.pushesTop)
        {
            mAnimator.Play("Eye_Fly");
            mSpeed.y = mMovingSpeed;
        }
        else
        {
            mAnimator.Play("Eye_Sleep");
            mSpeed.y = 0;
        }
    }
}
