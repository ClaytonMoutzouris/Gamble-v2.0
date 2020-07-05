using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public string abilityName;
    public string description;
    public AbilityType type;
    protected Entity owner;
    //AbilityTrigger trigger;

    public virtual void OnEquipTrigger(Player player) {
        player.abilities.Add(this);
        owner = player;
    }
    public virtual void OnHitTrigger(Attack attack, IHurtable entity) { }
    public virtual void OnDamagedTrigger(Attack attack) { }
    public virtual void OnJumpTrigger(Player player) { }
    public virtual void OnUnequipTrigger(Player player) {
        player.abilities.Remove(this);
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

    public virtual void OnOwnerDeath(Entity entity) {
    }

    public virtual void OnEnemyDeath(Enemy enemy) { }

    public static Ability GetEffectFromType(AbilityType type)
    {
        Ability effect = null;

        //Use this for testing specific effects
        //return new Flamewalker();

        switch (type)
        {
            case AbilityType.ExtraJump:
                effect = new ExtraJump();
                break;
            case AbilityType.Hover:
                effect = new Hover();
                break;
            case AbilityType.Lifesteal:
                effect = new Lifesteal();
                break;
            case AbilityType.DamageReflect:
                effect = new DamageReflect();
                break;
            case AbilityType.PoisonAttack:
                effect = new PoisonAttack();
                break;
            case AbilityType.StunAttack:
                effect = new StunAttack();
                break;
            case AbilityType.SuperSpeed:
                effect = new SuperSpeed();
                break;
            case AbilityType.SpikeProtection:
                effect = new SpikeProtection();
                break;
            case AbilityType.CrushProtection:
                effect = new CrushProtection();
                break;
            case AbilityType.Flamewalker:
                effect = new Flamewalker();
                break;
            case AbilityType.Heavy:
                effect = new Heavy();
                break;
            case AbilityType.Knockback:
                effect = new Knockback();
                break;
            case AbilityType.ExtraDamage:
                effect = new ExtraDamage();
                break;
            case AbilityType.CompanionDrone:
                effect = new CompanionDrone();
                break;
            case AbilityType.ChestFinder:
                effect = new ChestFinder();
                break;
            case AbilityType.MaxHPFromFood:
                effect = new MaxHPFromFood();
                break;
            case AbilityType.ReusableMedkits:
                effect = new ReusableMedkits();
                break;
            case AbilityType.PartyHeal:
                effect = new PartyHeal();
                break;
            case AbilityType.Aura:
                effect = new Aura();
                break;
            default:
                Debug.LogError("Failed to find effect of type : " + type);
                break;
        }

        return effect;
    }



}

