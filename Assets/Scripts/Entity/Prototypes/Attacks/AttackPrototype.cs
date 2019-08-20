using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPrototype : ScriptableObject
{
    public int damage;
    public float duration;
    public float cooldown;
    public int startUpFrames;

    public Vector2 offset = new Vector2(0,0);

    public List<WeaponAbility> abilities;

    public virtual string getToolTip()
    {
        string tooltip = "";
        tooltip += "\n" + damage + " DMG";


        return tooltip;
    }
}
