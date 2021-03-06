﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalentType { Pilot, Medic, Guard };

public class Talent : ScriptableObject
{
    public string talentName;
    public string description;
    public List<Ability> abilities;
    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryStatBonuses;
    public List<WeaponAttributeBonus> weaponBonuses;
    public List<Item> bonusItems;
    public Sprite icon;
    public bool repeatable = false;
    public int level = 0;
    //public Requirements requirements
    //public Prereqs prereqs

    public bool isLearned;


    public bool MeetsRequirements(Player player)
    {

        return true;
    }

    #region TriggerEffects
    public virtual void OnLearned(Player player, bool fromLoad = false)
    {
        isLearned = true;
        level++;

        foreach (Ability ability in abilities)
        {
            Ability abilityTemp = Instantiate(ability);
            abilityTemp.OnGainTrigger(player);
        }


        player.mStats.AddPrimaryBonuses(statBonuses);
        player.mStats.AddSecondaryBonuses(secondaryStatBonuses);

        foreach (WeaponAttributeBonus weaponBonus in weaponBonuses)
        {
            player.AddWeaponBonus(weaponBonus);
        }
        
        player.Health.UpdateHealth();

        if(!fromLoad)
        {
            foreach (Item item in bonusItems)
            {
                player.Inventory.AddItemToInventory(ItemDatabase.NewItem(item));
            }
        }

    }

    #endregion


    public TalentSaveData GetSaveData()
    {
        TalentSaveData saveData = new TalentSaveData
        {
            name = talentName,
            isLearned = isLearned,
            level = level
        };

        return saveData;
    }
}
