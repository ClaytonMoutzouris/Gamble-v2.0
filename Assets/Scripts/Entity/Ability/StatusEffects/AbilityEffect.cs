using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffect : StatusEffect
{
    public Ability abilityProto;
    Ability ability;

    public override bool OnApplyEffect(Entity effected)
    {
        if (!base.OnApplyEffect(effected))
        {
            return false;
        }

        ability = ScriptableObject.Instantiate(abilityProto);
        ability.OnEquipTrigger(effected);
        return true;
    }

    public override void UpdateEffect()
    {
        //effectedEntity.Body.mSpeed = Vector2.zero;

        base.UpdateEffect();
    }

    public override void OnEffectEnd()
    {
        ability.OnUnequipTrigger(EffectedEntity);

        base.OnEffectEnd();

    }
}