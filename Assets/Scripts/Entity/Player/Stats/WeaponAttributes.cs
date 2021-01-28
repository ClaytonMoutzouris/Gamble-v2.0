using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAttributesType { NumProjectiles, SpreadAngle, AmmoCapacity, ReloadTime, Damage, FireRate, ProjectileSpeed, AttackSpeed, WeaponReach, DamageType };

[System.Serializable]
public class WeaponAttributes
{
    public Dictionary<WeaponAttributesType, WeaponAttribute> attributes;

    public WeaponAttributes()
    {

        attributes = new Dictionary<WeaponAttributesType, WeaponAttribute>();

        //Create starting stats
        foreach (WeaponAttributesType type in System.Enum.GetValues(typeof(WeaponAttributesType)))
        {
            attributes.Add(type, new WeaponAttribute(type));
        }

    }

    public void SetStats(List<WeaponAttribute> attributesList)
    {
        attributes = new Dictionary<WeaponAttributesType, WeaponAttribute>();

        foreach (WeaponAttribute attribute in attributesList)
        {
            attributes.Add(attribute.type, attribute);
        }

    }

    public WeaponAttribute GetAttribute(WeaponAttributesType type)
    {
        if (attributes.ContainsKey(type)) {
            return attributes[type];
        }

        return null;
    }

    public void AddBonus(WeaponAttributeBonus bonus)
    {
        if (attributes.ContainsKey(bonus.type))
        {
            attributes[bonus.type].AddBonus(bonus);
        }
        else
        {
            Debug.Log("Does not contain key " + bonus.type);

        }
    }

    public void RemoveBonus(WeaponAttributeBonus bonus)
    {
        if (attributes.ContainsKey(bonus.type))
        {
            attributes[bonus.type].RemoveBonus(bonus);
        }
    }

    public void AddBonuses(List<WeaponAttributeBonus> bonuses)
    {
        foreach (WeaponAttributeBonus bonus in bonuses)
        {
            AddBonus(bonus);
        }

    }

    public void RemoveBonuses(List<WeaponAttributeBonus> bonuses)
    {
        foreach (WeaponAttributeBonus bonus in bonuses)
        {
            RemoveBonus(bonus);
        }

    }

    public void SetBaseAttributes()
    {
             List<WeaponAttribute> baseAttributes = new List<WeaponAttribute>{
                new WeaponAttribute(WeaponAttributesType.NumProjectiles, 1),
                new WeaponAttribute(WeaponAttributesType.SpreadAngle, 0),
                new WeaponAttribute(WeaponAttributesType.AmmoCapacity, 5),
                new WeaponAttribute(WeaponAttributesType.ReloadTime, 3),
                new WeaponAttribute(WeaponAttributesType.Damage, 5),
                new WeaponAttribute(WeaponAttributesType.FireRate, 3),
                new WeaponAttribute(WeaponAttributesType.ProjectileSpeed, 100)};

        SetStats(baseAttributes);
    }
}

[System.Serializable]
public class WeaponAttribute
{
    public WeaponAttributesType type;
    public float value;
    [HideInInspector]
    public List<WeaponAttributeBonus> bonuses;

    public WeaponAttribute(WeaponAttributesType t, float startingValue = 0)
    {
        type = t;
        value = startingValue;
        bonuses = new List<WeaponAttributeBonus>();
    }

    public void AddBonus(WeaponAttributeBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(WeaponAttributeBonus bonus)
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

        foreach (WeaponAttributeBonus bonus in bonuses)
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

        return (fullValue + fullValue * multiplier);
    }
}

[System.Serializable]
public class WeaponAttributeBonus
{
    public StatModType modType;
    public WeaponAttributesType type;
    public float bonusValue;

    public WeaponAttributeBonus(WeaponAttributesType t, float min, StatModType modType = StatModType.Add)
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