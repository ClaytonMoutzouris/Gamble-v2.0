using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Hostility { Friendly, Neutral, Hostile };
public enum TargetRange { Close, Near, Far, OutOfRange };
public abstract class Enemy : Entity, IHurtable
{
    public EnemyState mEnemyState = EnemyState.Idle;
    public EnemyType mEnemyType;

    //Behaviour
    [SerializeField]
    private Hostility hostility = Hostility.Neutral;

    //End of Behaviour

    private Hurtbox hurtBox;
    private Sightbox sight;
    [HideInInspector]
    public AttackManager mAttackManager;

    [SerializeField]
    private Entity target = null;

    [HideInInspector]
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

    public Entity Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    public Hostility Hostility
    {
        get
        {
            return hostility;
        }

        set
        {
            hostility = value;
        }
    }

    

    public Sightbox Sight
    {
        get
        {
            return sight;
        }
        set
        {
            sight = value;
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

        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, BodySize, Vector3.zero, new Vector3(1, 1, 1)));
        HurtBox.UpdatePosition();


        mStats = GetComponent<Stats>();
        sight = new Sightbox(this, new CustomAABB(transform.position, new Vector2(200, 200), Vector3.zero, new Vector3(1, 1, 1)));
        sight.UpdatePosition();


        EnemyHealthBar temp = Instantiate(Resources.Load<EnemyHealthBar>("Prefabs/UI/EnemyHealthBar"), transform) as EnemyHealthBar;
        temp.transform.localPosition = new Vector3(0, BodySize.y * 2 + 10);
        temp.InitHealthbar(this);
        mStats.health.healthbar = temp;
        
        mStats.health.healthbar.SetHealth(mStats.health);

        mAttackManager = GetComponent<AttackManager>();

        MeleeAttack defaultAttack = new MeleeAttack(this, 1f, 5, 2f, Range.Close, new Hitbox(this, new CustomAABB(transform.position, Body.mAABB.HalfSize, new Vector3(Body.mAABB.HalfSizeX, 0), new Vector3(1, 1, 1))));
        mAttackManager.AttackList.Add(defaultAttack);
        mAttackManager.meleeAttacks.Add(defaultAttack);
    }

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
        mAttackManager.UpdateAttacks();
    }



    public override void Die()
    {
        base.Die();
        DropLoot();
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


        base.ActuallyDie();
    }

    public virtual void GetHurt(Attack attack)
    {
        //Debug.Log("Dude is getting hurt");
        if (Hostility == Hostility.Neutral)
        {
            Hostility = Hostility.Hostile;
        }

        int damage = (int)mStats.health.LoseHP(attack.damage);
        ShowFloatingText(damage, Color.white);

        Debug.Log("Current health: " + mStats.health.currentHealth + " damage");

        if (mStats.health.currentHealth == 0)
        {
            Die();
        }
      
    }


}