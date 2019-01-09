using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    
    public string mName;
    public string mValue;
    public Sprite sprite;

    
}


public abstract class Equipment : Item
{

    public EquipmentSlot mSlot;

    public virtual void Equip()
    {

    }
}



