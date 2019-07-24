﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePrototype : EntityPrototype
{
    public bool pierce = false;
    public bool ignoreGravity = true;
    public bool angled = false;
    public float maxTime = 10;

    public AudioClip sfx;
    public int speed = 100;

}
