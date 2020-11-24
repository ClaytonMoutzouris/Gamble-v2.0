using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment
{
    //Do it with dictionary, or with slots
    //We will start with slots so we can see it in inspector

    List<EquipmentSlot> EquipmentSlots;
    EquipmentSlot gadgetSlot1;
    EquipmentSlot gadgetSlot2;
    Player mPlayer;

    public PlayerEquipment(Player player)
    {
        mPlayer = player;
        EquipmentSlots = new List<EquipmentSlot>();

        EquipmentSlots.Add(new EquipmentSlot(EquipmentSlotType.Head));
        EquipmentSlots.Add(new EquipmentSlot(EquipmentSlotType.Body));
        EquipmentSlots.Add(new EquipmentSlot(EquipmentSlotType.Gloves));
        EquipmentSlots.Add(new EquipmentSlot(EquipmentSlotType.Boots));
        EquipmentSlots.Add(new EquipmentSlot(EquipmentSlotType.Belt));
        EquipmentSlots.Add(new EquipmentSlot(EquipmentSlotType.Mainhand));
        EquipmentSlots.Add(new EquipmentSlot(EquipmentSlotType.Offhand));

        gadgetSlot1 = new EquipmentSlot(EquipmentSlotType.Gadget);
        gadgetSlot2 = new EquipmentSlot(EquipmentSlotType.Gadget);

        //Equipment.Add(new EquipmentSlot(EquipmentSlotType.Gadget));

    }
    public List<EquipmentSlot> GetSlots()
    {
        return EquipmentSlots;
    }

    public EquipmentSlot GetSlot(EquipmentSlotType type)
    {

        foreach(EquipmentSlot slot in EquipmentSlots)
        {
            if(slot.GetSlotType() == type)
            {
                return slot;
            }
        }

        return null;
    }

    public EquipmentSlot GetGadgetSlot(Equipment equip, bool unequip = false)
    {
        if(unequip)
        {
            if (gadgetSlot1.GetContents() == equip)
            {
                return gadgetSlot1;
            } else if(gadgetSlot2.GetContents() == equip)
            {
                return gadgetSlot2;
            }

            return null;
        } else
        {
            Equipment slot1 = gadgetSlot1.GetContents();
            Equipment slot2 = gadgetSlot2.GetContents();

            if(slot1 == null)
            {
                return gadgetSlot1;
            }

            if(slot2 == null)
            {
                return gadgetSlot2;
            }

            return gadgetSlot1;
        }

        return null;
    }

    public bool EquipItem(Equipment item)
    {
        if (mPlayer == null || mPlayer.Inventory == null)
            return false;


        EquipmentSlot equipSlot;

        if (item is Gadget gadget)
        {
            equipSlot = GetGadgetSlot(item);
        }
        else
        {
            equipSlot = GetSlot(item.mSlot);
        }

        if (equipSlot != null)
        {
            Equipment contents = equipSlot.GetContents();
            if (contents != null)
            {
                Unequip(contents);

            }

            equipSlot.SetContents(item);
            PlayerUIPanels.instance.playerPanels[mPlayer.mPlayerIndex].toolbeltUI.UpdateEquipmentNode(equipSlot);
            item.OnEquip(mPlayer);
            mPlayer.Inventory.FindSlotForItem(item).GetInventorySlot().UpdateSlotUI();

            return true;
        }

        return false;
    }

    public void Unequip(Equipment equipment)
    {
        EquipmentSlot equipSlot;

        if (equipment is Gadget gadget)
        {
            equipSlot = GetGadgetSlot(equipment, true);
        }
        else
        {
            equipSlot = GetSlot(equipment.mSlot);
        }

        if (equipSlot != null)
        {
            Equipment contents = equipSlot.GetContents();
            if (contents != null)
            {
                contents.OnUnequip(mPlayer);
                mPlayer.Inventory.FindSlotForItem(contents).GetInventorySlot().UpdateSlotUI();
                equipSlot.ClearSlot();
                PlayerUIPanels.instance.playerPanels[mPlayer.mPlayerIndex].toolbeltUI.UpdateEquipmentNode(equipSlot);

            }
        }
    }

    public Equipment GetGadget1()
    {
        return gadgetSlot1.GetContents();
    }

    public Equipment GetGadget2()
    {
        return gadgetSlot2.GetContents();
    }
}

public class EquipmentSlot
{
    EquipmentSlotType slotType;
    Equipment contents;

    public EquipmentSlot(EquipmentSlotType type)
    {
        slotType = type;
        contents = null;
    }

    public EquipmentSlotType GetSlotType()
    {
        return slotType;
    }

    public void SetContents(Equipment equip)
    {
        contents = equip;
        
    } 

    public Equipment GetContents()
    {
        return contents;
    }

    public void ClearSlot()
    {
        contents = null;
    }
}