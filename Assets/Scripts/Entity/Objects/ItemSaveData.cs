using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSaveData
{
    public string itemName;
    public Rarity rarity;
    //public int baseValue;
    //public Sprite sprite;
    //public bool isStackable = false;
    public bool identified = true;

}

[System.Serializable]
public class EquipmentSaveData : ItemSaveData
{
    public List<StatBonus> statBonuses = new List<StatBonus>();
    public List<string> abilityNames = new List<string>();
    public bool equipped = false;

}

[System.Serializable]
public class BiosampleSaveData : ItemSaveData
{
    public WorldType sampleWorldType;

}