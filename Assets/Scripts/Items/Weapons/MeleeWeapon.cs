using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public MeleeAttackPrototype attack;
    #region baseAttributes
    public List<WeaponAttribute> baseAttributes = new List<WeaponAttribute>{
        new WeaponAttribute(WeaponAttributesType.Damage, 5),
        new WeaponAttribute(WeaponAttributesType.DamageType, 0),
        new WeaponAttribute(WeaponAttributesType.AttackSpeed, 1),
        new WeaponAttribute(WeaponAttributesType.WeaponReach, 0),

    };
    #endregion

    public override void Initialize()
    {
        base.Initialize();
        //attack = Instantiate(attack);
        attributes = new WeaponAttributes();
        attributes.SetStats(baseAttributes);
    }

    public void SetAttack(AttackManager attackManager, int index)
    {
        //attackManager.AttackList[index] = attack;
    }

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        equipped.AttackManager.meleeAttacks[0] = new MeleeAttack(equipped, this);

    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        player.AttackManager.meleeAttacks[0] = player.defaultMelee;
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\n<color=white>" + attributes.GetAttribute(WeaponAttributesType.Damage).GetValue() + " Damage</color>";
        tooltip += "\n<color=white>" + AttackSpeedToString() + "</color>";

        return tooltip;
    }

    public string AttackSpeedToString()
    {
        string speed = "";
        float atkSpeed = attributes.GetAttribute(WeaponAttributesType.AttackSpeed).GetValue();
        if (atkSpeed > 10)
        {
            speed = "Very Fast";
        }
        else if (atkSpeed < 10 && atkSpeed >= 6)
        {
            speed = "Fast";
        }
        else if (atkSpeed < 6 && atkSpeed >= 3)
        {
            speed = "Medium";
        }
        else if (atkSpeed < 3 && atkSpeed >= 1)
        {
            speed = "Slow";
        }
        else if (atkSpeed < 1)
        {
            speed = "Very Slow";
        }

        speed += " Attack Speed";

        return speed;
    }

    public override bool Attack()
    {
        bool outcome = true;
        if (equipped.AttackManager.meleeAttacks[0].mIsActive || equipped.AttackManager.meleeAttacks[0].OnCooldown())
        {
            return false;
        }
        equipped.AttackManager.meleeAttacks[0] = new MeleeAttack(equipped, this);
        equipped.AttackManager.meleeAttacks[0].Activate();

        return outcome;
    }

    public override void OnUpdate()
    {

    }
}
