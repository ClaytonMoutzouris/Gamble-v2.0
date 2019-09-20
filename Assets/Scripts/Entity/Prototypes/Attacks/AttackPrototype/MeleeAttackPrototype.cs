using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackPrototype : AttackPrototype
{
    public MeleeAttackObjectPrototype meleeObjectPrototype;

    public override string GetToolTip()
    {
        string tooltip = base.GetToolTip();

        return tooltip;
    }
}
