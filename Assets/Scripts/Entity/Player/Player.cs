using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : Entity
{
    [System.Serializable]
    public enum PlayerState
    {
        Stand,
        Walk,
        Jump,
        GrabLedge,
        Climbing,
    };

    public List<Attack> mAttackList;


    public AudioClip mHitWallSfx;
    public AudioClip mJumpSfx;
    public AudioClip mWalkSfx;
    public AudioSource mAudioSource;

    public float mWalkSfxTimer = 0.0f;
    public const float cWalkSfxTime = 0.25f;
    public Bullet bullet;


    public int mPlayerIndex = 0;
    /// <summary>
    /// The number of frames passed from changing the state to jump.
    /// </summary>
    protected int mFramesFromJumpStart = 0;

    protected bool[] mInputs;
    protected bool[] mPrevInputs;

    public PlayerState mCurrentState = PlayerState.Stand;
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

    void OnDrawGizmos()
    {
        DrawAttackGizmos();
    }

    protected void DrawAttackGizmos()
    {
        if (mAttackList[0].mIsActive)
        {
            //calculate the position of the aabb's center
            var aabbPos = mAttackList[0].hitbox.collider.Center;

            //draw the aabb rectangle
            Gizmos.color = new Color(0, 1, 0, 1);
            Gizmos.DrawCube(aabbPos, mAttackList[0].hitbox.collider.HalfSize * 2.0f);
        }


    }



    public void SetCharacterWidth(Slider slider)
    {
        body.ScaleX = slider.value;
    }

    public void SetCharacterHeight(Slider slider)
    {
        body.ScaleY = slider.value;
    }

    public override void EntityInit()
    {
        mAudioSource = GetComponent<AudioSource>();

        body.mAABB.HalfSize = new Vector2(Constants.cHalfSizeX, Constants.cHalfSizeY);
        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;
        mClimbSpeed = Constants.cClimbSpeed;



        base.EntityInit();
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
        body.mSpeed.y = mJumpSpeed;
        mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
        mCurrentState = PlayerState.Jump;
    }

    public void Shoot()
    {
        Bullet temp = Instantiate(bullet, body.mAABB.Center, Quaternion.identity);
        temp.EntityInit();
        temp.direction = new Vector2(body.ScaleX, 0);
    }

    public override void EntityUpdate()
    {

        switch (mCurrentState)
        {
            case PlayerState.Stand:

                mWalkSfxTimer = cWalkSfxTime;
                mAnimator.Play("Stand");

                if (!body.mPS.pushesBottom)
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }

                if (body.mPS.onIce || body.mPS.isBounce)
                {
                   
                }
                else
                {
                    body.mSpeed = Vector2.zero;
                }

                if (Pressed(KeyInput.Shoot))
                {
                    Shoot();
                }
                

                if (!body.mPS.pushesBottom)
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }

                //if left or right key is pressed, but not both
                if (KeyState(KeyInput.GoRight) != KeyState(KeyInput.GoLeft))
                {
                    mCurrentState = PlayerState.Walk;
                    break;
                }
                else if (KeyState(KeyInput.Jump))
                {
                    Jump();
                    break;
                }

                if (KeyState(KeyInput.GoDown))
                {
                    body.mPS.tmpIgnoresOneWay = true;
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
                    if (body.mPS.onLadder)
                    {
                        body.mSpeed = Vector2.zero;
                        body.mPS.isClimbing = true;
                        mCurrentState = PlayerState.Climbing;
                        body.mIgnoresGravity = true;

                        break;
                    }

                    if (body.mPS.onDoor)
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
            case PlayerState.Walk:
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
                    mCurrentState = PlayerState.Stand;
                    //mSpeed = Vector2.zero;
                    break;
                }
                else if (KeyState(KeyInput.GoRight))
                {
                    if (body.mPS.pushesRightTile)
                    {
                        body.mSpeed.x = 0.0f;
                    }
                    else
                    {

                        body.mSpeed.x = mWalkSpeed;
                        
                    }
                    body.ScaleX = Mathf.Abs(body.ScaleX);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (body.mPS.pushesLeftTile)
                    {
                        body.mSpeed.x = 0.0f;
                    }
                    else
                    {
                        body.mSpeed.x = -mWalkSpeed;
                    }

                    body.ScaleX = -Mathf.Abs(body.ScaleX);
                }

                //if there's no tile to walk on, fall
                if (KeyState(KeyInput.Jump))
                {
                    Jump();
                    break;
                }
                else if (!body.mPS.pushesBottom)
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }

                if (KeyState(KeyInput.GoDown))
                    body.mPS.tmpIgnoresOneWay = true;

                if (KeyState(KeyInput.Climb) && body.mPS.onLadder)
                {
                    body.mSpeed = Vector2.zero;
                    body.mPS.isClimbing = true;
                    mCurrentState = PlayerState.Climbing;
                    body.mIgnoresGravity = true;

                    break;
                }

                break;
            case PlayerState.Jump:
                body.mIgnoresGravity = false;
                ++mFramesFromJumpStart;

                if (mFramesFromJumpStart <= Constants.cJumpFramesThreshold)
                {
                    if (body.mPS.pushesTop || body.mSpeed.y > 0.0f)
                        mFramesFromJumpStart = Constants.cJumpFramesThreshold + 1;
                    else if (KeyState(KeyInput.Jump))
                        body.mSpeed.y = mJumpSpeed;
                }

                mWalkSfxTimer = cWalkSfxTime;

                mAnimator.Play("Jump");
                /*
                mSpeed.y += Constants.cGravity * Time.deltaTime;

                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
                */
                if (!KeyState(KeyInput.Jump) && body.mSpeed.y > 0.0f && !body.mPS.isBounce)
                {
                    body.mSpeed.y = Mathf.Min(body.mSpeed.y, 200.0f);
                }

                if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                {
                    body.mSpeed.x = 0.0f;
                }
                else if (KeyState(KeyInput.GoRight))
                {
                    if (body.mPS.pushesRightTile)
                        body.mSpeed.x = 0.0f;
                    else
                        body.mSpeed.x = mWalkSpeed;
                    body.ScaleX = Mathf.Abs(body.ScaleX);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (body.mPS.pushesLeftTile)
                        body.mSpeed.x = 0.0f;
                    else
                        body.mSpeed.x = -mWalkSpeed;
                    body.ScaleX = -Mathf.Abs(body.ScaleX);
                }

                //if we hit the ground
                if (body.mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                    {
                        mCurrentState = PlayerState.Stand;
                        //mSpeed = Vector2.zero;
                        mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                    }
                    else	//either go right or go left are pressed so we change the state to walk
                    {
                        mCurrentState = PlayerState.Walk;
                        body.mSpeed.y = 0.0f;
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
            
                if (body.mSpeed.y <= 0.0f && !body.mPS.pushesTop
                    && ((body.mPS.pushesRight && mInputs[(int)KeyInput.GoRight]) || (body.mPS.pushesLeft && mInputs[(int)KeyInput.GoLeft])))
                {
                    //we'll translate the original aabb's HalfSize so we get a vector Vector2iing
                    //the top right corner of the aabb when we want to grab the right edge
                    //and top left corner of the aabb when we want to grab the left edge
                    Vector2 aabbCornerOffset;

                    if (body.mPS.pushesRight && mInputs[(int)KeyInput.GoRight])
                        aabbCornerOffset = body.mAABB.HalfSize;
                    else
                        aabbCornerOffset = new Vector2(-body.mAABB.HalfSizeX - 1.0f, body.mAABB.HalfSizeY);

                    int tileX, topY, bottomY;
                    tileX = mMap.GetMapTileXAtPoint(body.mAABB.CenterX + aabbCornerOffset.x);

                    if ((body.mPS.pushedLeft && body.mPS.pushesLeft) || (body.mPS.pushedRight && body.mPS.pushesRight))
                    {
                        topY = mMap.GetMapTileYAtPoint(body.mOldPosition.y + body.mAABB.OffsetY + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(body.mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
                    }
                    else
                    {
                        topY = mMap.GetMapTileYAtPoint(body.mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(body.mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
                    }

                    for (int y = topY; y >= bottomY; --y)
                    {
                        if (!mMap.IsObstacle(tileX, y)
                            && mMap.IsObstacle(tileX, y - 1))
                        {
                            //calculate the appropriate corner
                            var tileCorner = mMap.GetMapTilePosition(tileX, y - 1);
                            tileCorner.x -= Mathf.Sign(aabbCornerOffset.x) * MapManager.cTileSize / 2;
                            tileCorner.y += MapManager.cTileSize / 2;

                            //check whether the tile's corner is between our grabbing Vector2is
                            if (y > bottomY ||
                                ((body.mAABB.CenterY + aabbCornerOffset.y) - tileCorner.y <= Constants.cGrabLedgeEndY
                                && tileCorner.y - (body.mAABB.CenterY + aabbCornerOffset.y) >= Constants.cGrabLedgeStartY))
                            {
                                //save the tile we are holding so we can check later on if we can still hold onto it
                                mLedgeTile = new Vector2i(tileX, y - 1);

                                //calculate our position so the corner of our AABB and the tile's are next to each other
                                body.mPosition.y = tileCorner.y - aabbCornerOffset.y - body.mAABB.OffsetY - Constants.cGrabLedgeStartY + Constants.cGrabLedgeTileOffsetY;
                                body.mSpeed = Vector2.zero;

                                //finally grab the edge
                                mCurrentState = PlayerState.GrabLedge;
                                body.mIgnoresGravity = true;
                                body.ScaleX *= -1;
                                mAnimator.Play("GrabLedge");
                                mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                                break;
                                //mGame.PlayOneShot(SoundType.Character_LedgeGrab, mPosition, Game.sSfxVolume);
                            }
                        }
                    }
                }



                if (KeyState(KeyInput.GoDown))
                    body.mPS.tmpIgnoresOneWay = true;

                if (KeyState(KeyInput.Climb) && body.mPS.onLadder)
                {
                    //mLedgeTile = new Vector2i(tileX, y - 1);
                    body.mSpeed = Vector2.zero;
                    body.mPS.isClimbing = true;
                    mCurrentState = PlayerState.Climbing;
                    body.mIgnoresGravity = true;

                    break;
                }

                break;

            case PlayerState.GrabLedge:
                mAnimator.Play("GrabLedge");
                body.mIgnoresGravity = true;

                bool ledgeOnLeft = mLedgeTile.x * MapManager.cTileSize < body.mPosition.x;
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

                    mCurrentState = PlayerState.Jump;
                    //mGame.PlayOneShot(SoundType.Character_LedgeRelease, mPosition, Game.sSfxVolume);
                }
                else if (mInputs[(int)KeyInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    body.mSpeed.y = mJumpSpeed;
                    mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
                    mCurrentState = PlayerState.Jump;
                }

                //when the tile we grab onto gets destroyed
                if (!mMap.IsObstacle(mLedgeTile.x, mLedgeTile.y))
                    mCurrentState = PlayerState.Jump;

                break;
            case PlayerState.Climbing:
                int tx = 0, ty = 0;
                mMap.GetMapTileAtPoint(body.mAABB.Center, out tx, out ty);
                body.mPosition.x = tx * MapManager.cTileSize;

                if(Mathf.Abs(body.mSpeed.y) > 0)
                    mAnimator.Play("Climb");
                else
                    mAnimator.Play("LadderIdle");

                if (!body.mPS.onLadder)
                {
                    body.mPS.isClimbing = false;
                    mCurrentState = PlayerState.Stand;
                }
                if (KeyState(KeyInput.GoDown) == KeyState(KeyInput.Climb))
                {
                    body.mSpeed = Vector2.zero;
                }
                else if (KeyState(KeyInput.GoDown))
                {
                    if (body.mPS.pushesBottom)
                    {
                        body.mPS.isClimbing = false;
                        mCurrentState = PlayerState.Stand;
                        body.mSpeed.y = 0.0f;
                    }
                    else
                    {

                        body.mSpeed.y = -mClimbSpeed;
                    }
                   // ScaleX = Mathf.Abs(ScaleX);
                }
                else if (KeyState(KeyInput.Climb))
                {
                    if (body.mPS.pushesTop)
                        body.mSpeed.y = 0.0f;
                    else
                        body.mSpeed.y = mClimbSpeed;
                    //ScaleX = -Mathf.Abs(ScaleX);
                }

                if (Pressed(KeyInput.GoLeft) || Pressed(KeyInput.GoRight))
                {
                    body.mPS.isClimbing = false;
                    mCurrentState = PlayerState.Walk;

                    //ScaleX = -Mathf.Abs(ScaleX);
                }


                if (mInputs[(int)KeyInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    body.mPS.isClimbing = false;
                    body.mSpeed.y = mJumpSpeed;
                    mCurrentState = PlayerState.Jump;
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

        base.EntityUpdate();

        if (mInputs[(int)KeyInput.Attack])
        {
            mAttackList[0].Activate();
        }

            //Handle Attack stuff here
        if (mAttackList[0].mIsActive)
        {
            Debug.Log("Attack is active");
            mAttackList[0].hitbox.collider.Center = body.mAABB.Center + mAttackList[0].hitbox.collider.Offset;
            mAttackList[0].UpdateAttack();
        }

        if (body.mPS.pushedBottom && !body.mPS.pushesBottom || body.mPS.isClimbing)
            mFramesFromJumpStart = 0;

        if (body.mPS.pushesBottom && !body.mPS.pushedBottom)
            mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);

        UpdatePrevInputs();
    }

    public ItemObject CheckForItems()
    {
        //ItemObject item = null;
        for (int i = 0; i < body.mAllCollidingObjects.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if(body.mAllCollidingObjects[i].other.mCollisionType == CollisionType.Item)
            {
                return (ItemObject)body.mAllCollidingObjects[i].other.mEntity;
            }
        }

        return null;
    }

    public Chest CheckForChest()
    {
        //ItemObject item = null;
        for (int i = 0; i < body.mAllCollidingObjects.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (body.mAllCollidingObjects[i].other.mCollisionType == CollisionType.Chest)
            {
                return (Chest)body.mAllCollidingObjects[i].other.mEntity;
            }
        }

        return null;
    }
}
