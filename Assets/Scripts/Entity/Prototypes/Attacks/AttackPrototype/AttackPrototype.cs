using System.Collections;
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

    public AudioClip sfx;

    public virtual string GetToolTip()
    {
        string tooltip = "";
        tooltip += "\n<color=white>" + damage + " Damage</color>";
        tooltip += "\n<color=white>" + AttackSpeedToString() + "</color>";
        return tooltip;
    }

    public string AttackSpeedToString()
    {
        string speed = "";
        if(cooldown > 0 && cooldown < .5)
        {
            speed = "Fast";
        } else if(cooldown >= 0.5 && cooldown < 1)
        {
            speed = "Medium";
        }
        else if (cooldown >= 1)
        {
            speed = "Slow";
        }

        speed += " Attack Speed";

        return speed;
    }
}