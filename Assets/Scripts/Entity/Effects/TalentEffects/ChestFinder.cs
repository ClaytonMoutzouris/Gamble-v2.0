﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestFinder : Ability
{

    List<MiniMapIcon> icons;

    public ChestFinder()
    {
        abilityName = "ChestFinder";
        type = AbilityType.ChestFinder;
        icons = new List<MiniMapIcon>();
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        if (MiniMap.instance != null)
        {
            foreach(Chest chest in GameManager.instance.GetChests())
            {
                icons.Add(MiniMap.instance.AddStaticIcon(MinimapIconType.Chest, MapManager.instance.GetMapTileAtPoint(chest.Position)));
            }
        }
    }

    public override void OnUnequipTrigger(Player player)
    {
        base.OnUnequipTrigger(player);

        foreach (MiniMapIcon icon in icons)
        {
            MiniMap.instance.RemoveIcon(icon);
        }

        icons.Clear();
    }

    public override void OnMapChanged()
    {

        if (MiniMap.instance != null)
        {
            foreach (Chest chest in GameManager.instance.GetChests())
            {
                icons.Add(MiniMap.instance.AddStaticIcon(MinimapIconType.Chest, MapManager.instance.GetMapTileAtPoint(chest.Position)));
            }
        }
    }

    public override void OnOwnerDeath(Entity entity)
    {
        foreach(Player p in CrewManager.instance.players)
        {
            foreach(Ability effect in p.abilities)
            {
                if (effect is ChestFinder)
                {
                    return;
                }
            }
        }
        
        foreach (MiniMapIcon icon in icons)
        {
            GameObject.Destroy(icon);
        }
    }
}
