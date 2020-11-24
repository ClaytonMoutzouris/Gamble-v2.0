using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public abstract class EntityData {

    Vector2i tilePosition;
    public EntityType EntityType;
    public EntityData(int x, int y)
    {
        TilePosition = new Vector2i(x, y);
    }

    public EntityData()
    {
        TilePosition = new Vector2i(0, 0);
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

    public virtual void Save(BinaryWriter writer)
    {
        writer.Write((byte)TilePosition.x);
        writer.Write((byte)TilePosition.y);
        writer.Write((byte)EntityType);
    }

    public virtual void Load(BinaryReader reader)
    {
        int x = reader.ReadByte();
        int y = reader.ReadByte();

        tilePosition = new Vector2i(x, y);
        EntityType = (EntityType)reader.ReadByte();
    }

}

[System.Serializable]
public class EnemyData : EntityData
{
    public EnemyType type;
    public EnemyData(int x, int y, EnemyType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Enemy;
    }

    public EnemyData()
    {
        EntityType = EntityType.Enemy;
    }

    public override void Load(BinaryReader reader)
    {
        base.Load(reader);
        type = (EnemyType)reader.ReadByte();
    }

    public override void Save(BinaryWriter writer)
    {
        base.Save(writer);
        writer.Write((byte)type);
    }
}

[System.Serializable]
public class BossData : EntityData
{
    public BossType type;
    public BossData(int x, int y, BossType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Boss;
    }

    public BossData()
    {
        EntityType = EntityType.Boss;
    }

    public override void Load(BinaryReader reader)
    {
        base.Load(reader);
        type = (BossType)reader.ReadByte();

    }

    public override void Save(BinaryWriter writer)
    {
        base.Save(writer);
        writer.Write((byte)type);

    }
}

[System.Serializable]
public class MinibossData : EntityData
{
    public MinibossType type;
    public MinibossData(int x, int y, MinibossType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Miniboss;
    }

    public MinibossData()
    {
        EntityType = EntityType.Miniboss;
    }

    public override void Load(BinaryReader reader)
    {
        base.Load(reader);
        type = (MinibossType)reader.ReadByte();

    }

    public override void Save(BinaryWriter writer)
    {
        base.Save(writer);
        writer.Write((byte)type);

    }
}
[System.Serializable]
public class ObjectData : EntityData
{
    public ObjectType type;
    public ObjectData(int x, int y, ObjectType t) : base(x, y)
    {
        type = t;
        EntityType = EntityType.Object;

    }

    public ObjectData()
    {
        EntityType = EntityType.Object;
    }

    public override void Load(BinaryReader reader)
    {
        base.Load(reader);
        type = (ObjectType)reader.ReadByte();

    }

    public override void Save(BinaryWriter writer)
    {
        base.Save(writer);
        writer.Write((byte)type);

    }
}

[System.Serializable]
public class ItemObjectData : ObjectData
{
    public string itemType;
    public ItemObjectData(int x, int y, ObjectType t, string itemType) : base(x, y, t)
    {
        this.itemType = itemType;
    }

    public override void Load(BinaryReader reader)
    {
        base.Load(reader);
    }

    public override void Save(BinaryWriter writer)
    {
        base.Save(writer);
    }
}

[System.Serializable]
public class NPCData : EntityData
{
    public NPCType type;
    public NPCData(int x, int y, NPCType t) : base(x, y)
    {
        EntityType = EntityType.NPC;
        type = t;
    }

    public NPCData()
    {
        EntityType = EntityType.NPC;
    }

    public override void Load(BinaryReader reader)
    {
        base.Load(reader);
        type = (NPCType)reader.ReadByte();

    }

    public override void Save(BinaryWriter writer)
    {
        base.Save(writer);
        writer.Write((byte)type);
    }
}