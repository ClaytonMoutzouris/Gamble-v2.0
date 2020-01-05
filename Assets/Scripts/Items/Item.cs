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
    private InventorySlot inventorySlot = null;
    public bool isStackable = false;

    public virtual List<InventoryOption> GetInventoryOptions()
    {
        return new List<InventoryOption>();
    }

    public InventorySlot GetInventorySlot()
    {
        return inventorySlot;
    }

    public void SetInventorySlot(InventorySlot value)
    {
        inventorySlot = value;
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
        }

        return rarity;
    }
}


public abstract class Equipment : Item
{

    public EquipmentSlot mSlot;
    public List<StatBonus> statBonuses;
    public List<PlayerAbility> abilities;
    private List<EffectType> possibleEffects = new List<EffectType>();
    List<Effect> effects = new List<Effect>();
    //public Trait trait;
    public bool isEquipped;

    public List<Effect> Effects { get => effects; set => effects = value; }
    public List<EffectType> PossibleEffects { get => possibleEffects; set => possibleEffects = value; }

    public virtual void Randomize()
    {
        mRarity = (Rarity)Random.Range(0, (int)Rarity.Count);
        statBonuses.Clear();
        abilities.Clear();

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

        if(mRarity == Rarity.Legendary || mRarity == Rarity.Rare)
        {
            foreach (EffectType type in System.Enum.GetValues(typeof(EffectType)))
            {
                possibleEffects.Add(type);
            }
            Debug.Log("Randomizing Item, possible effects: " + PossibleEffects.Count);
            Effects.Add(Effect.GetEffectFromType(possibleEffects[Random.Range(0, possibleEffects.Count)]));
            //abilities.Add((PlayerAbility)Random.Range(0, (int)PlayerAbility.Count));
        }
        
        foreach(Effect effect in Effects)
        {
            Debug.Log(effect.type);
        }
    }

    public virtual void OnEquip(Player player)
    {

        player.mStats.AddBonuses(statBonuses);
        player.Health.UpdateHealth();
        foreach(PlayerAbility ability in abilities)
        {
            player.activeAbilities.Add(ability);
        }

        foreach(Effect effect in Effects)
        {
            effect.OnEquipTrigger(player);
        }
    }

    public virtual void OnUnequip(Player player)
    {
        player.mStats.RemoveBonuses(statBonuses);
        player.Health.UpdateHealth();
        foreach (PlayerAbility ability in abilities)
        {
            player.activeAbilities.Remove(ability);
        }

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
        foreach(StatBonus stat in statBonuses)
        {
            tooltip += "\n<color=green>" + stat.getTooltip() + "</color>";
        }

        return tooltip;

    }
}

public abstract class Weapon : Equipment
{
    public int damage;
    public List<AttackTrait> weaponAbilities;


}


