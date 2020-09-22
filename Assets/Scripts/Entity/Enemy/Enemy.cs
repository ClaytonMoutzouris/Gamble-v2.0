using UnityEngine;
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



    public Enemy(EnemyPrototype proto) : base(proto)
    {

        prototype = proto;

        mEnemyType = prototype.enemyType;
        mMovingSpeed = proto.movementSpeed;
        jumpHeight = proto.jumpHeight;

        mCollidesWith.Add(EntityType.Player);
        mCollidesWith.Add(EntityType.Obstacle);
        mCollidesWith.Add(EntityType.Platform);

        Body = new PhysicsBody(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, prototype.bodySize.y)));
        Body.mIgnoresGravity = proto.ignoreGravity;

        HurtBox = new Hurtbox(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, prototype.bodySize.y)));
        HurtBox.UpdatePosition();

        sight = new Sightbox(this, new CustomAABB(Position, new Vector2(prototype.sightRange, prototype.sightRange), new Vector2(0, prototype.bodySize.y)));
        sight.UpdatePosition();



        //Stats
        mStats = new Stats(this);
        mStats.SetStats(prototype.stats);
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

        Debug.Log("Enemy Scaled to level " + WorldManager.instance.NumCompletedWorlds());
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
        if(!(this is BossEnemy))
        {
            EnemyHealthBar temp = GameObject.Instantiate(Resources.Load<EnemyHealthBar>("Prefabs/UI/EnemyHealthBar"), Renderer.transform) as EnemyHealthBar;
            temp.transform.localPosition = new Vector3(0, Body.mAABB.HalfSizeY * 2 + 10);
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


        base.ActuallyDie();
    }

    public virtual void SetHostility(Hostility value)
    {
        Hostility = value;

    }

    public virtual void GetHurt(Attack attack)
    {
        if (Random.Range(0, 100) < 5 + mStats.GetStat(StatType.Luck).value)
        {
            ShowFloatingText("Missed", Color.blue);
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
        //Take 1 less damage for each point of defense
        damage -= mStats.GetStat(StatType.Defense).GetValue();
        if (damage < 0)
        {
            damage = 0;
        }
        mHealth.LoseHP(damage);
        ShowFloatingText(damage.ToString(), Color.white);

        if (mHealth.currentHealth == 0)
        {
            Die();
        }

    }

    public float GetMovementSpeed()
    {
        return mMovingSpeed + 10 * mStats.GetStat(StatType.Speed).GetValue();
    }

    public Entity GetEntity()
    {
        return this;
    }
}