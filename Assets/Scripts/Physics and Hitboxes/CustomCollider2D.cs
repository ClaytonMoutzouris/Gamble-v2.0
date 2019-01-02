using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState { Open, Closed, Colliding }

//There is one last type of hitbox to make, that would be vision/aggro
public enum ColliderType { Unknown, Hitbox, Hurtbox, Pushbox }

[System.Serializable]
public abstract class CustomCollider2D {

    //public ColliderType colliderType;
    public Entity mEntity;
    public CustomAABB mAABB;
    public ColliderState mState;
    public List<Vector2i> mAreas = new List<Vector2i>();
    public List<int> mIdsInAreas = new List<int>();

    public CustomCollider2D(Entity entity, CustomAABB aABB)
    {
        mEntity = entity;
        mAABB = aABB;
        mState = ColliderState.Open;
        mAreas = new List<Vector2i>();
        mIdsInAreas = new List<int>();

    }
    


    public void UpdatePosition()
    {
        //This should make sure the collider moves with whatever object it is attached to
        //and also keeps its scale correctly (though scaling isnt used yet)
        mAABB.Center = mEntity.Position;
        mAABB.Scale = mEntity.Scale;
    }

}
