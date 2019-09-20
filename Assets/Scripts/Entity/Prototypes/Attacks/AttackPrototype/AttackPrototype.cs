﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPrototype : ScriptableObject
{
    //This is how long the user is in the attack state when using this attack, kinda tied to animation
    public float duration;
    public float cooldown;
    public int startUpFrames;
    public Vector2 offset = new Vector2(0,0);
    //The animation performed by the user of this attack
    public RuntimeAnimatorController animationController;
    public int damage;
    public List<WeaponAbility> abilities = new List<WeaponAbility>();

    public AudioClip sfx;

    public virtual string GetToolTip()
    {
        string tooltip = "";

        return tooltip;
    }
}