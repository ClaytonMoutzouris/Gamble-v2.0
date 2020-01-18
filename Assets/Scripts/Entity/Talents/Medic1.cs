using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic1 : Talent
{
    public Medic1()
    {
        name = "Healer";
        description = "Gain 2 medipacs.";
    }

    public override void OnLearned(Player player)
    {
        base.OnLearned(player);
        player.Inventory.AddItemToInventory(ItemDatabase.NewItem(Resources.Load("Prototypes/Items/Medpack") as Item));
        player.Inventory.AddItemToInventory(ItemDatabase.NewItem(Resources.Load("Prototypes/Items/Medpack") as Item));

    }
}
