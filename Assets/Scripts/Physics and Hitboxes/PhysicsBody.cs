using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/* general object type
 * 
 * 
 */


    //This will serve as both the physical body with which entities will bump into eachother, and also as the bounds when entities are overlapping but do not block eachothers movement
    //Potentially we could split these
[System.Serializable]
public class PhysicsBody : CustomCollider2D
{
    public Game mGame;
    public MapManager mMap;
    //public Entity mEntity;



    

    /// <summary>
    /// The previous position.
    /// </summary>
    public Vector2 mOldPosition;
    //Entity can get this but not set it, maybe we should change that
    /// <summary>
    /// The current position.
    /// </summary>
    public Vector2 mPosition;


    //
    public Vector2 mReminder;
    public Vector2 mOffset;

    //These speeds are only used by the body, or atleast they should be
    /// <summary>
    /// The current speed in pixels/second. Basically pixel velocity, not an attribute
    /// </summary>
    public Vector2 mSpeed;
	/// <summary>
	/// The previous speed in pixels/second.
	/// </summary>
	public Vector2 mOldSpeed;

    //perhaps these things should inherit from customcollider, but for now having it as a member is fine

    //Probably better off in the entity class
    [SerializeField]
    public PositionState mPS;

    public Entity mMountParent = null;

    //
    public bool mIgnoresOneWay = false;
    public bool mOnOneWayPlatform = false;
    public bool mIsKinematic = false;
    public bool mIsHeavy = false;
    public bool mIgnoresGravity = false;


    public List<CollisionData> mCollisions;


    /// <summary>
    /// Depth for z-ordering the sprites.
    /// </summary>
    public float mSpriteDepth = -1.0f;

    //public Transform mTransform;

    //A physics body needs a reference to the entity it is apart of
    //We could also get this using GetComponent
    public PhysicsBody(Entity entity, CustomAABB aABB) : base(entity, aABB)
    {
        mCollisions = new List<CollisionData>();
        mGame = Game.instance;
        mMap = mGame.mMap;
        mEntity = entity;
        mAABB = aABB;

        mPS = new PositionState();
        //mEntity = mEntity;
        //All the basics that every physics object needs upon initialization
        //colliderType = ColliderType.Pushbox;
        //mAABB.Scale = Vector2.one;

        mPosition = RoundVector(mEntity.transform.position);

        //This should suffice for all physics bodies, maybe we'll come back to this
        mAABB.Center = mPosition;
        mAABB.OffsetY = mAABB.HalfSizeY;

        //Check to see if we're in editorMode




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
                case TileType.Door:
                    //If the players center is on the ladder tile, we can climb it
                    if (mMap.GetMapTilePosition(topRightTile.x, y) == mMap.GetMapTilePosition(mMap.GetMapTileAtPoint(mAABB.Center)))
                        mPS.onDoor = true;
                    break;
                case TileType.Bounce:
                case TileType.ConveyorLeft:
                case TileType.ConveyorRight:
                case TileType.IceBlock:
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
                case TileType.Door:
                    //If the players center is on the ladder tile, we can climb it
                    if (mMap.GetMapTilePosition(bottomLeftTile.x, y) == mMap.GetMapTilePosition(mMap.GetMapTileAtPoint(mAABB.Center)))
                        mPS.onDoor = true;
                    break;
                case TileType.Bounce:
                case TileType.IceBlock:
                case TileType.ConveyorLeft:
                case TileType.ConveyorRight:
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
                case TileType.Bounce:
                case TileType.ConveyorLeft:
                case TileType.ConveyorRight:
                case TileType.IceBlock:
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
                    topTileEdge = tileCenter.y + MapManager.cTileSize /2;

                    if (topTileEdge > bottomLeft.y+0.5f)
                    {
                        continue;
                    }
                    state.onOneWay = true;
                    state.oneWayY = bottomleftTile.y;

                    state.pushesBottomTile = true;
                    state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    return true;
                    //break;
                case TileType.Spikes:
                    tileCenter = mMap.GetMapTilePosition(x, bottomleftTile.y);
                    topTileEdge = tileCenter.y + MapManager.cTileSize / 2;
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
                    topTileEdge = tileCenter.y + MapManager.cTileSize / 2;
                    if (topTileEdge > bottomLeft.y + 0.5f)
                    {
                        continue;
                    }


                    //flag this to be dealt with after we've checked other tiles we're colliding with
                    mPS.isBounce = true;
                    mSpeed.y = Constants.cBounceSpeed;

                    state.onOneWay = false;
                    state.pushesBottomTile = true;
                    state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    return true;
                case TileType.Ladder:
                    //mPS.onLadder = true;
                    break;
                case TileType.ConveyorLeft:

                    state.onOneWay = false;
                    state.pushesBottomTile = true;
                    state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    state.onConveyorLeft = true;
                    return true;
                case TileType.ConveyorRight:

                    state.onOneWay = false;
                    state.pushesBottomTile = true;
                    state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    state.onConveyorRight = true;
                    return true;
                case TileType.IceBlock:

                    state.onOneWay = false;
                    state.pushesBottomTile = true;
                    state.bottomTile = new Vector2i(x, bottomleftTile.y);
                    state.onIce = true;
                    return true;
                    
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

    public void Move(Vector2 offset, Vector2 speed, ref Vector2 position, ref Vector2 reminder, CustomCollider2D collider, ref PositionState state)
    {
        reminder += offset;

        Vector2 topRight = collider.mAABB.Max();
        Vector2 bottomLeft = collider.mAABB.Min();

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
        mPS.onDoor = false;
        mPS.onIce = false;
        mPS.onConveyorLeft = false;
        mPS.onConveyorRight = false;

        if(mSpeed.y < 0)
        {
            mPS.isBounce = false;
        }

        Vector2 topRight = mAABB.Max();
        Vector2 bottomLeft = mAABB.Min();


            CollidesWithTiles(ref mPosition, ref topRight, ref bottomLeft, ref mPS);



        mOldSpeed = mSpeed;
        //This is the cutoff of updating
        //Lets try applying gravity here
        if(!mIgnoresGravity && !mPS.pushesBottom)
        ApplyGravity();

        if (mPS.pushesBottomTile)
            mSpeed.y = Mathf.Max(0.0f, mSpeed.y);
        if (mPS.pushesTopTile)
            mSpeed.y = Mathf.Min(0.0f, mSpeed.y);
        if (mPS.pushesLeftTile)
            mSpeed.x = Mathf.Max(0.0f, mSpeed.x);
        if (mPS.pushesRightTile)
            mSpeed.x = Mathf.Min(0.0f, mSpeed.x);

        /* Here is where we should update tile physics changes */
        HandleTilePhysics();


        //save the position to the oldPosition vector
        mOldPosition = mPosition;

        mOffset = mSpeed * Time.deltaTime;

        if (mMountParent != null)
        {
            if (HasCollisionDataFor(mMountParent.Body))
            {
                //if (mCollisionType == CollisionType.Player)
                   // Debug.Log("Player mounting " + mMountParent.name + " - Offset: " + mMountParent.mPosition + " , " + mMountParent.mOldPosition);
                Vector2 parentOffset = mMountParent.Body.mPosition - mMountParent.Body.mOldPosition;

                //This is my hacky way of fixing sliding off of a parent if its moving into another object, maybe
                if(parentOffset.x > 0 && !mMountParent.Body.mPS.pushesRight || parentOffset.x < 0 && !mMountParent.Body.mPS.pushesLeft)
                {
                    mOffset += mMountParent.Body.mPosition - mMountParent.Body.mOldPosition;
                }

            }
            else
            {
                mMountParent = null;
            }
        }

        mPosition += RoundVector(mOffset + mReminder);

        mAABB.Center = mPosition;
    }

    public void ApplyGravity()
    {
        mSpeed.y += Constants.cGravity * Time.deltaTime;

        mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
    }

    public void TryAutoMount(Entity platform)
    {
        if (mMountParent == null && !mIsHeavy)
        {
            mMountParent = platform;
            if (platform.mUpdateId > mEntity.mUpdateId)
                mGame.SwapUpdateIds(mEntity, platform);
        }
    }

    public void HandleTilePhysics()
    {
        /*
        if (mPS.onIce)
        {
            //mSpeed.x = mSpeed.x * Time.deltaTime;
        }
        else if (mPS.onConveyorLeft)
        {
            mSpeed.x += -Constants.cConveyorSpeed;
        }
        else if (mPS.onConveyorRight)
        {
            mSpeed.x +=  Constants.cConveyorSpeed;
        }

        mSpeed.x = Mathf.Clamp(mSpeed.x, -Constants.cMaxWalkSpeed, Constants.cMaxWalkSpeed);
        */
    }

    public void Crush()
    {
        //Kinematic things cant be spiked or crushed
        if (mIsKinematic || mIsHeavy)
            return;
        //Vector2 temp = mMap.GetMapTilePosition(mMap.mWidth/2 , mMap.mHeight / 2 );
        /*
        mEntity.transform.position = mMap.GetMapTilePosition(mMap.mMapData.startTile);
        mPosition = mEntity.transform.position;
        mAABB.Center = mPosition;
        mPS.Reset();
        */

        if(mEntity is IHurtable)
        {
            IHurtable hurtable = (IHurtable)mEntity;
            hurtable.GetHurt(Attack.CrushAttack());

        }
    }

    public void SetTilePosition(Vector2i tile)
    {
        mEntity.transform.position = mMap.GetMapTilePosition(tile) + new Vector2(0, -(MapManager.cTileSize/2));
        mPosition = mEntity.transform.position;
        mAABB.Center = mPosition;
    }

    private void UpdatePhysicsResponse()
    {
        //Kinematic objects do not receive force, only give
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

        //The total sum of all offsets from colliding forces
        Vector2 offsetSum = Vector2.zero;

        for (int i = 0; i < mCollisions.Count; ++i)
        {

            var other = mCollisions[i].other;
            var data = mCollisions[i];
            var overlap = data.overlap - offsetSum;

            //If this object does not collide with the other object or the other object is flagged for removal, ignore it
            if (!mEntity.mCollidesWith.Contains(other.mEntity.mEntityType) || other.mEntity.mToRemove)
                continue;

            //if (other.mUpdateId < mUpdateId)
            //    overlap -= other.mPosition - data.pos1;

            //If there is no overlap in the x axis (kind hard but sure)
            if (overlap.x == 0.0f)
            {
                if (other.mAABB.Center.x > mAABB.Center.x)
                {
                    mPS.pushesRightObject = true;
                    //mSpeed.x = Mathf.Min(mSpeed.x, 0.0f);
                }
                else
                {
                    mPS.pushesLeftObject = true;
                    //mSpeed.x = Mathf.Max(mSpeed.x, 0.0f);
                }
                continue;
            }
            else if (overlap.y == 0.0f)
            {
                if (other.mAABB.Center.y > mAABB.Center.y)
                {
                    mPS.pushesTopObject = true;
                    //mSpeed.y = Mathf.Min(mSpeed.y, 0.0f);
                }
                else
                {
                    TryAutoMount(other.mEntity);
                    mPS.pushesBottomObject = true;
                    //mSpeed.y = Mathf.Max(mSpeed.y, 0.0f);
                }
                continue;
            }

            //The absolute speed of object 1
            Vector2 absSpeed1 = new Vector2(Mathf.Abs(data.speed1.x), Mathf.Abs(data.speed1.y));

            //The absolute speed of object 2
            Vector2 absSpeed2 = new Vector2(Mathf.Abs(data.speed2.x), Mathf.Abs(data.speed2.y));

            Vector2 speedSum = absSpeed1 + absSpeed2;
            //Debug.Log(mEntity.name + " colliding with " + data.other.mEntity.name + " obj1 speed: " + absSpeed1 + " obj2 speed: " + absSpeed2 + " speed sum: " + speedSum);
            
            float speedRatioX, speedRatioY;

            if (other.mEntity.Body.mIsKinematic)
            {
                speedRatioX = speedRatioY = 1.0f;
            }
            else
            {
                if (speedSum.x == 0.0f)
                    speedRatioX = 0.5f;
                else
                    speedRatioX = absSpeed2.x / speedSum.x;

                if (speedSum.y == 0.0f)
                    speedRatioY = 0.5f;
                else
                    speedRatioY = absSpeed2.y / speedSum.y;
            }

            /*
            if (mIsHeavy)
                speedRatioY = 0;
            */


            //Determine which side has the smallest amount of overlap
            float smallestOverlap = Mathf.Min(Mathf.Abs(overlap.x), Mathf.Abs(overlap.y));

            if (smallestOverlap == Mathf.Abs(overlap.x))
            {
                float offsetX = 0;
                

                if (overlap.x < 0.0f)
                {
                    if (data.other.mEntity.Body.mPS.pushesRight)
                    {
                        offsetX = overlap.x;
                    } else
                    {
                        offsetX = overlap.x * speedRatioX;

                    }


                    mOffset.x += offsetX;
                    offsetSum.x += offsetX;

                    if (other.mEntity.Body.mIsHeavy && mPS.pushesLeftTile && Mathf.Abs(overlap.y) > Constants.cCrushCorrectThreshold)
                        Crush();

                    mPS.pushesRightObject = true;
                    mSpeed.x = Mathf.Min(mSpeed.x, 0.0f);
                }
                else
                {
                    if (data.other.mEntity.Body.mPS.pushesLeft)
                    {
                        offsetX = overlap.x;
                    }
                    else
                    {
                        offsetX = overlap.x * speedRatioX;

                    }

                    mOffset.x += offsetX;
                    offsetSum.x += offsetX;
                    if (other.mEntity.Body.mIsHeavy && mPS.pushesRightTile && Mathf.Abs(overlap.y) > Constants.cCrushCorrectThreshold)
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
                    if (other.mEntity.Body.mIsHeavy && mPS.pushesBottomTile && Mathf.Abs(overlap.x) > Constants.cCrushCorrectThreshold)
                    {
                        
                        Crush();
                    }
                    mPS.pushesTopObject = true;
                    mSpeed.y = Mathf.Min(mSpeed.y, 0.0f);
                }
                else
                {
                    if (other.mEntity.Body.mIsHeavy && mPS.pushesTopTile && Mathf.Abs(overlap.x) > Constants.cCrushCorrectThreshold)
                        Crush();

                    TryAutoMount(other.mEntity);
                    mPS.pushesBottomObject = true;
                    mSpeed.y = Mathf.Max(mSpeed.y, 0.0f);
                }
            }
        }
    }

    public bool HasCollisionDataFor(PhysicsBody other)
    {
        for (int i = 0; i < mCollisions.Count; ++i)
        {
            if (mCollisions[i].other == other)
                return true;
        }

        return false;
    }

    public void UpdatePhysicsP2()
    {
        //if (mType == ObjectType.Player) 
        //Debug.Log(name + " Offset " + mOffset);

        mPosition -= RoundVector(mOffset + mReminder);
        mAABB.Center = mPosition;

        UpdatePhysicsResponse();


        if (mOffset != Vector2.zero)
        {
            Move(mOffset, mSpeed, ref mPosition, ref mReminder, this, ref mPS);
        }

        mPS.pushesBottom = mPS.pushesBottomTile || mPS.pushesBottomObject;
        mPS.pushesRight = mPS.pushesRightTile || mPS.pushesRightObject;
        mPS.pushesLeft = mPS.pushesLeftTile || mPS.pushesLeftObject;
        mPS.pushesTop = mPS.pushesTopTile || mPS.pushesTopObject;

        

        //update the aabb
        mAABB.Center = mPosition;

        //apply the changes to the transform
        mEntity.transform.position = new Vector3(Mathf.Round(mPosition.x), Mathf.Round(mPosition.y), mSpriteDepth);
        mEntity.transform.localScale = new Vector3(mAABB.ScaleX, mAABB.ScaleY, 1.0f);
    }

 

}