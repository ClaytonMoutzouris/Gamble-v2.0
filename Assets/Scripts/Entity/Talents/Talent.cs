using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Talent
{
    public string name;
    public string description;
    public string level;
    public Player player;

    //public Requirements requirements
    //public Prereqs prereqs

    public bool isLearned;

    public Talent(Player player)
    {

    }

    public bool MeetsRequirements(Player player)
    {

        return true;
    }

    #region TriggerEffects
    public virtual void OnLearnedTrigger()
    {

    }

    #endregion

}
