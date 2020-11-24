using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{

    

    public Eye(EnemyPrototype proto) : base(proto)
    { 

        Body.mIgnoresOneWay = true;


    }

    public override void EntityUpdate()
    {
        //This is just a test, probably dont need to do it this way
        
        EnemyBehaviour.CheckForTargets(this);
        

        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                if (Target != null)
                {
                    Renderer.SetAnimState("Eye_Fly");

                    //Replace this with pathfinding to the target
                    Vector2 dir = (Target.Body.mAABB.Center - (Body.mAABB.Center)).normalized;

                    if (!mAttackManager.rangedAttacks[0].OnCooldown())
                    {
                        RangedAttack attack = mAttackManager.rangedAttacks[0];
                        attack.Activate(dir, Position);
                    }

                    Body.mSpeed = ((Vector2)Target.Position - Position).normalized * GetMovementSpeed();

                    if (Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile)
                    {
                        if(Target.Position.y < Position.y)
                        {
                            Body.mSpeed.y = -GetMovementSpeed();
                        } else
                        {
                            Body.mSpeed.y = GetMovementSpeed();
                        }
                    } else if (Body.mPS.pushesBottomTile || Body.mPS.pushesTopTile)
                    {
                        if (Target.Position.x < Position.x)
                        {
                            Body.mSpeed.x = -GetMovementSpeed();
                        }
                        else
                        {
                            Body.mSpeed.x = GetMovementSpeed();
                        }
                    }

                }
                else
                {
                    if (!Body.mPS.pushesTop)
                    {
                        Renderer.SetAnimState("Eye_Fly");
                        Body.mSpeed.y = GetMovementSpeed();
                    }
                    else
                    {
                        Renderer.SetAnimState("Eye_Sleep");
                        Body.mSpeed.y = 0;
                    }

                    Body.mSpeed.x = 0;


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
