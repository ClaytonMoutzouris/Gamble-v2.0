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
    public List<string> items = new List<string>();
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

        items = new List<string>();

        foreach(InventorySlot slot in player.Inventory.slots)
        {
            if(slot.item != null)
            {
                for(int i = 0; i < slot.amount; i++) {
                    items.Add(slot.item.mName);
                }
            }
        }

        colors = new List<SerializableColor>();
        foreach(Color color in ((PlayerRenderer)player.Renderer).colorSwapper.mBaseColors)
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
            foreach (string itemName in items)
            {
                player.Inventory.AddItemToInventory(ItemDatabase.GetItem(itemName));
            }
        }
        else
        {
            items = new List<string>();
        }

        player.playerLevel = playerLevel;
        player.playerExperience = playerExperience;

        List<Color> palette = new List<Color>();
        foreach(SerializableColor color in colors)
        {
            palette.Add(color);
        }

        player.SetColorPalette(palette);

    }
}
