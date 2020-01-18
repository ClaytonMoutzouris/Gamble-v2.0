using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectPrototype : EntityPrototype
{
    public Vector2 hitboxSize;

    public bool angled = false;
    public float maxTime = 1;
    public AudioClip sfx;

    public ParticleSystem particleSystem;

}
