using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapData : ScriptableObject {

    public MapType mapType;
    public WorldType type;
    public int sizeX = 100;
    public int sizeY = 100;

    public float gravity = Constants.cDefaultGravity;
    public int depth = 0;

}
