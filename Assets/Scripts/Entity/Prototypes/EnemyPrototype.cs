using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyPrototype : EntityPrototype
{
    public EnemyType enemyType;
    public int movementSpeed;
    public int sightRange = 50;

    public Hostility hostility = Hostility.Hostile;

    public int health;

    public List<RangedAttackPrototype> rangedAttacks;
    public List<MeleeAttackPrototype> meleeAttacks;

    //Attack attack;
}
