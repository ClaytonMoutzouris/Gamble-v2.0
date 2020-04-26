using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [HideInInspector]
    public PlayerInventory inventory;
    public Item item = null;
    public int amount = 0;
    private InventorySlotUI slotUI = null;

    public InventorySlot(PlayerInventory inventory)
    {
        this.inventory = inventory;
    }

    public InventorySlotUI GetInventorySlot()
    {
        return slotUI;
    }

    public void SetInventorySlot(InventorySlotUI value)
    {
        slotUI = value;
        value.slot = this;
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    public Item GetOneItem()
    {
        Item returnItem = item;
        amount--;

        if(amount <= 0)
        {
            ClearSlot();
        }
        //update ui
        slotUI.UpdateSlotUI();

        return returnItem;
    }

    public bool AddItemToSlot(Item newItem, int numItems = 1)
    {
        if(item == null)
        {
            item = newItem;
            amount = numItems;
            slotUI.UpdateSlotUI();
            return true;
        } else
        {
            if(item.isStackable && item.mName == newItem.mName)
            {
                amount+= numItems;
                slotUI.UpdateSlotUI();
                return true;
            } else
            {
                return false;
            }
        }
    }

    public void ClearSlot()
    {
        item = null;
        amount = 0;
        slotUI.UpdateSlotUI();
    }

    public void DropItem()
    {
        if (item == null)
            return;
        ItemObject temp = new ItemObject(item, Resources.Load("Prototypes/Entity/Objects/ItemObject") as EntityPrototype);
        temp.Spawn(inventory.mPlayer.Position + new Vector2(0, MapManager.cTileSize / 2));
        if (item is Equipment equip && equip.isEquipped)
        {
            UnequipItem();
        }
        GetOneItem();
        //temp.Body.mPosition = mPlayer.Position + new Vector3(0, MapManager.cTileSize / 2);
    }

    public void EquipItem()
    {


        if (item is Equipment equippable)
        {

            if (equippable.isEquipped)
            {
                return;
            }

            inventory.mPlayer.Equipment.EquipItem(equippable);



        }
    }

    public void UnequipItem()
    {
        if (item is Equipment equippable && equippable.isEquipped)
        {
            inventory.mPlayer.Equipment.Unequip(equippable.mSlot);
            slotUI.UpdateSlotUI();

        }
    }

    public bool UseItem()
    {
        if (item is ConsumableItem consumable)
        {
            if(consumable.Use(inventory.mPlayer))
            {
                GetOneItem();
            }
             
        }

        return false;
    }
}
