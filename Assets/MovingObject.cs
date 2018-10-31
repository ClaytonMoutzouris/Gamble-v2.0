using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Map mMap;
    public Transform mTransform;
    public Vector2 mOldPosition;
    public Vector2 mPosition;

    public Vector2 mOldSpeed;
    public Vector2 mSpeed;

    public Vector2 mScale;

    public AABB mAABB;
    public Vector2 mAABBOffset;

    public bool mPushedRightWall;
    public bool mPushesRightWall;

    public bool mPushedLeftWall;
    public bool mPushesLeftWall;

    public bool mWasOnGround;
    public bool mOnGround;

    public bool mWasAtCeiling;
    public bool mAtCeiling;

    public bool mOnOneWayPlatform = false;

    public void UpdatePhysics()
    {
        //assign the previous state of onGround, atCeiling, pushesRightWall, pushesLeftWall
        //before those get recalculated for this frame
        mWasOnGround = mOnGround;
        mPushedRightWall = mPushesRightWall;
        mPushedLeftWall = mPushesLeftWall;
        mWasAtCeiling = mAtCeiling;

        mOnOneWayPlatform = false;
        //save the speed to oldSpeed vector
        mOldSpeed = mSpeed;

        //save the position to the oldPosition vector
        mOldPosition = mPosition;

        //integrate the movement only if we're not tweening
        mPosition += mSpeed * Time.deltaTime;

        float groundY = 0;
        if (mSpeed.y <= 0.0f && HasGround(mOldPosition, mPosition, mSpeed, out groundY, ref mOnOneWayPlatform))
        {
            mPosition.y = groundY + mAABB.halfSize.y - mAABBOffset.y;
            mSpeed.y = 0.0f;
            mOnGround = true;
        }
        else
        { 
        mOnGround = false;
        }

        //update the aabb
        mAABB.center = mPosition + mAABBOffset;

        //apply the changes to the transform
        mTransform.localScale = new Vector3(mScale.x, mScale.y, 1.0f);
        mTransform.position = new Vector3(Mathf.Round(mPosition.x), Mathf.Round(mPosition.y), -1.0f);
    }

    public bool HasGround(Vector2 oldPosition, Vector2 position, Vector2 speed, out float groundY, ref bool onOneWayPlatform)
    {
        var oldCenter = oldPosition + mAABBOffset;
        var center = position + mAABBOffset;

        groundY = 0;

        var oldBottomLeft = oldCenter - mAABB.halfSize - Vector2.up + Vector2.right;
        var newBottomLeft = center - mAABB.halfSize - Vector2.up + Vector2.right;
        var newBottomRight = new Vector2(newBottomLeft.x + mAABB.halfSize.x * 2.0f - 2.0f, newBottomLeft.y);

        int endY = mMap.GetMapTileYAtPoint(newBottomLeft.y);
        int begY = Mathf.Max(mMap.GetMapTileYAtPoint(oldBottomLeft.y) - 1, endY);
        int dist = Mathf.Max(Mathf.Abs(endY - begY), 1);

        int tileIndexX;

        for (int tileIndexY = begY; tileIndexY >= endY; --tileIndexY)
        {
            var bottomLeft = Vector2.Lerp(newBottomLeft, oldBottomLeft, (float)Mathf.Abs(endY - tileIndexY) / dist);
            var bottomRight = new Vector2(bottomLeft.x + mAABB.halfSize.x * 2.0f - 2.0f, bottomLeft.y);

            for (var checkedTile = bottomLeft; ; checkedTile.x += Map.cTileSize)
            {
                checkedTile.x = Mathf.Min(checkedTile.x, bottomRight.x);

                tileIndexX = mMap.GetMapTileXAtPoint(checkedTile.x);

                groundY = (float)tileIndexY * Map.cTileSize + Map.cTileSize / 2.0f + mMap.mPosition.y;

                if (mMap.IsObstacle(tileIndexX, tileIndexY))
                {
                    onOneWayPlatform = false;
                    return true;
                }
                else if (mMap.IsOneWayPlatform(tileIndexX, tileIndexY) && Mathf.Abs(checkedTile.y - groundY) <= Constants.cOneWayPlatformThreshold + mOldPosition.y - position.y)
                    onOneWayPlatform = true;

                if (checkedTile.x >= bottomRight.x)
                {
                    if (onOneWayPlatform)
                        return true;
                    break;
                }
            }
        }
        
        return false;
    }
}