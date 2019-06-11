using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPrototype : ScriptableObject
{
    public int damage;
    public float duration;
    public float cooldown;
    public int startUpFrames;

}
