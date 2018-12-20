
using UnityEngine;

public class FallingRock : Entity
{
    public bool isTriggered = false;
    public float mTriggerTime = 5.0f;
    public float mTimeToTrigger = 0.0f;


    public override void EntityInit()
    {

        Body.mAABB.HalfSize = new Vector2(15.0f, 15.0f);
        Body.mSpeed = Vector2.zero;
        Body.mIsKinematic = true;


        base.EntityInit();

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
