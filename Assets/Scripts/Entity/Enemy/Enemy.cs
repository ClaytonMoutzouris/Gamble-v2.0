using UnityEngine;
public enum Hostility { Friendly, Neutral, Hostile };
public enum TargetRange { Close, Near, Far, OutOfRange };
public class Enemy : Entity, IHurtable
{
    public EnemyState mEnemyState = EnemyState.Moving;
    public EnemyType mEnemyType;
    public Health mHealth;
    EnemyPrototype prototype;
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



    public Enemy(EnemyPrototype proto) : base()
    {

        prototype = proto;
        mEntityType = EntityType.Enemy;

        mEnemyType = prototype.enemyType;
        mMovingSpeed = proto.movementSpeed;

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
        mHealth = new Health(prototype.health);

        mAttackManager = new AttackManager(this);

        //Debug.Log("Melee Attacks: " + prototype.meleeAttacks.Count);
        foreach (MeleeAttackPrototype meleeAttack in prototype.meleeAttacks)
        {
        MeleeAttack melee = new MeleeAttack(this, meleeAttack.duration, meleeAttack.damage, meleeAttack.cooldown, new Hitbox(this, new CustomAABB(Position, meleeAttack.hitboxSize, meleeAttack.hitboxOffset)));
        mAttackManager.meleeAttacks.Add(melee);
        //Debug.Log("Adding Slime melee attack");
        }

        foreach (RangedAttackPrototype rangedAttack in prototype.rangedAttacks)
        {
        mAttackManager.rangedAttacks.Add(new RangedAttack(this, rangedAttack.duration, rangedAttack.damage, rangedAttack.cooldown, rangedAttack.projectile));
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

    }



    public override void Die()
    {
        base.Die();
        DropLoot();
        HurtBox.mState = ColliderState.Closed;
    }

    public virtual void DropLoot()
    {
        ItemObject temp = new ItemObject(ItemDatabase.GetRandomItem());
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

        int damage = (int)mHealth.LoseHP(attack.damage);
        ShowFloatingText(damage, Color.white);


        if (mHealth.currentHealth == 0)
        {
            Die();
        }
      
    }


}