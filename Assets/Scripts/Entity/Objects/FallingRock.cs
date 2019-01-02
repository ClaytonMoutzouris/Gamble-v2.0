
using UnityEngine;

public class FallingRock : Entity
{
    public bool isTriggered = false;
    public float mTriggerTime = 5.0f;
    public float mTimeToTrigger = 0.0f;


    public override void EntityInit()
    {
        base.EntityInit();
        Body = new PhysicsBody(this, new CustomAABB(transform.position, new Vector2(15.0f, 15.0f), new Vector2(0, 15f), new Vector3(1, 1, 1)));

        Body.mSpeed = Vector2.zero;
        Body.mIsKinematic = true;



    }

    public override void EntityUpdate()
    {
        if (isTriggered)
        {
            Body.mIgnoresGravity = false;
        }

        base.EntityUpdate();
    }
}
