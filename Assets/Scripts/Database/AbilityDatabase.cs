using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityDatabase
{

    static Ability[] mAbilityDatabase;
    static Ability[] mItemAbilityDatabase;
    static AbilityTable[] mAbilityTables;

    public static bool InitializeDatabase()
    {
        mAbilityDatabase = Resources.LoadAll<Ability>("Prototypes/Abilities");
        mItemAbilityDatabase = Resources.LoadAll<Ability>("Prototypes/Abilities/Item Abilities");
        mAbilityTables = Resources.LoadAll<AbilityTable>("Prototypes/Abilities/AbilityTables");
        return true;
    }

    public static Ability GetAbility(string name)
    {

        foreach (Ability ability in mAbilityDatabase)
        {
            if (ability.abilityName.Equals(name))
            {
                return NewAbility(ability);

            }
        }

        return null;
    }

    public static Ability GetRandomAbility()
    {
        Ability ability = ScriptableObject.Instantiate(mAbilityDatabase[Random.Range(0, mAbilityDatabase.Length)]);
        return ability;
    }

    public static Ability GetItemAbility(Equipment item)
    {
        Ability ability = null;
        foreach (AbilityTable table in mAbilityTables)
        {
            if(table.equipmentType == item.mSlot)
            {
                ability = table.GetAbilityForItem(item);
            }
        }

        return ability;
    }

    public static Ability NewAbility(Ability ability)
    {
        Ability nItem = ScriptableObject.Instantiate(ability);
        return nItem;
    }

}
