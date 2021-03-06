﻿using UnityEngine;
public enum Hostility { Friendly, Neutral, Hostile };
public enum TargetRange { Close, Near, Far, OutOfRange };
public class Enemy : Entity, IHurtable
{
    public EnemyState mEnemyState = EnemyState.Moving;
    public EnemyType mEnemyType;
    public Health mHealth;
    protected EnemyPrototype prototype;
    public int jumpHeight = 0;
    public int ExpValue = 5;
    //Behaviour
    //End of Behaviour
    public Nest nestParent = null;
    private Hurtbox hurtBox;
    private Sightbox sight;
    [HideInInspector]
    public AttackManager mAttackManager;

    [SerializeField]
    private Entity target = null;


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



    public Enemy(EnemyPrototype proto) : base(proto)
    {

        prototype = proto;
        ExpValue = proto.expValue;
        mEnemyType = prototype.enemyType;
        mMovingSpeed = proto.movementSpeed;
        jumpHeight = proto.jumpHeight;

        mCollidesWith.Add(EntityType.Player);
        mCollidesWith.Add(EntityType.Obstacle);
        mCollidesWith.Add(EntityType.Platform);

        Body.mIgnoresGravity = proto.ignoreGravity;

        HurtBox = new Hurtbox(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, prototype.bodySize.y)));
        HurtBox.UpdatePosition();

        sight = new Sightbox(this, new CustomAABB(Position, new Vector2(prototype.sightRange, prototype.sightRange), new Vector2(0, prototype.bodySize.y)));
        sight.UpdatePosition();



        ScaleStatsToLevel();
        mHealth = new Health(this, prototype.health);
        


        mAttackManager = new AttackManager(this);

        //Debug.Log("Melee Attacks: " + prototype.meleeAttacks.Count);
        foreach (MeleeAttackPrototype meleeAttack in prototype.meleeAttacks)
        {
        MeleeAttack melee = new MeleeAttack(this, meleeAttack);
        mAttackManager.meleeAttacks.Add(melee);
        //Debug.Log("Adding Slime melee attack");
        }

        foreach (RangedAttackPrototype rangedAttack in prototype.rangedAttacks)
        {
        mAttackManager.rangedAttacks.Add(new RangedAttack(this, rangedAttack));
        }

        
    }

    public virtual void ScaleStatsToLevel()
    {
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            
            mStats.GetStat(type).value += WorldManager.instance.NumCompletedWorlds()*2;
        }

    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
        Renderer.SetSprite(prototype.sprite);
        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }
        HurtBox.UpdatePosition();
        sight.UpdatePosition();

        //Renderer.SetSprite(prototype.)
        if(!(this is BossEnemy) && !(this is EnemyPart))
        {
            EnemyHealthBar temp = GameObject.Instantiate(Resources.Load<EnemyHealthBar>("Prefabs/UI/EnemyHealthBar"), Renderer.transform) as EnemyHealthBar;
            temp.transform.localPosition = new Vector3(0, Body.mAABB.HalfSizeY * 2 + 20);
            temp.InitHealthbar(this);
            mHealth.healthbar = temp;
        }

    }


    public override void SecondUpdate()
    {
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();
        mAttackManager.SecondUpdate();

        HurtBox.UpdatePosition();
        sight.UpdatePosition();

    }


    public override void EntityUpdate()
    {
        base.EntityUpdate();
        mAttackManager.UpdateAttacks();
        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();

    }



    public override void Die()
    {
        if (mToRemove)
        {
            return;
        }


        CrewManager.instance.GainPartyEXP(ExpValue + ExpValue* WorldManager.instance.NumCompletedWorlds());
        base.Die();
        foreach(Attack attack in mAttackManager.meleeAttacks)
        {
            attack.Deactivate();
        }

        foreach (Attack attack in mAttackManager.rangedAttacks)
        {
            attack.Deactivate();
        }

        DropLoot();
        HurtBox.mState = ColliderState.Closed;
    }

    public virtual void DropLoot()
    {
        if(prototype.lootTable == null)
        {
            return;
        }

        foreach (Item item in prototype.lootTable.GetLoot())
        {
            ItemObject temp = new ItemObject(ItemDatabase.NewItem(item), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
            temp.Spawn(Position + new Vector2(0, MapManager.cTileSize / 2));
        }
    }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(HurtBox);
        CollisionManager.RemoveObjectFromAreas(sight);
        if (nestParent != null)
        {
            nestParent.spawns.Remove(this);
        }
        base.ActuallyDie();
    }

    public virtual void SetHostility(Hostility value)
    {
        Hostility = value;

    }

    public virtual void GetHurt(Attack attack)
    {
        if (Random.Range(0, 100) < 5 + mStats.GetSecondaryStat(SecondaryStatType.DodgeChance).GetValue())
        {
            ShowFloatingText("Missed", Color.grey);
            return;
        }
        



        //Debug.Log("Dude is getting hurt");
        if (Hostility == Hostility.Neutral)
        {
            SetHostility(Hostility.Hostile);
        }

        if(attack.mEntity != null && attack.mEntity is IHurtable)
        {
            Target = attack.mEntity;
        }

        int damage = attack.GetDamage();

        //Take 5% less damage for each point of defense
        //Limit defense to 80% reduction
        damage -= (int)(damage * Mathf.Min(0.8f, 0.01f * mStats.GetSecondaryStat(SecondaryStatType.DamageReduction).GetValue()));

        if (damage <= 0)
        {
            damage = 0;
        }
        else
        {
            //Crits
            if (attack.mEntity != null && Random.Range(0, 100) < attack.mEntity.mStats.GetSecondaryStat(SecondaryStatType.CritChance).GetValue())
            {
                damage *= 2;
                ShowFloatingText(damage.ToString(), Color.yellow, 2, 20, 2);
            }
            else
            {
                ShowFloatingText(damage.ToString(), attack.GetColorForDamageType());
            }

            mHealth.LoseHP(damage);

            foreach (Ability effect in abilities)
            {
                effect.OnDamagedTrigger(attack);
            }
        }




        if (mHealth.currentHealth == 0)
        {
            Die();
        }

    }

    public float GetMovementSpeed()
    {
        return mMovingSpeed + mMovingSpeed * 0.01f * mStats.GetSecondaryStat(SecondaryStatType.MoveSpeedBonus).GetValue();
    }

    public Entity GetEntity()
    {
        return this;
    }

    public void GainLife(int health, bool fromTrigger)
    {
        int life = (int)this.mHealth.GainHP(health);
        ShowFloatingText(life.ToString(), Color.green);

        //heals from triggers shouldnt also trigger
        if (!fromTrigger)
        {
            foreach (Ability effect in abilities)
            {
                effect.OnHealTrigger(this, life);
            }
        }
    }
}