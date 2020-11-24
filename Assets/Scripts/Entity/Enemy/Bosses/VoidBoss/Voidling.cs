using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voidling : Enemy
{
    public Entity host;
    public Vector2 hostOffset;

    public Voidling(EnemyPrototype proto) : base(proto)
    {
        Body.mIgnoresOneWay = true;

    }

    public override void EntityUpdate()
    {

        switch (host != null)
        {
            case true:
                HostUpdate();
                break;

            case false:
                Body.mState = ColliderState.Open;
                NoHostUpdate();
                break;
        }



        base.EntityUpdate();


    }

    public void HostUpdate()
    {
        Body.mSpeed = Vector2.zero;
        Position = host.Position + hostOffset;
        mAttackManager.meleeAttacks[0].Activate();

    }

    public void NoHostUpdate()
    {
        foreach (CollisionData col in Body.mCollisions)
        {
            if (col.other.mEntity is IHurtable && col.other.mEntity.hostility != Hostility)
            {
                host = col.other.mEntity;
                hostOffset = (col.pos1 - col.pos2) * 0.5f;
                Body.mState = ColliderState.Closed;
            }
        }

        EnemyBehaviour.CheckForTargets(this);

        if (Target != null)
        {

            Body.mSpeed = ((Vector2)Target.Position - Position).normalized * GetMovementSpeed();

        }
        else
        {
            Body.mSpeed = Vector2.zero;

        }
    }


}
