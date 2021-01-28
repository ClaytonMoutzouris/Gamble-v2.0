using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelDisplay : MonoBehaviour
{
    
    public void SetFuel(RangedWeapon weapon)
    {
        transform.localScale = new Vector3(((float)weapon.ammunitionCount/(float)weapon.attributes.GetAttribute(WeaponAttributesType.AmmoCapacity).GetValue()), 1, 1);
    }
}
