using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

    public Entity mEntity;
    public CustomAABB collider;
    private ColliderState mState = ColliderState.Open;

    public void GetHit(Attack attack)
    {
        Debug.Log(mEntity.name + " has been hit by an attack.");
    }

    void OnDrawGizmos()
    {
        DrawHitboxGizmos();
    }

    protected void DrawHitboxGizmos()
    {
        //calculate the position of the aabb's center
        var aabbPos = collider.Center;

        //draw the aabb rectangle
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(aabbPos, collider.HalfSize * 2.1f);


    }

}
