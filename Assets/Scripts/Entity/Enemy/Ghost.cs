using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{



    public Ghost(EnemyPrototype proto) : base(proto)
    {

        Body.mIsKinematic = false;
        Body.mIgnoresGravity = true;
        Body.mIgnoresOneWay = true;


    }

    public override void EntityUpdate()
    {
        //This is just a test, probably dont need to do it this way
        if (Hostility == Hostility.Hostile)
        {
            EnemyBehaviour.CheckForTargets(this);
        }

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                if (Target != null)
                {

                    Body.mSpeed = ((Vector2)Target.Position - Position).normalized * mMovingSpeed;


                }
                else
                {


                    Body.mSpeed = Vector2.zero;


                }

                break;

        }

        base.EntityUpdate();


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();

    }

}
