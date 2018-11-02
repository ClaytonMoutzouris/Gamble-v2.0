using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    None,
    Player,
    NPC,
}

public class MovingObject : MonoBehaviour
{

    public ObjectType mType;

    public GameManager mGame;
    public Map mMap;
    public Transform mTransform;
    public Vector2 mOldPosition;
    public Vector2 mPosition;

    public Vector2 mOldSpeed;
    public Vector2 mSpeed;

    public List<Vector2i> mAreas = new List<Vector2i>();
    public List<int> mIdsInAreas = new List<int>();
    //protected List<MovingObject> mFilteredObjects = new List<MovingObject>();

    public List<CollisionData> mAllCollidingObjects = new List<CollisionData>();

    private Vector2 mScale;
    public Vector2 Scale
    {
        set
        {
            mScale = value;
            mAABB.scale = new Vector2(Mathf.Abs(value.x), Mathf.Abs(value.y));
        }
        get { return mScale; }
    }
    public float ScaleX
    {
        set
        {
            mScale.x = value;
            mAABB.scale.x = Mathf.Abs(value);
        }
        get { return mScale.x; }
    }
    public float ScaleY
    {
        set
        {
            mScale.y = value;
            mAABB.scale.y = Mathf.Abs(value);
        }
        get { return mScale.y; }
    }

    public AABB mAABB;
    private Vector2 mAABBOffset;
    public Vector2 AABBOffset
    {
        set { mAABBOffset = value; }
        get { return new Vector2(mAABBOffset.x * mScale.x, mAABBOffset.y * mScale.y); }
    }

    public float AABBOffsetX
    {
        set { mAABBOffset.x = value; }
        get { return mAABBOffset.x * mScale.x; }
    }

    public float AABBOffsetY
    {
        set { mAABBOffset.y = value; }
        get { return mAABBOffset.y * mScale.y; }
    }

    //Position State variables

    public PositionState mPS;

    //public bool mSticksToSlope;
    public bool mIsKinematic = false;

    public bool mOnOneWayPlatform = false;

    /*
    public bool mPushesRight = false;
    public bool mPushesLeft = false;
    public bool mPushesBottom = false;
    public bool mPushesTop = false;

    public bool mPushedTop = false;
    public bool mPushedBottom = false;
    public bool mPushedRight = false;
    public bool mPushedLeft = false;

    public bool mPushesLeftObject = false;
    public bool mPushesRightObject = false;
    public bool mPushesBottomObject = false;
    public bool mPushesTopObject = false;

    public bool mPushedLeftObject = false;
    public bool mPushedRightObject = false;
    public bool mPushedBottomObject = false;
    public bool mPushedTopObject = false;

    public bool mPushesRightTile = false;
    public bool mPushesLeftTile = false;
    public bool mPushesBottomTile = false;
    public bool mPushesTopTile = false;

    public bool mPushedTopTile = false;
    public bool mPushedBottomTile = false;
    public bool mPushedRightTile = false;
    public bool mPushedLeftTile = false;
    */
    #region DrawGizmos
    void OnDrawGizmos()
    {
        DrawMovingObjectGizmos();
    }

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    protected void DrawMovingObjectGizmos()
    {
        //calculate the position of the aabb's center
        var aabbPos = transform.position + (Vector3)AABBOffset;

        //draw the aabb rectangle
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(aabbPos, mAABB.HalfSize * 2.0f);

        //draw the ground checking sensor
        Vector2 bottomLeft = aabbPos - new Vector3(mAABB.HalfSizeX, mAABB.HalfSizeY, 0.0f) - Vector3.up + Vector3.right;
        var bottomRight = new Vector2(bottomLeft.x + mAABB.HalfSizeX * 2.0f - 2.0f, bottomLeft.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottomLeft, bottomRight);

        //draw the ceiling checking sensor
        Vector2 topRight = aabbPos + new Vector3(mAABB.HalfSizeX, mAABB.HalfSizeY, 0.0f) + Vector3.up - Vector3.right;
        var topLeft = new Vector2(topRight.x - mAABB.HalfSizeX * 2.0f + 2.0f, topRight.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(topLeft, topRight);

        //draw left wall checking sensor
        bottomLeft = aabbPos - new Vector3(mAABB.HalfSizeX, mAABB.HalfSizeY, 0.0f) - Vector3.right;
        topLeft = bottomLeft;
        topLeft.y += mAABB.HalfSizeY * 2.0f;

        Gizmos.DrawLine(topLeft, bottomLeft);

        //draw right wall checking sensor

        bottomRight = aabbPos + new Vector3(mAABB.HalfSizeX, -mAABB.HalfSizeY, 0.0f) + Vector3.right;
        topRight = bottomRight;
        topRight.y += mAABB.HalfSizeY * 2.0f;

        Gizmos.DrawLine(topRight, bottomRight);
    }
    #endregion

    public bool HasCollisionDataFor(MovingObject other)
    {
        for (int i = 0; i < mAllCollidingObjects.Count; ++i)
        {
            if (mAllCollidingObjects[i].other == other)
                return true;
        }

        return false;
    }

    //Utility function for rounding when checking corners
    Vector2 RoundVector(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public void UpdatePhysics()
    {
        //assign the previous state of onGround, atCeiling, pushesRightWall, pushesLeftWall
        //before those get recalculated for this frame
        mPS.pushedBottom = mPS.pushesBottom;
        mPS.pushedRight = mPS.pushesRight;
        mPS.pushedLeft = mPS.pushesLeft;
        mPS.pushedTop = mPS.pushesTop;

        mPS.pushedBottomTile = mPS.pushesBottomTile;
        mPS.pushedLeftTile = mPS.pushesLeftTile;
        mPS.pushedRightTile = mPS.pushesRightTile;
        mPS.pushedTopTile = mPS.pushesTopTile;

        mPS.pushesBottomTile = mPS.pushesBottom;
        mPS.pushesTopTile = mPS.pushesTop;
        mPS.pushesRightTile = mPS.pushesRight;
        mPS.pushesLeftTile = mPS.pushesLeft;

        mPS.pushesBottomTile = mPS.pushesLeftTile = mPS.pushesRightTile = mPS.pushesTopTile =
        mPS.pushesBottomObject = mPS.pushesLeftObject = mPS.pushesRightObject = mPS.pushesTopObject = false;

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
            if (mOldPosition.x - mAABB.HalfSizeX + AABBOffsetX >= leftWallX)
            {
                mPosition.x = leftWallX + mAABB.HalfSizeX - AABBOffsetX;
                mPS.pushesLeftTile = true;
            }
            mSpeed.x = Mathf.Max(mSpeed.x, 0.0f);
        }
        else
            mPS.pushesLeftTile = false;

        if (mSpeed.x >= 0.0f && CollidesWithRightWall(mOldPosition, mPosition, out rightWallX))
        {
            if (mOldPosition.x + mAABB.HalfSizeX + AABBOffsetX <= rightWallX)
            {
                mPosition.x = rightWallX - mAABB.HalfSizeX - AABBOffsetX;
                mPS.pushesRightTile = true;
            }

            mSpeed.x = Mathf.Min(mSpeed.x, 0.0f);
        }
        else
            mPS.pushesRightTile = false;

        if (mSpeed.y <= 0.0f && HasGround(mOldPosition, mPosition, mSpeed, out groundY, ref mOnOneWayPlatform))
        {
            mPosition.y = groundY + mAABB.HalfSizeY - AABBOffsetY;
            mSpeed.y = 0.0f;
            mPS.pushesBottomTile = true;
        }
        else
            mPS.pushesBottomTile = false;

        if (mSpeed.y >= 0.0f && HasCeiling(mOldPosition, mPosition, out ceilingY))
        {
            mPosition.y = ceilingY - mAABB.HalfSizeY - AABBOffsetY - 1.0f;
            mSpeed.y = 0.0f;
            mPS.pushesTopTile = true;
        }
        else
            mPS.pushesTopTile = false;

        //update the aabb
        mAABB.center = mPosition + AABBOffset;
    }

    public void UpdatePhysicsPartTwo()
    {
        UpdatePhysicsResponse();

        mPS.pushesBottom = mPS.pushesBottomTile || mPS.pushesBottomObject;
        mPS.pushesRight = mPS.pushesRightTile || mPS.pushesRightObject;
        mPS.pushesLeft = mPS.pushesLeftTile || mPS.pushesLeftObject;
        mPS.pushesTop = mPS.pushesTopTile || mPS.pushesTopObject;

        //update the aabb
        mAABB.center = mPosition + AABBOffset;

        //apply the changes to the transform
        transform.position = new Vector3(Mathf.Round(mPosition.x), Mathf.Round(mPosition.y), -1.0f);
        transform.localScale = new Vector3(ScaleX, ScaleY, 1.0f);
    }

    private void UpdatePhysicsResponse()
    {
        if (mIsKinematic)
            return;

        mPS.pushedBottomObject = mPS.pushesBottomObject;
        mPS.pushedRightObject = mPS.pushesRightObject;
        mPS.pushedLeftObject = mPS.pushesLeftObject;
        mPS.pushedTopObject = mPS.pushesTopObject;

        mPS.pushesBottomObject = false;
        mPS.pushesRightObject = false;
        mPS.pushesLeftObject = false;
        mPS.pushesTopObject = false;

        Vector2 offsetSum = Vector2.zero;

        for (int i = 0; i < mAllCollidingObjects.Count; ++i)
        {
            var other = mAllCollidingObjects[i].other;
            var data = mAllCollidingObjects[i];
            var overlap = data.overlap - offsetSum;

            if (overlap.x == 0.0f)
            {
                if (other.mAABB.center.x > mAABB.center.x)
                {
                    mPS.pushesRightObject = true;
                    mSpeed.x = Mathf.Min(mSpeed.x, 0.0f);
                }
                else
                {
                    mPS.pushesLeftObject = true;
                    mSpeed.x = Mathf.Max(mSpeed.x, 0.0f);
                }
                continue;
            }
            else if (overlap.y == 0.0f)
            {
                if (other.mAABB.center.y > mAABB.center.y)
                {
                    mPS.pushesTopObject = true;
                    mSpeed.y = Mathf.Min(mSpeed.y, 0.0f);
                }
                else
                {
                    mPS.pushesBottomObject = true;
                    mSpeed.y = Mathf.Max(mSpeed.y, 0.0f);
                }
                continue;
            }

            Vector2 absSpeed1 = new Vector2(Mathf.Abs(data.pos1.x - data.oldPos1.x), Mathf.Abs(data.pos1.y - data.oldPos1.y));
            Vector2 absSpeed2 = new Vector2(Mathf.Abs(data.pos2.x - data.oldPos2.x), Mathf.Abs(data.pos2.y - data.oldPos2.y));
            Vector2 speedSum = absSpeed1 + absSpeed2;


            float speedRatioX, speedRatioY;

            if (other.mIsKinematic)
                speedRatioX = speedRatioY = 1.0f;
            else
            {
                if (speedSum.x == 0.0f && speedSum.y == 0.0f)
                {
                    speedRatioX = speedRatioY = 0.5f;
                }
                else if (speedSum.x == 0.0f)
                {
                    speedRatioX = 0.5f;
                    speedRatioY = absSpeed1.y / speedSum.y;
                }
                else if (speedSum.y == 0.0f)
                {
                    speedRatioX = absSpeed1.x / speedSum.x;
                    speedRatioY = 0.5f;
                }
                else
                {
                    speedRatioX = absSpeed1.x / speedSum.x;
                    speedRatioY = absSpeed1.y / speedSum.y;
                }
            }

            float offsetX = overlap.x * speedRatioX;
            float offsetY = overlap.y * speedRatioY;

            bool overlappedLastFrameX = Mathf.Abs(data.oldPos1.x - data.oldPos2.x) < mAABB.HalfSizeX + other.mAABB.HalfSizeX;
            bool overlappedLastFrameY = Mathf.Abs(data.oldPos1.y - data.oldPos2.y) < mAABB.HalfSizeY + other.mAABB.HalfSizeY;

            if ((!overlappedLastFrameX && overlappedLastFrameY)
                || (!overlappedLastFrameX && !overlappedLastFrameY && Mathf.Abs(overlap.x) <= Mathf.Abs(overlap.y)))
            {
                mPosition.x += offsetX;
                offsetSum.x += offsetX;

                if (overlap.x < 0.0f)
                {
                    mPS.pushesRightObject = true;
                    mSpeed.x = Mathf.Min(mSpeed.x, 0.0f);
                }
                else
                {
                    mPS.pushesLeftObject = true;
                    mSpeed.x = Mathf.Max(mSpeed.x, 0.0f);
                }
            }
            else //if (!overlappedLastFrameY)//if (Mathf.Abs(data.oldPos1.x - data.oldPos2.x) < mAABB.HalfSizeX + other.mAABB.HalfSizeX)
            {
                mPosition.y += offsetY;
                offsetSum.y += offsetY;

                if (overlap.y < 0.0f)
                {
                    mPS.pushesTopObject = true;
                    mSpeed.y = Mathf.Min(mSpeed.y, 0.0f);
                }
                else
                {
                    mPS.pushesBottomObject = true;
                    mSpeed.y = Mathf.Max(mSpeed.y, 0.0f);
                }
            }
        }
    }

    public bool HasGround(Vector2 oldPosition, Vector2 position, Vector2 speed, out float groundY, ref bool onOneWayPlatform)
    {
        var oldCenter = oldPosition + AABBOffset;
        var center = position + AABBOffset;

        groundY = 0;

        var oldBottomLeft = RoundVector(oldCenter - mAABB.HalfSize - Vector2.up + Vector2.right);

        var newBottomLeft = RoundVector(center - mAABB.HalfSize - Vector2.up + Vector2.right);
        var newBottomRight = RoundVector(new Vector2(newBottomLeft.x + mAABB.HalfSizeX * 2.0f - 2.0f, newBottomLeft.y));

        int endY = mMap.GetMapTileYAtPoint(newBottomLeft.y);
        int begY = Mathf.Max(mMap.GetMapTileYAtPoint(oldBottomLeft.y) - 1, endY);
        int dist = Mathf.Max(Mathf.Abs(endY - begY), 1);

        int tileIndexX;

        for (int tileIndexY = begY; tileIndexY >= endY; --tileIndexY)
        {
            var bottomLeft = Vector2.Lerp(newBottomLeft, oldBottomLeft, (float)Mathf.Abs(endY - tileIndexY) / dist);
            var bottomRight = new Vector2(bottomLeft.x + mAABB.HalfSizeX * 2.0f - 2.0f, bottomLeft.y);

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
        var center = position + AABBOffset;
        var oldCenter = oldPosition + AABBOffset;

        ceilingY = 0.0f;

        var oldTopRight = RoundVector(oldCenter + mAABB.HalfSize + Vector2.up - Vector2.right);

        var newTopRight = RoundVector(center + mAABB.HalfSize + Vector2.up - Vector2.right);
        var newTopLeft = RoundVector(new Vector2(newTopRight.x - mAABB.HalfSizeX * 2.0f + 2.0f, newTopRight.y));

        int endY = mMap.GetMapTileYAtPoint(newTopRight.y);
        int begY = Mathf.Min(mMap.GetMapTileYAtPoint(oldTopRight.y) + 1, endY);
        int dist = Mathf.Max(Mathf.Abs(endY - begY), 1);

        int tileIndexX;

        for (int tileIndexY = begY; tileIndexY <= endY; ++tileIndexY)
        {
            var topRight = Vector2.Lerp(newTopRight, oldTopRight, (float)Mathf.Abs(endY - tileIndexY) / dist);
            var topLeft = new Vector2(topRight.x - mAABB.HalfSizeX * 2.0f + 2.0f, topRight.y);

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
        var center = position + AABBOffset;
        var oldCenter = oldPosition + AABBOffset;

        wallX = 0.0f;

        var oldBottomLeft = RoundVector(oldCenter - mAABB.HalfSize - Vector2.right);

        var newBottomLeft = RoundVector(center - mAABB.HalfSize - Vector2.right);
        var newTopLeft = RoundVector(newBottomLeft + new Vector2(0.0f, mAABB.HalfSizeY * 2.0f));

        int tileIndexY;

        var endX = mMap.GetMapTileXAtPoint(newBottomLeft.x);
        var begX = Mathf.Max(mMap.GetMapTileXAtPoint(oldBottomLeft.x) - 1, endX);
        int dist = Mathf.Max(Mathf.Abs(endX - begX), 1);

        for (int tileIndexX = begX; tileIndexX >= endX; --tileIndexX)
        {
            var bottomLeft = Vector2.Lerp(newBottomLeft, oldBottomLeft, (float)Mathf.Abs(endX - tileIndexX) / dist);
            var topLeft = bottomLeft + new Vector2(0.0f, mAABB.HalfSizeY * 2.0f);

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
        var center = position + AABBOffset;
        var oldCenter = oldPosition + AABBOffset;

        wallX = 0.0f;

        var oldBottomRight = RoundVector(oldCenter + new Vector2(mAABB.HalfSizeX, -mAABB.HalfSizeY) + Vector2.right);

        var newBottomRight = RoundVector(center + new Vector2(mAABB.HalfSizeX, -mAABB.HalfSizeY) + Vector2.right);
        var newTopRight = RoundVector(newBottomRight + new Vector2(0.0f, mAABB.HalfSizeY * 2.0f));

        var endX = mMap.GetMapTileXAtPoint(newBottomRight.x);
        var begX = Mathf.Min(mMap.GetMapTileXAtPoint(oldBottomRight.x) + 1, endX);
        int dist = Mathf.Max(Mathf.Abs(endX - begX), 1);

        int tileIndexY;

        for (int tileIndexX = begX; tileIndexX <= endX; ++tileIndexX)
        {
            var bottomRight = Vector2.Lerp(newBottomRight, oldBottomRight, (float)Mathf.Abs(endX - tileIndexX) / dist);
            var topRight = bottomRight + new Vector2(0.0f, mAABB.HalfSizeY * 2.0f);

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