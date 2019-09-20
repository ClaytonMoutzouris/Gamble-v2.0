using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackPrototype : AttackPrototype
{
    public ProjectilePrototype projectilePrototype;
    public int numberOfProjectiles = 1;
    public float spreadAngle = 0;

    public override string GetToolTip()
    {
        string tooltip = base.GetToolTip();

        return tooltip;
    }
}
