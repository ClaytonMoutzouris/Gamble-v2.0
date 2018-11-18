
using UnityEngine;

public class FallingRock : MovingObject
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

    public void Init()
    {
        Vector2i temp = mMap.GetMapTileAtPoint(transform.position);
        transform.position = mMap.GetMapTilePosition(temp);
        mPosition = transform.position;

        mAABB.HalfSize = new Vector2(15.0f, 15.0f);
        mAABB.Center = mPosition;
        mSlopeWallHeight = 0;
        mSpeed = Vector2.zero;
            mIsKinematic = true;
        
        Scale = new Vector2(1.0f, 1.0f);

        mUpdateId = mGame.AddToUpdateList(this);


    }

    public void CustomUpdate()
    {
        if (isTriggered)
        {
            mSpeed.y = Constants.cMaxFallingSpeed;
        } else if(mTriggerTime > mTimeToTrigger)
        {
            mTimeToTrigger += Time.deltaTime;
        } else
        {
            isTriggered = true;
        }

        UpdatePhysics();
    }
}
