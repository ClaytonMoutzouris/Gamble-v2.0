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
    //Behaviour
    [SerializeField]
    private Hostility hostility = Hostility.Neutral;

    //End of Behaviour

    private Hurtbox hurtBox;
    private Sightbox sight;
    [HideInInspector]
    public AttackManager mAttackManager;

    [SerializeField]
    private Player target = null;

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

    public Player Target
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


        HurtBox = new Hurtbox(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, prototype.bodySize.y)));
        HurtBox.UpdatePosition();
        hostility = prototype.hostility;

        sight = new Sightbox(this, new CustomAABB(Position, new Vector2(prototype.sightRange, prototype.sightRange), new Vector2(0, prototype.bodySize.y)));
        sight.UpdatePosition();



        //Stats
        mStats = new Stats(this);
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
        EnemyHealthBar temp = GameObject.Instantiate(Resources.Load<EnemyHealthBar>("Prefabs/UI/EnemyHealthBar"), Renderer.transform) as EnemyHealthBar;
        temp.transform.localPosition = new Vector3(0, Body.mAABB.HalfSizeY * 2 + 10);
        temp.InitHealthbar(this);
        mHealth.healthbar = temp;
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
        ItemObject temp = new ItemObject(ItemDatabase.NewItem(prototype.lootTable[Random.Range(0, prototype.lootTable.Count)]), Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
        temp.Spawn(Position + new Vector2(0, MapManager.cTileSize / 2));
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

        int damage = (int)mHealth.LoseHP(attack.GetDamage());
        ShowFloatingText(damage.ToString(), Color.white);


        if (mHealth.currentHealth == 0)
        {
            Die();
        }
      
    }


}