using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A template to use for different types of maps
public class MapData : ScriptableObject {

    public MapType mapType;
    public WorldType type;
    public int sizeX = 100;
    public int sizeY = 100;

    public float gravity = Constants.cDefaultGravity;
    public int baseDepth = 6;
    public int depthVariance = 3;

}
