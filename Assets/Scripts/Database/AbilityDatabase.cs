using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityDatabase
{

    static Ability[] mAbilityDatabase;
    static Ability[] mItemAbilityDatabase;

    public static bool InitializeDatabase()
    {
        mAbilityDatabase = Resources.LoadAll<Ability>("Prototypes/Abilities");
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
        Ability item = ScriptableObject.Instantiate(mAbilityDatabase[Random.Range(0, mAbilityDatabase.Length)]);
        return item;
    }

    public static Ability NewAbility(Ability item)
    {
        Ability nItem = ScriptableObject.Instantiate(item);
        return nItem;
    }

}
