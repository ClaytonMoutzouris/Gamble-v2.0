using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState { Stand, Walk };

public class NPC : Entity, IHurtable, IInteractable
{
    public Health mHealth;
    protected NPCPrototype prototype;
    private Hostility hostility = Hostility.Neutral;
    public NPCState mNPCState = NPCState.Stand;
    public NPCType npcType;
    public List<string> dialogueLines;
    public int jumpHeight = 0;
    int dialogueProgress = 0;

    private Hurtbox hurtBox;
    private Sightbox sight;
    public AttackManager mAttackManager;
    private Entity target = null;
    public List<Item> npcWares;
    private string interactLabel = "<Talk>";


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

    public string InteractLabel { get => interactLabel; set => interactLabel = value; }

    public NPC(NPCPrototype proto) : base(proto)
    {
        prototype = proto;
        entityName = proto.possibleNames[Random.Range(0,proto.possibleNames.Count)];
        mMovingSpeed = proto.movementSpeed;
        mCollidesWith = proto.CollidesWith;
        npcType = proto.npcType;

        HurtBox = new Hurtbox(this, new CustomAABB(Position, prototype.bodySize, new Vector2(0, prototype.bodySize.y)));
        HurtBox.UpdatePosition();
        hostility = prototype.hostility;

        sight = new Sightbox(this, new CustomAABB(Position, new Vector2(prototype.sightRange, prototype.sightRange), new Vector2(0, prototype.bodySize.y)));
        sight.UpdatePosition();

        npcWares = new List<Item>();
        foreach (Item item in proto.wares.GetLoot())
        {
            npcWares.Add(ItemDatabase.NewItem(item));
        }


        //Stats
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

        dialogueLines = proto.dialogueLines;

        List<Color> pallete = new List<Color>();
        foreach (SwapIndex index in System.Enum.GetValues(typeof(SwapIndex)))
        {
            pallete.Add(new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
        }

        SetColorPalette(pallete);
    }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(HurtBox);
        CollisionManager.RemoveObjectFromAreas(sight);
        base.ActuallyDie();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public virtual void GetHurt(Attack attack)
    {
        //Debug.Log("Dude is getting hurt");
        if (Hostility == Hostility.Neutral)
        {
            Hostility = Hostility.Hostile;
        }

        int damage = (int)mHealth.LoseHP(attack.GetDamage());
        
        if(damage > 0)
        {
            ShowFloatingText(damage.ToString(), Color.white);
        }



        if (mHealth.currentHealth == 0)
        {
            Die();
        }
      
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

        //DropLoot();
        HurtBox.mState = ColliderState.Closed;
    }

    public override void EntityUpdate()
    {
        CheckForTargets();

        if(Target != null)
        {
            if ((Position - Target.Position).x > 0)
            {
                mDirection = EntityDirection.Left;
            }
            else
            {
                mDirection = EntityDirection.Right;

            }
        }

        base.EntityUpdate();
        mAttackManager.UpdateAttacks();
        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();
    }

    public void CheckForTargets()
    {
        this.Target = null;
        if (Sight.mEntitiesInSight != null)
        {
            foreach (Entity entity in Sight.mEntitiesInSight)
            {
                if (entity is Player)
                {
                    if (!((Player)entity).IsDead)
                    {
                        this.Target = (Player)entity;
                        break;
                    }
                }
            }
        }
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        mAttackManager.SecondUpdate();

        HurtBox.UpdatePosition();
        sight.UpdatePosition();
    }

    public override void Spawn(Vector2 spawnPoint)
    {
        base.Spawn(spawnPoint);
        Renderer.SetSprite(prototype.sprite);
        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }
        SetColorPalette();
        HurtBox.UpdatePosition();
        sight.UpdatePosition();
        //Renderer.SetSprite(prototype.)

    }

    public override string ToString()
    {
        return base.ToString();
    }

    public bool Interact(Player actor)
    {

        ShowFloatingText(dialogueLines[dialogueProgress], Color.white, 2.0f, 0, 1.5f);
        dialogueProgress++;

        if (dialogueProgress > dialogueLines.Count-1)
        {
            ShopScreenUI.instance.Open(actor, this);

            dialogueProgress = 0;

        }


        return true;
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
