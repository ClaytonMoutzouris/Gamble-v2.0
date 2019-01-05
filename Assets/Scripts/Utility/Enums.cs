using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType { Player, Enemy, Obstacle, Platform, Object, Projectile };
public enum EnemyType { Slime, CrimsonSlime, Eye, Treedude, Count };
public enum ObjectType { FallingRock, RollingBoulder, Chest, Item };

//Hub comes after count, because we dont want to randomly choose the hub (in most cases)
public enum MapType { Forest, Tundra, Lava, Count, Hub };

public enum TileType
{
    Empty,
    Block,
    OneWay,
    Spikes,
    Bounce,
    Ladder,
    LadderTop,
    Door,
    IceBlock,
    ConveyorRight,
    ConveyorLeft,
    Count,
}

public enum CollisionType
{
    None,
    Player,
    NPC,
    Enemy,
    Item,
    Obstacle,
    Platform,
    Chest,
    Projectile,
}

public enum AABBType
{
    Unknown = -1,
    Bounds,
    Hurtbox,
    Hitbox,
    Pushbox,
}