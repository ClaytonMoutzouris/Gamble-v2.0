﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : Entity, IHurtable
{
    [System.Serializable]
    public enum PlayerState { Stand, Walk, Jump, GrabLedge, Climb, Attacking, };

    public InputMode mInputMode = InputMode.Game;
    public float mJumpSpeed;
    public float mWalkSpeed;
    public float mClimbSpeed;
    public bool mDoubleJump = true;
    public float mTimeToExit = 1;
    public int playerIndex;
    public HealthBar mHealthBar;
    public bool mCannotClimb = false;

    public List<Projectile> bullets;
    public int activeBullet;
    [SerializeField]
    private PlayerState mCurrentState = PlayerState.Stand;
    public int mPlayerIndex = 0;

    public AudioClip mHitWallSfx;
    public AudioClip mJumpSfx;
    public AudioClip mWalkSfx;
    //public AudioSource mAudioSource;
    public MiniMapIcon MiniMapIcon;

    #region Hidden
    /// <summary>
    /// The number of frames passed from changing the state to jump.
    /// </summary>

    protected int mFramesFromJumpStart = 0;

    protected bool[] mInputs;
    protected bool[] mPrevInputs;
    [HideInInspector]
    public Vector2i mLedgeTile;
    [HideInInspector]
    public Vector2i mClimbTile;
    [HideInInspector]
    public float mExitDoorTimer = 0;
    [HideInInspector]
    public float mWalkSfxTimer = 0.0f;
    [HideInInspector]
    public const float cWalkSfxTime = 0.25f;
    [HideInInspector]
    public int mCannotGoLeftFrames = 0;
    [HideInInspector]
    public int mCannotGoRightFrames = 0;
    [HideInInspector]
    public Stats mStats;
    [HideInInspector]
    public AttackManager mAttackManager;
    private Hurtbox hurtBox;

    #endregion

    #region UI
    public PlayerInventoryUI InventoryUI;

    #endregion

    #region NotUsed
    private List<Vector2i> mPath = new List<Vector2i>();

    #endregion


    public Hurtbox HurtBox
    {
        get
        {
            return hurtBox;
        }

        set
        {
            hurtBox = value;
        }
    }


    public override void EntityInit()
    {
        base.EntityInit();

        Debug.Log("Setting player body" + Body);

        for(int c = 0; c < colorPallete.Count; c++)
        {
            colorPallete[c] = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
            ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        mStats = GetComponent<Stats>();
        mStats.health.healthbar = mHealthBar;

        if (mStats.health.healthbar != null)
        {
            mStats.health.healthbar.SetHealth(mStats.health);
        }

        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, new Vector2(Constants.cHalfSizeX, Constants.cHalfSizeY), Vector3.zero, new Vector3(1, 1, 1)));

        HurtBox.UpdatePosition();

        /*
        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;
        mClimbSpeed = Constants.cClimbSpeed;
        */


        mAttackManager = GetComponent<AttackManager>();
        //Hitbox hitbox = Instantiate<Hitbox>(HurtBox);
        MeleeAttack temp = new MeleeAttack(this, 0.5f, 50, .5f, Range.Close, new Hitbox(this, new CustomAABB(Body.mAABB.Center, new Vector3(5, 10, 0), new Vector3(8, 0, 0), new Vector3(1, 1, 1))));
        mAttackManager.AttackList.Add(temp);
        mAttackManager.meleeAttacks.Add(temp);
        foreach( Projectile projectile in bullets)
        {
            RangedAttack ranged = new RangedAttack(this, 0.05f, 5, 0.05f, Range.Far, projectile);
            mAttackManager.AttackList.Add(ranged);
        }


        if (MiniMap.instance != null)
        {
            MiniMapIcon = MiniMap.instance.AddStaticIcon(MinimapIconType.Player, mMap.GetMapTileAtPoint(Body.mPosition));
        }

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
        if (mCurrentState == PlayerState.Jump && !mDoubleJump)
            return;
        body.mSpeed.y = mJumpSpeed;
        mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
        mCurrentState = PlayerState.Jump;
    }

    public void ClimbLadder()
    {
        body.mPS.isClimbing = true;
        mCurrentState = PlayerState.Climb;
    }

    public void TryGrabLedge()
    {
        //we'll translate the original aabb's HalfSize so we get a vector Vector2iing
        //the top right corner of the aabb when we want to grab the right edge
        //and top left corner of the aabb when we want to grab the left edge
        Vector2 aabbCornerOffset;

        if (body.mPS.pushesRight && mInputs[(int)KeyInput.LeftStick_Right])
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
            if (!mMap.IsObstacle(tileX, y) && mMap.IsObstacle(tileX, y - 1))
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
                    body.mAABB.ScaleX *= -1;
                    mAnimator.Play("GrabLedge");
                    //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                    break;
                    //mGame.PlayOneShot(SoundType.Character_LedgeGrab, mPosition, Game.sSfxVolume);
                }
            }
        }
    }

    public bool PickUp(ItemObject itemObject)
    {
        Debug.Log("You picked up " + itemObject.name);
        //mAllCollidingObjects.Remove(item);
        mGame.FlagObjectForRemoval(itemObject);
        InventoryUI.AddItem(itemObject.mItemData);
        return true;
    }

    public void UpdateAnimator()
    {
        foreach(Attack attack in mAttackManager.AttackList)
        {
            if (attack.mIsActive)
            {
                mAnimator.Play("Attack");
                return;
            }
        }

        if (mCurrentState == PlayerState.Climb) {
            //Update the animator
            if (Mathf.Abs(body.mSpeed.y) > 0)
                mAnimator.Play("Climb");
            else
                mAnimator.Play("LadderIdle");
        } else
        {
            mAnimator.Play(mCurrentState.ToString());

        }

    }


    public override void EntityUpdate()
    {
        //
        if (Pressed(KeyInput.Inventory))
        {
            PlayerUIPanels.instance.OpenClosePanel(playerIndex-1);

        }


        mAttackManager.UpdateAttacks();
        
        if(mCannotClimb && !Body.mPS.onLadder)
        {
            mCannotClimb = false;
        }


        //Handle each of the players states
        switch (mCurrentState)
        {
            
            case PlayerState.Stand:

                mWalkSfxTimer = cWalkSfxTime;

                if (!body.mPS.pushesBottom)
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }


                body.mSpeed = Vector2.zero;
                
                

                //if left or right key is pressed, but not both
                if (KeyState(KeyInput.LeftStick_Right) != KeyState(KeyInput.LeftStick_Left))
                {
                    mCurrentState = PlayerState.Walk;
                    break;
                }


                if (Pressed(KeyInput.Item))
                {
                    GainLife(5);
                }

                if (KeyState(KeyInput.LeftStick_Down))
                {
                    if (KeyState(KeyInput.LeftStick_Down) && Pressed(KeyInput.Jump))
                    {
                        if (body.mPS.onOneWay)
                        {
                            body.mPS.tmpIgnoresOneWay = true;
                            break;
                        }
                    }


                }

                if (Pressed(KeyInput.LeftStick_Down))
                {
                    ItemObject item = CheckForItems();
                    if (item != null)
                    {
                        Debug.Log("You picked up " + item.name);
                        //mAllCollidingObjects.Remove(item);
                        PickUp(item);
                    }
                }


                if (Pressed(KeyInput.Jump))
                {
                    Jump();
                    break;
                }

                if (KeyState(KeyInput.LeftStick_Up) )
                {
                    if (body.mPS.onLadder)
                    {
                        body.mSpeed = Vector2.zero;
                        body.mPS.isClimbing = true;
                        mCurrentState = PlayerState.Climb;
                        body.mIgnoresGravity = true;

                        break;
                    }

                    if (body.mPS.onDoor)
                    {
                        mExitDoorTimer += Time.deltaTime;
                        if(mExitDoorTimer > Constants.exitDoorTime)
                        {
                            //load map
                            mGame.mMapChangeFlag = true;
                            mExitDoorTimer = 0;
                        }

                    }

                    if (Pressed(KeyInput.LeftStick_Up))
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

                mWalkSfxTimer += Time.deltaTime;

                if (mWalkSfxTimer > cWalkSfxTime)
                {
                    mWalkSfxTimer = 0.0f;
                    mAudioSource.PlayOneShot(mWalkSfx);
                }

                if (!body.mPS.pushesBottom)
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }

                //if both or neither left nor right keys are pressed then stop walking and stand

                if (KeyState(KeyInput.LeftStick_Right) == KeyState(KeyInput.LeftStick_Left))
                {
                    mCurrentState = PlayerState.Stand;
                    //mSpeed = Vector2.zero;
                    break;
                }
                else if (KeyState(KeyInput.LeftStick_Right))
                {
                    if (body.mPS.pushesRightTile)
                    {
                        body.mSpeed.x = 0.0f;
                    }
                    else
                    {

                        body.mSpeed.x = mWalkSpeed;
                        
                    }
                    body.mAABB.ScaleX = Mathf.Abs(body.mAABB.ScaleX);
                }
                else if (KeyState(KeyInput.LeftStick_Left))
                {
                    if (body.mPS.pushesLeftTile)
                    {
                        body.mSpeed.x = 0.0f;
                    }
                    else
                    {
                        body.mSpeed.x = -mWalkSpeed;
                    }

                    body.mAABB.ScaleX = -Mathf.Abs(body.mAABB.ScaleX);
                }

                if (KeyState(KeyInput.LeftStick_Down))
                {
                    if (KeyState(KeyInput.LeftStick_Down) && Pressed(KeyInput.Jump))
                    {
                        if (body.mPS.onOneWay)
                        {
                            body.mPS.tmpIgnoresOneWay = true;
                            break;
                        }

                    }

                    if (Pressed(KeyInput.LeftStick_Down))
                    {
                        ItemObject item = CheckForItems();
                        if (item != null)
                        {
                            Debug.Log("You picked up " + item.name);
                            //mAllCollidingObjects.Remove(item);
                            PickUp(item);
                        }
                    }
                }


                if (Pressed(KeyInput.Jump))
                {
                    Jump();
                    break;
                }


                if (KeyState(KeyInput.LeftStick_Up) && body.mPS.onLadder)
                {
                    body.mSpeed = Vector2.zero;
                    body.mPS.isClimbing = true;
                    mCurrentState = PlayerState.Climb;
                    body.mIgnoresGravity = true;

                    break;
                }

                break;
            case PlayerState.Jump:

                //we do not ignore gravity while in the air
                body.mIgnoresGravity = false;
                ++mFramesFromJumpStart;

                if (mFramesFromJumpStart <= Constants.cJumpFramesThreshold)
                {
                    if (body.mPS.pushesTop || body.mSpeed.y > 0.0f)
                        mFramesFromJumpStart = Constants.cJumpFramesThreshold + 1;
                    else if (Pressed(KeyInput.Jump))
                        body.mSpeed.y = mJumpSpeed;

                    
                }


                // we can climb ladders from this state
                if ((KeyState(KeyInput.LeftStick_Up) || KeyState(KeyInput.LeftStick_Down)) && body.mPS.onLadder && (!mCannotClimb || Pressed(KeyInput.LeftStick_Up)))
                {
                    mCurrentState = PlayerState.Climb;
                    break;
                }

                /*
                if (Pressed(KeyInput.Jump) && mDoubleJump)
                {
                    Jump();
                }
                 *
                 * /

                mWalkSfxTimer = cWalkSfxTime;

                /*
                mSpeed.y += Constants.cGravity * Time.deltaTime;

                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
                */
                if (!KeyState(KeyInput.Jump) && body.mSpeed.y > 0.0f && !body.mPS.isBounce)
                {
                    body.mSpeed.y = Mathf.Min(body.mSpeed.y, 200.0f);
                }

                if (KeyState(KeyInput.LeftStick_Right) == KeyState(KeyInput.LeftStick_Left))
                {
                    body.mSpeed.x = 0.0f;
                }
                else if (KeyState(KeyInput.LeftStick_Right))
                {
                    if (body.mPS.pushesRightTile)
                        body.mSpeed.x = 0.0f;
                    else
                        body.mSpeed.x = mWalkSpeed;
                    body.mAABB.ScaleX = Mathf.Abs(body.mAABB.ScaleX);
                }
                else if (KeyState(KeyInput.LeftStick_Left))
                {
                    if (body.mPS.pushesLeftTile)
                        body.mSpeed.x = 0.0f;
                    else
                        body.mSpeed.x = -mWalkSpeed;
                    body.mAABB.ScaleX = -Mathf.Abs(body.mAABB.ScaleX);
                }

                //if we hit the ground
                if (body.mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (KeyState(KeyInput.LeftStick_Right) == KeyState(KeyInput.LeftStick_Left))
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
                    mInputs[(int)KeyInput.LeftStick_Left] = false;
                }
                if (mCannotGoRightFrames > 0)
                {
                    --mCannotGoRightFrames;
                    mInputs[(int)KeyInput.LeftStick_Right] = false;
                }
            
                if (body.mSpeed.y <= 0.0f && !body.mPS.pushesTop
                    && ((body.mPS.pushesRight && mInputs[(int)KeyInput.LeftStick_Right]) || (body.mPS.pushesLeft && mInputs[(int)KeyInput.LeftStick_Left])))
                {
                    TryGrabLedge();
                }


                if (KeyState(KeyInput.LeftStick_Down))
                {
                    body.mPS.tmpIgnoresOneWay = true;
                }




                break;

            case PlayerState.GrabLedge:
                body.mIgnoresGravity = true;

                bool ledgeOnLeft = mLedgeTile.x * MapManager.cTileSize < body.mPosition.x;
                bool ledgeOnRight = !ledgeOnLeft;

                //if down button is held then drop down
                if (KeyState(KeyInput.LeftStick_Down)
                    || (KeyState(KeyInput.LeftStick_Left) && ledgeOnRight)
                    || (KeyState(KeyInput.LeftStick_Right) && ledgeOnLeft))
                {
                    if (ledgeOnLeft)
                        mCannotGoLeftFrames = 3;
                    else
                        mCannotGoRightFrames = 3;

                    mCurrentState = PlayerState.Jump;
                    //mGame.PlayOneShot(SoundType.Character_LedgeRelease, mPosition, Game.sSfxVolume);
                }
                else if (Pressed(KeyInput.Jump))
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
            case PlayerState.Climb:
                mCannotClimb = true;
                /*
                                //Update the animator
                                if (Mathf.Abs(body.mSpeed.y) > 0)
                                else
                                    mAnimator.Play("LadderIdle");
                */
                //On a ladder, we always assume we are still until we receive input
                body.mSpeed = Vector2.zero;
                body.mPS.isClimbing = true;
                //While we are climbing, we always ignore gravity
                body.mIgnoresGravity = true;

                int tx = 0, ty = 0;
                mMap.GetMapTileAtPoint(body.mAABB.Center, out tx, out ty);
                body.mPosition.x = tx * MapManager.cTileSize;

                

                if (!body.mPS.onLadder)
                {
                    body.mPS.isClimbing = false;
                    mCurrentState = PlayerState.Stand;
                }


                else if (KeyState(KeyInput.LeftStick_Down))
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
                else if (KeyState(KeyInput.LeftStick_Up))
                {
                    if (body.mPS.pushesTop)
                        body.mSpeed.y = 0.0f;
                    else
                        body.mSpeed.y = mClimbSpeed;
                    //ScaleX = -Mathf.Abs(ScaleX);
                }

                if (Pressed(KeyInput.LeftStick_Left) || Pressed(KeyInput.LeftStick_Right))
                {
                    body.mPS.isClimbing = false;
                    mCurrentState = PlayerState.Walk;

                    //ScaleX = -Mathf.Abs(ScaleX);
                }


                if (Pressed(KeyInput.Jump))
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    body.mPS.isClimbing = false;
                    body.mSpeed.y = mJumpSpeed;
                    mCurrentState = PlayerState.Jump;
                }
                break;
        }


        if (Pressed(KeyInput.RangeSwap))
        {
            activeBullet++;
            if(activeBullet >= bullets.Count)
            {
                activeBullet = 0;
            }
        }

        if (KeyState(KeyInput.Attack))
        {
            mAttackManager.AttackList[0].Activate();
        }

        if(KeyState(KeyInput.RightStick_Left) || KeyState(KeyInput.RightStick_Right) ||
            KeyState(KeyInput.RightStick_Up) || KeyState(KeyInput.RightStick_Down))     
        {
                RangedAttack attack = (RangedAttack)mAttackManager.AttackList[activeBullet + 1];
                attack.Activate(GetAim());
        }


        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();


        CollisionManager.UpdateAreas(HurtBox);
        //HurtBox.mCollisions.Clear();

        //Pretty sure this lets use jump forever

        if (body.mPS.pushedBottom && !body.mPS.pushesBottom || Body.mPS.isClimbing)
        mFramesFromJumpStart = 0;

        //Update the animator last
        UpdateAnimator();


        UpdatePrevInputs();
    }

    public Vector2 GetAim()
    {
        Vector2 aim = Vector2.zero;

        if (KeyState(KeyInput.RightStick_Down)){
            aim += Vector2.down;

        }
        else if (KeyState(KeyInput.RightStick_Up))
        {
            aim += Vector2.up;

        }

        if (KeyState(KeyInput.RightStick_Left))
        {
            aim += Vector2.left;

        }
        else if (KeyState(KeyInput.RightStick_Right))
        {
            aim += Vector2.right;

        }


        return aim;
    }

    public override void SecondUpdate()
    {
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();
        mAttackManager.SecondUpdate();
        HurtBox.UpdatePosition();

        if(MiniMapIcon != null)
        {
            MiniMapIcon.UpdateIcon(mMap.GetMapTileAtPoint(Body.mPosition));
        }
    }

    public void GetHurt(Attack attack)
    {
        int damage = (int)mStats.health.LoseHP(attack.damage);
        ShowFloatingText(damage, Color.red);

        if (mStats.health.currentHealth == 0)
        {
            Die();
        }

    }

    public void GainLife(int health)
    {
        int life = (int)mStats.health.GainHP(health);
        ShowFloatingText(life, Color.green);


    }


    public override void Die()
    {

        //The player never dies
        
        //base.Die();

        //HurtBox.mState = ColliderState.Closed;
        //HurtBox.mCollisions.Clear();
        
    }

    public override void Destroy()
    {
        base.Destroy();
        HurtBox.mState = ColliderState.Closed;
    }

    public override void ActuallyDie()
    {
        
        //we have to remove the hitboxes
        foreach (Attack attack in mAttackManager.AttackList)
        {
            if (attack is MeleeAttack)
            {
                MeleeAttack temp = (MeleeAttack)attack;
                CollisionManager.RemoveObjectFromAreas(temp.hitbox);
            }

        }

        //Do other stuff first because the base destroys the object
        base.ActuallyDie();
    }

    public ItemObject CheckForItems()
    {
        //ItemObject item = null;
        for (int i = 0; i < body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if(body.mCollisions[i].other.mEntity is ItemObject)
            {
                return (ItemObject)body.mCollisions[i].other.mEntity;
            }
        }

        return null;
    }

    public Chest CheckForChest()
    {
        //ItemObject item = null;
        for (int i = 0; i < body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (body.mCollisions[i].other.mEntity is Chest)
            {
                return (Chest)body.mCollisions[i].other.mEntity;
            }
        }

        return null;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(mStats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Body.mAABB.Center + (Body.mAABB.HalfSizeY + 3) * Vector3.up, new Vector3(30 * (mStats.health.currentHealth / mStats.health.maxHealth), 6, 1));
        }


        Gizmos.color = new Color(1,0,0, 0.5f);

        if (HurtBox != null)
            Gizmos.DrawCube(Position, HurtBox.mAABB.halfSize * 2);

        
    }

}
