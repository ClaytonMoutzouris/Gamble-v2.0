using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment
{
    //Do it with dictionary, or with slots
    //We will start with slots so we can see it in inspector

    Dictionary<EquipmentSlot, Equipment> Equipment;
    Player mPlayer;

    public PlayerEquipment(Player player)
    {
        mPlayer = player;
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
                Equipment[item.mSlot].OnUnequip(mPlayer);
                Equipment[item.mSlot].GetInventorySlot().SetEquipped(false);
            }

            Equipment[item.mSlot] = item;
            item.OnEquip(mPlayer);

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
                Equipment[slot].OnUnequip(mPlayer);
                Equipment[slot].GetInventorySlot().SetEquipped(false);
            }
        }
    }

}
