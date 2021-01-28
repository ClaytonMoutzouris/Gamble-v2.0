using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityDirection { Left = -1, Right = 1 };
public enum Alignment { Player, Neutral, Enemy };
public enum AbilityFlag { CrushProtection, SpikeProtection, Hover, Heavy, Sturdy }
[System.Serializable]
public class Entity {

    public string entityName;
    public EntityType mEntityType;
    public Hostility hostility = Hostility.Neutral;
    public List<EntityType> mCollidesWith;
    public List<StatusEffect> statusEffects;

    public float mMovingSpeed;
    public int crushDamage = 999;
    public Vector2 Position;
    public EntityDirection mDirection = EntityDirection.Right;
    #region HiddenInInspector
    protected EntityPrototype prototype;
    private EntityRenderer renderer;
    private PhysicsBody body;
    private GameManager game;
    private MapManager map;
    [HideInInspector]
    public bool mToRemove = false;
    [HideInInspector]
    public int mUpdateId = -1; //The order in the update list that the entity is updated, this matters for things like mounting
    #endregion
    protected bool isSpawned = false;
    public bool ignoreTilemap = false;
    public AbilityFlags abilityFlags;
    public List<Ability> abilities;
    public Stats mStats;

    public Vector2 gravityVector = Vector2.down;
    public float gravityMultiplier = 1;
    public float baseGravityMultiplier = 1;

    public AudioClip instadeathSFX;

    public struct AbilityFlags
    {

        Dictionary<AbilityFlag, bool> abilityFlags;


        public void Initialize()
        {
            abilityFlags = new Dictionary<AbilityFlag, bool>();

            foreach(AbilityFlag ability in System.Enum.GetValues(typeof(AbilityFlag)))
            {
                abilityFlags.Add(ability, false);
            }
        }

        public bool GetFlag(AbilityFlag ability)
        {
            return abilityFlags[ability];
        }

        public void SetFlag(AbilityFlag ability, bool value)
        {
            abilityFlags[ability] = value;
        }
    }


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

    public GameManager Game
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
        prototype = ScriptableObject.Instantiate(proto);
        mEntityType = prototype.entityType;
        entityName = proto.mName;
        Game = GameManager.instance;
        Map = MapManager.instance;
        ignoreTilemap = proto.ignoreTilemap;
        hostility = prototype.hostility;
        abilities = new List<Ability>();
        
        //mRenderer = GameObject.Instantiate<EntityRenderer>();
        mCollidesWith = proto.CollidesWith;
        statusEffects = new List<StatusEffect>();
        abilityFlags = new AbilityFlags();
        abilityFlags.Initialize();
        baseGravityMultiplier = proto.baseGravityMultiplier;
        gravityMultiplier = baseGravityMultiplier;
        crushDamage = proto.crushDamage;
        //if (colorPallete != null && colorPallete.Count > 0)
        //ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);
        foreach(AbilityFlag flag in proto.abilityFlags)
        {
            abilityFlags.SetFlag(flag, true);

        }

        foreach(Ability ability in proto.baseAbilities)
        {
            Ability temp = ScriptableObject.Instantiate(ability);
            temp.OnGainTrigger(this);
        }

        mUpdateId = Game.AddToUpdateList(this);

        Body = new PhysicsBody(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.y)));
        Body.mIsKinematic = proto.kinematic;
        Body.mIgnoresGravity = proto.ignoreGravity;
        List<Color> palette = proto.colorPallete;
        //Stats
        mStats = new Stats();
        mStats.SetStats(prototype.stats);

        instadeathSFX = Resources.Load<AudioClip>("Sounds/SFX/splatter");
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
        if(prototype.particleEffects != null)
        {
            Renderer.AddVisualEffect(prototype.particleEffects, prototype.bodySize*Vector2.up);
        }

        Body.UpdatePosition();
        isSpawned = true;
        foreach (ColorSwapNode node in prototype.colorNodes)
        {
            node.trueColour = node.baseColour;
        }
        Renderer.colorSwapper.SwapColors(prototype.colorNodes);

    }

    public virtual void EntityUpdate()
    {

        foreach (IContactTrigger contact in CheckForContacts())
        {
            contact.Contact(this);
        }
        //Any way to do this earlier?
        UpdateStatusEffects();

        foreach (Ability effect in abilities)
        {
            effect.OnUpdate();
        }

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
        foreach (Ability effect in abilities)
        {
            effect.OnSecondUpdate();
        }
        Renderer.Draw();
        
    }

    public void UpdateStatusEffects()
    {
        List<StatusEffect> toRemove = new List<StatusEffect>();
        foreach (StatusEffect effect in statusEffects)
        {
            effect.UpdateEffect();
            if (effect.Expired)
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

        List<Ability> tempAbilities = new List<Ability>();
        tempAbilities.AddRange(abilities);
 
        foreach (Ability effect in tempAbilities)
        {
            effect.OnOwnerDeath(this);
        }

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

    public virtual void Crush(Entity crusher = null)
    {
        //Kinematic things cant be spiked or crushed
        if (Body.mIsKinematic || abilityFlags.GetFlag(AbilityFlag.Heavy))
            return;


        if (this is IHurtable hurtable)
        {
            if (crusher != null)
            {
                if (abilityFlags.GetFlag(AbilityFlag.CrushProtection))
                {
                    hurtable.GetHurt(Attack.ProtectedCrushAttack());
                }
                else
                {
                    hurtable.GetHurt(new Attack(crusher.crushDamage));

                }

            }
            else
            {
                if (abilityFlags.GetFlag(AbilityFlag.CrushProtection))
                {
                    hurtable.GetHurt(Attack.ProtectedCrushAttack());
                } else
                {
                    hurtable.GetHurt(Attack.CrushAttack());
                }
            }
        }
    }
    

    public virtual void Spiked()
    {
        //Kinematic things cant be spiked or crushed
        if (Body.mIsKinematic || abilityFlags.GetFlag(AbilityFlag.Heavy))
            return;

        if (this is IHurtable hurtable)
        {
            if (abilityFlags.GetFlag(AbilityFlag.SpikeProtection))
            {
                hurtable.GetHurt(Attack.ProtectedCrushAttack());
            }
            else
            {
                hurtable.GetHurt(Attack.CrushAttack());
                SoundManager.instance.PlaySingle(instadeathSFX);
            }
        }
    }

    public virtual void Lava()
    {
        //everything dies to lava
        if (this is IHurtable hurtable)
        {

            hurtable.GetHurt(Attack.CrushAttack());
            
        }
    }

    public void SetColorPalette(List<Color> palette)
    {
        prototype.colorPallete = palette;
        if (Renderer != null)
        {
            Renderer.colorSwap.SetBaseColors(prototype.colorPallete);
        }

    }

    public void SetColorPalette()
    {
        if (Renderer != null)
        {
            Renderer.colorSwap.SetBaseColors(prototype.colorPallete);
        }

    }

    //Function for deriving the speed value from the speed stat
    public virtual float GetSpeed()
    {
        return 0;
    }

    public virtual void ShowFloatingText(string text, Color color, float dTime = 1, float sSpeed = 15, float sizeMult = 1.0f)
    {
        FloatingText floatingText = GameObject.Instantiate(Resources.Load<FloatingText>("Prefabs/UI/FloatingText") as FloatingText, Position, Quaternion.identity);
        floatingText.SetOffset(Vector3.up * (Body.mAABB.HalfSize.y * 2 + 10));
        floatingText.text.characterSize = floatingText.text.characterSize * sizeMult;
        floatingText.duration = dTime;
        floatingText.scrollSpeed = sSpeed;
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






