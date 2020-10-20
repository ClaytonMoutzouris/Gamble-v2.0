using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    
    public string itemName;
    public Rarity rarity;
    public int baseValue;
    public Sprite sprite;
    public bool isStackable = false;
    public bool identified = true;
    public bool autoPickup = false;

    public virtual List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>();
    }

    public virtual bool Identify()
    {
        if (!identified)
        {
            identified = true;
            return true;
        }

        return false;

    }

    public virtual string GetTooltip()
    {
        string tooltip = "";

        tooltip += "<color=" + GetColorStringFromRarity() + ">" + itemName+"</color>";

        return tooltip;
    }
    
    public string GetColorStringFromRarity()
    {
        string rarity = "grey";
        switch(this.rarity)
        {
            case Rarity.Common:
                rarity = "white";
                break;
            case Rarity.Uncommon:
                rarity = "green";
                break;
            case Rarity.Rare:
                rarity = "blue";
                break;
            case Rarity.Legendary:
                rarity = "orange";
                break;
            case Rarity.Artifact:
                rarity = "red";
                break;
        }

        return rarity;
    }

    public Color GetColorFromRarity()
    {
        Color rarity = Color.grey;
        switch (this.rarity)
        {
            case Rarity.Common:
                rarity = Color.white;
                break;
            case Rarity.Uncommon:
                rarity = Color.green;
                break;
            case Rarity.Rare:
                rarity = Color.blue;
                break;
            case Rarity.Legendary:
                rarity = Color.yellow;
                break;
            case Rarity.Artifact:
                rarity = Color.red;
                break;
        }

        return rarity;
    }

    public virtual int GetValue()
    {
        int value = 0;

        value += baseValue + baseValue * (int)rarity;

        return value;
    }

    public virtual int GetSellValue()
    {
        int sellValue = 0;

        sellValue = GetValue() / 2;

        return sellValue;
    }

    public virtual ItemSaveData GetSaveData()
    {
        ItemSaveData data = new ItemSaveData
        {
            itemName = itemName,
            identified = identified,
            rarity = rarity
        };

        return data;
    }

    public virtual void LoadItemData(ItemSaveData data)
    {
        identified = data.identified;
        rarity = data.rarity;
    }
}

public abstract class Equipment : Item
{

    public EquipmentSlotType mSlot;
    public List<StatBonus> baseBonuses;
    List<StatBonus> statBonuses = new List<StatBonus>();
    //public List<PlayerAbility> abilities;
    public List<Ability> baseEffects = new List<Ability>();
    List<Ability> effects = new List<Ability>();
    //public Trait trait;
    public bool isEquipped;

    public List<Ability> Effects { get => effects; set => effects = value; }

    public virtual void RandomizeStats()
    {

        if(rarity != Rarity.Artifact)
        {
            int rarityRoll = Random.Range(0, 100) + 10* WorldManager.instance.NumCompletedWorlds();

            if (rarityRoll < 50)
            {
                rarity = Rarity.Common;
            }
            else if (rarityRoll >= 50 && rarityRoll < 80)
            {
                rarity = Rarity.Uncommon;
            }
            else if (rarityRoll >= 80 && rarityRoll < 100)
            {
                rarity = Rarity.Rare;

            }
            else if (rarityRoll >= 100)
            {
                rarity = Rarity.Legendary;
            }
        }
        
        foreach(Ability ability in baseEffects)
        {
            Effects.Add(AbilityDatabase.NewAbility(ability));
        }


        //statBonuses.Clear();
        //abilities.Clear();

        int minBonus = 1;
        int maxBonus = 2;
        int numBonuses = (int)rarity;
        StatType statType;

        List<StatType> remainingTypes = new List<StatType> ();
        
        remainingTypes.AddRange(System.Enum.GetValues(typeof(StatType)) as IEnumerable<StatType>);

        for (int i = 0; i < numBonuses; i++)
        {
            int r = Random.Range(0, remainingTypes.Count);
            statType = remainingTypes[r];
            remainingTypes.RemoveAt(r);
            statBonuses.Add(new StatBonus(statType, Random.Range(minBonus, maxBonus + (int)rarity)));
        }

        if(rarity == Rarity.Legendary || rarity == Rarity.Rare || rarity == Rarity.Artifact)
        {
            Effects.Add(AbilityDatabase.GetItemAbility());
            //abilities.Add((PlayerAbility)Random.Range(0, (int)PlayerAbility.Count));
        }
        
    }

    public virtual void OnEquip(Player player)
    {
        isEquipped = true;


        player.mStats.AddBonuses(statBonuses);
        player.mStats.AddBonuses(baseBonuses);
        player.Health.UpdateHealth();

        //This adds the abilities of the equipped item to the player
        foreach(Ability effect in Effects)
        {
            effect.OnGainTrigger(player);
        }

        //This calls the updatetrigger for all effects on the player
        foreach (Ability ability in player.abilities)
        {
            ability.OnEquippedTrigger(player, this);
        }
    }

    public virtual void OnUnequip(Player player)
    {
        isEquipped = false;

        player.mStats.RemoveBonuses(statBonuses);
        player.mStats.RemoveBonuses(baseBonuses);

        player.Health.UpdateHealth();

        //This calls the updatetrigger for all effects on the player
        foreach (Ability ability in player.abilities)
        {
            ability.OnUnequipTrigger(player, this);
        }

        foreach (Ability effect in Effects)
        {
            effect.OnRemoveTrigger(player);
        }
    }

    public override List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>(){
            InventoryOption.Equip,
            InventoryOption.Move,
            InventoryOption.Drop,
            InventoryOption.Cancel };
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();

        tooltip += "\n<color=white>" + mSlot.ToString() + "</color>";
        foreach (Ability effect in Effects)
        {
            tooltip += "\n<color=magenta>" + effect.abilityName + "</color>";
        }
        foreach (StatBonus stat in baseBonuses)
        {
            tooltip += "\n<color=white>" + stat.GetTooltip() + "</color>";
        }
        foreach (StatBonus stat in statBonuses)
        {
            tooltip += "\n<color=green>" + stat.GetTooltip() + "</color>";
        }

        return tooltip;

    }

    public override ItemSaveData GetSaveData()
    {
        EquipmentSaveData data = new EquipmentSaveData
        {
            itemName = itemName,
            identified = identified,
            rarity = rarity,
            statBonuses = statBonuses,
            equipped = isEquipped
        };

        foreach(Ability ability in effects)
        {
            data.abilityNames.Add(ability.abilityName);
        }

        return data;
    }

    public override void LoadItemData(ItemSaveData data)
    {
        base.LoadItemData(data);

        if(data is EquipmentSaveData equipmentData)
        {
            statBonuses = equipmentData.statBonuses;
           
            foreach (string abilityName in equipmentData.abilityNames)
            {
                Effects.Add(AbilityDatabase.GetAbility(abilityName));
            }
        }
    }
}

public abstract class Weapon : Equipment
{
    public int damage;


}


