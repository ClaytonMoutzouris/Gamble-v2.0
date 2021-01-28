
using UnityEngine;

public enum PlatformAxis { Vertical, Horizontal };
public class MovingPlatform : Entity
{
    [HideInInspector]
    public bool mWait;
    [HideInInspector]
    public float mTimer = 0.0f;
    [HideInInspector]
    public float mWaitTime = 3.0f;

    PlatformAxis axis;

    public MovingPlatform(EntityPrototype proto) : base(proto)
    {
        mWait = false;

        Body.mIsKinematic = true;
        mMovingSpeed = 50;

        int r = Random.Range(0, 2);
        if(r == 1)
        {
            axis = PlatformAxis.Vertical;
            Body.mSpeed.y = -mMovingSpeed;

        }
        else
        {
            axis = PlatformAxis.Horizontal;
            Body.mSpeed.x = -mMovingSpeed;

        }

    }

    public void VerticalPlatform()
    {
        if (Body.mPS.pushesBottomTile)
            Body.mSpeed.y = mMovingSpeed;

        if (Body.mPS.pushesTopTile)
            Body.mSpeed.y = -mMovingSpeed;
    }

    public void HorizontalPlatform()
    {
        if (Body.mPS.pushesLeftTile)
            Body.mSpeed.x = mMovingSpeed;

        if (Body.mPS.pushesRightTile)
            Body.mSpeed.x = -mMovingSpeed;
    }

    public override void EntityUpdate()
    {
        switch (axis)
        {
            case PlatformAxis.Vertical:
                VerticalPlatform();
                break;
            case PlatformAxis.Horizontal:
                HorizontalPlatform();
                break;
        }

        base.EntityUpdate();
    }
}
