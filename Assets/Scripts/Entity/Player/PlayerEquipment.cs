using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment
{
    //Do it with dictionary, or with slots
    //We will start with slots so we can see it in inspector

    Dictionary<EquipmentSlot, Equipment> Equipment;
    Player mPlayer;

    public PlayerEquipment()
    {
        Equipment = new Dictionary<EquipmentSlot, Equipment>();

        foreach(EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
        {
            Equipment.Add(slot, null);
        }
    }

    public bool EquipItem(Equipment item)
    {
        if (Equipment.ContainsKey(item.mSlot))
        {
            if (Equipment[item.mSlot] != null)
            {
                Debug.Log("Item in the slot");
                Equipment[item.mSlot].GetInventorySlot().SetEquipped(false);
            }

            Equipment[item.mSlot] = item;

            return true;
        }

        return false;
    }

    public void Unequip(EquipmentSlot slot)
    {
        if (Equipment.ContainsKey(slot))
        {
            if (Equipment[slot] != null)
            {
                Equipment[slot].GetInventorySlot().SetEquipped(false);
            }
        }
    }

}
