
using UnityEngine;

public class FallingRock : PhysicsObject
{
    public bool isTriggered = false;
    public float mTriggerTime = 5.0f;
    public float mTimeToTrigger = 0.0f;

    public void Start()
    {
        if (!mMap)
        {
            mMap = Map.instance;
        }

        if (!mGame)
        {
            mGame = Game.instance;
        }
        if (mUpdateId < 0)
        {
            Init();
            //mSpeed.x = 0;
        }
    }

    public override void Init()
    {

        mAABB.HalfSize = new Vector2(15.0f, 15.0f);
        mSpeed = Vector2.zero;
        mIsKinematic = true;
        
        Scale = new Vector2(1.0f, 1.0f);

        base.Init();

    }

    public override void CustomUpdate()
    {
        if (isTriggered)
        {
            mIgnoresGravity = false;
        }

        UpdatePhysics();
    }
}
