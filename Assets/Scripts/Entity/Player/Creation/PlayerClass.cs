﻿using System.Collections;
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
    public int startingMoney = 20;

    public TalentTree talentTree;

    public void LoadClass(Player player, bool fromSave = false)
    {
        player.mStats.SetStats(baseStats);

        if (!fromSave)
        {
            foreach (Equipment equipment in startingEquipment)
            {
                Equipment temp = Instantiate(equipment);

                player.Inventory.AddItemToInventory(temp);
                player.Equipment.EquipItem(temp);
            }

            foreach (Item item in startingInventory)
            {
                Item temp = Instantiate(item);

                player.Inventory.AddItemToInventory(temp);
            }

            for(int i = 0; i < startingMoney; i++)
            {
                Item temp = ItemDatabase.GetItem("Blueium");
                player.Inventory.AddItemToInventory(temp);

            }

        }
        player.talentTree = Instantiate(talentTree);
        player.talentTree.GetNewTree(player);
        PlayerUIPanels.instance.playerPanels[player.mPlayerIndex].talentTree.SetTalentTree(player);
    }
}
