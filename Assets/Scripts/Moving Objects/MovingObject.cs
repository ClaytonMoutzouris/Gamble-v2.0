using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ObjectType
{
    None,
    Player,
    NPC,
    MovingPlatform,
    Enemy,
    FallingRock,
    RollingBoulder,
}

public class MovingObject : MonoBehaviour 
{
    public ObjectType mType;
    /// <summary>
    /// The previous position.
    /// </summary>
    public Vector2 mOldPosition;

    public int mSlopeWallHeight;
    /// <summary>
    /// The current position.
    /// </summary>
    public Vector2 mPosition;
    public Vector2 mReminder;
    public Vector2 mOffset;

    public int mUpdateId = -1;

    private Vector2 mScale;
    public Vector2 Scale
    {
        set {
            mScale = value;
            mAABB.Scale = new Vector2(Mathf.Abs(value.x), Mathf.Abs(value.y));
        }
        get { return mScale; }
    }
    public float ScaleX
    {
        set
        {
            mScale.x = value;
            mAABB.ScaleX = Mathf.Abs(value);
        }
        get { return mScale.x; }
    }
    public float ScaleY
    {
        set
        {
            mScale.y = value;
            mAABB.ScaleY = Mathf.Abs(value);
        }
        get { return mScale.y; }
    }

    /// <summary>
    /// The current speed in pixels/second.
    /// </summary>
    public Vector2 mSpeed;
	
	/// <summary>
	/// The previous speed in pixels/second.
	/// </summary>
	public Vector2 mOldSpeed;

    [Serializable]
    public struct PositionState
    {
        public bool pushesRight;
        public bool pushesLeft;
        public bool pushesBottom;
        public bool pushesTop;

        public bool pushedTop;
        public bool pushedBottom;
        public bool pushedRight;
        public bool pushedLeft;

        public bool pushedLeftObject;
        public bool pushedRightObject;
        public bool pushedBottomObject;
        public bool pushedTopObject;

        public bool pushesLeftObject;
        public bool pushesRightObject;
        public bool pushesBottomObject;
        public bool pushesTopObject;

        public bool pushedLeftTile;
        public bool pushedRightTile;
        public bool pushedBottomTile;
        public bool pushedTopTile;

        public bool pushesLeftTile;
        public bool pushesRightTile;
        public bool pushesBottomTile;
        public bool pushesTopTile;

        public bool onOneWay;
        public bool tmpIgnoresOneWay;
        public bool tmpSticksToSlope;
        public int oneWayY;
        public bool onLadder;
        public bool isClimbing;
        public bool isBounce;

        public Vector2i leftTile;
        public Vector2i rightTile;
        public Vector2i topTile;
        public Vector2i bottomTile;

        public void Reset()
        {
            leftTile = rightTile = topTile = bottomTile = new Vector2i(-1, -1);
            oneWayY = -1;

            pushesRight = false;
            pushesLeft = false;
            pushesBottom = false;
            pushesTop = false;

            pushedTop = false;
            pushedBottom = false;
            pushedRight = false;
            pushedLeft = false;

            pushedLeftObject = false;
            pushedRightObject = false;
            pushedBottomObject = false;
            pushedTopObject = false;

            pushesLeftObject = false;
            pushesRightObject = false;
            pushesBottomObject = false;
            pushesTopObject = false;

            pushedLeftTile = false;
            pushedRightTile = false;
            pushedBottomTile = false;
            pushedTopTile = false;

            pushesLeftTile = false;
            pushesRightTile = false;
            pushesBottomTile = false;
            pushesTopTile = false;

            onOneWay = false;
            onLadder = false;
            isClimbing = false;
            isBounce = false;
        }
    }
    /// <summary>
    /// The AABB for collision queries.
    /// </summary>
    public AABB mAABB;

    public PositionState mPS;

    public Game mGame;
    public Map mMap;

    public MovingObject mMountParent = null;

    public bool mIgnoresOneWay = false;
    public bool mOnOneWayPlatform = false;
    public bool mSticksToSlope = true;
    public bool mIsKinematic = false;

    [NonSerialized]
    public List<Vector2i> mAreas = new List<Vector2i>();
    [NonSerialized]
    public List<int> mIdsInAreas = new List<int>();
    [NonSerialized]
    protected List<MovingObject> mFilteredObjects = new List<MovingObject>();
    [NonSerialized]
    public List<CollisionData> mAllCollidingObjects = new List<CollisionData>();
    /// <summary>
    /// Depth for z-ordering the sprites.
    /// </summary>
    public float mSpriteDepth = -1.0f;

    public Transform mTransform;

	void OnDrawGizmos()
	{
		DrawMovingObjectGizmos ();
	}

    /// <summary>
    /// Draws the aabb and ceiling, ground and wall sensors .
    /// </summary>
    protected void DrawMovingObjectGizmos()
	{
		//calculate the position of the aabb's center
		var aabbPos = (Vector3)mAABB.Center;
		
		//draw the aabb rectangle
		Gizmos.color = Color.yellow;
   		Gizmos.DrawWireCube(aabbPos, mAABB.HalfSize*2.0f);
		
		//draw the ground checking sensor
		Vector2 bottomLeft = aabbPos - new Vector3(mAABB.HalfSizeX, mAABB.HalfSizeY, 0.0f) - Vector3.up + Vector3.right;
		var bottomRight = new Vector2(bottomLeft.x + mAABB.HalfSizeX*2.0f - 2.0f, bottomLeft.y);
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(bottomLeft, bottomRight);
		
		//draw the ceiling checking sensor
		Vector2 topRight = aabbPos + new Vector3(mAABB.HalfSizeX, mAABB.HalfSizeY, 0.0f) + Vector3.up - Vector3.right;
		var topLeft = new Vector2(topRight.x - mAABB.HalfSizeX*2.0f + 2.0f, topRight.y);
		
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

    public bool HasCollisionDataFor(MovingObject other)
    {
        for (int i = 0; i < mAllCollidingObjects.Count; ++i)
        {
            if (mAllCollidingObjects[i].other == other)
                return true;
        }

        return false;
    }

    public Vector2 RoundVector(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public void CollidesWithTiles(ref Vector2 position, ref Vector2 topRight, ref Vector2 bottomLeft, ref PositionState state)
    {
        Vector2 pos = position, tr = topRight, bl = bottomLeft;
        CollidesWithTileTop(ref position, ref topRight, ref bottomLeft, ref state);
        CollidesWithTileBottom(ref position, ref topRight, ref bottomLeft, ref state);
        CollidesWithTileLeft(ref pos, ref tr, ref bl, ref state);
        CollidesWithTileRight(ref pos, ref tr, ref bl, ref state);
    }

    public bool CollidesWithTileRight(ref Vector2 position, ref Vector2 topRight, ref Vector2 bottomLeft, ref PositionState state, bool move = false)
    {
        Vector2i topRightTile = mMap.GetMapTileAtPoint(new Vector2(topRight.x + 0.5f, topRight.y - 0.5f));
        Vector2i bottomLeftTile = mMap.GetMapTileAtPoint(new Vector2(bottomLeft.x + 0.5f, bottomLeft.y + 0.5f));

        //mPS.rightInOneWay = false;


        for (int y = bottomLeftTile.y; y <= topRightTile.y; ++y)
        {
            var tileCollisionType = mMap.GetTile(topRightTile.x, y);

            switch (tileCollisionType)
            {
                default://slope
                    break;
                case TileType.Empty:
                    break;
                case TileType.OneWay:

                    

                    //This is a temporary fix to a problem where the player will stop in the middle of a one way platform
                    //It's a bit of a hack job but it works well enough
                    //NOTE: This doesnt work when dealing with edge cases


                    // mPS.rightInOneWay = true;
                    //if (mPS.leftInOneWay && mPS.rightInOneWay)
                    //   mPS.tmpIgnoresOneWay = true;
                    break;
                case TileType.LadderTop:
                case TileType.Ladder:
                    //If the players center is on the ladder tile, we can climb it
                    if (mMap.GetMapTilePosition(topRightTile.x, y) == mMap.GetMapTilePosition(mMap.GetMapTileAtPoint(mAABB.Center)))
                        mPS.onLadder = true;
                    break;
                case TileType.Block:
                    state.pushesRightTile = true;
                    state.rightTile = new Vector2i(topRightTile.x, y);
                    return true;
            }
        }

        return false;
    }

    public bool CollidesWithTileLeft(ref Vector2 position, ref Vector2 topRight, ref Vector2 bottomLeft, ref PositionState state, bool move = false)
    {
        Vector2i topRightTile = mMap.GetMapTileAtPoint(new Vector2(topRight.x - 0.5f, topRight.y - 0.5f));
        Vector2i bottomLeftTile = mMap.GetMapTileAtPoint(new Vector2(bottomLeft.x - 0.5f, bottomLeft.y + 0.5f));
        //mPS.leftInOneWay = false;


        for (int y = bottomLeftTile.y; y <= topRightTile.y; ++y)
        {
            var tileCollisionType = mMap.GetTile(bottomLeftTile.x, y);


            switch (tileCollisionType)
            {
                default://slope
                    break;
                case TileType.Empty:
                    break;
                case TileType.OneWay:
                    
                    //This is a temporary fix to a problem where the player will stop in the middle of a one way platform
                    //It's a bit of a hack job but it works well enough
                    //NOTE: This doesnt work when dealing with edge cases

                    // mPS.leftInOneWay = true;
                    //if (mPS.leftInOneWay && mPS.rightInOneWay)
                    //   mPS.tmpIgnoresOneWay = true;
                    break;
                case TileType.LadderTop:
                case TileType.Ladder:
                    //If the players center is on the ladder tile, we can climb it
                    if (mMap.GetMapTilePosition(bottomLeftTile.x, y) == mMap.GetMapTilePosition(mMap.GetMapTileAtPoint(mAABB.Center)))
                        mPS.onLadder = true;
                    break;
                case TileType.Block:
                    state.pushesLeftTile = true;
                    state.leftTile = new Vector2i(bottomLeftTile.x, y);
                    return true;
            }
        }

        return false;
    }

    public bool CollidesWithTileTop(ref Vector2 position, ref Vector2 topRight, ref Vector2 bottomLeft, ref PositionState state)
    {
        Vector2i topRightTile = mMap.GetMapTileAtPoint(new Vector2(topRight.x - 0.5f, topRight.y + 0.5f));
        Vector2i bottomleftTile = mMap.GetMapTileAtPoint(new Vector2(bottomLeft.x + 0.5f, bottomLeft.y + 0.5f));

        for (int x = bottomleftTile.x; x <= topRightTile.x; ++x)
        {
            var tileCollisionType = mMap.GetTile(x, topRightTile.y);

            switch (tileCollisionType)
            {
                default://slope
                    break;
                case TileType.Empty:
                    if (mPS.isClimbing)
                    {
                        state.pushesTopTile = true;
                        state.topTile = new Vector2i(x, topRightTile.y);
                        return true;
                    }
                    break;
                case TileType.OneWay:
                    break;
                case TileType.Ladder:
                    //mPS.onLadder = true;
                    break;
                case TileType.Block:
                    state.pushesTopTile = true;
                    state.topTile = new Vector2i(x, topRightTile.y);
                    return true;
            }
        }

        return false;
    }

    public bool CollidesWithTileBottom(ref Vector2 position, ref Vector2 topRight, ref Vector2 bottomLeft, ref PositionState state)
    {
        Vector2i topRightTile = mMap.GetMapTileAtPoint(new Vector2(topRight.x - 0.5f, topRight.y - 0.5f));
        Vector2i bottomleftTile = mMap.GetMapTileAtPoint(new Vector2(bottomLeft.x + 0.5f, bottomLeft.y - 0.5f));
        bool isOneWay;
        bool spiked = false;
        bool bounced = false;

        for (int x = bottomleftTile.x; x <= topRightTile.x; ++x)
        {
            var tileCollisionType = mMap.GetTile(x, bottomleftTile.y);

            //Check if we're on a one way platform
            isOneWay = (tileCollisionType == TileType.OneWay || tileCollisionType == TileType.LadderTop);
            if ((mIgnoresOneWay || state.tmpIgnoresOneWay) && isOneWay)
                continue;

            Vector2 tileCenter;
            float topTileEdge;

            switch (tileCollisionType)
            {
                default://slope
                    break;
                case TileType.Empty:
                    /*
                    if (mPS.isClimbing)
                    {
                        state.pushesBottomTile = true;
                        state.bottomTile = new Vector2i(x, bottomleftTile.y);
                        return true;
                    }
                    */
                    break;
                case TileType.LadderTop:
                case TileType.OneWay:
                    tileCenter = mMap.GetMapTilePosition(x, bottomleftTile.y);
                    topTileEdge = tileCenter.y + Map.cTileSize /2;

                    if (topTileEdge > bottomLeft.y+0.5f)
                    {
                        continue;
                    }
                    state.onOneWay = true;
                    state.oneWayY = bottomleftTile.y;

                    state.pushesBottomTile = true;
                    state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    return true;
                    break;
                case TileType.Spikes:
                    tileCenter = mMap.GetMapTilePosition(x, bottomleftTile.y);
                    topTileEdge = tileCenter.y + Map.cTileSize / 2;
                    if (topTileEdge > bottomLeft.y + 0.5f)
                    {
                        continue;
                    }

                    if (mSpeed.y < 0)
                    {
                        //flag this to be dealt with after we've checked other tiles we're colliding with
                        spiked = true;
                    }
                    break;
                case TileType.Bounce:
                    tileCenter = mMap.GetMapTilePosition(x, bottomleftTile.y);
                    topTileEdge = tileCenter.y + Map.cTileSize / 2;
                    if (topTileEdge > bottomLeft.y + 0.5f)
                    {
                        continue;
                    }

                    if (mSpeed.y < 0)
                    {
                        //flag this to be dealt with after we've checked other tiles we're colliding with
                        bounced = true;
                    }
                    //state.pushesBottomTile = true;
                    //state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    break;
                case TileType.Ladder:
                    //mPS.onLadder = true;
                    break;
                case TileType.Block:
                    state.onOneWay = false;
                    state.pushesBottomTile = true;
                    state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    return true;
            }
        }


        //Stepping on spikes kills you
        if (spiked)
        {
            Crush();
        }

        //Stepping on the bounce pad launches you
        if (bounced)
        {
            mPS.isBounce = true;
            mSpeed.y = Constants.cBounceSpeed;
        }

        return false;
    }

    private void MoveX(ref Vector2 position, ref bool foundObstacleX, float offset, float step, ref Vector2 topRight, ref Vector2 bottomLeft, ref PositionState state)
    {
        while (!foundObstacleX && offset != 0.0f)
        {
            offset -= step;

            if (step > 0.0f)
                foundObstacleX = CollidesWithTileRight(ref position, ref topRight, ref bottomLeft, ref state, true);
            else
                foundObstacleX = CollidesWithTileLeft(ref position, ref topRight, ref bottomLeft, ref state, true);

            if (!foundObstacleX)
            {
                position.x += step;
                topRight.x += step;
                bottomLeft.x += step;

                CollidesWithTileTop(ref position, ref topRight, ref bottomLeft, ref state);
                CollidesWithTileBottom(ref position, ref topRight, ref bottomLeft, ref state);
            }
        }
    }

    private void MoveY(ref Vector2 position, ref bool foundObstacleY, float offset, float step, ref Vector2 topRight, ref Vector2 bottomLeft, ref PositionState state)
    {
        while (!foundObstacleY && offset != 0.0f)
        {
            offset -= step;

            if (step > 0.0f)
                foundObstacleY = CollidesWithTileTop(ref position, ref topRight, ref bottomLeft, ref state);
            else
                foundObstacleY = CollidesWithTileBottom(ref position, ref topRight, ref bottomLeft, ref state);

            if (!foundObstacleY)
            {
                position.y += step;
                topRight.y += step;
                bottomLeft.y += step;

                CollidesWithTileLeft(ref position, ref topRight, ref bottomLeft, ref state);
                CollidesWithTileRight(ref position, ref topRight, ref bottomLeft, ref state);
            }
        }
    }

    public void Move(Vector2 offset, Vector2 speed, ref Vector2 position, ref Vector2 reminder, AABB aabb, ref PositionState state)
    {
        reminder += offset;

        Vector2 topRight = aabb.Max();
        Vector2 bottomLeft = aabb.Min();

        bool foundObstacleX = false, foundObstacleY = false;

        var step = new Vector2(Mathf.Sign(offset.x), Mathf.Sign(offset.y));
        var move = new Vector2(Mathf.Round(reminder.x), Mathf.Round(reminder.y));
        reminder -= move;

        if (move.x == 0.0f && move.y == 0.0f)
            return;
        else if (move.x != 0.0f && move.y == 0.0f)
        {
            MoveX(ref position, ref foundObstacleX, move.x, step.x, ref topRight, ref bottomLeft, ref state);

            if (step.x > 0.0f)
                state.pushesLeftTile = CollidesWithTileLeft(ref position, ref topRight, ref bottomLeft, ref state);
            else
                state.pushesRightTile = CollidesWithTileRight(ref position, ref topRight, ref bottomLeft, ref state);
        }
        else if (move.y != 0.0f && move.x == 0.0f)
        {
            MoveY(ref position, ref foundObstacleY, move.y, step.y, ref topRight, ref bottomLeft, ref state);

            if (step.y > 0.0f)
                state.pushesBottomTile = CollidesWithTileBottom(ref position, ref topRight, ref bottomLeft, ref state);
            else
                state.pushesTopTile = CollidesWithTileTop(ref position, ref topRight, ref bottomLeft, ref state);

            if (!mIgnoresOneWay && state.tmpIgnoresOneWay && mMap.GetMapTileYAtPoint(bottomLeft.y - 0.5f) != state.oneWayY)
                state.tmpIgnoresOneWay = false;
        }
        else
        {
            float speedRatio = Mathf.Abs(speed.y) / Mathf.Abs(speed.x);
            float vertAccum = 0.0f;

            while (!foundObstacleX && !foundObstacleY && (move.x != 0.0f || move.y != 0.0f))
            {
                vertAccum += Mathf.Sign(step.y) * speedRatio;

                MoveX(ref position, ref foundObstacleX, step.x, step.x, ref topRight, ref bottomLeft, ref state);
                move.x -= step.x;

                while (!foundObstacleY && move.y != 0.0f && (Mathf.Abs(vertAccum) >= 1.0f || move.x == 0.0f))
                {
                    move.y -= step.y;
                    vertAccum -= step.y;

                    MoveY(ref position, ref foundObstacleX, step.y, step.y, ref topRight, ref bottomLeft, ref state);
                }
            }

            if (step.x > 0.0f)
                state.pushesLeftTile = CollidesWithTileLeft(ref position, ref topRight, ref bottomLeft, ref state);
            else
                state.pushesRightTile = CollidesWithTileRight(ref position, ref topRight, ref bottomLeft, ref state);

            if (step.y > 0.0f)
                state.pushesBottomTile = CollidesWithTileBottom(ref position, ref topRight, ref bottomLeft, ref state);
            else
                state.pushesTopTile = CollidesWithTileTop(ref position, ref topRight, ref bottomLeft, ref state);

            //if we've fallen farther than the oneway tile threshold, we can again check for one way tiles
            if (!mIgnoresOneWay && state.tmpIgnoresOneWay && mMap.GetMapTileYAtPoint(bottomLeft.y - 0.5f) != state.oneWayY)
                state.tmpIgnoresOneWay = false;


        }
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

        mPS.onLadder = false;


        if(mSpeed.y < 0)
        {
            mPS.isBounce = false;
        }

        Vector2 topRight = mAABB.Max();
        Vector2 bottomLeft = mAABB.Min();
        if (transform.name == "MovingPlatformWee")
        {
            //Debug.Log("TR: " + topRight + " BL: " + bottomLeft);
        }

            CollidesWithTiles(ref mPosition, ref topRight, ref bottomLeft, ref mPS);

        //save the speed to oldSpeed vector
        if(transform.name == "MovingPlatformWee")
        {
           // Debug.Log("Name:" + transform.name + "- Old Speed: " + mOldSpeed + ", Speed: " + mSpeed);
           // Debug.Log("Bot: " + mPS.pushesBottomTile + "Top: " + mPS.pushesTopTile + "Left: " + mPS.pushesLeftTile + "Right: " + mPS.pushesRightTile);
        }
        mOldSpeed = mSpeed;

        if (mPS.pushesBottomTile)
            mSpeed.y = Mathf.Max(0.0f, mSpeed.y);
        if (mPS.pushesTopTile)
            mSpeed.y = Mathf.Min(0.0f, mSpeed.y);
        if (mPS.pushesLeftTile)
            mSpeed.x = Mathf.Max(0.0f, mSpeed.x);
        if (mPS.pushesRightTile)
            mSpeed.x = Mathf.Min(0.0f, mSpeed.x);

        //save the position to the oldPosition vector
        mOldPosition = mPosition;

        mOffset = mSpeed * Time.deltaTime;

        if (mMountParent != null)
        {
            if (HasCollisionDataFor(mMountParent))
                mOffset += mMountParent.mPosition - mMountParent.mOldPosition;
            else
                mMountParent = null;
        }

        mPosition += RoundVector(mOffset + mReminder);

        mAABB.Center = mPosition;
    }

    public void TryAutoMount(MovingObject platform)
    {
        if (mMountParent == null)
        {
            mMountParent = platform;
            if (platform.mUpdateId > mUpdateId)
                mGame.SwapUpdateIds(this, platform);
        }
    }

    public void Crush()
    {
        //Kinematic things cant be spiked
        if (mIsKinematic)
            return;
        //Vector2 temp = mMap.GetMapTilePosition(mMap.mWidth/2 , mMap.mHeight / 2 );
        transform.position = mMap.GetMapTilePosition(mMap.mMapData.startTile);
        mPosition = transform.position;
        mAABB.Center = mPosition;
        mPS.Reset();
    }

    public void SetTilePosition(Vector2i tile)
    {
        transform.position = mMap.GetMapTilePosition(tile);
        mPosition = transform.position;
        mAABB.Center = mPosition;
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

            //if (other.mUpdateId < mUpdateId)
            //    overlap -= other.mPosition - data.pos1;

            if (overlap.x == 0.0f)
            {
                if (other.mAABB.Center.x > mAABB.Center.x)
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
                if (other.mAABB.Center.y > mAABB.Center.y)
                {
                    mPS.pushesTopObject = true;
                    mSpeed.y = Mathf.Min(mSpeed.y, 0.0f);
                }
                else
                {
                    TryAutoMount(other);
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
                if (speedSum.x == 0.0f)
                    speedRatioX = 0.5f;
                else
                    speedRatioX = absSpeed1.x / speedSum.x;

                if (speedSum.y == 0.0f)
                    speedRatioY = 0.5f;
                else
                    speedRatioY = absSpeed1.y / speedSum.y;
            }

            float smallestOverlap = Mathf.Min(Mathf.Abs(overlap.x), Mathf.Abs(overlap.y));

            if (smallestOverlap == Mathf.Abs(overlap.x))
            {
                float offsetX = overlap.x * speedRatioX;

                mOffset.x += offsetX;
                offsetSum.x += offsetX;

                if (overlap.x < 0.0f)
                {
                    if (other.mIsKinematic && mPS.pushesLeftTile && Mathf.Abs(overlap.y) > Constants.cCrushCorrectThreshold)
                        Crush();

                    mPS.pushesRightObject = true;
                    mSpeed.x = Mathf.Min(mSpeed.x, 0.0f);
                }
                else
                {
                    if (other.mIsKinematic && mPS.pushesRightTile && Mathf.Abs(overlap.y) > Constants.cCrushCorrectThreshold)
                        Crush();

                    mPS.pushesLeftObject = true;
                    mSpeed.x = Mathf.Max(mSpeed.x, 0.0f);
                }
            }
            else
            {
                float offsetY = overlap.y * speedRatioY;

                mOffset.y += offsetY;
                offsetSum.y += offsetY;

                if (overlap.y < 0.0f)
                {
                    //We should set an error for overlap, here is the basic implementation
                    if (other.mIsKinematic && mPS.pushesBottomTile && Mathf.Abs(overlap.x) > Constants.cCrushCorrectThreshold)
                    {
                        Debug.Log("Crush Overlap y" + overlap.y);
                        Debug.Log("Crush Overlap x" + overlap.x);
                        
                        Crush();
                    }
                    mPS.pushesTopObject = true;
                    mSpeed.y = Mathf.Min(mSpeed.y, 0.0f);
                }
                else
                {
                    if (other.mIsKinematic && mPS.pushesTopTile && Mathf.Abs(overlap.x) > Constants.cCrushCorrectThreshold)
                        Crush();

                    TryAutoMount(other);
                    mPS.pushesBottomObject = true;
                    mSpeed.y = Mathf.Max(mSpeed.y, 0.0f);
                }
            }
        }
    }

    public void UpdatePhysicsP2()
    {
        mPosition -= RoundVector(mOffset + mReminder);
        mAABB.Center = mPosition;

        UpdatePhysicsResponse();

        if (mOffset != Vector2.zero)
            Move(mOffset, mSpeed, ref mPosition, ref mReminder, mAABB, ref mPS);

        mPS.pushesBottom = mPS.pushesBottomTile || mPS.pushesBottomObject;
        mPS.pushesRight = mPS.pushesRightTile || mPS.pushesRightObject;
        mPS.pushesLeft = mPS.pushesLeftTile || mPS.pushesLeftObject;
        mPS.pushesTop = mPS.pushesTopTile || mPS.pushesTopObject;

        if (!mPS.tmpSticksToSlope && mPS.pushesTop || mSpeed.y <= 0.0f)
            mPS.tmpSticksToSlope = true;

        //update the aabb
        mAABB.Center = mPosition;

        //apply the changes to the transform
        transform.position = new Vector3(Mathf.Round(mPosition.x), Mathf.Round(mPosition.y), mSpriteDepth);
        transform.localScale = new Vector3(ScaleX, ScaleY, 1.0f);
    }
}