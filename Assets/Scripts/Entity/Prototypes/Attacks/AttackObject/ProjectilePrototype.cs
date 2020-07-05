using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePrototype : AttackObjectPrototype
{
    public bool pierce = false;
    public bool collidesWithTiles = true;
    public bool bouncy = false;

    public int speed = 100;
    public int frameDelay = 0;
}
