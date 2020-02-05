using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePrototype : EntityPrototype
{
    public int movementSpeed;

    public Hostility hostility = Hostility.Friendly;

    public AttackPrototype attack;

}