using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableNode
{
    public int weight;
    public Item item;
    public int minDropNum = 1;
    public int maxDropNum = 1;

}
