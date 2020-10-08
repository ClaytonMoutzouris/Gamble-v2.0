using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleEffectLocation { Center, Top, Bottom, Left, Right };
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
    public ParticleEffectLocation effectLocation = ParticleEffectLocation.Center;

    public double Elapsed { get => elapsed; set => elapsed = value; }
    public bool Expired { get => expired; set => expired = value; }
    public Entity EffectedEntity { get => effectedEntity; set => effectedEntity = value; }
    public Sprite icon;

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

        if(effected is Player player)
        {
            player.playerPanel.effectUI.AddEffect(this);
        }

        timeStamp = Time.time;

        EffectedEntity = effected;

        EffectedEntity.statusEffects.Add(this);
        if(effectPrefab != null)
        {
            effectObject = EffectedEntity.Renderer.AddVisualEffect(effectPrefab, GetOffsetFromLocation());
        }

        return true;
    }

    public Vector2 GetOffsetFromLocation()
    {
        Vector2 offset = effectOffset;

        switch(effectLocation)
        {
            case ParticleEffectLocation.Center:
                offset = effectedEntity.Body.mAABB.HalfSizeY * Vector2.up + effectOffset;
                break;
            case ParticleEffectLocation.Top:
                offset = effectedEntity.Body.mAABB.HalfSizeY * Vector2.up + effectedEntity.Body.mAABB.HalfSize.y * Vector2.up + effectOffset;
                break;
            case ParticleEffectLocation.Bottom:
                offset = effectedEntity.Body.mAABB.HalfSizeY * Vector2.up + effectedEntity.Body.mAABB.HalfSize.y * Vector2.down + effectOffset;
                break;
            case ParticleEffectLocation.Left:
                offset = effectedEntity.Body.mAABB.HalfSizeY * Vector2.up + effectedEntity.Body.mAABB.HalfSize.x * Vector2.left + effectOffset;
                break;
            case ParticleEffectLocation.Right:
                offset = effectedEntity.Body.mAABB.HalfSizeY*Vector2.up + effectedEntity.Body.mAABB.HalfSize.x * Vector2.right + effectOffset;
                break;
        }

        return offset;
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
        if (EffectedEntity is Player player)
        {
            player.playerPanel.effectUI.RemoveEffect(this);
        }
        //effectedEntity.statusEffects.Remove(this);
        Expired = true;
    }
}