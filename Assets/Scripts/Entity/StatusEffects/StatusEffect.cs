using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public Entity effected;
    public StatusEffectType type;
    float duration = 1;
    float elapsed = 0;

    public StatusEffect(Entity effected, float duration)
    {
        this.duration = duration;

        //Apply the effect
        //Apply(effected);
    }

    public virtual void UpdateEffect()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= duration)
        {
            RemoveEffect();
        }
    }

    public virtual void RemoveEffect()
    {

        effected = null;
    }

    public virtual void Apply(Entity entity)
    {
        effected = entity;
        effected.statusEffects.Add(this);
    }

}

public class Stunned : StatusEffect
{

    public Stunned(Entity effected, float duration) : base(effected, duration)
    {
        type = StatusEffectType.Stunned;
        Apply(effected);
    }

    public override void Apply(Entity entity)
    {
        base.Apply(entity);

        
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
        
    }

    public override void UpdateEffect()
    {
        base.UpdateEffect();
        effected.Body.mSpeed = Vector2.zero;
    }
}
