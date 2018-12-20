using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState { Open, Closed, Colliding }

public enum ColliderType { Unknown, Hitbox, Hurtbox, Pushbox }

[System.Serializable]
public class CustomCollider2D  {

    public ColliderType colliderType;
    public Entity mEntity;
    public CustomAABB mAABB;
    public ColliderState mState;
    public List<CustomCollider2D> mCollisions;
    public List<Vector2i> mAreas = new List<Vector2i>();
    public List<int> mIdsInAreas = new List<int>();

    public CustomCollider2D(Entity entity)
    {
        mCollisions = new List<CustomCollider2D>();
        mAreas = new List<Vector2i>();
        mIdsInAreas = new List<int>();
        mState = ColliderState.Open;
        mEntity = entity;

        mAABB = new CustomAABB();
        mAABB.HalfSize = mEntity.Body.mAABB.HalfSize;
        mAABB.Center = mEntity.Body.mAABB.Center;
        mAABB.Offset = mEntity.Body.mOffset;
    }

    public void UpdatePosition()
    {
        mAABB.Center = mEntity.Body.mAABB.Center;
        mAABB.Scale = mEntity.Body.Scale;
    }

    void UpdateHitbox()
    {
        if (mState == ColliderState.Closed) { return; }

        if (mCollisions.Count > 0)
        {
            mState = ColliderState.Colliding;
        }
        else
        {
            mState = ColliderState.Open;
        }

    }

}
