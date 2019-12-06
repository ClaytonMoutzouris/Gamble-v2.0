using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockbox : CustomCollider2D
{

    public List<Hitbox> mCollisions;
    public List<Hitbox> mDealtWith;

    public int Collisions
    {
        get
        {
            return mCollisions.Count;
        }
    }
    public Blockbox(Entity entity, CustomAABB aABB) : base(entity, aABB)
    {
        mCollisions = new List<Hitbox>();
        mDealtWith = new List<Hitbox>();
    }

    public override void UpdatePosition()
    {
        mAABB.ScaleX = (int)mEntity.mDirection;
        mAABB.Center = mEntity.Position;
    }
}

