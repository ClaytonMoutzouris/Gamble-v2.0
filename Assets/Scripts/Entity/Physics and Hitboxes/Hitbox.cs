using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState { Open, Closed, Colliding }

[System.Serializable]
public class Hitbox
{

    public CustomAABB collider;
    public ColliderState state;




}
