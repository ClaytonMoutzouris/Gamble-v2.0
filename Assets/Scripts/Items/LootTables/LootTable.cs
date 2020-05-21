using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : ScriptableObject
{
    public List<LootTableNode> nodes;
    public List<Item> guaranteedDrops;

    //Old method using weights
    public List<Item> GetLootOld(int numRandomItems = 1)
    {
        List<Item> items = new List<Item>();

        int weightTotal = 0;
        List<int> ranges = new List<int>();

        foreach(LootTableNode node in nodes)
        {
            weightTotal += node.dropChance;
            ranges.Add(weightTotal);
        }

        int random;
        
        for(int r = 0; r < numRandomItems; r++)
        {
            random = Random.Range(0, weightTotal);

            for (int i = 0; i < ranges.Count; i++)
            {
                if (random < ranges[i])
                {
                    if (nodes[i].item != null)
                    {
                        items.Add(nodes[i].item);
                    }
                    break;
                }
            }
        }


        foreach(Item node in guaranteedDrops)
        {
            items.Add(node);
        }

        return items;
    }

    public List<Item> GetLoot()
    {
        List<Item> items = new List<Item>();
        
        foreach(LootTableNode node in nodes)
        {
            if (node.dropChance >= Random.Range(0, 100) + 1)
            {
                for(int i = 0; i < Random.Range(node.minDropNum, node.maxDropNum+1); i++)
                {
                    items.Add(node.item);
                }
            }
        }

        

        return items;
    }

}
