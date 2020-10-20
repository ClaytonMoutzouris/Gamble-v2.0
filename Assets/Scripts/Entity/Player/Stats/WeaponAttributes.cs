using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAttributeTypes { Damage, Count }
//problem with this is they are null until instantiated
public class WeaponAttributes
{
    WeaponAttribute[] attributes = new WeaponAttribute[(int)WeaponAttributeTypes.Count];


}

public class WeaponAttribute
{

}