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

public class ObjectData : EntityData
{
    public ObjectType type;
    public ObjectData(int x, int y, ObjectType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Object;

    }
}