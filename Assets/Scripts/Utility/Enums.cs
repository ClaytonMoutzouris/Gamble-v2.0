using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityType { Player, Enemy, Obstacle, Platform, Object, Projectile, Boss, NPC };
public enum SortingLayerEnum { Default = 0, Background, Object, Boss, Enemy, Player, PlayerWeapon, ItemObject, Projectile };
public enum EnemyType { Slime, Eye, WurmAlien, Hedgehog, Snek, Snowball, Ghost, Treedude, Stag, Snowdrift, FrogLegs, Nest, Count, Boss };
public enum NPCType { Standard, Shopkeeper };

public enum BossType { LavaBoss, CatBoss, SharkBoss, HedgehogBoss, TentacleBoss, VoidBoss, Count };
public enum ObjectType { FallingRock, RollingBoulder, Chest, Item , FlowerBed, Tree, Medbay, Door, NavSystem, BouncePad, Spikes, Iceblock };
public enum Rarity { Common, Uncommon, Rare, Legendary, Artifact, Count }
public enum StatusEffectType {  Poisoned, Burned, Frozen, Stunned };
public enum WeaponAbility { };
public enum PlayerAbility { Hover, Invisible, Count };
public enum ItemType { };
public enum EquipmentSlot { Head, Body, Gloves, Boots, Belt, Mainhand, Offhand };
public enum EffectType { ExtraJump, Hover, Lifesteal, DamageReflect, PoisonAttack, StunAttack, SuperSpeed, SpikeProtection, CrushProtection, Flamewalker, Heavy, ExtraDamage, Knockback, CompanionDrone, ChestFinder, StatsFromFood, ReusableMedkits, PartyHeal, Count };
//Hub comes after count, because we dont want to randomly choose the hub (in most cases)
public enum WorldType { Forest, Tundra, Lava, Purple, Yellow, Void, Count, Hub};
public enum MapType {  Hub, World, BossMap };
public enum SurfaceLayer { Above, Surface, Inner, Count };

public enum PlayerClassType { Pilot, Guardian, Medic };
public enum PlayerBackgroundType { Terran, Shroom, Xorpan };
//public enum TalentType {  };

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
    FallingRock,
    Chest,
    FlowerBed,
    StartTile,
    DestructibleBlock,
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