using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    
    public string mName;
    public Rarity mRarity;
    public string mValue;
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

    public virtual string getTooltip()
    {
        string tooltip = "";

        tooltip += "<color=" + getColorFromRarity() + ">" + mName+"</color>";

        return tooltip;
    }
    
    public string getColorFromRarity()
    {
        string rarity = "grey";
        switch(mRarity)
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
}


public abstract class Equipment : Item
{

    public EquipmentSlot mSlot;
    public List<StatBonus> baseBonuses;
    List<StatBonus> statBonuses = new List<StatBonus>();
    //public List<PlayerAbility> abilities;
    private List<EffectType> possibleEffects = new List<EffectType>();
    public List<EffectType> baseEffects;
    List<Effect> effects = new List<Effect>();
    //public Trait trait;
    public bool isEquipped;

    public List<Effect> Effects { get => effects; set => effects = value; }
    public List<EffectType> PossibleEffects { get => possibleEffects; set => possibleEffects = value; }

    public virtual void RandomizeStats()
    {

        if(mRarity != Rarity.Artifact)
        {
            int rarityRoll = Random.Range(0, 100) + 10* WorldManager.instance.NumCompletedWorlds();

            if (rarityRoll < 50)
            {
                mRarity = Rarity.Common;
            }
            else if (rarityRoll >= 50 && rarityRoll < 80)
            {
                mRarity = Rarity.Uncommon;
            }
            else if (rarityRoll >= 80 && rarityRoll < 100)
            {
                mRarity = Rarity.Rare;

            }
            else if (rarityRoll >= 100)
            {
                mRarity = Rarity.Legendary;
            }
        }
        
        foreach(EffectType type in baseEffects)
        {
            Effects.Add(Effect.GetEffectFromType(type));
        }


        //statBonuses.Clear();
        //abilities.Clear();

        int minBonus = 1;
        int maxBonus = 2;
        int numBonuses = (int)mRarity;
        StatType statType;

        List<StatType> remainingTypes = new List<StatType> ();
        
        remainingTypes.AddRange(System.Enum.GetValues(typeof(StatType)) as IEnumerable<StatType>);

        for (int i = 0; i < numBonuses; i++)
        {
            int r = Random.Range(0, remainingTypes.Count);
            statType = remainingTypes[r];
            remainingTypes.RemoveAt(r);
            statBonuses.Add(new StatBonus(statType, Random.Range(minBonus, maxBonus + (int)mRarity)));
        }

        if(mRarity == Rarity.Legendary || mRarity == Rarity.Rare || mRarity == Rarity.Artifact)
        {
            Effects.Add(Effect.GetEffectFromType((EffectType)Random.Range(0, (int)EffectType.Heavy)));
            //abilities.Add((PlayerAbility)Random.Range(0, (int)PlayerAbility.Count));
        }
        
    }

    public virtual void OnEquip(Player player)
    {
        isEquipped = true;


        player.mStats.AddBonuses(statBonuses);
        player.mStats.AddBonuses(baseBonuses);
        player.Health.UpdateHealth();

        foreach(Effect effect in Effects)
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

        foreach (Effect effect in Effects)
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

    public override string getTooltip()
    {
        string tooltip = base.getTooltip();

        tooltip += "\n<color=white>" + mSlot.ToString() + "</color>";
        foreach (Effect effect in Effects)
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


