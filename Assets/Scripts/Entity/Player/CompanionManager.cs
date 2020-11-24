using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager
{

    List<Companion> companions = new List<Companion>();

    public List<Companion> Companions { get => companions; set => companions = value; }

    public void AddCompanion(Companion companion)
    {
        
        Companions.Add(companion);
        if(Companions.Count%2 == 0)
        {
            companion.offset = -1*(Companions.Count/2) * new Vector2(12, 0) + Vector2.up * 32;
        } else
        {
            companion.offset = (Companions.Count/2) * new Vector2(12, 0) + Vector2.up * 32;
        }
    }

    public void RemoveCompanion(Companion companion)
    {
        if (Companions.Contains(companion))
        {
            Companions.Remove(companion);
        }
    }

}
