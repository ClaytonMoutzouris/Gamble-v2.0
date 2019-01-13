using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity, IHurtable
{
    [SerializeField]
    private Hurtbox hurtBox;
    public Sightbox sight;
    public AttackManager mAttackManager;
    public Entity target = null;

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
        sight = new Sightbox(this, new CustomAABB(transform.position, new Vector2(200, 200), Vector3.zero, new Vector3(1, 1, 1)));
        sight.UpdatePosition();



        mAttackManager = GetComponent<AttackManager>();


            MeleeAttack defaultAttack = new MeleeAttack(this, .5f, 5, new Hitbox(this, new CustomAABB(transform.position, Body.mAABB.HalfSize, new Vector3(Body.mAABB.HalfSizeX, 0), new Vector3(1, 1, 1))));
        mAttackManager.AttackList.Add(defaultAttack);
        mAttackManager.meleeAttacks.Add(defaultAttack);

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
        mAttackManager.SecondUpdate();

        HurtBox.UpdatePosition();
        sight.UpdatePosition();

    }

    public void EnemyUpdate()
    {
        //First enemies check sight
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

        if (target != null)
        {
            mAttackManager.AttackList[0].Activate();
        }

        mAttackManager.UpdateAttacks();
    }

    public override void Die()
    {
        base.Die();

        HurtBox.mState = ColliderState.Closed;
    }

    public virtual void DropLoot()
    {
        ItemObject temp = Instantiate(Resources.Load<ItemObject>("Prefabs/ItemObject")) as ItemObject;
        temp.SetItem(ItemDatabase.GetRandomItem());
        temp.EntityInit();
        temp.Body.mPosition = Body.mPosition + new Vector2(0, MapManager.cTileSize / 2);
    }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(HurtBox);
        CollisionManager.RemoveObjectFromAreas(sight);

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