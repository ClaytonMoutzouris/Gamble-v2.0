using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayerSaveData
{
    public string playerName;
    //public TalentTree talentTree;
    public float currentHealth;
    public float baseHP;
    //public List<Item> items;
    public List<ItemSaveData> items = new List<ItemSaveData>();
    public List<TalentSaveData> talents = new List<TalentSaveData>();
    //public PlayerClass playerClass;
    public PlayerClassType classType;
    public int playerLevel;
    public int playerExperience;
    public List<SerializableColor> colors;

    public PlayerSaveData(Player player)
    {
        playerName = player.entityName;

        currentHealth = player.Health.currentHealth;
        baseHP = player.Health.baseHP;

        classType = player.playerClass.classType;
        Debug.Log("Saving player with Data " + classType);
        playerLevel = player.playerLevel;
        playerExperience = player.playerExperience;

        talents = new List<TalentSaveData>();

        foreach(Talent talent in player.talentTree.GetAllTalents())
        {
            if(talent.isLearned) {
                Debug.Log("Saving Talent " + talent.talentName);
                talents.Add(talent.GetSaveData());
            } 
        }

        items = new List<ItemSaveData>();

        foreach(InventorySlot slot in player.Inventory.slots)
        {
            if(slot.item != null)
            {
                for(int i = 0; i < slot.amount; i++) {
                    ItemSaveData data = slot.item.GetSaveData();
                    items.Add(data);
                }
            }
        }

        colors = new List<SerializableColor>();
        foreach(Color color in ((PlayerRenderer)player.Renderer).colorSwap.mBaseColors)
        {
            colors.Add(color);
        }
    }

    public void SetPlayersData(Player player)
    {
        player.entityName = playerName;
        player.Health.baseHP = baseHP;
        player.Health.currentHealth = currentHealth;
        player.Health.UpdateHealth();

        if(items != null)
        {
            foreach (ItemSaveData itemData in items)
            {
                Item item = ItemDatabase.GetItem(itemData.itemName);
                item.LoadItemData(itemData);
                player.Inventory.AddItemToInventory(item);
                if(itemData is EquipmentSaveData equipmentData)
                {
                    if(equipmentData.equipped)
                    {
                        if (item is Equipment equipment)
                        {
                            player.Equipment.EquipItem(equipment);
                        }
                    }
                }
            }
        }
        else
        {
            items = new List<ItemSaveData>();
        }

        player.playerLevel = playerLevel;
        player.playerExperience = playerExperience;

        player.talentTree.skillPoints = player.playerLevel;

        //Im assuming the talent tree is set up by now
        if (talents != null)
        {
            foreach (TalentSaveData talentData in talents)
            {
                Debug.Log("Loading Talent " + talentData.name);

                if (talentData.isLearned)
                {
                    for(int i = 0; i < talentData.level; i++)
                    {
                        player.playerPanel.talentTree.LearnTalent(talentData.name, true);
                    }
                }

            }
        }
        else
        {
            talents = new List<TalentSaveData>();
        }


        List<Color> palette = new List<Color>();
        foreach(SerializableColor color in colors)
        {
            palette.Add(color);
        }

        player.SetColorPalette(palette);

    }
}
