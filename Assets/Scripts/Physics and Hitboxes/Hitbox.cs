using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A hitbox shouldnt know what it does when its hitting something, just that it hit something
public class Hitbox : CustomCollider2D
{

    public List<IHurtable> mCollisions;
    public List<IHurtable> mDealtWith;
    public List<Blockbox> mBlocks;
    public AttackObject attackObject;

    public int Collisions
    {
        get
        {
            return mCollisions.Count;
        }
    }
    public Hitbox(AttackObject entity, CustomAABB aABB) : base(entity, aABB)
    {
        attackObject = entity;
        mCollisions = new List<IHurtable>();
        mDealtWith = new List<IHurtable>();
    }

    public void Blocked()
    {
        mCollisions.Clear();
        mState = ColliderState.Closed;
        attackObject.Blocked();
    }
}
