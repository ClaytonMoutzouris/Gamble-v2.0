using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestFinder : Effect
{

    List<MiniMapIcon> icons;

    public ChestFinder()
    {
        effectName = "ChestFinder";
        type = EffectType.ChestFinder;
        icons = new List<MiniMapIcon>();
    }

    public override void OnEquipTrigger(Player player)
    {
        base.OnEquipTrigger(player);

        if (MiniMap.instance != null)
        {
            foreach(Chest chest in LevelManager.instance.GetChests())
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
            GameObject.Destroy(icon);
        }
    }

    public override void OnMapChanged()
    {
        foreach(MiniMapIcon icon in icons)
        {
            GameObject.Destroy(icon);
        }

        if (MiniMap.instance != null)
        {
            foreach (Chest chest in LevelManager.instance.GetChests())
            {
                icons.Add(MiniMap.instance.AddStaticIcon(MinimapIconType.Chest, MapManager.instance.GetMapTileAtPoint(chest.Position)));
            }
        }
    }

    public override void OnPlayerDeath(Player player)
    {
        foreach(Player p in LevelManager.instance.players)
        {
            foreach(Effect effect in p.itemEffects)
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
