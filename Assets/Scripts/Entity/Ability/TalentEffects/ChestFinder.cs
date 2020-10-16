using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestFinder : Ability
{
    List<MiniMapIcon> icons = new List<MiniMapIcon>();

    public override void OnGainTrigger(Entity player)
    {
        base.OnGainTrigger(player);

        if (MiniMap.instance != null)
        {
            foreach(Chest chest in GameManager.instance.GetChests())
            {
                icons.Add(MiniMap.instance.AddStaticIcon(MinimapIconType.Chest, MapManager.instance.GetMapTileAtPoint(chest.Position)));
            }
        }
    }

    public override void OnRemoveTrigger(Entity entity)
    {
        base.OnRemoveTrigger(entity);

        foreach (Player p in CrewManager.instance.players)
        {
            if (p == null)
                continue;
            foreach (Ability effect in p.abilities)
            {
                if (effect is ChestFinder)
                {
                    return;
                }
            }
        }

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

}
