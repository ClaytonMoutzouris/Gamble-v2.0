using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPrototype : EntityPrototype
{
    public NPCType npcType;

    public int movementSpeed;
    public int jumpHeight = 120;
    public int sightRange = 50;

    public int health;

    public List<RangedAttackPrototype> rangedAttacks;
    public List<MeleeAttackPrototype> meleeAttacks;

    public List<Item> wares;
    public List<string> dialogueLines;


    //Attack attack;
}