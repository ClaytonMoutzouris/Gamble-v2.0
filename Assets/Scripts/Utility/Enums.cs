using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType { Enemy, Object, Player };
public enum EnemyType { Slime, CrimsonSlime, Count };
public enum ObjectType { FallingRock, RollingBoulder, Chest, Item };

public enum MapType { Forest, Tundra, Count };

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
}