using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogBeast : Miniboss
{
    public BogBeast(EnemyPrototype proto) : base(proto)
    {
        Body.mIsKinematic = true;

    }

    public override void Die()
    {
        base.Die();
    }

    public override void DropLoot()
    {
        base.DropLoot();
    }

    public override void EntityUpdate()
    {

        EnemyBehaviour.CheckForTargets(this);


        switch (mEnemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:


                if (Target != null)
                {
                    Vector2 positionVector = Target.Position - Position;

                    if (positionVector.x > 0)
                    {

                        mDirection = EntityDirection.Right;
                    }
                    else
                    {

                        mDirection = EntityDirection.Left;
                    }



                    if (EnemyBehaviour.TargetInRange(this, Target, 320))
                    {
                        Vector2 spawnPoint = MapManager.instance.GetMapTilePosition(MapManager.instance.GetFloorTile(MapManager.instance.GetMapTileAtPoint(Target.Position)));
                        Debug.Log("Spawnpoint of the thing " + spawnPoint);
                        mAttackManager.rangedAttacks[0].Activate(Vector2.right, spawnPoint);
                        Body.mSpeed.x = 0;
                        Renderer.SetAnimState("BogBeast_Attack1");
                    }
                    else
                    {
                        Renderer.SetAnimState("BogBeast_Walk");

                        if (positionVector.y <= 32 && Body.mPS.onOneWay)
                        {
                            Body.mPS.tmpIgnoresOneWay = true;
                        }

                        Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;

                    }

                }
                else
                {
                    if (Body.mPS.pushedLeftTile)
                    {

                        mDirection = EntityDirection.Right;

                    }
                    else if (Body.mPS.pushedRightTile)
                    {
                        mDirection = EntityDirection.Left;
                    }
                    Body.mSpeed.x = GetMovementSpeed() * (int)mDirection;


                }



                break;
            case EnemyState.Jumping:

                break;
        }

        base.EntityUpdate();


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
    }

    protected override void Idle()
    {
        base.Idle();
    }
}
