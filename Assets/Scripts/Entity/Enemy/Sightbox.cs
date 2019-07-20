using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sightbox : CustomCollider2D
{
    public List<Entity> mEntitiesInSight;

    public Sightbox(Entity entity, CustomAABB aABB) : base(entity, aABB)
    {
        mEntitiesInSight = new List<Entity>();
    }
    
}
