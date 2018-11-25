using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : PhysicsObject {

    public float mMovingSpeed;
    public AABB mHitbox;
    void OnDrawGizmos()
    {
        DrawSlimeGizmos();
        DrawMovingObjectGizmos();
    }

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    protected void DrawSlimeGizmos()
    {
        //calculate the position of the aabb's center
        var aabbPos = (Vector3)mHitbox.Center;

        //draw the aabb rectangle
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(aabbPos, mHitbox.HalfSize * 2.0f); 
       
    }
    public void Start()
    {
        if (mUpdateId < 0)
        {
            Init();
            //mSpeed.x = 0;
        }
    }

    public void Init()
    {
        mPosition = RoundVector(transform.position);

        mHitbox.HalfSize = new Vector2(5.0f, 5.0f);
        mHitbox.Center = mPosition;

        mAABB.HalfSize = new Vector2(10.0f, 5.0f);
        mAABB.Center = mPosition;
        mMovingSpeed = 50.0f;
        mIsKinematic = false;
        int r = Random.Range(0, 2);
            mSpeed.x = mMovingSpeed;
       
        
        Scale = new Vector2(1.0f, 1.0f);
        mAABB.OffsetY = mAABB.HalfSizeY;

        mUpdateId = mGame.AddToUpdateList(this);


    }

    public override void CustomUpdate()
    {
        if (!mPS.pushesBottom)
        {
            mSpeed.y += Constants.cGravity * Time.deltaTime;

            mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
        }


        if (!mPS.pushesRightTile && !mPS.pushedLeftTile)
            mSpeed.x = mMovingSpeed;

        if (mPS.pushesLeftTile || mPS.pushesRightTile)
        {
            mMovingSpeed *= -1;

            mSpeed.x = mMovingSpeed;
        }

        //This is just a test, probably dont need to do it this way
        base.CustomUpdate();
        //UpdatePhysics();

        //make sure the hitbox follows the object
        mHitbox.Center = mAABB.Center;
    }
}
