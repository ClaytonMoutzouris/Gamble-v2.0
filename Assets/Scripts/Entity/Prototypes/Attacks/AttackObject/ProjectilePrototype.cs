using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePrototype : AttackObjectPrototype
{
    public bool pierce = false;
    public bool ignoreGravity = true;
    public bool collidesWithTiles = true;

    public int speed = 100;

}
