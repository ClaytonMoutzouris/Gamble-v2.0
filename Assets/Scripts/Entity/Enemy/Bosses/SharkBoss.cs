using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBoss : BossEnemy
{
    #region SetInInspector
    public Vector3 attackAnchor;

    #endregion




    public override void EntityInit()
    {
        base.EntityInit();
        base.EnemyInit();
        Body.mIsKinematic = true;
        Body.mIsHeavy = true;

    }

    public override void EntityUpdate()
    {

        mAttackManager.UpdateAttacks();
        base.EntityUpdate();



        switch (mBossState)
        {
            case BossState.Idle:

                CheckForTargets();

                break;
            case BossState.Aggrivated:
                CheckForTargets();
                Debug.Log("Got here 1");
                if (Target != null)
                {
                    //Replace this with pathfinding to the target
                    Vector2 dir = (Target.Body.mAABB.Center - (body.mAABB.Center + attackAnchor)).normalized;

                    if (!mAttackManager.AttackList[0].onCooldown)
                    {
                        RangedAttack attack = (RangedAttack)mAttackManager.AttackList[0];
                        attack.Activate(dir);
                    }



                    if (dir.x < 0)
                    {
                        body.mAABB.ScaleX = 1;
                    }
                    else
                    {
                        body.mAABB.ScaleX = -1;

                    }

                }
                else
                {
                   


                }

                body.mSpeed.x = mMovingSpeed;

                break;
            }


        CollisionManager.UpdateAreas(HurtBox);

        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

        //make sure the hitbox follows the object
    }



    public override void Shoot(Projectile prefab, Attack attack, Vector2 Dir)
    {
        Dir = Dir + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized*.3f;
        Projectile temp = Instantiate(prefab, body.mAABB.Center + attackAnchor, Quaternion.identity);
        temp.EntityInit();
        temp.Attack = attack;
        temp.Owner = this;
        temp.SetInitialDirection(Dir);

    }

}
