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

        tooltip += "<color=" + GetColorFromRarity() + ">" + itemName+"</color>";

        return tooltip;
    }
    
    public string GetColorFromRarity()
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
}

public abstract class Equipment : Item
{

    public EquipmentSlotType mSlot;
    public List<StatBonus> baseBonuses;
    List<StatBonus> statBonuses = new List<StatBonus>();
    //public List<PlayerAbility> abilities;
    public List<Ability> baseEffects;
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
            Effects.Add(AbilityDatabase.GetRandomAbility());
            //abilities.Add((PlayerAbility)Random.Range(0, (int)PlayerAbility.Count));
        }
        
    }

    public virtual void OnEquip(Player player)
    {
        isEquipped = true;


        player.mStats.AddBonuses(statBonuses);
        player.mStats.AddBonuses(baseBonuses);
        player.Health.UpdateHealth();

        foreach(Ability effect in Effects)
        {
            effect.OnEquipTrigger(player);
        }
    }

    public virtual void OnUnequip(Player player)
    {
        isEquipped = false;

        player.mStats.RemoveBonuses(statBonuses);
        player.mStats.RemoveBonuses(baseBonuses);

        player.Health.UpdateHealth();

        foreach (Ability effect in Effects)
        {
            effect.OnUnequipTrigger(player);
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
            tooltip += "\n<color=magenta>" + effect.ToString() + "</color>";
        }
        foreach (StatBonus stat in baseBonuses)
        {
            tooltip += "\n<color=white>" + stat.getTooltip() + "</color>";
        }
        foreach (StatBonus stat in statBonuses)
        {
            tooltip += "\n<color=green>" + stat.getTooltip() + "</color>";
        }

        return tooltip;

    }
}

public abstract class Weapon : Equipment
{
    public int damage;


}


