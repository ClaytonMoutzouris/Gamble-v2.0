using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType { Player, Enemy, Obstacle, Platform, Object, Projectile, Boss, NPC, Miniboss };
public enum SortingLayerEnum { Default = 0, Background, Object, Boss, Enemy, Player, PlayerWeapon, ItemObject, Projectile };
public enum EnemyType { Slime, Eye, WurmAlien, Hedgehog, Snek, Snowball, Ghost, Treedude, Stag, Snowdrift, FrogLegs, Nest, Crawler, Nipper, PhoenixEgg, Boss=-1, Miniboss=-2 };
public enum NPCType { Standard, Shopkeeper };

public enum BossType { LavaBoss, CatBoss, SharkBoss, HedgehogBoss, TentacleBoss, VoidBoss, Count };
public enum MinibossType { BogBeast }
public enum ObjectType { FallingRock, RollingBoulder, Chest, Item , FlowerBed, Tree, Medbay, Door, NavSystem, BouncePad, Spikes, Iceblock, AnalysisCom, SmallGatherable, LargeGatherable, SaveMachine, BreakableTile };
public enum Rarity { Common, Uncommon, Rare, Legendary, Artifact, Count }
public enum StatusEffectType {  Poisoned, Burned, Frozen, Stunned, KnockedBack };
public enum EquipmentSlotType { Head, Body, Gloves, Boots, Belt, Mainhand, Offhand, Gadget };
public enum CompanionType { Drone, Eyebat };
public enum TalentAbilityType { }
//Hub comes after count, because we dont want to randomly choose the hub (in most cases)
public enum WorldType { Forest, Tundra, Lava, Purple, Yellow, Void, Count, Hub};
public enum MapType {  Hub, World, BossMap };
public enum SurfaceLayer { Above, Surface, Inner, Count };

[System.Serializable]
public enum PlayerClassType { Pilot, Guardian, Medic };
public enum PlayerBackgroundType { Terran, Shroom, Xorpan };
//public enum TalentType {  };

//This is for handling the tilemaps at runtime, based on what type of tile each tile is
//This needs to be split into tiles that matter vs tiles used only for room creation
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
    FallingRock,
    Chest,
    FlowerBed,
    StartTile,
    BreakableTile,
    Water,
    Lava,
    Updraft,
    Gate,
    MinibossSpawn,
    SmallOre,
    LargeOre,
    InvisibleBlock,
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
    MediumEnemy,
    LargeEnemy,
    ObstacleBlock1,
    FallingRock,
    Chest,
    FlowerBed,
    Tree,
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