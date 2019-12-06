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

    public int jumpHeight = 0;
    int dialogueProgress = 0;

    private Hurtbox hurtBox;
    private Sightbox sight;
    public AttackManager mAttackManager;
    private Entity target = null;

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

    public NPC(NPCPrototype proto) : base(proto)
    {
        prototype = proto;

        mMovingSpeed = proto.movementSpeed;
        mCollidesWith = proto.CollidesWith;
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
            MeleeAttack melee = new MeleeAttack(this, meleeAttack);
            mAttackManager.meleeAttacks.Add(melee);
            //Debug.Log("Adding Slime melee attack");
        }

        foreach (RangedAttackPrototype rangedAttack in prototype.rangedAttacks)
        {
            mAttackManager.rangedAttacks.Add(new RangedAttack(this, rangedAttack));
        }
    }

    public override void ActuallyDie()
    {
        CollisionManager.RemoveObjectFromAreas(HurtBox);
        CollisionManager.RemoveObjectFromAreas(sight);
        base.ActuallyDie();
    }

    public override void Crush()
    {
        base.Crush();
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

        int damage = (int)mHealth.LoseHP(attack.damage);
        ShowFloatingText(damage.ToString(), Color.white);


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
        base.EntityUpdate();
        mAttackManager.UpdateAttacks();
        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(Sight);
        Sight.mEntitiesInSight.Clear();
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

    public override void ShowFloatingText(string text, Color color)
    {
        base.ShowFloatingText(text, color);
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

    }

    public override string ToString()
    {
        return base.ToString();
    }

    public bool Interact(Player actor)
    {
        switch(dialogueProgress)
        {
            case 0:
                ShowFloatingText("Hello Friend", Color.white);
                break;
            case 1:
                ShowFloatingText("I Have Some Goods", Color.white);

                break;
            case 2:
                ShowFloatingText("Want To Browse My Wares?", Color.white);

                break;
        }

        dialogueProgress++;

        if (dialogueProgress >= 3)
        {
            dialogueProgress = 0;

        }
        return true;
    }
}
