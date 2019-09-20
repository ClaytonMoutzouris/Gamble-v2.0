using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPrototype : ScriptableObject
{
    public EntityType entityType;
    public string mName;
    public Vector2 bodySize;
    public List<EntityType> CollidesWith;
    public bool ignoreTilemap = false;

    public Sprite sprite;
    public SortingLayerEnum sortingLayer;
    public RuntimeAnimatorController animationController;

}
