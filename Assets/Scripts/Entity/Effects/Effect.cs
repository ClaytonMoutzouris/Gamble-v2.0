using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public string effectName;
    public string description;
    public EffectType type;
    //AbilityTrigger trigger;

    public virtual void OnEquipTrigger(Player player) { }
    public virtual void OnHitTrigger(Player player) { }
    public virtual void OnDamagedTrigger(Player player) { }
    public virtual void OnJumpTrigger(Player player) { }
    public virtual void OnUnequipTrigger(Player player) { }

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
        }

        return effect;
    }
}

