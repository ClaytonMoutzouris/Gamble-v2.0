using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBoss : Enemy
{
    public Entity target = null;
    public Bullet VolcanicBombPrefab;


    public override void EntityInit()
    {
        base.EntityInit();
        EnemyInit();

        mAnimator = GetComponent<Animator>();

        Body = new PhysicsBody(this, new CustomAABB(transform.position, BodySize, new Vector2(0, BodySize.y), new Vector3(1, 1, 1)));
        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, BodySize, Vector3.zero, new Vector3(1, 1, 1)));
        sight = new Sightbox(this, new CustomAABB(transform.position, new Vector2(150, 150), Vector3.zero, new Vector3(1, 1, 1)));

        Body.mIsKinematic = false;
        //Body.mIgnoresGravity = true;

        HurtBox.UpdatePosition();
        sight.UpdatePosition();

        RangedAttack ranged = new RangedAttack(this, 0.2f, 10, VolcanicBombPrefab);
        mAttackManager.AttackList.Add(ranged);

    }

    public override void EntityUpdate()
    {
        mAttackManager.UpdateAttacks();
        target = null;
        if (sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in sight.mEntitiesInSight)
            {
                if (entity is Player)
                {
                    target = entity;
                    break;
                }
            }
        }

        if (body.mPS.pushesLeftTile || body.mPS.pushesRightTile)
        {
            mMovingSpeed *= -1;

        }

        if (target != null)
        {
            HasTargetUpdate();
        }
        else
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
        //mAnimator.Play("Eye_Fly");
        //This works amazing!

        if(target.Position.x > Position.x)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed);
            Body.mAABB.ScaleX = -1;
        } else if (target.Position.x < Position.x)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
            Body.mAABB.ScaleX = 1;
        }

        body.mSpeed.x = mMovingSpeed;

        mAttackManager.AttackList[0].Activate();
        //body.mSpeed = ((Vector2)target.Position - body.mPosition).normalized * Mathf.Abs(mMovingSpeed);


    }

    void NoTargetUpdate()
    {
        if (Body.mSpeed.x > 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed);
            Body.mAABB.ScaleX = -1;
        }
        else if (Body.mSpeed.x < 0)
        {
            mMovingSpeed = Mathf.Abs(mMovingSpeed) * -1;
            Body.mAABB.ScaleX = 1;

        }

        


        body.mSpeed.x = mMovingSpeed;
    }

    public override void Shoot(Bullet prefab, Attack attack)
    {
        Bullet temp = Instantiate(prefab, body.mAABB.Center, Quaternion.identity);
        temp.EntityInit();
        temp.Attack = attack;
        temp.Owner = this;
        temp.SetInitialDirection(new Vector2(Random.Range(-.7f, .7f), 1));
        
    }
}
