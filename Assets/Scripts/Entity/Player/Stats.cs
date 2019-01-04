using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats : MonoBehaviour
{
    public Entity mEntity;
    public Health health;
    public List<Stat> stats;

    public void Start()
    {
        mEntity = GetComponent<Entity>();
    }


}

public enum StatType { Strength, Agility, Intelligence, Luck };

[System.Serializable]
public struct Stat
{
    public StatType type;
    public int value;

}