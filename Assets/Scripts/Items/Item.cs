using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    
    public string mName;
    public string mValue;
    public Sprite sprite;
    private InventorySlot inventorySlot = null;

    public virtual List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>();
    }

    public InventorySlot GetInventorySlot()
    {
        return inventorySlot;
    }

    public void SetInventorySlot(InventorySlot value)
    {
        inventorySlot = value;
    }

    public virtual string getTooltip()
    {
        string tooltip = "";

        tooltip += "<color=orange>"+mName+"</color>";

        return tooltip;
    }
    
}


public abstract class Equipment : Item
{

    public EquipmentSlot mSlot;
    public List<StatBonus> statBonuses;
    public List<PlayerAbility> abilities;
    //public Trait trait;
    public bool isEquipped;

    public virtual void OnEquip(Player player)
    {

        player.mStats.AddBonuses(statBonuses);
        foreach(PlayerAbility ability in abilities)
        {
            player.activeAbilities.Add(ability);
        }

    }

    public virtual void OnUnequip(Player player)
    {
        player.mStats.RemoveBonuses(statBonuses);
        foreach(PlayerAbility ability in abilities)
        {
            player.activeAbilities.Remove(ability);
        }
    }

    public override List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>(){
            InventoryOption.Equip,
            InventoryOption.Move,
            InventoryOption.Drop,
            InventoryOption.Cancel };
    }

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();
        tooltip += "\n<color=black>" + mSlot.ToString() + "</color>";
        foreach (PlayerAbility ability in abilities)
        {
            tooltip += "\n<color=magenta>" + ability.ToString() + "</color>";
        }
        foreach(StatBonus stat in statBonuses)
        {
            tooltip += "\n<color=green>" + stat.getTooltip() + "</color>";
        }

        return tooltip;

    }
}

public abstract class Weapon : Equipment
{

}


