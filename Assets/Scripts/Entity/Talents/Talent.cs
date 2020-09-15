using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalentType { Pilot, Medic, Guard };

public class Talent : ScriptableObject
{
    public string name;
    public string description;
    public string level;
    public List<Ability> abilities;
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
        isLearned = true;

        foreach (Ability ability in abilities)
        {
            Ability abilityTemp = Instantiate(ability);
            abilityTemp.OnEquipTrigger(player);
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
