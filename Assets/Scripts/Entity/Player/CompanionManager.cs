using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager
{

    List<Companion> companions = new List<Companion>();

    public void AddCompanion(Companion companion)
    {
        
        companions.Add(companion);
        if(companions.Count%2 == 0)
        {
            companion.offset = -1*(companions.Count/2) * new Vector2(32, 0) + Vector2.up * 32;
        } else
        {
            companion.offset = (companions.Count/2) * new Vector2(32, 0) + Vector2.up * 32;
        }
    }

    public void RemoveCompanion(Companion companion)
    {
        if (companions.Contains(companion))
        {
            companions.Remove(companion);
        }
    }

}
