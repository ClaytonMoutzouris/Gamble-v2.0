using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public string effectName;
    public string description;
    public EffectType type;
    protected Player owner;
    //AbilityTrigger trigger;

    public virtual void OnEquipTrigger(Player player) {
        player.itemEffects.Add(this);
        owner = player;
    }
    public virtual void OnHitTrigger(Attack attack, IHurtable entity) { }
    public virtual void OnDamagedTrigger(Attack attack) { }
    public virtual void OnJumpTrigger(Player player) { }
    public virtual void OnUnequipTrigger(Player player) {
        player.itemEffects.Remove(this);
        owner = null;

    }
    public virtual void OnUpdate() { }
    public virtual void OnSecondUpdate() { }

    public virtual void OnWalkTrigger(Player player)
    {

    }

    public virtual void OnHealTrigger(Player player, int heals)
    {

    }

    public virtual void OnConsumeItem(Player player, ConsumableItem item) { }
    public virtual void OnMapChanged(){ }

    public virtual void OnPlayerDeath(Player player) { }

    public virtual void OnEnemyDeath(Enemy enemy) { }


    public virtual void OnLearned(Player player) {
        owner = player;
    }

    public static Effect GetEffectFromType(EffectType type)
    {
        Effect effect = null;

        //Use this for testing specific effects
        //return new Flamewalker();

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
            case EffectType.Flamewalker:
                effect = new Flamewalker();
                break;
            case EffectType.Heavy:
                effect = new Heavy();
                break;
            case EffectType.Knockback:
                effect = new Knockback();
                break;
            case EffectType.ExtraDamage:
                effect = new ExtraDamage();
                break;
            case EffectType.CompanionDrone:
                effect = new CompanionDrone();
                break;
            case EffectType.ChestFinder:
                effect = new ChestFinder();
                break;
            case EffectType.MaxHPFromFood:
                effect = new MaxHPFromFood();
                break;
            case EffectType.ReusableMedkits:
                effect = new ReusableMedkits();
                break;
            case EffectType.PartyHeal:
                effect = new PartyHeal();
                break;
            case EffectType.Aura:
                effect = new Aura();
                break;
            default:
                Debug.LogError("Failed to find effect of type : " + type);
                break;
        }

        return effect;
    }



}

