using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Character : PhysicsObject
{
    [System.Serializable]
    public enum CharacterState
    {
        Stand,
        Walk,
        Jump,
        GrabLedge,
        Climbing,
    };

    public AudioClip mHitWallSfx;
    public AudioClip mJumpSfx;
    public AudioClip mWalkSfx;
    public AudioSource mAudioSource;

    public float mWalkSfxTimer = 0.0f;
    public const float cWalkSfxTime = 0.25f;



    public int mPlayerIndex = 0;
    public Animator mAnimator;
    public List<Color> colorPallete;
    /// <summary>
    /// The number of frames passed from changing the state to jump.
    /// </summary>
    protected int mFramesFromJumpStart = 0;

    protected bool[] mInputs;
    protected bool[] mPrevInputs;

    public CharacterState mCurrentState = CharacterState.Stand;
    public float mJumpSpeed;
    public float mWalkSpeed;
    public float mClimbSpeed;

    public List<Vector2i> mPath = new List<Vector2i>();

    public Vector2i mLedgeTile;
    public Vector2i mClimbTile;
    public float mExitDoorTimer = 0;
    public float mTimeToExit = 1;

    public int mCannotGoLeftFrames = 0;
    public int mCannotGoRightFrames = 0;

    /// <summary>
    /// Raises the draw gizmos event.
    /// </summary>
    void OnDrawGizmos()
    {
        DrawMovingObjectGizmos();

        //calculate the position of the aabb's center
        var aabbPos = (Vector3)mAABB.Center;

        //draw grabbing line
        float dir;

        if (ScaleX == 0.0f)
            dir = 1.0f;
        else
            dir = Mathf.Sign(ScaleX);

        Gizmos.color = Color.blue;
        Vector2 halfSize = mAABB.HalfSize;
        var grabVectorTopLeft = new Vector2(aabbPos.x, aabbPos.y)
            + new Vector2(-(halfSize.x + 1.0f) * dir, halfSize.y);
        grabVectorTopLeft.y -= Constants.cGrabLedgeStartY;

        //tk2dSprite sprite = GetComponent<tk2dSprite>();

        //sprite.spriteId

        var grabVectorBottomLeft = new Vector2(aabbPos.x, aabbPos.y)
            + new Vector2(-(halfSize.x + 1.0f) * dir, halfSize.y);
        grabVectorBottomLeft.y -= Constants.cGrabLedgeEndY;
        var grabVectorTopRight = grabVectorTopLeft + Vector2.right * (halfSize.x + 1.0f) * 2.0f * dir;
        var grabVectorBottomRight = grabVectorBottomLeft + Vector2.right * (halfSize.x + 1.0f) * 2.0f * dir;

        Gizmos.DrawLine(grabVectorTopLeft, grabVectorBottomLeft);
        Gizmos.DrawLine(grabVectorTopRight, grabVectorBottomRight);
    }

    public void SetCharacterWidth(Slider slider)
    {
        ScaleX = slider.value;
    }

    public void SetCharacterHeight(Slider slider)
    {
        ScaleY = slider.value;
    }

    public override void ObjectInit()
    {
        mAudioSource = GetComponent<AudioSource>();

        mAABB.HalfSize = new Vector2(Constants.cHalfSizeX, Constants.cHalfSizeY);
        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;
        mClimbSpeed = Constants.cClimbSpeed;

        ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);


        base.ObjectInit();
    }

    public void SetInputs(bool[] inputs, bool[] prevInputs)
    {
        mInputs = inputs;
        mPrevInputs = prevInputs;
    }

    protected bool Released(KeyInput key)
    {
        return (!mInputs[(int)key] && mPrevInputs[(int)key]);
    }

    protected bool KeyState(KeyInput key)
    {
        return (mInputs[(int)key]);
    }

    protected bool Pressed(KeyInput key)
    {
        return (mInputs[(int)key] && !mPrevInputs[(int)key]);
    }

    public void UpdatePrevInputs()
    {
        var count = (byte)KeyInput.Count;

        for (byte i = 0; i < count; ++i)
            mPrevInputs[i] = mInputs[i];
    }
    
    public void Jump()
    {
        mSpeed.y = mJumpSpeed;
        mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
        mCurrentState = CharacterState.Jump;
    }

    public override void CustomUpdate()
    {

        switch (mCurrentState)
        {
            case CharacterState.Stand:

                mWalkSfxTimer = cWalkSfxTime;
                mAnimator.Play("Stand");

                if (!mPS.pushesBottom)
                {
                    mCurrentState = CharacterState.Jump;
                    break;
                }

                if (mPS.onIce || mPS.isBounce)
                {
                   
                }
                else
                { 
                    mSpeed = Vector2.zero;
                }

                

                if (!mPS.pushesBottom)
                {
                    mCurrentState = CharacterState.Jump;
                    break;
                }

                //if left or right key is pressed, but not both
                if (KeyState(KeyInput.GoRight) != KeyState(KeyInput.GoLeft))
                {
                    mCurrentState = CharacterState.Walk;
                    break;
                }
                else if (KeyState(KeyInput.Jump))
                {
                    Jump();
                    break;
                }

                if (KeyState(KeyInput.GoDown))
                {
                    mPS.tmpIgnoresOneWay = true;
                    if (Pressed(KeyInput.GoDown))
                    {
                        ItemObject item = CheckForItems();
                        if (item != null)
                        {
                            Debug.Log("You picked up " + item.name);
                            //mAllCollidingObjects.Remove(item);
                            mGame.FlagObjectForRemoval(item);
                        }
                    }
                }
                if (KeyState(KeyInput.Climb) )
                {
                    if (mPS.onLadder)
                    {
                        mSpeed = Vector2.zero;
                        mPS.isClimbing = true;
                        mCurrentState = CharacterState.Climbing;
                        mIgnoresGravity = true;

                        break;
                    }

                    if (mPS.onDoor)
                    {
                        mExitDoorTimer += Time.deltaTime;
                        if(mExitDoorTimer > mTimeToExit)
                        {
                            //load map
                            mGame.mMapChangeFlag = true;
                            mExitDoorTimer = 0;
                        }

                    }

                    if (Pressed(KeyInput.Climb))
                    {
                        Chest chest = CheckForChest();
                        if (chest != null)
                        {
                            //Debug.Log("You picked up " + item.name);
                            //mAllCollidingObjects.Remove(item);
                            //mGame.FlagObjectForRemoval(item);
                            chest.OpenChest();
                        }
                    }
                } else
                {
                    mExitDoorTimer = 0;
                }

                break;
            case CharacterState.Walk:
                mAnimator.Play("Walk");

                mWalkSfxTimer += Time.deltaTime;

                if (mWalkSfxTimer > cWalkSfxTime)
                {
                    mWalkSfxTimer = 0.0f;
                    mAudioSource.PlayOneShot(mWalkSfx);
                }

                //if both or neither left nor right keys are pressed then stop walking and stand

                if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                {
                    mCurrentState = CharacterState.Stand;
                    //mSpeed = Vector2.zero;
                    break;
                }
                else if (KeyState(KeyInput.GoRight))
                {
                    if (mPS.pushesRightTile)
                    {
                        mSpeed.x = 0.0f;
                    }
                    else
                    {
                       
                            mSpeed.x = mWalkSpeed;
                        
                    }
                    ScaleX = Mathf.Abs(ScaleX);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (mPS.pushesLeftTile)
                    {
                        mSpeed.x = 0.0f;
                    }
                    else
                    {
                         mSpeed.x = -mWalkSpeed;
                    }
                
                    ScaleX = -Mathf.Abs(ScaleX);
                }

                //if there's no tile to walk on, fall
                if (KeyState(KeyInput.Jump))
                {
                    Jump();
                    break;
                }
                else if (!mPS.pushesBottom)
                {
                    mCurrentState = CharacterState.Jump;
                    break;
                }

                if (KeyState(KeyInput.GoDown))
                    mPS.tmpIgnoresOneWay = true;

                if (KeyState(KeyInput.Climb) && mPS.onLadder)
                {
                    mSpeed = Vector2.zero;
                    mPS.isClimbing = true;
                    mCurrentState = CharacterState.Climbing;
                    mIgnoresGravity = true;

                    break;
                }

                break;
            case CharacterState.Jump:
                mIgnoresGravity = false;
                ++mFramesFromJumpStart;

                if (mFramesFromJumpStart <= Constants.cJumpFramesThreshold)
                {
                    if (mPS.pushesTop || mSpeed.y > 0.0f)
                        mFramesFromJumpStart = Constants.cJumpFramesThreshold + 1;
                    else if (KeyState(KeyInput.Jump))
                        mSpeed.y = mJumpSpeed;
                }

                mWalkSfxTimer = cWalkSfxTime;

                mAnimator.Play("Jump");
                /*
                mSpeed.y += Constants.cGravity * Time.deltaTime;

                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
                */
                if (!KeyState(KeyInput.Jump) && mSpeed.y > 0.0f && !mPS.isBounce)
                {
                    mSpeed.y = Mathf.Min(mSpeed.y, 200.0f);
                }

                if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                {
                    mSpeed.x = 0.0f;
                }
                else if (KeyState(KeyInput.GoRight))
                {
                    if (mPS.pushesRightTile)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = mWalkSpeed;
                    ScaleX = Mathf.Abs(ScaleX);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (mPS.pushesLeftTile)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = -mWalkSpeed;
                    ScaleX = -Mathf.Abs(ScaleX);
                }

                //if we hit the ground
                if (mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                    {
                        mCurrentState = CharacterState.Stand;
                        //mSpeed = Vector2.zero;
                        mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                    }
                    else	//either go right or go left are pressed so we change the state to walk
                    {
                        mCurrentState = CharacterState.Walk;
                        mSpeed.y = 0.0f;
                        mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                    }
                }

                if (mCannotGoLeftFrames > 0)
                {
                    --mCannotGoLeftFrames;
                    mInputs[(int)KeyInput.GoLeft] = false;
                }
                if (mCannotGoRightFrames > 0)
                {
                    --mCannotGoRightFrames;
                    mInputs[(int)KeyInput.GoRight] = false;
                }
            
                if (mSpeed.y <= 0.0f && !mPS.pushesTop
                    && ((mPS.pushesRight && mInputs[(int)KeyInput.GoRight]) || (mPS.pushesLeft && mInputs[(int)KeyInput.GoLeft])))
                {
                    //we'll translate the original aabb's HalfSize so we get a vector Vector2iing
                    //the top right corner of the aabb when we want to grab the right edge
                    //and top left corner of the aabb when we want to grab the left edge
                    Vector2 aabbCornerOffset;

                    if (mPS.pushesRight && mInputs[(int)KeyInput.GoRight])
                        aabbCornerOffset = mAABB.HalfSize;
                    else
                        aabbCornerOffset = new Vector2(-mAABB.HalfSizeX - 1.0f, mAABB.HalfSizeY);

                    int tileX, topY, bottomY;
                    tileX = mMap.GetMapTileXAtPoint(mAABB.CenterX + aabbCornerOffset.x);

                    if ((mPS.pushedLeft && mPS.pushesLeft) || (mPS.pushedRight && mPS.pushesRight))
                    {
                        topY = mMap.GetMapTileYAtPoint(mOldPosition.y + mAABB.OffsetY + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
                    }
                    else
                    {
                        topY = mMap.GetMapTileYAtPoint(mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
                    }

                    for (int y = topY; y >= bottomY; --y)
                    {
                        if (!mMap.IsObstacle(tileX, y)
                            && mMap.IsObstacle(tileX, y - 1))
                        {
                            //calculate the appropriate corner
                            var tileCorner = mMap.GetMapTilePosition(tileX, y - 1);
                            tileCorner.x -= Mathf.Sign(aabbCornerOffset.x) * Map.cTileSize / 2;
                            tileCorner.y += Map.cTileSize / 2;

                            //check whether the tile's corner is between our grabbing Vector2is
                            if (y > bottomY ||
                                ((mAABB.CenterY + aabbCornerOffset.y) - tileCorner.y <= Constants.cGrabLedgeEndY
                                && tileCorner.y - (mAABB.CenterY + aabbCornerOffset.y) >= Constants.cGrabLedgeStartY))
                            {
                                //save the tile we are holding so we can check later on if we can still hold onto it
                                mLedgeTile = new Vector2i(tileX, y - 1);

                                //calculate our position so the corner of our AABB and the tile's are next to each other
                                mPosition.y = tileCorner.y - aabbCornerOffset.y - mAABB.OffsetY - Constants.cGrabLedgeStartY + Constants.cGrabLedgeTileOffsetY;
                                mSpeed = Vector2.zero;

                                //finally grab the edge
                                mCurrentState = CharacterState.GrabLedge;
                                mIgnoresGravity = true;
                                ScaleX *= -1;
                                mAnimator.Play("GrabLedge");
                                mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                                break;
                                //mGame.PlayOneShot(SoundType.Character_LedgeGrab, mPosition, Game.sSfxVolume);
                            }
                        }
                    }
                }



                if (KeyState(KeyInput.GoDown))
                    mPS.tmpIgnoresOneWay = true;

                if (KeyState(KeyInput.Climb) && mPS.onLadder)
                {
                    //mLedgeTile = new Vector2i(tileX, y - 1);
                    mSpeed = Vector2.zero;
                    mPS.isClimbing = true;
                    mCurrentState = CharacterState.Climbing;
                    mIgnoresGravity = true;

                    break;
                }

                break;

            case CharacterState.GrabLedge:
                mAnimator.Play("GrabLedge");
                mIgnoresGravity = true;

                bool ledgeOnLeft = mLedgeTile.x * Map.cTileSize < mPosition.x;
                bool ledgeOnRight = !ledgeOnLeft;

                //if down button is held then drop down
                if (mInputs[(int)KeyInput.GoDown]
                    || (mInputs[(int)KeyInput.GoLeft] && ledgeOnRight)
                    || (mInputs[(int)KeyInput.GoRight] && ledgeOnLeft))
                {
                    if (ledgeOnLeft)
                        mCannotGoLeftFrames = 3;
                    else
                        mCannotGoRightFrames = 3;

                    mCurrentState = CharacterState.Jump;
                    //mGame.PlayOneShot(SoundType.Character_LedgeRelease, mPosition, Game.sSfxVolume);
                }
                else if (mInputs[(int)KeyInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    mSpeed.y = mJumpSpeed;
                    mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
                    mCurrentState = CharacterState.Jump;
                }

                //when the tile we grab onto gets destroyed
                if (!mMap.IsObstacle(mLedgeTile.x, mLedgeTile.y))
                    mCurrentState = CharacterState.Jump;

                break;
            case CharacterState.Climbing:
                int tx = 0, ty = 0;
                mMap.GetMapTileAtPoint(mAABB.Center, out tx, out ty);
                mPosition.x = tx * Map.cTileSize;

                mAnimator.Play("Stand");
                if (!mPS.onLadder)
                {
                    mPS.isClimbing = false;
                    mCurrentState = CharacterState.Stand;
                }
                if (KeyState(KeyInput.GoDown) == KeyState(KeyInput.Climb))
                {
                    mSpeed = Vector2.zero;
                }
                else if (KeyState(KeyInput.GoDown))
                {
                    if (mPS.pushesBottom)
                    {
                        mPS.isClimbing = false;
                        mCurrentState = CharacterState.Stand;
                        mSpeed.y = 0.0f;
                    }
                    else
                    {
                        
                        mSpeed.y = -mClimbSpeed;
                    }
                   // ScaleX = Mathf.Abs(ScaleX);
                }
                else if (KeyState(KeyInput.Climb))
                {
                    if (mPS.pushesTop)
                        mSpeed.y = 0.0f;
                    else
                        mSpeed.y = mClimbSpeed;
                    //ScaleX = -Mathf.Abs(ScaleX);
                }

                if (Pressed(KeyInput.GoLeft) || Pressed(KeyInput.GoRight))
                {
                    mPS.isClimbing = false;
                    mCurrentState = CharacterState.Walk;

                    //ScaleX = -Mathf.Abs(ScaleX);
                }


                if (mInputs[(int)KeyInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    mPS.isClimbing = false;
                    mSpeed.y = mJumpSpeed;
                    mCurrentState = CharacterState.Jump;
                }
                break;
        }
        /*
        if(mSpeed.x > Constants.cMaxWalkSpeed)
        {

        }else if(mSpeed.x < -Constants.cMaxWalkSpeed)
        {

        }
        */
        //mSpeed.x = Mathf.Clamp(mSpeed.x, -Constants.cMaxWalkSpeed, Constants.cMaxWalkSpeed);
       

        //if (mAllCollidingObjects.Count > 0)
        //    GetComponent<SpriteRenderer>().color = Color.black;
        //else
        //    GetComponent<SpriteRenderer>().color = Color.white;

        UpdatePhysics();

        if (mPS.pushedBottom && !mPS.pushesBottom || mPS.isClimbing)
            mFramesFromJumpStart = 0;

        if (mPS.pushesBottom && !mPS.pushedBottom)
            mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);

        UpdatePrevInputs();
    }

    public ItemObject CheckForItems()
    {
        //ItemObject item = null;
        for (int i = 0; i < mAllCollidingObjects.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if(mAllCollidingObjects[i].other.mType == CollisionType.Item)
            {
                return (ItemObject)mAllCollidingObjects[i].other;
            }
        }

        return null;
    }

    public Chest CheckForChest()
    {
        //ItemObject item = null;
        for (int i = 0; i < mAllCollidingObjects.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (mAllCollidingObjects[i].other.mType == CollisionType.Chest)
            {
                return (Chest)mAllCollidingObjects[i].other;
            }
        }

        return null;
    }
}
