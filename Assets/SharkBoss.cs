using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBoss : Enemy
{
    #region SetInInspector
    public BossState mBossState = BossState.Idle;
    public Projectile bullets;
    public bool bossTrigger = false;
    public Vector3 attackAnchor;

    #endregion




    public override void EntityInit()
    {
        base.EntityInit();

        body.mIsKinematic = true;


        EnemyInit();

        mAttackManager.AttackList.Clear();
        mEnemyState = EnemyState.Idle;
        RangedAttack ranged = new RangedAttack(this, 0.1f, 7, .1f, Range.Far, bullets);
        mAttackManager.AttackList.Add(ranged);

        RangedAttack ranged2 = new RangedAttack(this, 0.1f, 30, .1f, Range.Far, bullets);
        mAttackManager.AttackList.Add(ranged2);
    }

    public override void EntityUpdate()
    {

        EnemyUpdate();
        base.EntityUpdate();



        switch (mEnemyState)
        {
            case EnemyState.Idle:

                this.Target = null;
                if (Sight.mEntitiesInSight != null)
                {
                    foreach (Entity entity in Sight.mEntitiesInSight)
                    {
                        if (entity is Player)
                        {
                            this.Target = entity;
                            TriggerBoss();
                            break;
                        }
                    }
                }

                break;
            case EnemyState.Aggrivated:

                if (Target != null)
                {
                    //Replace this with pathfinding to the target
                    Vector2 dir = ((Vector2)Target.Position - Body.mPosition).normalized;

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


        //HurtBox.mCollisions.Clear();
        //UpdatePhysics();

        //make sure the hitbox follows the object
    }

    public void TriggerBoss()
    {
        if (bossTrigger)
            return;

        mEnemyState = EnemyState.Aggrivated;

        SoundManager.instance.PlayMusic(2);
    }


    public override void SecondUpdate()
    {
        base.SecondUpdate();

    }

    public override void Shoot(Projectile prefab, Attack attack, Vector2 Dir)
    {
        Dir = Dir + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized*0.3f;
        Projectile temp = Instantiate(prefab, body.mAABB.Center + attackAnchor, Quaternion.identity);
        temp.EntityInit();
        temp.Attack = attack;
        temp.Owner = this;
        temp.SetInitialDirection(Dir);

    }


}
