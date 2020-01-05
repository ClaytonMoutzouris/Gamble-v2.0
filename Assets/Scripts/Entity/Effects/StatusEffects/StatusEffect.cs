using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
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

    public StatusEffect(Entity effected, float duration = 5)
    {
        effectDuration = duration;
        effectedEntity = effected;
        Elapsed = 0f;
        timeStamp = Time.time;

        //Calling this in the base and the derived class causes double application
        //OnApplyEffect();
    }

    public virtual void OnApplyEffect()
    {
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

}

public class Stunned : StatusEffect
{

    public Stunned(Entity effected, float duration = 3) : base(effected, duration)
    {
        type = StatusEffectType.Stunned;
        effectPrefab = Resources.Load<ParticleSystem>("ParticleEffects/StunEffect");
        effectOffset = effectedEntity.Body.mAABB.HalfSizeY * 2 * Vector2.up;
        OnApplyEffect();
    }

    public override void OnApplyEffect()
    {
        foreach (StatusEffect sEffect in effectedEntity.statusEffects)
        {
            if (sEffect is Stunned)
            {
                sEffect.Elapsed = 0;
                return;
            }
        }

        base.OnApplyEffect();
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

    float tickFrequency = 2;
    float tickTimer = 0;
    int tickDamage = 1;
    Attack attack;

    public Poisoned(Entity effected, float duration = 10, int damage = 1) : base(effected, duration)
    {
        type = StatusEffectType.Poisoned;
        tickDamage = damage;
        attack = new Attack(tickDamage);
        effectPrefab = Resources.Load<ParticleSystem>("ParticleEffects/PoisonEffect");
        effectOffset =effectedEntity.Body.mAABB.HalfSizeY * Vector2.up;
        OnApplyEffect();
        
    }

    public override void OnApplyEffect()
    {
        //If the effect isnt stackable and the entity is already effected by the same effect
        //we refresh the timer and do not add a new one
        foreach(StatusEffect sEffect in effectedEntity.statusEffects)
        {
            if(sEffect is Poisoned)
            {
                sEffect.Elapsed = 0;
                return;
            }
        }

        base.OnApplyEffect();

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