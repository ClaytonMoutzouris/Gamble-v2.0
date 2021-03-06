﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase {

    static Item[] mItemDatabase;

    public static bool InitializeDatabase()
    {
        mItemDatabase = Resources.LoadAll<Item>("Prototypes/Items");

        return true;
    }

    public static Item GetItem(string name)
    {

        foreach(Item item in mItemDatabase)
        {
            if(item.itemName.Equals(name))
            {
                Item temp = ScriptableObject.Instantiate(item);
                temp.Initialize();

                return temp;

            }
        }

        return null;
    }

    public static Item GetRandomItem()
    {
        Item item = ScriptableObject.Instantiate(mItemDatabase[Random.Range(0, mItemDatabase.Length)]);
        item.Initialize();

        if (item is Equipment equipment)
        {
            equipment.RandomizeStats();
            item = equipment;
        }
        return item;
    }

    public static Item NewItem(Item item)
    {
        Item nItem = ScriptableObject.Instantiate(item);
        nItem.Initialize();

        if (nItem is Equipment equipment)
        {
            equipment.RandomizeStats();
            nItem = equipment;
        }

        return nItem;
    }

    public static Item GetKey()
    {
        Item item = ScriptableObject.Instantiate(mItemDatabase[Random.Range(0, mItemDatabase.Length)]);
        item.Initialize();

        return item;
    }
   
}
