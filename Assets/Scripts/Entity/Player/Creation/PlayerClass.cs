using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerClass : ScriptableObject
{
    public string className;
    public string classDescription;
    public PlayerClassType classType;

    public List<Stat> baseStats;

    public List<Equipment> startingEquipment;
    public List<Item> startingInventory;

    public TalentTree talentTree;

    public void LoadClass(Player player, bool fromSave = false)
    {
        player.mStats.SetStats(baseStats);

        if (!fromSave)
        {
            foreach (Equipment equipment in startingEquipment)
            {
                Equipment temp = (Equipment)ItemDatabase.NewItem(equipment);

                player.Inventory.AddItemToInventory(temp);
                player.Equipment.EquipItem(temp);
            }

            foreach (Item item in startingInventory)
            {
                Item temp = ItemDatabase.NewItem(item);
                temp = Instantiate(temp);

                player.Inventory.AddItemToInventory(temp);
            }
        }
        player.talentTree = Instantiate(talentTree);
        player.talentTree.GetNewTree();
        PlayerUIPanels.instance.playerPanels[player.mPlayerIndex].talentTree.SetTalentTree(player);
    }
}
