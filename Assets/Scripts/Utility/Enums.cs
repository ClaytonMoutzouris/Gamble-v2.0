using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType { Player, Enemy, Obstacle, Platform, Object, Projectile, Boss };
public enum EnemyType { Slime, Eye, Roller, WurmAlien, Hedghehog, Treedude, Count, Boss };
public enum BossType { LavaBoss, CatBoss, SharkBoss, HedgehogBoss, Count }
public enum ObjectType { FallingRock, RollingBoulder, Chest, Item };

public enum ItemType { };
public enum EquipmentSlot { Head, Body, Gloves, Boots, LeftHand, RightHand };

//Hub comes after count, because we dont want to randomly choose the hub (in most cases)
public enum WorldType { Forest, Tundra, Lava, Purple, Yellow, Count, Hub };
public enum MapType {  Hub, World, BossMap };
public enum SurfaceLayer { Above, Surface, Inner, Count };

//This is for handling the tilemaps at runtime, based on what type of tile each tile is
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
    SmallEnemy,
    LargeEnemy,
    ObstacleBlock1, //suppose a size of 2x2
    ObstacleBlock2, //suppose size of 2x4
    ObstacleBlock3, //suppose a size of 3x3
    Boss,
    Count,
}

//This enum is for generating levels
public enum RoomTile
{
    Empty,
    Block,
    OneWay,
    Spikes,
    Bounce,
    Ladder,
    LadderTop,
    SmallEnemy,
    LargeEnemy,
    ObstacleBlock1, //suppose a size of 2x2
    ObstacleBlock2, //suppose size of 2x4
    ObstacleBlock3, //suppose a size of 3x3
    Count,

}

//This is for defining smaller portions of rooms, each type should contain an exact definition
public enum TileType3
{

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