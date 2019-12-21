using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public string effectName;
    public string description;
    public EffectType type;
    //AbilityTrigger trigger;

    public virtual void OnEquipTrigger(Player player) {
        player.itemEffects.Add(this);
    }
    public virtual void OnHitTrigger(Attack attack, IHurtable entity) { }
    public virtual void OnDamagedTrigger(Player player) { }
    public virtual void OnJumpTrigger(Player player) { }
    public virtual void OnUnequipTrigger(Player player) {
        player.itemEffects.Remove(this);
    }

    public static Effect GetEffectFromType(EffectType type)
    {
        Effect effect = null;
        switch(type)
        {
            case EffectType.ExtraJump:
                effect = new ExtraJump();
                break;
            case EffectType.Hover:
                effect = new Hover();
                break;
            case EffectType.Lifesteal:
                effect = new Lifesteal();
                break;
        }

        return effect;
    }
}

