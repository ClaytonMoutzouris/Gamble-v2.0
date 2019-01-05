using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity, IHurtable
{
    [SerializeField]
    private Hurtbox hurtBox;
    public Sightbox sight;


    public EnemyType mEnemyType;

    public Stats mStats;

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

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (mStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Body.mAABB.Center + (Body.mAABB.HalfSizeY + 3) * Vector3.up, new Vector3(30 * (mStats.health.currentHealth / mStats.health.maxHealth), 6, 1));
        }

        if (sight != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sight.mAABB.Center, sight.mAABB.HalfSize.x);
        }
    }

    public virtual void EnemyInit()
    {
        mStats = GetComponent<Stats>();
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
        sight.UpdatePosition();

    }

    public override void Die()
    {
        base.Die();

        HurtBox.mState = ColliderState.Closed;
        //HurtBox.mCollisions.Clear();
    }

    public virtual void DropLoot()
    {
        ItemObject temp = Instantiate(ItemDatabase.GetRandomItem());
        temp.EntityInit();
        temp.Body.mPosition = Body.mPosition + new Vector2(0, MapManager.cTileSize / 2);
    }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(HurtBox);
        DropLoot();

        base.ActuallyDie();
    }

    public virtual void GetHurt(Attack attack)
    {
        Debug.Log("Dude is getting hurt");

            mStats.health.LoseHP(attack.damage);
        Debug.Log("Current health: " + mStats.health.currentHealth + " damage");

        if (mStats.health.currentHealth == 0)
            {
                Die();
            }

        
    }
}