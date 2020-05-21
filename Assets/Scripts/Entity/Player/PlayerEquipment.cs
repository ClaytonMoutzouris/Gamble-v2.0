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
        if (mPlayer == null || mPlayer.Inventory == null)
            return false;

        if (Equipment.ContainsKey(item.mSlot))
        {
            if (Equipment[item.mSlot] != null)
            {
                InventorySlot slot = mPlayer.Inventory.FindSlotForItem(Equipment[item.mSlot]);
                if (slot != null)
                {
                    slot.UnequipItem();

                }
            }

            Equipment[item.mSlot] = item;
            item.OnEquip(mPlayer);
            mPlayer.Inventory.FindSlotForItem(Equipment[item.mSlot]).GetInventorySlot().UpdateSlotUI();

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

            }
        }
    }

    public Equipment GetSlotContents(EquipmentSlot slot)
    {
        if (Equipment.ContainsKey(slot))
        {
                return Equipment[slot];
        }

        return null;
    }

    public Gadget GetGadget()
    {
        if(Equipment.ContainsKey(EquipmentSlot.Gadget))
        {
            if(Equipment[EquipmentSlot.Gadget] is Gadget gadget)
            {
                return gadget;
            }
            
        }

        return null;
    }
}
