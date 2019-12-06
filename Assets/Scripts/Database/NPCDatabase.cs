using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCDatabase
{

    static List<NPCPrototype> mNPCDatabase;

    static private bool isDatabaseLoaded = false;

    static public void LoadDatabase()
    {
        if (isDatabaseLoaded) return;
        isDatabaseLoaded = true;
        LoadDatabaseForce();
    }


    public static void LoadDatabaseForce()
    {
        ValidateDatabase();
        NPCPrototype[] resources = Resources.LoadAll<NPCPrototype>(@"Prototypes/Entity/NPC"); // Load all items from the Resources/Items folder
        foreach (NPCPrototype npc in resources)
        {
            if (!mNPCDatabase.Contains(npc)) // If list doesn't contain item then add it 
            {
                mNPCDatabase.Add(npc);
            }
        }
    }

    static private void ValidateDatabase() // Is list null and/or loaded?
    {
        if (mNPCDatabase == null) mNPCDatabase = new List<NPCPrototype>(); // If list is null, create list
        if (!isDatabaseLoaded) LoadDatabase(); // If database is not loaded, load database
    }

    public static NPCPrototype GetNPCPrototype(NPCType id)
    {
        ValidateDatabase();

        foreach (NPCPrototype npc in mNPCDatabase)
        {
            if (npc.npcType == id)
            {
                return ScriptableObject.Instantiate(npc) as NPCPrototype;
            }
        }

        return null;

    }

}
