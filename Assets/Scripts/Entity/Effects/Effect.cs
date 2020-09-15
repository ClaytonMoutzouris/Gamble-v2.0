using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectArgs { Effected, Effector, Value1, Value2, Value3 }

public abstract class Effect : ScriptableObject
{
    public string description;
    protected Entity effected;

    public virtual void ApplyEffect(List<Object> args)
    {
        //effected = (Entity)args[(int)EffectArgs.Effected];
    }

    public virtual void RemoveEffect()
    {

    }

}

