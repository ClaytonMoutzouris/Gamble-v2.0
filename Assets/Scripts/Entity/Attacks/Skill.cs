using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public Entity owner;

    public Skill(Entity entity)
    {
        owner = entity;
    }

    public virtual void Activate()
    {

    }
}
