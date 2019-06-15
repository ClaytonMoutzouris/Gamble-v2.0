using UnityEngine;
public enum Hostility { Friendly, Neutral, Hostile };
public enum TargetRange { Close, Near, Far, OutOfRange };
public class Enemy : Entity, IHurtable
{
    public EnemyState mEnemyState = EnemyState.Idle;
    public EnemyType mEnemyType;
    public Health mHealth;
    [SerializeField]
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

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (sight != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sight.mAABB.Center, sight.mAABB.HalfSize.x);
        }
    }

    public virtual void EnemyInit()
    {

        mAnimator = GetComponent<Animator>();
        mEnemyType = prototype.enemyType;

        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, prototype.bodySize, Vector3.zero, new Vector3(1, 1, 1)));
        HurtBox.UpdatePosition();
        hostility = prototype.hostility;

        sight = new Sightbox(this, new CustomAABB(transform.position, new Vector2(prototype.sightRange, prototype.sightRange), Vector3.zero, new Vector3(1, 1, 1)));
        sight.UpdatePosition();


        EnemyHealthBar temp = Instantiate(Resources.Load<EnemyHealthBar>("Prefabs/UI/EnemyHealthBar"), transform) as EnemyHealthBar;
        temp.transform.localPosition = new Vector3(0, Body.mAABB.HalfSizeY * 2 + 10);
        temp.InitHealthbar(this);

        //Stats
        mStats = new Stats(this);
        mHealth = new Health(prototype.health, temp);

        mAttackManager = GetComponent<AttackManager>();

        foreach(AttackPrototype attack in prototype.attacks)
        {
            if(attack is MeleeAttackPrototype)
            {
                MeleeAttackPrototype meleeAttack = (MeleeAttackPrototype)attack;
                MeleeAttack melee = new MeleeAttack(this, meleeAttack.duration, meleeAttack.damage, meleeAttack.cooldown, new Hitbox(this, new CustomAABB(transform.position, meleeAttack.hitboxSize, meleeAttack.hitboxOffset, new Vector3(1, 1, 1))));
                mAttackManager.AttackList.Add(melee);
                mAttackManager.meleeAttacks.Add(melee);

            }
            else if (attack is RangedAttackPrototype)
            {
                RangedAttackPrototype rangedAttack = (RangedAttackPrototype)attack;
                mAttackManager.AttackList.Add(new RangedAttack(this, rangedAttack.duration, rangedAttack.damage, rangedAttack.cooldown, rangedAttack.projectile));
            }
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

        int damage = (int)mHealth.LoseHP(attack.damage);
        ShowFloatingText(damage, Color.white);

        Debug.Log("Current health: " + mHealth.currentHealth + " damage");

        if (mHealth.currentHealth == 0)
        {
            Die();
        }
      
    }


}