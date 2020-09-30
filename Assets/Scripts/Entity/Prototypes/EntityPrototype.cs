using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPrototype : ScriptableObject
{
    public EntityType entityType;
    public Hostility hostility;
    public string mName;
    public Vector2 bodySize;
    public List<EntityType> CollidesWith;
    public List<AbilityFlag> abilityFlags;
    public bool ignoreTilemap = false;
    public bool ignoreGravity = false;

    public Sprite sprite;
    public SortingLayerEnum sortingLayer;
    public RuntimeAnimatorController animationController;

    public ParticleSystem particleEffects;
    public int crushDamage = 0;
    public bool kinematic = false;

    public List<Color> colorPallete;

}
