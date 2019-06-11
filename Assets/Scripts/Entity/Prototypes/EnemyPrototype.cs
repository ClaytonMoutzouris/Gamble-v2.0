using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrototype : ScriptableObject
{
    public EnemyType enemyType;
    public string mName;
    public int movementSpeed;
    public int sightRange = 50;

    public Hostility hostility = Hostility.Hostile;
    public Vector2 bodySize;

    public int health;

    public List<AttackPrototype> attacks;
    //Attack attack;
}
