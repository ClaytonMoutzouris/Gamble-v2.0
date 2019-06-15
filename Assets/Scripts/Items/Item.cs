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
    public List<Stat> stats;
    public bool isEquipped;

    public virtual void Equip(Player player)
    {
        foreach(Stat stat in stats)
        {

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
}



