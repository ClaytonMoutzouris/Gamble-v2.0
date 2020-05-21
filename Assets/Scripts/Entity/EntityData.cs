using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EntityData {

    Vector2i tilePosition;
    public EntityType EntityType;
    public EntityData(int x, int y)
    {
        TilePosition = new Vector2i(x, y);
    }

    public Vector2i TilePosition
    {
        get
        {
            return tilePosition;
        }

        set
        {
            tilePosition = value;
        }
    }

}

public class EnemyData : EntityData
{
    public EnemyType type;
    public EnemyData(int x, int y, EnemyType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Enemy;
    }
}

public class BossData : EntityData
{
    public BossType type;
    public BossData(int x, int y, BossType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Boss;
    }
}

public class MinibossData : EntityData
{
    public MinibossType type;
    public MinibossData(int x, int y, MinibossType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Miniboss;
    }
}

public class ObjectData : EntityData
{
    public ObjectType type;
    public ObjectData(int x, int y, ObjectType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Object;

    }
}

public class ItemObjectData : ObjectData
{
    public string itemType;
    public ItemObjectData(int x, int y, ObjectType t, string itemType) : base(x, y, t)
    {
        this.itemType = itemType;
    }
}

public class NPCData : EntityData
{
    public NPCType type;
    public NPCData(int x, int y, NPCType t) : base(x, y)
    {
        EntityType = EntityType.NPC;
        type = t;
    }
}