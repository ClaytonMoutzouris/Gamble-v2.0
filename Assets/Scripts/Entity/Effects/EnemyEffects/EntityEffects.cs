using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTriggers { Apply, Remove, OnDeath };

public class EntityEffects : ScriptableObject
{

    public string name;
    public string description;
    Entity enemy;

    //Effect Triggers
    public virtual void Apply(Entity entity)
    {
        entity.entityEffects.Add(this);
    }

    public virtual void Trigger()
    {

    }
    public virtual void OnHitTrigger(Attack attack, IHurtable entity) { }
    public virtual void OnDamagedTrigger(Attack attack) { }
    public virtual void OnJumpTrigger(Entity entity) { }
    public virtual void OnUnequipTrigger(Entity entity)
    {
        entity.entityEffects.Remove(this);
    }
    public virtual void OnUpdateTrigger(Entity entity) { }
    public virtual void OnWalkTrigger(Entity entity)
    {

    }
    public virtual void OnHealTrigger(Entity entity, int heals)
    {

    }
    public virtual void OnConsumeItem(Entity entity, ConsumableItem item) { }
    public virtual void OnMapChanged() { }
    public virtual void OnDeath(Entity entity) { }
    public virtual void OnLearned(Entity entity) { }
}
