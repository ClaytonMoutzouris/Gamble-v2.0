﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrototype : EntityPrototype
{
    public EnemyType enemyType;
    public int movementSpeed;
    public int jumpHeight = 120;
    public int sightRange = 50;

    public Hostility hostility = Hostility.Hostile;

    public int health;

    public List<RangedAttackPrototype> rangedAttacks;
    public List<MeleeAttackPrototype> meleeAttacks;

    public List<Item> lootTable;
    //Attack attack;
}
