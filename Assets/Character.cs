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

    [HideInInspector]
    public CharacterState mCurrentState = CharacterState.Stand;
    public float mJumpSpeed;
    public float mWalkSpeed;

    public List<Vector2i> mPath = new List<Vector2i>();

    public Vector2i mLedgeTile;
    public float mLedgeGrabOffset;

    public int mCannotGoLeftFrames = 0;
    public int mCannotGoRightFrames = 0;

    public void CharacterInit(bool[] inputs, bool[] prevInputs)
    {
        mTransform = transform;
        mScale = Vector2.one;

        mInputs = inputs;
        mPrevInputs = prevInputs;

        //mAudioSource = GetComponent<AudioSource>();
        mPosition = transform.position;

        mAABB.halfSize = new Vector2(Constants.cHalfSizeX, Constants.cHalfSizeY);

        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;

        mAABBOffset.y = mAABB.halfSize.y;
        mLedgeGrabOffset = 4.0f;
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

                if (!mOnGround)
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
                    if (mPushesRightWall)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = mWalkSpeed;
                    mScale.x = -Mathf.Abs(mScale.x);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (mPushesLeftWall)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = -mWalkSpeed;
                    mScale.x = Mathf.Abs(mScale.x);
                }

                //if there's no tile to walk on, fall
                if (KeyState(KeyInput.Jump))
                {
                    mSpeed.y = mJumpSpeed;
                   // mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
                    mCurrentState = CharacterState.Jump;
                    break;
                }
                else if (!mOnGround)
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
                    if (mPushesRightWall)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = mWalkSpeed;
                    mScale.x = -Mathf.Abs(mScale.x);
                }
                else if (KeyState(KeyInput.GoLeft))
                {
                    if (mPushesLeftWall)
                        mSpeed.x = 0.0f;
                    else
                        mSpeed.x = -mWalkSpeed;
                    mScale.x = Mathf.Abs(mScale.x);
                }

                //if we hit the ground
                if (mOnGround)
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


                if (mSpeed.y <= 0.0f && !mAtCeiling && ((mPushesRightWall && KeyState(KeyInput.GoRight)) || (mPushesLeftWall && KeyState(KeyInput.GoLeft))))
                {
                    Vector2 aabbCornerOffset;

                    if (mPushesRightWall && mInputs[(int)KeyInput.GoRight])
                        aabbCornerOffset = mAABB.halfSize;
                    else
                        aabbCornerOffset = new Vector2(-mAABB.halfSize.x - 1.0f, mAABB.halfSize.y);

                    int tileX, topY, bottomY;
                    tileX = mMap.GetMapTileXAtPoint(mAABB.center.x + aabbCornerOffset.x);

                    if ((mPushedLeftWall && mPushesLeftWall) || (mPushedRightWall && mPushesRightWall))
                    {
                        topY = mMap.GetMapTileYAtPoint(mOldPosition.y + mAABBOffset.y + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(mAABB.center.y + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
                    }
                    else
                    {
                        topY = mMap.GetMapTileYAtPoint(mAABB.center.y + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
                        bottomY = mMap.GetMapTileYAtPoint(mAABB.center.y + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
                    }

                    for (int y = topY; y >= bottomY; --y)
                    {
                        if (!mMap.IsObstacle(tileX, y) && mMap.IsObstacle(tileX, y - 1))
                        {
                            var tileCorner = mMap.GetMapTilePosition(tileX, y - 1);
                            tileCorner.x -= Mathf.Sign(aabbCornerOffset.x) * Map.cTileSize / 2;
                            tileCorner.y += Map.cTileSize / 2;

                            if (y > bottomY || ((mAABB.center.y + aabbCornerOffset.y) - tileCorner.y <= Constants.cGrabLedgeEndY && tileCorner.y - (mAABB.center.y + aabbCornerOffset.y) >= Constants.cGrabLedgeStartY))
                            {
                                mLedgeTile = new Vector2i(tileX, y - 1);
                                mPosition.y = tileCorner.y - aabbCornerOffset.y - mAABBOffset.y - Constants.cGrabLedgeStartY + Constants.cGrabLedgeTileOffsetY;

                                mSpeed = Vector2.zero;
                                mCurrentState = CharacterState.GrabLedge;
                                break;
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
