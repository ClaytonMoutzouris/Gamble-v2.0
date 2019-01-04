using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{
    public Entity target = null;

    

    public override void EntityInit()
    {
        base.EntityInit();
        EnemyInit();

        mAnimator = GetComponent<Animator>();

        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(16.0f, 10.0f), new Vector2(0, 10.0f), new Vector3(1, 1, 1)));
        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, new Vector2(16.0f, 10.0f), Vector3.zero, new Vector3(1, 1, 1)));
        sight = new Sightbox(this, new CustomAABB(transform.position, new Vector2(200, 200), Vector3.zero, new Vector3(1, 1, 1)));

        Body.mIsKinematic = false;
        Body.mIgnoresGravity = true;

        HurtBox.UpdatePosition();
        sight.UpdatePosition();

    }

    public override void EntityUpdate()
    {
        target = null;
        if(sight.mEntitiesInSight != null)
        {
            foreach(Entity entity in sight.mEntitiesInSight)
            {
                if(entity is Player)
                {
                    target = entity;
                    break;
                }
            }
        }

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
