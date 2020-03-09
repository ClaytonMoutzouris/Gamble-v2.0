using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrototype : EntityPrototype
{
    public EnemyType enemyType;
    public int movementSpeed = 50;
    public int jumpHeight = 160;
    public int sightRange = 320;

    public int health;

    public List<RangedAttackPrototype> rangedAttacks;
    public List<MeleeAttackPrototype> meleeAttacks;

    public List<Item> lootTable;
    //Attack attack;
}
