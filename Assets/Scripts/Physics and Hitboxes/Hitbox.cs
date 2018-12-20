using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Hitbox
{
    //Entity mEntity;
    public CustomAABB collider;
    public ColliderState state;
    public List<Hurtbox> colliders;

    public List<Vector2i> mAreas = new List<Vector2i>();
    public List<int> mIdsInAreas = new List<int>();

    void UpdateHitbox()
    {
        if(state == ColliderState.Closed) { return; }

        if(colliders.Count > 0)
        {
            state = ColliderState.Colliding;


        } else
        {

        }

    }


}
