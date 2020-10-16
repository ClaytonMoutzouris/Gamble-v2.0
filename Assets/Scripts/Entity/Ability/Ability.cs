using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public string description;
    protected Entity owner;
    //AbilityTrigger trigger;

    public virtual void OnGainTrigger(Entity entity) {
        entity.abilities.Add(this);
        //player.playerPanel.uiPlayerTab.effectContainer.AddEffect(this);
        owner = entity;
    }

    public virtual void OnEquippedTrigger(Entity entity, Equipment equipment)
    {

    }

    public virtual void OnUnequipTrigger(Entity entity, Equipment equipment)
    {

    }
    public virtual void OnHitTrigger(AttackObject attack, IHurtable entity) { }
    public virtual void OnDamagedTrigger(Attack attack) { }
    public virtual void OnJumpTrigger(Player player) { }
    public virtual void OnRemoveTrigger(Entity entity) {
        //player.playerPanel.uiPlayerTab.effectContainer.RemoveEffect(this);
        entity.abilities.Remove(this);
        owner = null;

    }
    public virtual void OnUpdate() { }
    public virtual void OnSecondUpdate() { }

    public virtual void OnWalkTrigger(Player player)
    {

    }

    public virtual void OnHealTrigger(IHurtable player, int heals)
    {

    }

    public virtual void OnConsumeItem(Player player, ConsumableItem item) { }
    public virtual void OnMapChanged(){ }

    public virtual void OnOwnerDeath(Entity entity) {
        OnRemoveTrigger(entity);
    }

    public virtual void OnEnemyDeath(Enemy enemy) { }

    public override string ToString()
    {
        return abilityName;
    }

}

