using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : ScriptableObject
{
    public string className;
    public string classDescription;
    public PlayerClassType classType;

    public List<Stat> baseStats;

    public List<Equipment> startingEquipment;
    public List<Item> startingInventory;

    public TalentTree talentTree;

    public void LoadClass(Player player)
    {
        player.mStats.SetStats(baseStats);

        foreach(Equipment equipment in startingEquipment)
        {
            Equipment temp = (Equipment)ItemDatabase.NewItem(equipment);
            
            int index = player.Inventory.AddItemToInventory(temp);
            player.Inventory.EquipItem(index);
            //player.Equipment.EquipItem(temp);
        }

        foreach (Item item in startingInventory)
        {
            Item temp = ItemDatabase.NewItem(item);
            player.Inventory.AddItemToInventory(item);
        }

        talentTree = Instantiate<TalentTree>(talentTree);
        player.talentTree = talentTree;
        PlayerUIPanels.instance.playerPanels[player.mPlayerIndex].talentTree.SetTalentTree(player);
    }
}
