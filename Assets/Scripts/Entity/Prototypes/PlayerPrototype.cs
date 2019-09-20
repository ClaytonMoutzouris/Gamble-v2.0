using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrototype : EntityPrototype
{
    public int baseHealth;
    public int jumpSpeed;
    public int walkSpeed;
    public int climbSpeed;

    public List<RangedAttackPrototype> rangedAttacks;
    public List<MeleeAttackPrototype> meleeAttacks;

    public List<Color> colorPallete;
}
