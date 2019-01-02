using UnityEngine;
using System.Collections;

//This is better known as a physics object Collision Data, but right now hurt and hitboxes use this class but make no use of most of the info.
[System.Serializable]
public class CollisionData
{
    public CollisionData(CustomCollider2D other, Vector2 overlap = default(Vector2), Vector2 speed1 = default(Vector2), Vector2 speed2 = default(Vector2), Vector2 oldPos1 = default(Vector2), Vector2 oldPos2 = default(Vector2), Vector2 pos1 = default(Vector2), Vector2 pos2 = default(Vector2))
    {
        this.other = other;
        this.overlap = overlap;
        this.speed1 = speed1;
        this.speed2 = speed2;
        this.oldPos1 = oldPos1;
        this.oldPos2 = oldPos2;
        this.pos1 = pos1;
        this.pos2 = pos2;
    }

    public CustomCollider2D other;
    public Vector2 overlap;
    public Vector2 speed1, speed2;
    public Vector2 oldPos1, oldPos2, pos1, pos2;
}


//If it only stores what got hit, do we really need this?

public class HitData
{
    public IHurtable other;

    public HitData(IHurtable other)
    {
        this.other = other;
    }

}

