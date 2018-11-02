using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Character : MovingObject {

    [System.Serializable]
    public enum CharacterState
    {
        Stand,
        Walk,
        Jump,
        GrabLedge,
    };

    public float mWalkSfxTimer = 0.0f;
    public const float cWalkSfxTime = 0.25f;

    public Animator mAnimator;

    /// <summary>
    /// The number of frames passed from changing the state to jump.
    /// </summary>
    protected int mFramesFromJumpStart = 0;

    protected bool[] mInputs;
    protected bool[] mPrevInputs;

    
    public CharacterState mCurrentState = CharacterState.Stand;
    public float mJumpSpeed;
    public float mWalkSpeed;

    public List<Vector2i> mPath = new List<Vector2i>();

    public Vector2i mLedgeTile;

    public int mCannotGoLeftFrames = 0;
    public int mCannotGoRightFrames = 0;


    void OnDrawGizmos()
    {
        DrawMovingObjectGizmos();

        //calculate the position of the aabb's center
        var aabbPos = transform.position + new Vector3(AABBOffsetX, AABBOffsetY, 0.0f);

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

    /*
    public void SetCharacterWidth(Slider slider)
    {
        ScaleX = slider.value;
    }

    public void SetCharacterHeight(Slider slider)
    {
        ScaleY = slider.value;
    }
    */

    public void CharacterInit(bool[] inputs, bool[] prevInputs)
    {
        mTransform = transform;
        Scale = Vector2.one;

        mInputs = inputs;
        mPrevInputs = prevInputs;

        //mAudioSource = GetComponent<AudioSource>();
        mPosition = transform.position;

        mAABB.HalfSize = new Vector2(Constants.cHalfSizeX, Constants.cHalfSizeY);

        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;

        AABBOffsetY = mAABB.HalfSizeY;

        //mGame.mObjects.Add(this);
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

    public void CharacterUpdate()
    {
        switch (mCurrentState)
        {
            case CharacterState.Stand:

                //mWalkSfxTimer = cWalkSfxTime;
                //mAnimator.Play("Stand");

                mSpeed = Vector2.zero;

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
                    mSpeed.y = mJumpSpeed;
                    //mAudioSource.PlayOneShot(mJumpSfx);
                    mCurrentState = CharacterState.Jump;
                    break;
                }

                if (KeyState(KeyInput.GoDown))
                {
                    Debug.Log("Pushed Down : One Way - "  + mOnOneWayPlatform);
                    if (mOnOneWayPlatform)
                        mPosition.y -= Constants.cOneWayPlatformThreshold;
                }

                break;
            case CharacterState.Walk:
                /*
                mAnimator.Play("Walk");

                mWalkSfxTimer += Time.deltaTime;

                if (mWalkSfxTimer > cWalkSfxTime)
                {
                 //   mWalkSfxTimer = 0.0f;
                 //   mAudioSource.PlayOneShot(mWalkSfx);
                }
                */
                //if both or neither left nor right keys are pressed then stop walking and stand

                if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                {
                    mCurrentState = CharacterState.Stand;
                    mSpeed = Vector2.zero;
                    break;
                }
                else if (KeyState(KeyInput.GoRight))
                {
                    if (mPS.pushesRight)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = mWalkSpeed;
                    ScaleX = -Mathf.Abs(ScaleX);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (mPS.pushesLeft)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = -mWalkSpeed;
                    ScaleX = Mathf.Abs(ScaleX);
                }

                //if there's no tile to walk on, fall
                if (KeyState(KeyInput.Jump))
                {
                    mSpeed.y = mJumpSpeed;
                   // mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
                    mCurrentState = CharacterState.Jump;
                    break;
                }
                else if (!mPS.pushesBottom)
                {
                    mCurrentState = CharacterState.Jump;
                    break;
                }

                if (KeyState(KeyInput.GoDown))
                {
                    if (mOnOneWayPlatform)
                        mPosition.y -= Constants.cOneWayPlatformThreshold;
                }

                break;
            case CharacterState.Jump:

                ++mFramesFromJumpStart;

                if (mFramesFromJumpStart <= Constants.cJumpFramesThreshold)
                {
                    if (mPS.pushesTop || mSpeed.y > 0.0f)
                        mFramesFromJumpStart = Constants.cJumpFramesThreshold + 1;
                    else if (KeyState(KeyInput.Jump))
                        mSpeed.y = mJumpSpeed;
                }
                // mWalkSfxTimer = cWalkSfxTime;

                // mAnimator.Play("Jump");

                mSpeed.y += Constants.cGravity * Time.deltaTime;

                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);

                if (!KeyState(KeyInput.Jump) && mSpeed.y > 0.0f)
                {
                    mSpeed.y = Mathf.Min(mSpeed.y, 200.0f);
                }

                if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                {
                    mSpeed.x = 0.0f;
                }
                else if (KeyState(KeyInput.GoRight))
                {
                    if (mPS.pushesRight)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = mWalkSpeed;
                    ScaleX = -Mathf.Abs(ScaleX);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (mPS.pushesLeft)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = -mWalkSpeed;
                    ScaleX = Mathf.Abs(ScaleX);
                }

                //if we hit the ground
                if (mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (mInputs[(int)KeyInput.GoRight] == mInputs[(int)KeyInput.GoLeft])
                    {
                        mCurrentState = CharacterState.Stand;
                        mSpeed = Vector2.zero;
                        //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                    }
                    else    //either go right or go left are pressed so we change the state to walk
                    {
                        mCurrentState = CharacterState.Walk;
                        mSpeed.y = 0.0f;
                        //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
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
                    tileX = mMap.GetMapTileXAtPoint(mAABB.center.x + aabbCornerOffset.x);

                    if ((mPS.pushedLeft && mPS.pushesLeft) || (mPS.pushedRight && mPS.pushesRight))
                    {
                        topY = mMap.GetMapTileYAtPoint(mOldPosition.y + AABBOffsetY + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(mAABB.center.y + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
                    }
                    else
                    {
                        topY = mMap.GetMapTileYAtPoint(mAABB.center.y + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(mAABB.center.y + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
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
                                ((mAABB.center.y + aabbCornerOffset.y) - tileCorner.y <= Constants.cGrabLedgeEndY
                                && tileCorner.y - (mAABB.center.y + aabbCornerOffset.y) >= Constants.cGrabLedgeStartY))
                            {
                                //save the tile we are holding so we can check later on if we can still hold onto it
                                mLedgeTile = new Vector2i(tileX, y - 1);

                                //calculate our position so the corner of our AABB and the tile's are next to each other
                                mPosition.y = tileCorner.y - aabbCornerOffset.y - AABBOffsetY - Constants.cGrabLedgeStartY + Constants.cGrabLedgeTileOffsetY;
                                mSpeed = Vector2.zero;

                                //finally grab the edge
                                mCurrentState = CharacterState.GrabLedge;
                                //mAnimator.Play("GrabLedge");
                                //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                                break;
                                //mGame.PlayOneShot(SoundType.Character_LedgeGrab, mPosition, Game.sSfxVolume);
                            }
                        }
                    }
                }

                break;

            case CharacterState.GrabLedge:
                bool ledgeOnLeft = mLedgeTile.x * Map.cTileSize < mPosition.x;
                bool ledgeOnRight = !ledgeOnLeft;
                if (mInputs[(int)KeyInput.GoDown] || (mInputs[(int)KeyInput.GoLeft] && ledgeOnRight) || (mInputs[(int)KeyInput.GoRight] && ledgeOnLeft))
                {
                    if (ledgeOnLeft)
                        mCannotGoLeftFrames = 3;
                    else
                        mCannotGoRightFrames = 3;

                    mCurrentState = CharacterState.Jump;
                }
                else if (mInputs[(int)KeyInput.Jump])
                {
                    mSpeed.y = mJumpSpeed;
                    mCurrentState = CharacterState.Jump;
                }

                break;
        }

        UpdatePhysics();

        if(mPS.pushedBottom && !mPS.pushesBottom)
        {
            mFramesFromJumpStart = 0;
        }

        //if (mOnGround && !mWasOnGround)
            //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);

        UpdatePrevInputs();
    }

    public void UpdatePrevInputs()
    {
        var count = (byte)KeyInput.Count;

        for (byte i = 0; i < count; ++i)
            mPrevInputs[i] = mInputs[i];
    }


}
