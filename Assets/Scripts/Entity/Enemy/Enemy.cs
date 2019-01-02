using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity, IHurtable
{
    [SerializeField]
    private Hurtbox hurtBox;

    public EnemyType mEnemyType;

    public Hurtbox HurtBox
    {
        get
        {
            return hurtBox;
        }

        set
        {
            hurtBox = value;
        }
    }

    /*
    public virtual void EntityInit()
    {
        base.EntityInit();
    }
    */

    public override void SecondUpdate()
    {
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();

        HurtBox.UpdatePosition();

    }

    public override void Die()
    {
        base.Die();

        HurtBox.mState = ColliderState.Closed;
        //HurtBox.mCollisions.Clear();
    }

    public virtual void GetHurt(Attack attack)
    {
        Debug.Log(name + " was hurt by " + attack.mEntity.name);
        Die();
    }
}