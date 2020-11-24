using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Entity
{
    public Blockbox shieldBox;
    Player owner;
    ForceFieldGadget gadget;
    Vector2 ownerOffset;

    public ForceField(EntityPrototype proto, Player owner, Vector2 offset, ForceFieldGadget gadget) : base(proto)
    {
        this.owner = owner;
        this.gadget = gadget;
        ownerOffset = offset;
        mMovingSpeed = 0;
        Body.mIgnoresGravity = true;
        shieldBox = new Blockbox(this, new CustomAABB(Position, proto.bodySize, new Vector2(0, proto.bodySize.y)));
        shieldBox.UpdatePosition();
    }

    public override void Die()
    {
        base.Die();
        gadget.ForceField = null;
        gadget.isActive = false;
        shieldBox.mState = ColliderState.Closed;

    }

    public override void ActuallyDie()
    {

        //before we remove it from the update list, we have to remove it from the update areas
        CollisionManager.RemoveObjectFromAreas(shieldBox);
        base.ActuallyDie();
    }

    public override void EntityUpdate()
    {

        foreach (Hitbox hit in shieldBox.mCollisions)
        {
            //Debug.Log("Something in the collisions");
            if (!shieldBox.mDealtWith.Contains(hit))
            {
                hit.mState = ColliderState.Closed;
                hit.mCollisions.Clear();
                shieldBox.mDealtWith.Add(hit);

            }
        }

        Position = owner.Position;


        base.EntityUpdate();

        CollisionManager.UpdateAreas(shieldBox);

    }

    public override void SecondUpdate()
    {
        base.SecondUpdate();
        Position = owner.Position;
        shieldBox.UpdatePosition();
    }
}
