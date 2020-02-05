using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityDirection { Left = -1, Right = 1 };
public enum Alignment { Player, Neutral, Enemy };

[System.Serializable]
public class Entity {

    public string Name;
    public EntityType mEntityType;
    public Hostility hostility = Hostility.Neutral;
    public List<EntityType> mCollidesWith;
    public List<StatusEffect> statusEffects;
    public float mMovingSpeed;
    public Vector2 Position;
    public EntityDirection mDirection = EntityDirection.Right;
    #region HiddenInInspector
    protected EntityPrototype prototype;
    private EntityRenderer renderer;
    private PhysicsBody body;
    private LevelManager game;
    private MapManager map;
    [HideInInspector]
    public bool mToRemove = false;
    [HideInInspector]
    public int mUpdateId = -1; //The order in the update list that the entity is updated, this matters for things like mounting
    #endregion
    protected bool isSpawned = false;
    public bool ignoreTilemap = false;

    public bool spikeProtection = false;
    public bool crushProtection = false;




    #region Accesors
    public PhysicsBody Body
    {
        get
        {
            return body;
        }

        set
        {
            body = value;
        }
    }

    public Vector3 Scale
    {
        get
        {
            //Ok this is pretty nasty, ill try and fix this
            return body.mAABB.Scale;
        }
    }

    public EntityRenderer Renderer
    {
        get
        {
            return renderer;
        }

        set
        {
            renderer = value;
        }
    }

    public LevelManager Game
    {
        get
        {
            return game;
        }

        set
        {
            game = value;
        }
    }

    public MapManager Map
    {
        get
        {
            return map;
        }

        set
        {
            map = value;
        }
    }

    public bool IsSpawned
    {
        get
        {
            return isSpawned;
        }

        set
        {
            isSpawned = value;
        }
    }

    #endregion


    public Entity(EntityPrototype proto)
    {
        prototype = proto;
        mEntityType = prototype.entityType;
        Game = LevelManager.instance;
        Map = Game.mMap;
        ignoreTilemap = proto.ignoreTilemap;
        hostility = prototype.hostility;

        //mRenderer = GameObject.Instantiate<EntityRenderer>();
        mCollidesWith = proto.CollidesWith;
        statusEffects = new List<StatusEffect>();
        //if (colorPallete != null && colorPallete.Count > 0)
        //ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        mUpdateId = Game.AddToUpdateList(this);
    }

    public virtual void Spawn(Vector2 spawnPoint)
    {
        GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/EntityRenderer")) as GameObject;
        Renderer = gameObject.GetComponent<EntityRenderer>();
        Renderer.SetEntity(this);
        Renderer.SetSprite(prototype.sprite);

        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
        }

        Position = spawnPoint + body.mOffset;
        Renderer.Sprite.sortingLayerName = prototype.sortingLayer.ToString();
        Renderer.Draw();
        Body.UpdatePosition();
        isSpawned = true;
    }

    public virtual void EntityUpdate()
    {

        foreach (IContactTrigger contact in CheckForContacts())
        {
            contact.Contact(this);
        }
        //Any way to do this earlier?
        UpdateStatusEffects();


        Body.UpdatePhysics();
        //Renderer.Draw();

        //Update the areas of the the colliders
        CollisionManager.UpdateAreas(Body);
        //After updating the areas, clear all the collisions for this frame so that we dont have any remnants from last frame
        Body.mCollisions.Clear();


        //Right now it works without doing this here, but ill leave it until im sure its fine to remove
        //mHurtBox.UpdatePosition();

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void SecondUpdate()
    {
        Body.UpdatePhysicsP2();
        
        Renderer.Draw();
        
    }

    public void UpdateStatusEffects()
    {
        List<StatusEffect> toRemove = new List<StatusEffect>();
        foreach (StatusEffect effect in statusEffects)
        {
            effect.UpdateEffect();
            if (effect.expired)
            {
                //Debug.Log(effect + " is ready to end.");
                toRemove.Add(effect);
            }
        }

        foreach (StatusEffect effect in toRemove)
        {
            statusEffects.Remove(effect);
        }

        toRemove.Clear();
    }

    public virtual void Die()
    {
        mToRemove = true;
        Body.mState = ColliderState.Closed;
    }

    public virtual void Destroy()
    {
        mToRemove = true;
        Body.mState = ColliderState.Closed;
    }

    public virtual void ActuallyDie()
    {

        //before we remove it from the update list, we have to remove it from the update areas
        CollisionManager.RemoveObjectFromAreas(Body);
        Game.RemoveFromUpdateList(this);
        if (Renderer != null)
        {
            UnityEngine.Object.Destroy(Renderer.gameObject);
        }
    }

    public virtual void Crush()
    {
        //Kinematic things cant be spiked or crushed
        if (Body.mIsKinematic || Body.mIsHeavy)
            return;

        if (this is IHurtable hurtable)
        {
            if(crushProtection)
            {
                hurtable.GetHurt(Attack.ProtectedCrushAttack());
            }
            else
            {
                hurtable.GetHurt(Attack.CrushAttack());
            }
        }
    }

    public virtual void Spiked()
    {
        //Kinematic things cant be spiked or crushed
        if (Body.mIsKinematic || Body.mIsHeavy)
            return;

        if (this is IHurtable hurtable)
        {
            if(spikeProtection)
            {
                hurtable.GetHurt(Attack.ProtectedCrushAttack());
            }
            else
            {
                hurtable.GetHurt(Attack.CrushAttack());
            }
        }
    }

    public virtual void ShowFloatingText(String text, Color color)
    {
        FloatingText floatingText = GameObject.Instantiate(Resources.Load<FloatingText>("Prefabs/UI/FloatingText") as FloatingText, Position, Quaternion.identity);
        floatingText.SetOffset(Vector3.up * (Body.mAABB.HalfSize.y * 2 + 7));
        floatingText.GetComponent<TextMesh>().text = "" + text;
        floatingText.GetComponent<TextMesh>().color = color;
    }

    public List<IContactTrigger> CheckForContacts()
    {
        List<IContactTrigger> triggers = new List<IContactTrigger>();
        //ItemObject item = null;
        for (int i = 0; i < Body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (Body.mCollisions[i].other.mEntity is IContactTrigger)
            {
                triggers.Add((IContactTrigger)Body.mCollisions[i].other.mEntity);
            }
        }

        return triggers;
    }
}






