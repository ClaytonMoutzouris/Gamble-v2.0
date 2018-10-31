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

    //Utility function for rounding when checking corners
    Vector2 RoundVector(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

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

        float groundY = 0.0f, ceilingY = 0.0f;
        float rightWallX = 0.0f, leftWallX = 0.0f;

        if (mSpeed.x <= 0.0f && CollidesWithLeftWall(mOldPosition, mPosition, out leftWallX))
        {
            if (mOldPosition.x - mAABB.halfSize.x + mAABBOffset.x >= leftWallX)
            {
                mPosition.x = leftWallX + mAABB.halfSize.x - mAABBOffset.x;
                mPushesLeftWall = true;
            }
            mSpeed.x = Mathf.Max(mSpeed.x, 0.0f);
        }
        else
        {
            mPushesLeftWall = false;
        }

        if (mSpeed.x >= 0.0f && CollidesWithRightWall(mOldPosition, mPosition, out rightWallX))
        {
            if (mOldPosition.x + mAABB.halfSize.x + mAABBOffset.x <= rightWallX)
            {
                mPosition.x = rightWallX - mAABB.halfSize.x - mAABBOffset.x;
                mPushesRightWall = true;
            }

            mSpeed.x = Mathf.Min(mSpeed.x, 0.0f);
        }
        else
        {
            mPushesRightWall = false;
        }

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

        if (mSpeed.y >= 0.0f && HasCeiling(mOldPosition, mPosition, out ceilingY))
        {
            mPosition.y = ceilingY - mAABB.halfSize.y - mAABBOffset.y - 1.0f;
            mSpeed.y = 0.0f;
            mAtCeiling = true;
        }
        else
        {
            mAtCeiling = false;
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

        var oldBottomLeft = RoundVector(oldCenter - mAABB.halfSize - Vector2.up + Vector2.right);

        var newBottomLeft = RoundVector(center - mAABB.halfSize - Vector2.up + Vector2.right);
        var newBottomRight = RoundVector(new Vector2(newBottomLeft.x + mAABB.halfSize.x * 2.0f - 2.0f, newBottomLeft.y));

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

    public bool HasCeiling(Vector2 oldPosition, Vector2 position, out float ceilingY)
    {
        var center = position + mAABBOffset;
        var oldCenter = oldPosition + mAABBOffset;

        ceilingY = 0.0f;

        var oldTopRight = RoundVector(oldCenter + mAABB.halfSize + Vector2.up - Vector2.right);

        var newTopRight = RoundVector(center + mAABB.halfSize + Vector2.up - Vector2.right);
        var newTopLeft = RoundVector(new Vector2(newTopRight.x - mAABB.halfSize.x * 2.0f + 2.0f, newTopRight.y));

        int endY = mMap.GetMapTileYAtPoint(newTopRight.y);
        int begY = Mathf.Min(mMap.GetMapTileYAtPoint(oldTopRight.y) + 1, endY);
        int dist = Mathf.Max(Mathf.Abs(endY - begY), 1);

        int tileIndexX;

        for (int tileIndexY = begY; tileIndexY <= endY; ++tileIndexY)
        {
            var topRight = Vector2.Lerp(newTopRight, oldTopRight, (float)Mathf.Abs(endY - tileIndexY) / dist);
            var topLeft = new Vector2(topRight.x - mAABB.halfSize.x * 2.0f + 2.0f, topRight.y);

            for (var checkedTile = topLeft; ; checkedTile.x += Map.cTileSize)
            {
                checkedTile.x = Mathf.Min(checkedTile.x, topRight.x);

                tileIndexX = mMap.GetMapTileXAtPoint(checkedTile.x);

                if (mMap.IsObstacle(tileIndexX, tileIndexY))
                {
                    ceilingY = (float)tileIndexY * Map.cTileSize - Map.cTileSize / 2.0f + mMap.mPosition.y;
                    return true;
                }

                if (checkedTile.x >= topRight.x)
                    break;
            }
        }

        return false;
    }

    public bool CollidesWithLeftWall(Vector2 oldPosition, Vector2 position, out float wallX)
    {
        var center = position + mAABBOffset;
        var oldCenter = oldPosition + mAABBOffset;

        wallX = 0.0f;

        var oldBottomLeft = RoundVector(oldCenter - mAABB.halfSize - Vector2.right);

        var newBottomLeft = RoundVector(center - mAABB.halfSize - Vector2.right);
        var newTopLeft = RoundVector(newBottomLeft + new Vector2(0.0f, mAABB.halfSize.y * 2.0f));

        int tileIndexY;

        var endX = mMap.GetMapTileXAtPoint(newBottomLeft.x);
        var begX = Mathf.Max(mMap.GetMapTileXAtPoint(oldBottomLeft.x) - 1, endX);
        int dist = Mathf.Max(Mathf.Abs(endX - begX), 1);

        for (int tileIndexX = begX; tileIndexX >= endX; --tileIndexX)
        {
            var bottomLeft = Vector2.Lerp(newBottomLeft, oldBottomLeft, (float)Mathf.Abs(endX - tileIndexX) / dist);
            var topLeft = bottomLeft + new Vector2(0.0f, mAABB.halfSize.y * 2.0f);

            for (var checkedTile = bottomLeft; ; checkedTile.y += Map.cTileSize)
            {
                checkedTile.y = Mathf.Min(checkedTile.y, topLeft.y);

                tileIndexY = mMap.GetMapTileYAtPoint(checkedTile.y);

                if (mMap.IsObstacle(tileIndexX, tileIndexY))
                {
                    wallX = (float)tileIndexX * Map.cTileSize + Map.cTileSize / 2.0f + mMap.mPosition.x;
                    return true;
                }

                if (checkedTile.y >= topLeft.y)
                    break;
            }
        }

        return false;
    }

    public bool CollidesWithRightWall(Vector2 oldPosition, Vector2 position, out float wallX)
    {
        var center = position + mAABBOffset;
        var oldCenter = oldPosition + mAABBOffset;

        wallX = 0.0f;

        var oldBottomRight = RoundVector(oldCenter + new Vector2(mAABB.halfSize.x, -mAABB.halfSize.y) + Vector2.right);

        var newBottomRight = RoundVector(center + new Vector2(mAABB.halfSize.x, -mAABB.halfSize.y) + Vector2.right);
        var newTopRight = RoundVector(newBottomRight + new Vector2(0.0f, mAABB.halfSize.y * 2.0f));

        var endX = mMap.GetMapTileXAtPoint(newBottomRight.x);
        var begX = Mathf.Min(mMap.GetMapTileXAtPoint(oldBottomRight.x) + 1, endX);
        int dist = Mathf.Max(Mathf.Abs(endX - begX), 1);

        int tileIndexY;

        for (int tileIndexX = begX; tileIndexX <= endX; ++tileIndexX)
        {
            var bottomRight = Vector2.Lerp(newBottomRight, oldBottomRight, (float)Mathf.Abs(endX - tileIndexX) / dist);
            var topRight = bottomRight + new Vector2(0.0f, mAABB.halfSize.y * 2.0f);

            for (var checkedTile = bottomRight; ; checkedTile.y += Map.cTileSize)
            {
                checkedTile.y = Mathf.Min(checkedTile.y, topRight.y);

                tileIndexY = mMap.GetMapTileYAtPoint(checkedTile.y);

                if (mMap.IsObstacle(tileIndexX, tileIndexY))
                {
                    wallX = (float)tileIndexX * Map.cTileSize - Map.cTileSize / 2.0f + mMap.mPosition.x;
                    return true;
                }

                if (checkedTile.y >= topRight.y)
                    break;
            }
        }

        return false;
    }
}