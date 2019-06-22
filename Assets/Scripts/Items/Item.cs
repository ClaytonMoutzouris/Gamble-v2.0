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
    
}


public abstract class Equipment : Item
{

    public EquipmentSlot mSlot;
    public List<StatBonus> statBonuses;
    //public Trait trait;
    public bool isEquipped;

    public virtual void OnEquip(Player player)
    {

        player.mStats.AddBonuses(statBonuses);

    }

    public virtual void OnUnequip(Player player)
    {
        player.mStats.RemoveBonuses(statBonuses);
    }

    public override List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>(){
            InventoryOption.Equip,
            InventoryOption.Move,
            InventoryOption.Drop,
            InventoryOption.Cancel };
    }
}

public abstract class Weapon : Equipment
{

}


