using UnityEngine;
using System.Collections;

[System.Serializable]
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


[System.Serializable]
public enum TileContentType
{
    Empty,
    Block,
    OneWay,
    Spikes,
    EnemySpawn,
    Chest,
    

}

public enum TileCollisionType
{
    Empty,
    Block,
    OneWay,
    IceBlock,
    ConveyorRight,
    ConveyorLeft,
    Count,
}



