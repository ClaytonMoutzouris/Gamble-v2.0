using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangedWeaponAttributesType { NumProjectiles, SpreadAngle, AmmoCapacity, ReloadTime, Damage, FireRate, ProjectileSpeed }

[System.Serializable]
public class RangedWeaponAttributes
{
    public Dictionary<RangedWeaponAttributesType, RangedWeaponAttribute> attributes;

    public RangedWeaponAttributes()
    {

        attributes = new Dictionary<RangedWeaponAttributesType, RangedWeaponAttribute>();

        //Create starting stats
        foreach (RangedWeaponAttributesType type in System.Enum.GetValues(typeof(RangedWeaponAttributesType)))
        {
            attributes.Add(type, new RangedWeaponAttribute(type));
        }

    }

    public void SetStats(List<RangedWeaponAttribute> attributesList)
    {
        this.attributes.Clear();

        foreach (RangedWeaponAttribute attribute in attributesList)
        {
            this.attributes.Add(attribute.type, attribute);
        }

    }

    public RangedWeaponAttribute GetAttribute(RangedWeaponAttributesType type)
    {
        return attributes[type];
    }

    public void AddBonus(RangedWeaponAttributeBonus bonus)
    {
        attributes[bonus.type].AddBonus(bonus);
    }

    public void RemoveBonus(RangedWeaponAttributeBonus bonus)
    {
        attributes[bonus.type].RemoveBonus(bonus);
    }

    public void AddBonuses(List<RangedWeaponAttributeBonus> bonuses)
    {
        foreach (RangedWeaponAttributeBonus bonus in bonuses)
        {
            attributes[bonus.type].AddBonus(bonus);
        }

    }

    public void RemoveBonuses(List<RangedWeaponAttributeBonus> bonuses)
    {
        foreach (RangedWeaponAttributeBonus bonus in bonuses)
        {
            attributes[bonus.type].RemoveBonus(bonus);
        }

    }
}

[System.Serializable]
public class RangedWeaponAttribute
{
    public RangedWeaponAttributesType type;
    public float value;
    [HideInInspector]
    public List<RangedWeaponAttributeBonus> bonuses;

    public RangedWeaponAttribute(RangedWeaponAttributesType t, float startingValue = 0)
    {
        type = t;
        value = startingValue;
        bonuses = new List<RangedWeaponAttributeBonus>();
    }

    public void AddBonus(RangedWeaponAttributeBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(RangedWeaponAttributeBonus bonus)
    {
        bonuses.Remove(bonus);
    }

    public float GetBaseValue()
    {
        return value;
    }

    public float GetValue()
    {
        float fullValue = value;
        float multiplier = 0;

        foreach (RangedWeaponAttributeBonus bonus in bonuses)
        {
            switch (bonus.modType)
            {
                case StatModType.Add:
                    fullValue += bonus.bonusValue;
                    break;
                case StatModType.Mult:
                    multiplier += bonus.bonusValue;
                    break;
                default:

                    break;
            }
        }

        return (int)(fullValue + fullValue * multiplier);
    }
}

[System.Serializable]
public class RangedWeaponAttributeBonus
{
    public StatModType modType;
    public RangedWeaponAttributesType type;
    public float bonusValue;

    public RangedWeaponAttributeBonus(RangedWeaponAttributesType t, float min, StatModType modType = StatModType.Add)
    {
        type = t;
        bonusValue = min;
        this.modType = modType;
    }

    public string GetTooltip()
    {
        string tooltip = "";
        if (bonusValue >= 0)
        {
            tooltip += "+";
        }

        switch (modType)
        {
            case StatModType.Add:
                tooltip += bonusValue + " " + type.ToString();
                break;
            case StatModType.Mult:
                tooltip += bonusValue * 100 + "% " + type.ToString();
                break;
        }

        return tooltip;
    }
}