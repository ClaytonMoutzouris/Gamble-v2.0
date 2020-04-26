using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : ScriptableObject
{
    public List<LootTableNode> nodes;
    public List<Item> guaranteedDrops;


    public List<Item> GetLoot()
    {
        List<Item> items = new List<Item>();

        int weightTotal = 0;
        List<int> ranges = new List<int>();

        foreach(LootTableNode node in nodes)
        {
            weightTotal += node.weight;
            ranges.Add(weightTotal);
        }

        int random = Random.Range(0, weightTotal);

        for(int i = 0; i < ranges.Count; i++)
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

        foreach(Item node in guaranteedDrops)
        {
            items.Add(node);
        }

        return items;
    }
}
