using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    Entity effectedEntity;
    public List<EntityType> uneffectedTypes;
    public StatusEffectClass effectClass;
    //base duration is 1 second
    public double effectDuration = 1;
    double elapsed = 0;
    double timeStamp;
    bool expired = false;
    public bool stackable = false;
    public ParticleSystem effectPrefab;
    GameObject effectObject;
    public Vector2 effectOffset = Vector2.zero;


    public double Elapsed { get => elapsed; set => elapsed = value; }
    public bool Expired { get => expired; set => expired = value; }
    public Entity EffectedEntity { get => effectedEntity; set => effectedEntity = value; }

    public virtual bool OnApplyEffect(Entity effected)
    {
        if (uneffectedTypes.Contains(effected.mEntityType))
        {
            return false;
        }

        if (!stackable)
        {
            foreach (StatusEffect sEffect in effected.statusEffects)
            {
                if (sEffect.name.Equals(name))
                {
                    //Refresh the cooldown but don't apply another instance of the effect
                    sEffect.Elapsed = 0;
                    return false;
                }
            }
        }

        timeStamp = Time.time;

        EffectedEntity = effected;

        EffectedEntity.statusEffects.Add(this);
        if(effectPrefab != null)
        {
            effectObject = EffectedEntity.Renderer.AddVisualEffect(effectPrefab, effectOffset);
        }

        return true;
    }

    public virtual void UpdateEffect()
    {
        Elapsed += Time.deltaTime;
        //Debug.Log()
        //Debug.Log(Time.timeScale);
        //framesElapsed++;
        //Debug.Log(type + " frames elapsed " + framesElapsed);


        if (Elapsed >= effectDuration)
        {
            OnEffectEnd();
        }
    }

    public virtual void OnEffectEnd()
    {
        EffectedEntity.Renderer.RemoveVisualEffect(effectObject);

        //effectedEntity.statusEffects.Remove(this);
        Expired = true;
    }
}