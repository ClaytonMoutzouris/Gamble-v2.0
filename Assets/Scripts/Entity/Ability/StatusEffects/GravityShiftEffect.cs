using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityShiftEffect : StatusEffect
{
    float previousMultiplier;
    Vector2 previousVector;

    public bool changeVector = true;
    public bool changeMultiplier = true;
    public Vector2 gravityVector = new Vector2(0, -1);
    public float gravityMultiplier = 1;

    public override bool OnApplyEffect(Entity effected)
    {

        if (!base.OnApplyEffect(effected))
        {
            return false;
        }

        if(changeMultiplier)
        {
            previousMultiplier = EffectedEntity.gravityMultiplier;
            EffectedEntity.gravityMultiplier = gravityMultiplier;
        }

        if(changeVector)
        {
            previousVector = EffectedEntity.gravityVector;
            EffectedEntity.gravityVector = gravityVector;
        }


        return true;
    }

    public override void UpdateEffect()
    {
        //effectedEntity.Body.mSpeed = Vector2.zero;

        base.UpdateEffect();
    }

    public override void OnEffectEnd()
    {
        if(changeVector)
        {
            EffectedEntity.gravityVector = previousVector;
        }

        if (changeMultiplier)
        {
            EffectedEntity.gravityMultiplier = EffectedEntity.baseGravityMultiplier;
        }
        base.OnEffectEnd();

    }
}