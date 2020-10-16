using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelDisplay : MonoBehaviour
{
    
    public void SetFuel(RangedWeapon weapon)
    {
        transform.localScale = new Vector3(((float)weapon.ammunitionCount/(float)weapon.ammunitionCapacity), 1, 1);
    }
}
