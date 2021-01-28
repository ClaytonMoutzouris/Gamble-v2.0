using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : StatusEffect
{

    public List<Ability> abilitiesGranted;
    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryBonuses;
    public List<WeaponAttributeBonus> weaponBonuses;
    List<Ability> abilities = new List<Ability>();

    public override bool OnApplyEffect(Entity effected)
    {
        if (!base.OnApplyEffect(effected))
        {
            return false;
        }

        foreach(Ability ability in abilitiesGranted)
        {
            Ability temp = ScriptableObject.Instantiate(ability);
            abilities.Add(temp);
            temp.OnGainTrigger(effected);
        }


        EffectedEntity.mStats.AddPrimaryBonuses(statBonuses);
        EffectedEntity.mStats.AddSecondaryBonuses(secondaryBonuses);


        if(effected is Player player)
        {
            foreach (WeaponAttributeBonus weaponBonus in weaponBonuses)
            {
                player.AddWeaponBonus(weaponBonus);
            }
        }

        return true;
    }

    public override void UpdateEffect()
    {
        //effectedEntity.Body.mSpeed = Vector2.zero;

        base.UpdateEffect();
    }

    public override void OnEffectEnd()
    {
        foreach(Ability ability in abilities)
        {
            ability.OnRemoveTrigger(EffectedEntity);
        }

        abilities.Clear();

        EffectedEntity.mStats.RemovePrimaryBonuses(statBonuses);
        EffectedEntity.mStats.RemoveSecondaryBonuses(secondaryBonuses);

        if (EffectedEntity is Player player)
        {
            foreach (WeaponAttributeBonus weaponBonus in weaponBonuses)
            {
                player.RemoveWeaponBonus(weaponBonus);
            }
        }
        base.OnEffectEnd();

    }
}