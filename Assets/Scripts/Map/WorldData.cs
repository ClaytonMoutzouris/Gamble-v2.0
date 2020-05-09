using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : ScriptableObject
{
    public string worldName;
    public WorldType type;
    public string worldDescription;
    public List<MapData> maps;

    public float gravity = Constants.cDefaultGravity;
    public List<Sprite> tileSprites;

}
