using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalentType { Pilot, Medic, Guard };

public class Talent : ScriptableObject
{
    public string name;
    public string description;
    public string level;
    public List<AbilityType> effectTypes;
    public List<StatBonus> statBonuses;
    public List<Item> bonusItems;
    public Sprite icon;
    public bool repeatable = false;
    //public Requirements requirements
    //public Prereqs prereqs

    public bool isLearned;


    public bool MeetsRequirements(Player player)
    {

        return true;
    }

    #region TriggerEffects
    public virtual void OnLearned(Player player)
    {
        Debug.Log("ON LEARNED " + name);
        isLearned = true;

        foreach (AbilityType effect in effectTypes)
        {
            Ability e = Ability.GetEffectFromType(effect);
            e.OnEquipTrigger(player);
        }


        player.mStats.AddBonuses(statBonuses);
        player.Health.UpdateHealth();


        foreach (Item item in bonusItems)
        {
            player.Inventory.AddItemToInventory(ItemDatabase.NewItem(item));
        }
    }

    #endregion

}
