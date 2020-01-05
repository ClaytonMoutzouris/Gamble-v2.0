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
    public virtual void OnDamagedTrigger(Attack attack) { }
    public virtual void OnJumpTrigger(Player player) { }
    public virtual void OnUnequipTrigger(Player player) {
        player.itemEffects.Remove(this);
    }

    public static Effect GetEffectFromType(EffectType type)
    {
        Effect effect = null;

        //Use this for testing specific effects
        //return new StunAttack();

        switch (type)
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
            case EffectType.DamageReflect:
                effect = new DamageReflect();
                break;
            case EffectType.PoisonAttack:
                effect = new PoisonAttack();
                break;
            case EffectType.StunAttack:
                effect = new StunAttack();
                break;
            case EffectType.SuperSpeed:
                effect = new SuperSpeed();
                break;
            case EffectType.SpikeProtection:
                effect = new SpikeProtection();
                break;
            case EffectType.CrushProtection:
                effect = new CrushProtection();
                break;
            default:
                Debug.LogError("Failed to find effect of type : " + type);
                break;
        }

        return effect;
    }
}

