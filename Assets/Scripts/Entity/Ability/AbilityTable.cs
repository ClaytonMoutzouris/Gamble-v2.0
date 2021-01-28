using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTable : ScriptableObject
{
    public EquipmentSlotType equipmentType;
    public List<Ability> rareAbilities;
    public List<Ability> legendaryAbilities;
    public List<Ability> artifactAbilities;



    public Ability GetAbilityForItem(Item item)
    {
        Ability ability = null;
        switch(item.rarity)
        {
            case Rarity.Common:

                break;

            case Rarity.Uncommon:

                break;
            case Rarity.Rare:
                ability = rareAbilities[Random.Range(0, rareAbilities.Count)];
                break;
            case Rarity.Legendary:
                ability = legendaryAbilities[Random.Range(0, legendaryAbilities.Count)];
                break;
            case Rarity.Artifact:
                ability = artifactAbilities[Random.Range(0, artifactAbilities.Count)];
                break;
        }

        if(ability == null)
        {
            return null;
        }

        return Instantiate(ability);
    }

}
