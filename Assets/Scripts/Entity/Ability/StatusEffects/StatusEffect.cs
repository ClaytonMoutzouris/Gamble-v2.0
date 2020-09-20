using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public Entity effectedEntity;
    public StatusEffectType type;
    //base duration is 1 second
    double effectDuration = 1;
    double elapsed = 0;
    double timeStamp;
    int framesElapsed = 0;
    public bool expired = false;
    bool stackable = false;
    protected ParticleSystem effectPrefab;
    public GameObject effectObject;
    public Vector2 effectOffset = Vector2.zero;


    public double Elapsed { get => elapsed; set => elapsed = value; }

    public StatusEffect()
    {

        Elapsed = 0f;
        timeStamp = Time.time;

    }

    public virtual void OnApplyEffect(Entity effected, float duration = 5)
    {
        effectDuration = duration;
        effectedEntity = effected;

        effectedEntity.statusEffects.Add(this);
        if(effectPrefab != null)
        {
            effectObject = effectedEntity.Renderer.AddVisualEffect(effectPrefab, effectOffset);
        }
    }

    public virtual void UpdateEffect()
    {
        Elapsed += Time.deltaTime;
        //Debug.Log()
        //Debug.Log(Time.timeScale);
        //framesElapsed++;
        //Debug.Log(type + " frames elapsed " + framesElapsed);


        if (Elapsed >= effectDuration)
        {
            OnEffectEnd();
        }
    }

    public virtual void OnEffectEnd()
    {
        effectedEntity.Renderer.RemoveVisualEffect(effectObject);

        //effectedEntity.statusEffects.Remove(this);
        expired = true;
    }

    public static StatusEffect GetEffectFromType(StatusEffectType type)
    {
        StatusEffect statusEffect = null;
        switch(type)
        {
            case StatusEffectType.Poisoned:
                statusEffect = new Poisoned();
                break;
            case StatusEffectType.Stunned:
                statusEffect = new Stunned();
                break;
            case StatusEffectType.Frozen:
                statusEffect = new Stunned();
                break;
            case StatusEffectType.Burned:
                statusEffect = new Poisoned();
                break;
            case StatusEffectType.Bleeding:
                statusEffect = new Bleeding();
                break;
        }

        return statusEffect;
    }
}

public class Stunned : StatusEffect
{

    public Stunned() : base()
    {
        type = StatusEffectType.Stunned;
        effectPrefab = Resources.Load<ParticleSystem>("ParticleEffects/StunEffect");

    }

    public override void OnApplyEffect(Entity effected, float duration = 3)
    {
        //If the entity is already stunned, just refresh the stun duration
        foreach (StatusEffect sEffect in effected.statusEffects)
        {
            if (sEffect is Stunned)
            {
                sEffect.Elapsed = 0;
                return;
            }
        }

        effectOffset = effected.Body.mAABB.HalfSizeY * 2 * Vector2.up;


        base.OnApplyEffect(effected, duration);

        effectedEntity.Body.mSpeed = Vector2.zero;

        
    }

    public override void UpdateEffect()
    {
        effectedEntity.Body.mSpeed = Vector2.zero;

        base.UpdateEffect();
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
        
    }
}


public class Poisoned : StatusEffect
{

    float tickFrequency = 1;
    float tickTimer = 0;
    int tickDamage = 1;
    Attack attack;

    public Poisoned(int damage = 1) : base()
    {
        type = StatusEffectType.Poisoned;
        tickDamage = damage;
        attack = new Attack(tickDamage);
        effectPrefab = Resources.Load<ParticleSystem>("ParticleEffects/PoisonEffect");
        
    }

    public override void OnApplyEffect(Entity effected, float duration = 10)
    {
        //If the effect isnt stackable and the entity is already effected by the same effect
        //we refresh the timer and do not add a new one
        foreach(StatusEffect sEffect in effected.statusEffects)
        {
            if(sEffect is Poisoned)
            {
                sEffect.Elapsed = 0;
                return;
            }
        }
        effectOffset = effected.Body.mAABB.HalfSizeY * Vector2.up;

        base.OnApplyEffect(effected, duration);

    }

    public override void UpdateEffect()
    {
        tickTimer += Time.deltaTime;

        if(tickTimer >= tickFrequency)
        {
            OnTickTrigger();
        }
        base.UpdateEffect();
    }

    public void OnTickTrigger()
    {
        tickTimer = 0;
        
        if(effectedEntity is IHurtable hurtable)
        {
            hurtable.GetHurt(attack);
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
    }
}

/*
public class KnockedBack : StatusEffect
{

    public KnockedBack() : base()
    {
        type = StatusEffectType.KnockedBack;
        //effectPrefab = Resources.Load<ParticleSystem>("ParticleEffects/StunEffect");
        //effectOffset = effectedEntity.Body.mAABB.HalfSizeY * 2 * Vector2.up;
    }

    public override void OnApplyEffect(Entity effected, float duration = 3)
    {
        //If the entity is already stunned, just refresh the stun duration
        foreach (StatusEffect sEffect in effectedEntity.statusEffects)
        {
            if (sEffect is KnockedBack)
            {
                sEffect.Elapsed = 0;
                return;
            }
        }

        if(effected is Enemy enemy)
        {
            enemy.mEnemyState = EnemyState.Jumping;

        }

        Vector2 posDifference = Vector2.right*(int)effected.mDirection;

        effected.Body.mSpeed = ((int)effected.mDirection*Vector2.left+Vector2.up*0.5f)*500;

        base.OnApplyEffect(effected, duration);
        effectedEntity.Body.mSpeed = Vector2.zero;


    }

    public override void UpdateEffect()
    {
        effectedEntity.Body.mSpeed = Vector2.zero;

        base.UpdateEffect();
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();

    }
}
*/

public class Bleeding : StatusEffect
{

    float tickFrequency = 2;
    float tickTimer = 0;
    int tickDamage = 2;
    Attack attack;

    public Bleeding(int damage = 2) : base()
    {
        type = StatusEffectType.Bleeding;
        tickDamage = damage;
        attack = new Attack(tickDamage);
        effectPrefab = Resources.Load<ParticleSystem>("ParticleEffects/BleedEffect");
        
    }

    public override void OnApplyEffect(Entity effected, float duration = 10)
    {
        //If the effect isnt stackable and the entity is already effected by the same effect
        //we refresh the timer and do not add a new one
        foreach(StatusEffect sEffect in effected.statusEffects)
        {
            if(sEffect is Bleeding)
            {
                sEffect.Elapsed = 0;
                return;
            }
        }
        effectOffset = effected.Body.mAABB.HalfSizeY * Vector2.up;

        base.OnApplyEffect(effected, duration);

    }

    public override void UpdateEffect()
    {
        tickTimer += Time.deltaTime;

        if(tickTimer >= tickFrequency)
        {
            OnTickTrigger();
        }
        base.UpdateEffect();
    }

    public void OnTickTrigger()
    {
        tickTimer = 0;
        
        if(effectedEntity is IHurtable hurtable)
        {
            hurtable.GetHurt(attack);
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
    }
}