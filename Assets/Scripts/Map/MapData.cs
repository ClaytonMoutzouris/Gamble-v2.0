using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A template to use for different types of maps
public class MapData : ScriptableObject {

    public MapType mapType;
    public WorldType type;
    public int sizeX = 10;
    public int sizeY = 10;
    public bool hasMiniboss = false;
    //Only matters for non boss
    public int roomSizeX = 10;
    public int roomSizeY = 10;
    public float gravity = Constants.cDefaultGravity;
    public int baseDepth = 6;
    public int depthVariance = 3;
    public List<EnemyType> smallEnemies;
    public List<EnemyType> largeEnemies;

    public List<Sprite> tileSprites;
    public LootTable chestLoot;
}
