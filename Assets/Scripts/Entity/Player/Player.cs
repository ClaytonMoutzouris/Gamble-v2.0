using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LocalCoop;

public enum PlayerState { Stand, Walk, Jump, GrabLedge, Climb, Attacking, Jetting, Dead };


public class Player : Entity, IHurtable
{
    private Health health;
    PlayerPrototype prototype;
    public float mJumpSpeed;
    public float mWalkSpeed;
    public float mClimbSpeed;
    public bool mDoubleJump = true;
    public float mTimeToExit = 1;
    public HealthBar mHealthBar;
    public bool mCannotClimb = false;
    private PlayerInventory inventory;
    private PlayerEquipment equipment;
    private PlayerInputController input;
    public List<ProjectilePrototype> bullets;
    public int activeBullet;
    public PlayerState mCurrentState = PlayerState.Stand;
    public int mPlayerIndex;
    public Stats mStats;
    private AttackManager attackManager;
    public MeleeAttack defaultMelee;
    public RangedAttack defaultRanged;

    public List<PlayerAbility> activeAbilities;

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

    public AttackManager AttackManager
    {
        get
        {
            return attackManager;
        }

        set
        {
            attackManager = value;
        }
    }

    public PlayerInventory Inventory
    {
        get
        {
            return inventory;
        }

        set
        {
            inventory = value;
        }
    }

    public PlayerEquipment Equipment
    {
        get
        {
            return equipment;
        }

        set
        {
            equipment = value;
        }
    }

    public Health Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public PlayerInputController Input
    {
        get
        {
            return input;
        }

        set
        {
            input = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return mCurrentState == PlayerState.Dead;
        }

    }

    public void SetInput(PlayerGamepadInput input)
    {
        this.Input = new PlayerInputController(this, input);
    }

    public Player(PlayerPrototype proto, int index) : base(proto)
    {
        prototype = proto;
        bullets = new List<ProjectilePrototype>();
        mPlayerIndex = index;
        Body = new PhysicsBody(this, new CustomAABB(Position, new Vector2(proto.bodySize.x, proto.bodySize.y), new Vector2(0, proto.bodySize.y)));


        mWalkSpeed = prototype.walkSpeed;
        mJumpSpeed = prototype.jumpSpeed;
        mClimbSpeed = prototype.climbSpeed;
        mCollidesWith = proto.CollidesWith;

        HealthBar bar = PlayerUIPanels.instance.playerPanels[index].healthBar;

        Health = new Health(prototype.baseHealth, bar);


        
        for (int c = 0; c < prototype.colorPallete.Count; c++)
        {
            prototype.colorPallete[c] = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
        

        mStats = new Stats(this, PlayerUIPanels.instance.playerPanels[mPlayerIndex].uiPlayerTab.statContainer);

        Inventory = new PlayerInventory(this);
        Equipment = new PlayerEquipment(this);
        activeAbilities = new List<PlayerAbility>();
        //mInput = PlayerInputManager.singleton.;
        //CustomEventSystem eventSystem = GetComponent<CustomEventSystem>();
        //EventSystem.current.SetSelectedGameObject(PauseMenu.instance.defaultObject);


        HurtBox = new Hurtbox(this, new CustomAABB(Position, new Vector2(proto.bodySize.x, proto.bodySize.y), new Vector2(0, proto.bodySize.y)));
        HurtBox.UpdatePosition();

        /*
        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;
        mClimbSpeed = Constants.cClimbSpeed;
        */


        AttackManager = new AttackManager(this);

        foreach (MeleeAttackPrototype meleeAttack in prototype.meleeAttacks)
        {
            MeleeAttack melee = new MeleeAttack(this, meleeAttack.duration, meleeAttack.damage, meleeAttack.cooldown, new Hitbox(this, new CustomAABB(Position, meleeAttack.hitboxSize, meleeAttack.hitboxOffset)), meleeAttack.abilities);
            AttackManager.meleeAttacks.Add(melee);
            defaultMelee = melee;
            //Debug.Log("Adding Slime melee attack");
        }

        foreach (RangedAttackPrototype rangedAttack in prototype.rangedAttacks)
        {
            RangedAttack ranged = new RangedAttack(this, rangedAttack.duration, rangedAttack.damage, rangedAttack.cooldown, rangedAttack.numberOfProjectiles, rangedAttack.spreadAngle, rangedAttack.projectile, rangedAttack.offset, rangedAttack.abilities);
            AttackManager.rangedAttacks.Add(ranged);
            defaultRanged = ranged;

        }



        if (MiniMap.instance != null)
        {
            MiniMapIcon = MiniMap.instance.AddStaticIcon(MinimapIconType.Player, Map.GetMapTileAtPoint(Position));
        }

    }

    public override void Spawn(Vector2 spawnPoint)
    {
        //base.Spawn(spawnPoint);
        GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/PlayerRenderer")) as GameObject;
        Renderer = gameObject.GetComponent<PlayerRenderer>();
        Renderer.SetEntity(this);

        if (prototype.animationController != null)
        {
            Renderer.Animator.runtimeAnimatorController = prototype.animationController;
            //ColorSwap.SwapSpritesTexture(Renderer.GetComponent<SpriteRenderer>(), prototype.colorPallete);
            Renderer.colorSwapper.SetBaseColors(prototype.colorPallete);
        }

        Position = spawnPoint;
        Renderer.Draw();
        Body.UpdatePosition();
        isSpawned = true;

        HurtBox.UpdatePosition();
    }

    public override void EntityUpdate()
    {
        if (Input == null)
        {
            Debug.Log("Input is null");
            return;
        }

        if (Input.playerButtonInput[(int)ButtonInput.Pause])
        {
            LevelManager.instance.PauseGame(mPlayerIndex);
            //PauseMenu.instance.defaultObject;
        }

        if(mCurrentState == PlayerState.Dead)
        {
            return;
        }

        if (Input.playerButtonInput[(int)ButtonInput.Select])
        {
            MiniMap.instance.Toggle();
        }
        //
        if (Input.playerButtonInput[(int)ButtonInput.Inventory] && !Input.previousButtonInput[(int)ButtonInput.Inventory])
        {
            PlayerUIPanels.instance.OpenClosePanel(mPlayerIndex);
            if (Input.inputState == PlayerInputState.Inventory)
            {
                Input.inputState = PlayerInputState.Game;
            }
            else
            {
                Input.inputState = PlayerInputState.Inventory;
            }
        }


        AttackManager.UpdateAttacks();

        if (mCannotClimb && !Body.mPS.onLadder && !Input.playerButtonInput[(int)ButtonInput.Jump])
        {
            mCannotClimb = false;
        }


        if (Input.playerButtonInput[(int)ButtonInput.Item] && !Input.previousButtonInput[(int)ButtonInput.Item])
        {
            GainLife(5);
        }

        //Check to see if a player is trying to pick up an item
        if (Input.playerButtonInput[(int)ButtonInput.DPad_Down] && !Input.previousButtonInput[(int)ButtonInput.DPad_Down])
        {
            ItemObject item = CheckForItems();
            if (item != null)
            {
                //mAllCollidingObjects.Remove(item);
                PickUp(item);
            }
        }

        //Check to see if a player is trying to open a chest
        if (Input.playerButtonInput[(int)ButtonInput.DPad_Up] && !Input.previousButtonInput[(int)ButtonInput.DPad_Up])
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

                if (!Body.mPS.pushesBottom)
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }


                Body.mSpeed = Vector2.zero;

                //Check to see if the player is trying to pass through a one way
                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0 && Input.playerButtonInput[(int)ButtonInput.Jump])
                {
                    if (Body.mPS.onOneWay)
                    {
                        Body.mPS.tmpIgnoresOneWay = true;
                        break;
                    }

                }

                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    break;
                }

                //if left or right key is pressed, but not both
                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] != 0)
                {
                    mCurrentState = PlayerState.Walk;
                    break;
                }




                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
                {
                    if (Body.mPS.onLadder)
                    {
                        Body.mSpeed = Vector2.zero;
                        Body.mPS.isClimbing = true;
                        mCurrentState = PlayerState.Climb;
                        Body.mIgnoresGravity = true;

                        break;
                    }

                    if (Body.mPS.onDoor)
                    {
                        mExitDoorTimer += Time.deltaTime;
                        if (mExitDoorTimer > Constants.exitDoorTime)
                        {
                            //load map
                            Game.mMapChangeFlag = true;
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
                    SoundManager.instance.PlaySingle(mWalkSfx);
                }

                if (!Body.mPS.pushesBottom)
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }

                //Check to see if the player is trying to pass through a one way
                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0 && Input.playerButtonInput[(int)ButtonInput.Jump])
                {
                    if (Body.mPS.onOneWay)
                    {
                        Body.mPS.tmpIgnoresOneWay = true;
                        break;
                    }

                }

                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    break;
                }

                //if both or neither left nor right keys are pressed then stop walking and stand

                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                {
                    mCurrentState = PlayerState.Stand;
                    //mSpeed = Vector2.zero;
                    break;
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
                {
                    if (Body.mPS.pushesRightTile)
                    {
                        Body.mSpeed.x = 0.0f;
                    }
                    else
                    {

                        Body.mSpeed.x = mWalkSpeed;

                    }
                    mDirection = EntityDirection.Right;
                    //Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
                {
                    if (Body.mPS.pushesLeftTile)
                    {
                        Body.mSpeed.x = 0.0f;
                    }
                    else
                    {
                        Body.mSpeed.x = -mWalkSpeed;
                    }
                    mDirection = EntityDirection.Left;
                    //Renderer.Sprite.flipX = true;
                    //Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
                }

                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                    {
                        if (Body.mPS.onOneWay)
                        {
                            Body.mPS.tmpIgnoresOneWay = true;
                            break;
                        }

                    }

                }


                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0 && Body.mPS.onLadder)
                {
                    Body.mSpeed = Vector2.zero;
                    Body.mPS.isClimbing = true;
                    mCurrentState = PlayerState.Climb;
                    Body.mIgnoresGravity = true;

                    break;
                }

                break;
            case PlayerState.Jump:

                //we do not ignore gravity while in the air
                Body.mIgnoresGravity = false;
                ++mFramesFromJumpStart;

                if (mFramesFromJumpStart <= Constants.cJumpFramesThreshold)
                {
                    if (Body.mPS.pushesTop || Body.mSpeed.y > 0.0f)
                        mFramesFromJumpStart = Constants.cJumpFramesThreshold + 1;
                    else if (Input.playerButtonInput[(int)ButtonInput.Jump])
                        Body.mSpeed.y = mJumpSpeed;


                }


                // we can climb ladders from this state
                if ((Input.playerAxisInput[(int)AxisInput.LeftStickY] != 0) && Body.mPS.onLadder && !mCannotClimb)
                {
                    mCurrentState = PlayerState.Climb;
                    break;
                }


                if (mDoubleJump && Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    JetMode();
                    break;
                }



                mWalkSfxTimer = cWalkSfxTime;

                /*
                mSpeed.y += Constants.cGravity * Time.deltaTime;

                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
                */
                if (!Input.playerButtonInput[(int)ButtonInput.Jump] && Body.mSpeed.y > 0.0f && !Body.mPS.isBounce)
                {
                    Body.mSpeed.y = Mathf.Min(Body.mSpeed.y, 200.0f);
                }

                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                {
                    Body.mSpeed.x = 0.0f;
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
                {
                    if (Body.mPS.pushesRightTile)
                        Body.mSpeed.x = 0.0f;
                    else
                        Body.mSpeed.x = mWalkSpeed;
                    mDirection = EntityDirection.Right;
                    //Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
                {
                    if (Body.mPS.pushesLeftTile)
                        Body.mSpeed.x = 0.0f;
                    else
                        Body.mSpeed.x = -mWalkSpeed;
                    mDirection = EntityDirection.Left;
                    //Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
                }

                //if we hit the ground
                if (Body.mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                    {
                        mCurrentState = PlayerState.Stand;
                        //mSpeed = Vector2.zero;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                    else	//either go right or go left are pressed so we change the state to walk
                    {
                        mCurrentState = PlayerState.Walk;
                        Body.mSpeed.y = 0.0f;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                }

                if (mCannotGoLeftFrames > 0)
                {
                    --mCannotGoLeftFrames;
                    Body.mSpeed.x = Mathf.Max(Body.mSpeed.x, 0);
                }
                if (mCannotGoRightFrames > 0)
                {
                    --mCannotGoRightFrames;
                    Body.mSpeed.x = Mathf.Min(Body.mSpeed.x, 0);
                }

                if (Body.mSpeed.y <= 0.0f && !Body.mPS.pushesTop
                    && ((Body.mPS.pushesRight && Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0) || (Body.mPS.pushesLeft && Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)))
                {
                    TryGrabLedge();
                }


                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    Body.mPS.tmpIgnoresOneWay = true;
                }




                break;

            case PlayerState.GrabLedge:
                Body.mIgnoresGravity = true;

                bool ledgeOnLeft = mLedgeTile.x * MapManager.cTileSize < Position.x;
                bool ledgeOnRight = !ledgeOnLeft;

                //if down button is held then drop down
                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0
                    || (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0 && ledgeOnRight)
                    || (Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0 && ledgeOnLeft))
                {
                    if (ledgeOnLeft)
                        mCannotGoLeftFrames = 3;
                    else
                        mCannotGoRightFrames = 3;

                    mCurrentState = PlayerState.Jump;
                    //mGame.PlayOneShot(SoundType.Character_LedgeRelease, mPosition, Game.sSfxVolume);
                }
                else if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    Body.mSpeed.y = mJumpSpeed;
                    SoundManager.instance.PlaySingle(mJumpSfx);
                    mCurrentState = PlayerState.Jump;
                }

                //when the tile we grab onto gets destroyed
                if (!Map.IsObstacle(mLedgeTile.x, mLedgeTile.y))
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
                Body.mSpeed = Vector2.zero;
                Body.mPS.isClimbing = true;
                //While we are climbing, we always ignore gravity
                Body.mIgnoresGravity = true;

                int tx = 0, ty = 0;
                Map.GetMapTileAtPoint(Body.mAABB.Center, out tx, out ty);
                Position.x = tx * MapManager.cTileSize;



                if (!Body.mPS.onLadder)
                {
                    Body.mPS.isClimbing = false;
                    mCurrentState = PlayerState.Stand;
                }

                if (Input.playerButtonInput[(int)ButtonInput.LeftStick_Left] || Input.playerButtonInput[(int)ButtonInput.LeftStick_Right])
                {
                    Body.mPS.isClimbing = false;
                    mCurrentState = PlayerState.Jump;

                    //ScaleX = -Mathf.Abs(ScaleX);
                }


                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    Body.mPS.isClimbing = false;
                    Body.mSpeed.y = mJumpSpeed;
                    mCurrentState = PlayerState.Jump;
                }


                else if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    if (Body.mPS.pushesBottom)
                    {
                        Body.mPS.isClimbing = false;
                        mCurrentState = PlayerState.Stand;
                        Body.mSpeed.y = 0.0f;
                    }
                    else
                    {

                        Body.mSpeed.y = -mClimbSpeed;
                    }
                    // ScaleX = Mathf.Abs(ScaleX);
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
                {
                    if (Body.mPS.pushesTop)
                        Body.mSpeed.y = 0.0f;
                    else
                        Body.mSpeed.y = mClimbSpeed;
                    //ScaleX = -Mathf.Abs(ScaleX);
                }

                
                break;
            case PlayerState.Jetting:
                Body.mPS.isJetting = true;
                Body.mSpeed = Vector2.zero;
                Body.mIgnoresGravity = true;
                mCannotClimb = true;




                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
                {
                    if (Body.mPS.pushesTop)
                        Body.mSpeed.y = 0.0f;
                    else
                        Body.mSpeed.y = mClimbSpeed;
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    Body.mSpeed.y = -mClimbSpeed;
                }

                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                {
                    Body.mSpeed.x = 0.0f;
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
                {
                    if (Body.mPS.pushesRightTile)
                        Body.mSpeed.x = 0.0f;
                    else
                        Body.mSpeed.x = mWalkSpeed;
                    mDirection = EntityDirection.Right;
                    //Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
                {
                    if (Body.mPS.pushesLeftTile)
                        Body.mSpeed.x = 0.0f;
                    else
                        Body.mSpeed.x = -mWalkSpeed;
                    mDirection = EntityDirection.Left;
                    //Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
                }

                //if we hit the ground
                if (Body.mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                    {
                        mCurrentState = PlayerState.Stand;
                        //mSpeed = Vector2.zero;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                    else	//either go right or go left are pressed so we change the state to walk
                    {
                        mCurrentState = PlayerState.Walk;
                        Body.mSpeed.y = 0.0f;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                }

                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    mCurrentState = PlayerState.Jump;
                    break;
                }


                break;
        }

        /*
        if (Input.playerButtonInput[(int)ButtonInput.Swap] && !Input.previousButtonInput[(int)ButtonInput.Swap])
        {
            activeBullet++;
            if (activeBullet >= bullets.Count)
            {
                activeBullet = 0;
            }
        }
        */

        if (Input.playerButtonInput[(int)ButtonInput.Attack] && !Input.previousButtonInput[(int)ButtonInput.Attack])
        {
            AttackManager.meleeAttacks[0].Activate();
        }

        if (Input.playerAxisInput[(int)AxisInput.RightStickX] != 0 || Input.playerAxisInput[(int)AxisInput.RightStickY] != 0)
        {
            RangedAttack attack = AttackManager.rangedAttacks[0];
            Vector2 aim = GetAim();
            ((PlayerRenderer)Renderer).SetWeaponRotation(aim);
            attack.Activate(GetAim(), Position);

        }


        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();


        CollisionManager.UpdateAreas(HurtBox);
        //HurtBox.mCollisions.Clear();

        //Pretty sure this lets use jump forever

        if (Body.mPS.pushedBottom && !Body.mPS.pushesBottom || Body.mPS.isClimbing)
            mFramesFromJumpStart = 0;

        //Update the animator last
        UpdateAnimator();


    }

    public void Jump()
    {

        if (mCurrentState == PlayerState.Jump && !mDoubleJump)
            return;
        Body.mSpeed.y += mJumpSpeed;
        SoundManager.instance.PlaySingle(mJumpSfx);
        mCurrentState = PlayerState.Jump;

    }

    public void JetMode()
    {
        if (mCurrentState == PlayerState.Jetting)
            return;
        Body.mSpeed = Vector2.zero;
        mCurrentState = PlayerState.Jetting;

    }

    public void ClimbLadder()
    {
        Body.mPS.isClimbing = true;
        mCurrentState = PlayerState.Climb;
    }

    public void TryGrabLedge()
    {
        //we'll translate the original aabb's HalfSize so we get a vector Vector2iing
        //the top right corner of the aabb when we want to grab the right edge
        //and top left corner of the aabb when we want to grab the left edge
        Vector2 aabbCornerOffset;

        if (Body.mPS.pushesRight && Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
            aabbCornerOffset = Body.mAABB.HalfSize;
        else
            aabbCornerOffset = new Vector2(-Body.mAABB.HalfSizeX - 1.0f, Body.mAABB.HalfSizeY);

        int tileX, topY, bottomY;
        tileX = Map.GetMapTileXAtPoint(Body.mAABB.CenterX + aabbCornerOffset.x);

        if ((Body.mPS.pushedLeft && Body.mPS.pushesLeft) || (Body.mPS.pushedRight && Body.mPS.pushesRight))
        {
            topY = Map.GetMapTileYAtPoint(Body.mOldPosition.y + Body.mAABB.OffsetY + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
            bottomY = Map.GetMapTileYAtPoint(Body.mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
        }
        else
        {
            topY = Map.GetMapTileYAtPoint(Body.mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeStartY);
            bottomY = Map.GetMapTileYAtPoint(Body.mAABB.CenterY + aabbCornerOffset.y - Constants.cGrabLedgeEndY);
        }

        for (int y = topY; y >= bottomY; --y)
        {
            if (!Map.IsObstacle(tileX, y) && Map.IsObstacle(tileX, y - 1))
            {
                //calculate the appropriate corner
                var tileCorner = Map.GetMapTilePosition(tileX, y - 1);
                tileCorner.x -= Mathf.Sign(aabbCornerOffset.x) * MapManager.cTileSize / 2;
                tileCorner.y += MapManager.cTileSize / 2;

                //check whether the tile's corner is between our grabbing Vector2is
                if (y > bottomY ||
                    ((Body.mAABB.CenterY + aabbCornerOffset.y) - tileCorner.y <= Constants.cGrabLedgeEndY
                    && tileCorner.y - (Body.mAABB.CenterY + aabbCornerOffset.y) >= Constants.cGrabLedgeStartY))
                {
                    //save the tile we are holding so we can check later on if we can still hold onto it
                    mLedgeTile = new Vector2i(tileX, y - 1);

                    //calculate our position so the corner of our AABB and the tile's are next to each other
                    Position.y = tileCorner.y - aabbCornerOffset.y - Body.mAABB.OffsetY - Constants.cGrabLedgeStartY + Constants.cGrabLedgeTileOffsetY;
                    Body.mSpeed = Vector2.zero;

                    //finally grab the edge
                    mCurrentState = PlayerState.GrabLedge;
                    Body.mIgnoresGravity = true;
                    mDirection = (EntityDirection)((int)mDirection * -1);
                    //Body.mAABB.ScaleX *= -1;
                    Renderer.SetAnimState("GrabLedge");
                    //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                    break;
                    //mGame.PlayOneShot(SoundType.Character_LedgeGrab, mPosition, Game.sSfxVolume);
                }
            }
        }
    }

    public bool PickUp(ItemObject itemObject)
    {
        //mAllCollidingObjects.Remove(item);
        Game.FlagObjectForRemoval(itemObject);

        //Should be something like itemObject.pickup(Inventory inventory);

        Inventory.AddItemToInventory(itemObject.mItemData);
        return true;
    }

    public void UpdateAnimator()
    {
        if (activeAbilities.Contains(PlayerAbility.Invisible))
        {
            Renderer.Sprite.color = Color.clear;
            return;
        }
        else
        {
            Renderer.Sprite.color = Color.white;

        }

        foreach (Attack attack in AttackManager.meleeAttacks)
        {
            if (attack.mIsActive)
            {
                Renderer.SetAnimState("Attack");
                return;
            }
        }

        if (mCurrentState == PlayerState.Climb)
        {
            //Update the animator
            if (Mathf.Abs(Body.mSpeed.y) > 0)
                Renderer.SetAnimState("Climb");
            else
                Renderer.SetAnimState("LadderIdle");
        }
        else
        {
            Renderer.SetAnimState(mCurrentState.ToString());

        }

    }

    public Vector2 GetAim(bool freeAxis = true)
    {

        if (freeAxis)
        {
            return new Vector2(Input.playerAxisInput[(int)AxisInput.RightStickX], Input.playerAxisInput[(int)AxisInput.RightStickY]).normalized;
        }

        Vector2 aim = Vector2.zero;

        if (Input.playerAxisInput[(int)AxisInput.RightStickY] < 0)
        {
            aim += Vector2.down;

        }
        else if (Input.playerAxisInput[(int)AxisInput.RightStickY] > 0)
        {
            aim += Vector2.up;

        }

        if (Input.playerAxisInput[(int)AxisInput.RightStickX] < 0)
        {
            aim += Vector2.left;

        }
        else if (Input.playerAxisInput[(int)AxisInput.RightStickX] > 0)
        {
            aim += Vector2.right;

        }

        return aim;
    }

    public override void SecondUpdate()
    {
        if (mCurrentState == PlayerState.Dead)
        {
            return;
        }
        //Should be in the habit of doing this first, i guess
        base.SecondUpdate();
        AttackManager.SecondUpdate();
        HurtBox.UpdatePosition();

        if (MiniMapIcon != null)
        {
            MiniMapIcon.UpdateIcon(Map.GetMapTileAtPoint(Position));
        }
    }

    public void GetHurt(Attack attack)
    {
        int damage = (int)Health.LoseHP(attack.damage);
        ShowFloatingText(damage, Color.red);

        if (Health.currentHealth == 0)
        {
            Die();
        }

        //Debug.Log("Hurt by " + );

    }

    public void GainLife(int health)
    {
        int life = (int)this.Health.GainHP(health);
        ShowFloatingText(life, Color.green);


    }


    public override void Die()
    {

        //The player never dies
        Body.mState = ColliderState.Closed;
        mCurrentState = PlayerState.Dead;
        HurtBox.mState = ColliderState.Closed;

    }

    public void Ressurect()
    {
        Body.mState = ColliderState.Open;
        mCurrentState = PlayerState.Stand;
        HurtBox.mState = ColliderState.Open;
        health.currentHealth = health.maxHealth/2;
    }

    public override void Destroy()
    {
        base.Destroy();
        HurtBox.mState = ColliderState.Closed;
    }

    public override void ActuallyDie()
    {

        //we have to remove the hitboxes
        foreach (MeleeAttack attack in AttackManager.meleeAttacks)
        {
                CollisionManager.RemoveObjectFromAreas(attack.hitbox);
            
        }

        CollisionManager.RemoveObjectFromAreas(HurtBox);
        //CollisionManager.RemoveObjectFromAreas(sight);

        //Do other stuff first because the base destroys the object
        base.ActuallyDie();
    }

    public ItemObject CheckForItems()
    {
        //ItemObject item = null;
        for (int i = 0; i < Body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (Body.mCollisions[i].other.mEntity is ItemObject)
            {
                return (ItemObject)Body.mCollisions[i].other.mEntity;
            }
        }

        return null;
    }

    public Chest CheckForChest()
    {
        //ItemObject item = null;
        for (int i = 0; i < Body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (Body.mCollisions[i].other.mEntity is Chest)
            {
                return (Chest)Body.mCollisions[i].other.mEntity;
            }
        }

        return null;
    }

}
