using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrototype : ScriptableObject
{
    public TileType type;
    public List<Sprite> sprites;
    public RuntimeAnimatorController animationController;

}
