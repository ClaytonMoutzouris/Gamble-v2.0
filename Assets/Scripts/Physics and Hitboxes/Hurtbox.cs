using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hurtbox : CustomCollider2D
{

    public Hurtbox(Entity entity, CustomAABB aABB) : base(entity, aABB)
    {

    }

}
