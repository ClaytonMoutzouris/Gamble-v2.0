using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPrototype : ScriptableObject
{
    public EntityType entityType;
    public Hostility hostility;
    public string mName;
    public Vector2 bodySize;
    public Vector2 bodyOffset;

    public List<EntityType> CollidesWith;
    public List<AbilityFlag> abilityFlags;
    public List<Ability> baseAbilities;
    public bool ignoreTilemap = false;
    public bool ignoreGravity = false;
    public Sprite sprite;
    public SortingLayerEnum sortingLayer;
    public RuntimeAnimatorController animationController;

    public ParticleSystem particleEffects;
    public int crushDamage = 0;
    public bool kinematic = false;

    public List<Color> colorPallete;
    public List<ColorSwapNode> colorNodes;
    public float baseGravityMultiplier = 1;
    public List<Stat> stats = new List<Stat>() { new Stat(StatType.Attack, 0), new Stat(StatType.Defense, 0), new Stat(StatType.Constitution, 0), new Stat(StatType.Speed, 0), new Stat(StatType.Luck, 0) };

    public AudioClip[] interactSFX;
}
