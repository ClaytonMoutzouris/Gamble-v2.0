using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LocalCoop;



public class Player : Entity, IHurtable
{
    public Health mHealth;
    [System.Serializable]
    public enum PlayerState { Stand, Walk, Jump, GrabLedge, Climb, Attacking, Jetting };
    public float mJumpSpeed;
    public float mWalkSpeed;
    public float mClimbSpeed;
    public bool mDoubleJump = true;
    public float mTimeToExit = 1;
    public HealthBar mHealthBar;
    public bool mCannotClimb = false;
    public PlayerInventory mInventory;
    public PlayerEquipment mEquipment;
    [SerializeField]
    public PlayerInputController mInput;
    public List<Projectile> bullets;
    public int activeBullet;
    [SerializeField]
    private PlayerState mCurrentState = PlayerState.Stand;
    public int mPlayerIndex;
    public Stats mStats;


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
    public AttackManager mAttackManager;
    private Hurtbox hurtBox;

    #endregion

    #region UI

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

    public void SetInput(PlayerGamepadInput input)
    {
        mInput = gameObject.AddComponent<PlayerInputController>();
        mInput.mGamepadInput = input;
    }

    public override void EntityInit()
    {


        base.EntityInit();

        Debug.Log("Setting player body" + Body);

        for (int c = 0; c < colorPallete.Count; c++)
        {
            colorPallete[c] = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
        ColorSwap.SwapSpritesTexture(GetComponent<SpriteRenderer>(), colorPallete);

        mStats = new Stats(this, PlayerUIPanels.instance.playerPanels[mPlayerIndex].uiPlayerTab.statContainer);
        mHealth = new Health(50, mHealthBar);

        mInventory = new PlayerInventory(this);
        mEquipment = new PlayerEquipment();

        //mInput = PlayerInputManager.singleton.;
        //CustomEventSystem eventSystem = GetComponent<CustomEventSystem>();
        //EventSystem.current.SetSelectedGameObject(PauseMenu.instance.defaultObject);


        HurtBox = new Hurtbox(this, new CustomAABB(transform.position, new Vector2(Constants.cHalfSizeX, Constants.cHalfSizeY), Vector3.zero, new Vector3(1, 1, 1)));
        HurtBox.UpdatePosition();

        /*
        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;
        mClimbSpeed = Constants.cClimbSpeed;
        */


        mAttackManager = GetComponent<AttackManager>();
        //Hitbox hitbox = Instantiate<Hitbox>(HurtBox);
        MeleeAttack temp = new MeleeAttack(this, 0.5f, 50, .1f, new Hitbox(this, new CustomAABB(Body.mAABB.Center, new Vector3(5, 10, 0), new Vector3(8, 0, 0), new Vector3(1, 1, 1))));
        mAttackManager.AttackList.Add(temp);
        mAttackManager.meleeAttacks.Add(temp);
        foreach (Projectile projectile in bullets)
        {
            RangedAttack ranged = new RangedAttack(this, 0.05f, 5, 0.05f, projectile);
            mAttackManager.AttackList.Add(ranged);
        }


        if (MiniMap.instance != null)
        {
            MiniMapIcon = MiniMap.instance.AddStaticIcon(MinimapIconType.Player, mMap.GetMapTileAtPoint(Body.mPosition));
        }

    }

    public override void EntityUpdate()
    {
        if (mInput == null)
        {
            return;
        }

        if (mInput.playerButtonInput[(int)ButtonInput.Pause])
        {
            LevelManager.instance.PauseGame(mPlayerIndex);
            //PauseMenu.instance.defaultObject;
        }

        if (mInput.playerButtonInput[(int)ButtonInput.Select])
        {
            MiniMap.instance.Toggle();
        }
        //
        if (mInput.playerButtonInput[(int)ButtonInput.Inventory] && !mInput.previousButtonInput[(int)ButtonInput.Inventory])
        {
            PlayerUIPanels.instance.OpenClosePanel(mPlayerIndex);
            if (mInput.inputState == PlayerInputState.Inventory)
            {
                mInput.inputState = PlayerInputState.Game;
            }
            else
            {
                mInput.inputState = PlayerInputState.Inventory;
            }
        }


        mAttackManager.UpdateAttacks();

        if (mCannotClimb && !Body.mPS.onLadder)
        {
            mCannotClimb = false;
        }


        if (mInput.playerButtonInput[(int)ButtonInput.Item] && !mInput.previousButtonInput[(int)ButtonInput.Item])
        {
            GainLife(5);
        }

        //Check to see if a player is trying to pick up an item
        if (mInput.playerButtonInput[(int)ButtonInput.DPad_Down] && !mInput.previousButtonInput[(int)ButtonInput.DPad_Down])
        {
            ItemObject item = CheckForItems();
            if (item != null)
            {
                Debug.Log("You picked up " + item.name);
                //mAllCollidingObjects.Remove(item);
                PickUp(item);
            }
        }

        //Check to see if a player is trying to open a chest
        if (mInput.playerButtonInput[(int)ButtonInput.DPad_Up] && !mInput.previousButtonInput[(int)ButtonInput.DPad_Up])
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

                //Check to see if the player is trying to pass through a one way
                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] < 0 && mInput.playerButtonInput[(int)ButtonInput.Jump])
                {
                    if (body.mPS.onOneWay)
                    {
                        body.mPS.tmpIgnoresOneWay = true;
                        break;
                    }

                }

                if (mInput.playerButtonInput[(int)ButtonInput.Jump] && !mInput.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    break;
                }

                //if left or right key is pressed, but not both
                if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] != 0)
                {
                    mCurrentState = PlayerState.Walk;
                    break;
                }




                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
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
                        if (mExitDoorTimer > Constants.exitDoorTime)
                        {
                            //load map
                            mGame.mMapChangeFlag = true;
                            mExitDoorTimer = 0;
                        }

                    }


                }
                else
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

                //Check to see if the player is trying to pass through a one way
                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] < 0 && mInput.playerButtonInput[(int)ButtonInput.Jump])
                {
                    if (body.mPS.onOneWay)
                    {
                        body.mPS.tmpIgnoresOneWay = true;
                        break;
                    }

                }

                if (mInput.playerButtonInput[(int)ButtonInput.Jump] && !mInput.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    break;
                }

                //if both or neither left nor right keys are pressed then stop walking and stand

                if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                {
                    mCurrentState = PlayerState.Stand;
                    //mSpeed = Vector2.zero;
                    break;
                }
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
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
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
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

                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    if (mInput.playerButtonInput[(int)ButtonInput.Jump] && !mInput.previousButtonInput[(int)ButtonInput.Jump])
                    {
                        if (body.mPS.onOneWay)
                        {
                            body.mPS.tmpIgnoresOneWay = true;
                            break;
                        }

                    }

                }


                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] > 0 && body.mPS.onLadder)
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
                    else if (mInput.playerButtonInput[(int)ButtonInput.Jump])
                        body.mSpeed.y = mJumpSpeed;


                }


                // we can climb ladders from this state
                if ((mInput.playerAxisInput[(int)AxisInput.LeftStickY] != 0) && body.mPS.onLadder && (!mCannotClimb || mInput.playerAxisInput[(int)AxisInput.LeftStickY] > 0))
                {
                    mCurrentState = PlayerState.Climb;
                    break;
                }


                if (mDoubleJump && mInput.playerButtonInput[(int)ButtonInput.Jump] && !mInput.previousButtonInput[(int)ButtonInput.Jump])
                {
                    JetMode();
                    break;
                }



                mWalkSfxTimer = cWalkSfxTime;

                /*
                mSpeed.y += Constants.cGravity * Time.deltaTime;

                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
                */
                if (!mInput.playerButtonInput[(int)ButtonInput.Jump] && body.mSpeed.y > 0.0f && !body.mPS.isBounce)
                {
                    body.mSpeed.y = Mathf.Min(body.mSpeed.y, 200.0f);
                }

                if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                {
                    body.mSpeed.x = 0.0f;
                }
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
                {
                    if (body.mPS.pushesRightTile)
                        body.mSpeed.x = 0.0f;
                    else
                        body.mSpeed.x = mWalkSpeed;
                    body.mAABB.ScaleX = Mathf.Abs(body.mAABB.ScaleX);
                }
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
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
                    if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
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
                    body.mSpeed.x = Mathf.Max(body.mSpeed.x, 0);
                }
                if (mCannotGoRightFrames > 0)
                {
                    --mCannotGoRightFrames;
                    body.mSpeed.x = Mathf.Min(body.mSpeed.x, 0);
                }

                if (body.mSpeed.y <= 0.0f && !body.mPS.pushesTop
                    && ((body.mPS.pushesRight && mInput.playerAxisInput[(int)AxisInput.LeftStickX] > 0) || (body.mPS.pushesLeft && mInput.playerAxisInput[(int)AxisInput.LeftStickX] < 0)))
                {
                    TryGrabLedge();
                }


                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    body.mPS.tmpIgnoresOneWay = true;
                }




                break;

            case PlayerState.GrabLedge:
                body.mIgnoresGravity = true;

                bool ledgeOnLeft = mLedgeTile.x * MapManager.cTileSize < body.mPosition.x;
                bool ledgeOnRight = !ledgeOnLeft;

                //if down button is held then drop down
                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] < 0
                    || (mInput.playerAxisInput[(int)AxisInput.LeftStickX] < 0 && ledgeOnRight)
                    || (mInput.playerAxisInput[(int)AxisInput.LeftStickX] > 0 && ledgeOnLeft))
                {
                    if (ledgeOnLeft)
                        mCannotGoLeftFrames = 3;
                    else
                        mCannotGoRightFrames = 3;

                    mCurrentState = PlayerState.Jump;
                    //mGame.PlayOneShot(SoundType.Character_LedgeRelease, mPosition, Game.sSfxVolume);
                }
                else if (mInput.playerButtonInput[(int)ButtonInput.Jump] && !mInput.previousButtonInput[(int)ButtonInput.Jump])
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


                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
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
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
                {
                    if (body.mPS.pushesTop)
                        body.mSpeed.y = 0.0f;
                    else
                        body.mSpeed.y = mClimbSpeed;
                    //ScaleX = -Mathf.Abs(ScaleX);
                }

                if (mInput.previousButtonInput[(int)ButtonInput.LeftStick_Left] || mInput.previousButtonInput[(int)ButtonInput.LeftStick_Left])
                {
                    body.mPS.isClimbing = false;
                    mCurrentState = PlayerState.Jump;

                    //ScaleX = -Mathf.Abs(ScaleX);
                }


                if (mInput.playerButtonInput[(int)ButtonInput.Jump] && !mInput.previousButtonInput[(int)ButtonInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    body.mPS.isClimbing = false;
                    body.mSpeed.y = mJumpSpeed;
                    mCurrentState = PlayerState.Jump;
                }
                break;
            case PlayerState.Jetting:
                body.mPS.isJetting = true;
                body.mSpeed = Vector2.zero;
                body.mIgnoresGravity = true;




                if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
                {
                    if (body.mPS.pushesTop)
                        body.mSpeed.y = 0.0f;
                    else
                        body.mSpeed.y = mClimbSpeed;
                }
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    body.mSpeed.y = -mClimbSpeed;
                }

                if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                {
                    body.mSpeed.x = 0.0f;
                }
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
                {
                    if (body.mPS.pushesRightTile)
                        body.mSpeed.x = 0.0f;
                    else
                        body.mSpeed.x = mWalkSpeed;
                    body.mAABB.ScaleX = Mathf.Abs(body.mAABB.ScaleX);
                }
                else if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
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
                    if (mInput.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
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

                if (mInput.playerButtonInput[(int)ButtonInput.Jump] && !mInput.previousButtonInput[(int)ButtonInput.Jump])
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }


                break;
        }


        if (mInput.playerButtonInput[(int)ButtonInput.Swap] && !mInput.previousButtonInput[(int)ButtonInput.Swap])
        {
            activeBullet++;
            if (activeBullet >= bullets.Count)
            {
                activeBullet = 0;
            }
        }

        if (mInput.playerButtonInput[(int)ButtonInput.Attack] && !mInput.previousButtonInput[(int)ButtonInput.Attack])
        {
            mAttackManager.AttackList[0].Activate();
        }

        if (mInput.playerAxisInput[(int)AxisInput.RightStickX] != 0 || mInput.playerAxisInput[(int)AxisInput.RightStickY] != 0)
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


    }

    public void Jump()
    {

        if (mCurrentState == PlayerState.Jump && !mDoubleJump)
            return;
        body.mSpeed.y = mJumpSpeed;
        mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
        mCurrentState = PlayerState.Jump;

        body.mSpeed.y = mJumpSpeed;
    }

    public void JetMode()
    {
        if (mCurrentState == PlayerState.Jetting)
            return;
        body.mSpeed = Vector2.zero;
        mCurrentState = PlayerState.Jetting;

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

        if (body.mPS.pushesRight && mInput.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
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

        //Should be something like itemObject.pickup(Inventory inventory);

        mInventory.AddItemToInventory(itemObject.mItemData);
        return true;
    }

    public void UpdateAnimator()
    {
        foreach (Attack attack in mAttackManager.AttackList)
        {
            if (attack.mIsActive)
            {
                mAnimator.Play("Attack");
                return;
            }
        }

        if (mCurrentState == PlayerState.Climb)
        {
            //Update the animator
            if (Mathf.Abs(body.mSpeed.y) > 0)
                mAnimator.Play("Climb");
            else
                mAnimator.Play("LadderIdle");
        }
        else
        {
            mAnimator.Play(mCurrentState.ToString());

        }

    }

    public Vector2 GetAim(bool freeAxis = true)
    {

        if (freeAxis)
        {
            return new Vector2(mInput.playerAxisInput[(int)AxisInput.RightStickX], mInput.playerAxisInput[(int)AxisInput.RightStickY]).normalized;
        }

        Vector2 aim = Vector2.zero;

        if (mInput.playerAxisInput[(int)AxisInput.RightStickY] < 0)
        {
            aim += Vector2.down;

        }
        else if (mInput.playerAxisInput[(int)AxisInput.RightStickY] > 0)
        {
            aim += Vector2.up;

        }

        if (mInput.playerAxisInput[(int)AxisInput.RightStickX] < 0)
        {
            aim += Vector2.left;

        }
        else if (mInput.playerAxisInput[(int)AxisInput.RightStickX] > 0)
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

        if (MiniMapIcon != null)
        {
            MiniMapIcon.UpdateIcon(mMap.GetMapTileAtPoint(Body.mPosition));
        }
    }

    public void GetHurt(Attack attack)
    {
        int damage = (int)mHealth.LoseHP(attack.damage);
        ShowFloatingText(damage, Color.red);

        if (mHealth.currentHealth == 0)
        {
            Die();
        }

    }

    public void GainLife(int health)
    {
        int life = (int)mHealth.GainHP(health);
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

        CollisionManager.RemoveObjectFromAreas(HurtBox);
        //CollisionManager.RemoveObjectFromAreas(sight);

        //Do other stuff first because the base destroys the object
        base.ActuallyDie();
    }

    public ItemObject CheckForItems()
    {
        //ItemObject item = null;
        for (int i = 0; i < body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (body.mCollisions[i].other.mEntity is ItemObject)
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

        Gizmos.color = new Color(1, 0, 0, 0.5f);

        if (HurtBox != null)
            Gizmos.DrawCube(Position, HurtBox.mAABB.halfSize * 2);


    }

}
