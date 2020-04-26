using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LocalCoop;

public enum PlayerState { Idle, Attacking, Blocking, Dead };
public enum MovementState { Stand, Walk, Jump, GrabLedge, Climb, Jetting, Swimming };

public class Player : Entity, IHurtable
{
    private Health health;
    PlayerPrototype prototype;
    public float mJumpSpeed;
    public float mWalkSpeed;
    public float mClimbSpeed;
    public bool mCanHover = false;
    public int mNumJumps = 1;
    public int mJumpCount = 0;

    public float mTimeToExit = 1;
    public HealthBar mHealthBar;
    public bool mCannotClimb = false;
    private PlayerInventory inventory;
    private PlayerEquipment equipment;
    private PlayerInputController input;
    public int activeBullet;
    public PlayerState mCurrentState = PlayerState.Idle;
    public MovementState movementState = MovementState.Stand;
    public int mPlayerIndex;
    public Stats mStats;
    public PlayerClass playerClass;
    private AttackManager attackManager;
    public MeleeAttack defaultMelee;
    public RangedAttack defaultRanged;
    public Blockbox blockbox;

    public TalentTree talentTree;

    public List<Effect> itemEffects;

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
    public PlayerPanel playerPanel;
    
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

    public void SetColorPalette(List<Color> palette)
    {
        prototype.colorPallete = palette;
    }

    public Player(PlayerPrototype proto, PlayerClass playerClass, int index) : base(proto)
    {
        prototype = ScriptableObject.Instantiate<PlayerPrototype>(proto);
        this.playerClass = ScriptableObject.Instantiate<PlayerClass>(playerClass);
        mPlayerIndex = index;
        Body = new PhysicsBody(this, new CustomAABB(Position, new Vector2(proto.bodySize.x, proto.bodySize.y), new Vector2(0, proto.bodySize.y)));
        Inventory = new PlayerInventory(this);
        Equipment = new PlayerEquipment(this);
        activeAbilities = new List<PlayerAbility>();
        itemEffects = new List<Effect>();
        mStats = new Stats(this, PlayerUIPanels.instance.playerPanels[mPlayerIndex].uiPlayerTab.statContainer);

        mWalkSpeed = prototype.walkSpeed;
        mJumpSpeed = prototype.jumpSpeed;
        mClimbSpeed = prototype.climbSpeed;
        mCollidesWith = proto.CollidesWith;
        





        //mInput = PlayerInputManager.singleton.;
        //CustomEventSystem eventSystem = GetComponent<CustomEventSystem>();
        //EventSystem.current.SetSelectedGameObject(PauseMenu.instance.defaultObject);

        HealthBar bar = PlayerUIPanels.instance.playerPanels[index].healthBar;

        Health = new Health(this, prototype.baseHealth, bar);

        HurtBox = new Hurtbox(this, new CustomAABB(Position, new Vector2(proto.bodySize.x, proto.bodySize.y), new Vector2(0, proto.bodySize.y)));
        HurtBox.UpdatePosition();

        blockbox = new Blockbox(this, new CustomAABB(Position, new Vector2(8, 12), new Vector2(proto.bodySize.x, proto.bodySize.y)));
        blockbox.UpdatePosition();
        blockbox.mState = ColliderState.Closed;
        /*
        mJumpSpeed = Constants.cJumpSpeed;
        mWalkSpeed = Constants.cWalkSpeed;
        mClimbSpeed = Constants.cClimbSpeed;
        */

        AttackManager = new AttackManager(this);

        foreach (MeleeAttackPrototype meleeAttack in prototype.meleeAttacks)
        {
            MeleeAttack melee = new MeleeAttack(this, meleeAttack);
            AttackManager.meleeAttacks.Add(melee);
            defaultMelee = melee;
            //Debug.Log("Adding Slime melee attack");
        }

        foreach (RangedAttackPrototype rangedAttack in prototype.rangedAttacks)
        {
            RangedAttack ranged = new RangedAttack(this, rangedAttack);
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
        ((PlayerRenderer)Renderer).SetPlayer(this);

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

    //Function for deriving the speed value from the speed stat
    public float GetSpeed()
    {
        float waterReduction = 1;

        if(Body.mPS.inWater)
        {
            waterReduction = 0.8f;
        }
        return (mWalkSpeed + 10*mStats.getStat(StatType.Speed).GetValue())*waterReduction;
    }

    public void UpdateShield()
    {

        foreach (Hitbox hit in blockbox.mCollisions)
        {
            //Debug.Log("Something in the collisions");
            if (!blockbox.mDealtWith.Contains(hit))
            {
                hit.mState = ColliderState.Closed;
                hit.mCollisions.Clear();
                blockbox.mDealtWith.Add(hit);

            }
        }
    }

    public void HandlePlayerPanelInput()
    {
        //Swap between panel tabs
        if (Input.playerButtonInput[(int)ButtonInput.Teleport] & !input.previousButtonInput[(int)ButtonInput.Teleport])
        {
            playerPanel.NextTabLeft();
        }

        if (Input.playerButtonInput[(int)ButtonInput.Block] & !input.previousButtonInput[(int)ButtonInput.Block])
        {
            playerPanel.NextTabRight();
        }

        //Inventory specific
        if (playerPanel.selectedTabIndex == PlayerPanelTab.Inventory)
        {
            if (Input.playerButtonInput[(int)ButtonInput.InventoryDrop])
            {
                Inventory.slots[Inventory.inventoryUI.currentSlot.slotID].DropItem();
            }

            if (Input.playerButtonInput[(int)ButtonInput.InventoryMove])
            {
                Inventory.inventoryUI.MoveItem(Inventory.inventoryUI.currentSlot.slotID);
            }


            if (Input.playerButtonInput[(int)ButtonInput.InventorySort])
            {
                Inventory.SortInventory();
            }
        }
        
    }

    public override void EntityUpdate()
    {
        if (Input == null)
        {
            Debug.Log("Input is null");
            return;
        }

        if(Input.inputState == PlayerInputState.NavigationMenu)
        {
            if (Input.playerButtonInput[(int)ButtonInput.Attack])
            {
                NavigationMenu.instance.Close();
            }
        }

        if (Input.playerButtonInput[(int)ButtonInput.Pause])
        {
            LevelManager.instance.PauseGame(mPlayerIndex);
            //PauseMenu.instance.defaultObject;
        }

        if (Input.playerButtonInput[(int)ButtonInput.BeamUp] & !input.previousButtonInput[(int)ButtonInput.BeamUp])
        {
            Game.warpToHubFlag = true;
            return;
        }


        if (Input.playerButtonInput[(int)ButtonInput.SkipLevel])
        {
            Game.mMapChangeFlag = true;
        }


        if (Input.playerButtonInput[(int)ButtonInput.Teleport])
        {
            Position = Position + GetAim()*150;
            //Game.mMapChangeFlag = true;
        }

        if (mCurrentState == PlayerState.Dead)
        {
            return;
        }

        if (Input.playerButtonInput[(int)ButtonInput.Select])
        {
            MiniMap.instance.Toggle();
        }

        if(Input.inputState == PlayerInputState.Inventory)
        {
            HandlePlayerPanelInput();
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
        UpdateShield();
        

        if (mCannotClimb && !Body.mPS.onLadder && !Input.playerButtonInput[(int)ButtonInput.Jump])
        {
            mCannotClimb = false;
        }


        if (Input.playerButtonInput[(int)ButtonInput.Item] && !Input.previousButtonInput[(int)ButtonInput.Item])
        {
            UseFirstHealingItem();
        }

        //Check to see if a player is trying to pick up an item
        if (Input.playerButtonInput[(int)ButtonInput.LeftStick_Down])
        {
            ILootable loot = CheckForItems();

            if (loot != null)
            {
                loot.Loot(this);
            }

        }

        IInteractable interactable = CheckForInteractables();
        if (interactable != null)
        {
            ((PlayerRenderer)Renderer).ShowButtonTooltip(true);
                    //Check to see if a player is trying to pick up an item
            if (Input.playerButtonInput[(int)ButtonInput.DPad_Down] && !Input.previousButtonInput[(int)ButtonInput.DPad_Down])
            {
                    interactable.Interact(this);
            } 
        }
        else
        {
            ((PlayerRenderer)Renderer).ShowButtonTooltip(false);
        }

        //Check for grounded
        if (Body.mPS.pushesBottom || Body.mPS.isClimbing)
        {
            mJumpCount = 0;
        }


        //Handle each of the players states
        switch (movementState)
        {

            case MovementState.Stand:

                mWalkSfxTimer = cWalkSfxTime;

                if (!Body.mPS.pushesBottom)
                {
                    movementState = MovementState.Jump;
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
                    //break;
                }

                //if left or right key is pressed, but not both
                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] != 0)
                {
                    movementState = MovementState.Walk;
                    break;
                }




                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
                {
                    if (Body.mPS.onLadder)
                    {
                        Body.mSpeed = Vector2.zero;
                        Body.mPS.isClimbing = true;
                        movementState = MovementState.Climb;
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
            case MovementState.Walk:

                mWalkSfxTimer += Time.deltaTime;

                if (mWalkSfxTimer > cWalkSfxTime)
                {
                    mWalkSfxTimer = 0.0f;
                    SoundManager.instance.PlaySingle(mWalkSfx);
                }

                if (!Body.mPS.pushesBottom)
                {
                    movementState = MovementState.Jump;
                    break;
                }

                //Trigger on walk effects
                foreach (Effect effect in itemEffects)
                {
                    effect.OnWalkTrigger(this);
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
                    //break;
                }

                //if both or neither left nor right keys are pressed then stop walking and stand

                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                {
                    movementState = MovementState.Stand;
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

                        Body.mSpeed.x = GetSpeed();

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
                        Body.mSpeed.x = -GetSpeed();
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
                    movementState = MovementState.Climb;
                    Body.mIgnoresGravity = true;

                    break;
                }

                break;
            case MovementState.Jump:
                if(Body.mPS.inWater)
                {
                    movementState = MovementState.Swimming;
                }
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
                    movementState = MovementState.Climb;
                    break;
                }


                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    //break;
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
                        Body.mSpeed.x = GetSpeed();
                    mDirection = EntityDirection.Right;
                    //Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
                {
                    if (Body.mPS.pushesLeftTile)
                        Body.mSpeed.x = 0.0f;
                    else
                        Body.mSpeed.x = -GetSpeed();
                    mDirection = EntityDirection.Left;
                    //Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
                }

                //if we hit the ground
                if (Body.mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                    {
                        movementState = MovementState.Stand;
                        //mSpeed = Vector2.zero;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                    else	//either go right or go left are pressed so we change the state to walk
                    {
                        movementState = MovementState.Walk;
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

            case MovementState.GrabLedge:

                Body.mIgnoresGravity = true;

                bool ledgeOnLeft = mLedgeTile.x * MapManager.cTileSize < Position.x;
                bool ledgeOnRight = !ledgeOnLeft;

                //if down button is held then drop down
                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < -0.5f
                    || (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0 && ledgeOnRight)
                    || (Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0 && ledgeOnLeft))
                {
                    if (ledgeOnLeft)
                        mCannotGoLeftFrames = 3;
                    else
                        mCannotGoRightFrames = 3;

                    movementState = MovementState.Jump;
                    //mGame.PlayOneShot(SoundType.Character_LedgeRelease, mPosition, Game.sSfxVolume);
                }
                else if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    Jump();
                }

                //when the tile we grab onto gets destroyed
                if (!Map.IsObstacle(mLedgeTile.x, mLedgeTile.y))
                    movementState = MovementState.Jump;

                break;
            case MovementState.Climb:
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
                    movementState = MovementState.Stand;
                    break;
                }


                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    Body.mPS.isClimbing = false;
                    if (Input.playerAxisInput[(int)AxisInput.LeftStickY] >= 0)
                    {
                        Jump();
                    }
                    else
                    {
                        movementState = MovementState.Jump;
                    }
                    break;
                }


                else if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    if (Body.mPS.pushesBottom)
                    {
                        Body.mPS.isClimbing = false;
                        movementState = MovementState.Stand;
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
            case MovementState.Jetting:
                Body.mPS.isJetting = true;
                Body.mSpeed = Vector2.zero;
                Body.mIgnoresGravity = true;
                mCannotClimb = true;




                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
                {
                    if (Body.mPS.pushesTop)
                        Body.mSpeed.y = 0.0f;
                    else
                        Body.mSpeed.y = GetSpeed();
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
                {
                    Body.mSpeed.y = -GetSpeed();
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
                        Body.mSpeed.x = GetSpeed();
                    mDirection = EntityDirection.Right;
                    //Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
                {
                    if (Body.mPS.pushesLeftTile)
                        Body.mSpeed.x = 0.0f;
                    else
                        Body.mSpeed.x = -GetSpeed();
                    mDirection = EntityDirection.Left;
                    //Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
                }

                //if we hit the ground
                if (Body.mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                    {
                        movementState = MovementState.Stand;
                        //mSpeed = Vector2.zero;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                    else	//either go right or go left are pressed so we change the state to walk
                    {
                        movementState = MovementState.Walk;
                        Body.mSpeed.y = 0.0f;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                }

                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    movementState = MovementState.Jump;
                    break;
                }


                break;
                case MovementState.Swimming:

                if (!Body.mPS.inWater)
                {
                    movementState = MovementState.Jump;
                    Body.mSpeed.y *= 2;
                    return;
                }
                //we do not ignore gravity while in the air
                Body.mIgnoresGravity = false;
                ++mFramesFromJumpStart;

                if (mFramesFromJumpStart <= Constants.cJumpFramesThreshold)
                {
                    if (Body.mPS.pushesTop || Body.mSpeed.y > 0.0f)
                        mFramesFromJumpStart = Constants.cJumpFramesThreshold + 1;
                    else if (Input.playerButtonInput[(int)ButtonInput.Jump])
                        Body.mSpeed.y = mJumpSpeed/1.5f;


                }


                // we can climb ladders while swimming
                if ((Input.playerAxisInput[(int)AxisInput.LeftStickY] != 0) && Body.mPS.onLadder && !mCannotClimb)
                {
                    movementState = MovementState.Climb;
                    break;
                }


                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    //break;
                }



                mWalkSfxTimer = cWalkSfxTime;

                /*
                mSpeed.y += Constants.cGravity * Time.deltaTime;

                mSpeed.y = Mathf.Max(mSpeed.y, Constants.cMaxFallingSpeed);
                */
                if (!Input.playerButtonInput[(int)ButtonInput.Jump] && Body.mSpeed.y > 0.0f && !Body.mPS.isBounce)
                {
                    Body.mSpeed.y = Mathf.Min(Body.mSpeed.y, 150.0f);
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
                        Body.mSpeed.x = GetSpeed();
                    mDirection = EntityDirection.Right;
                    //Body.mAABB.ScaleX = Mathf.Abs(Body.mAABB.ScaleX);
                }
                else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
                {
                    if (Body.mPS.pushesLeftTile)
                        Body.mSpeed.x = 0.0f;
                    else
                        Body.mSpeed.x = -GetSpeed();
                    mDirection = EntityDirection.Left;
                    //Body.mAABB.ScaleX = -Mathf.Abs(Body.mAABB.ScaleX);
                }

                //if we hit the ground
                if (Body.mPS.pushesBottom)
                {
                    //if there's no movement change state to standing
                    if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0)
                    {
                        movementState = MovementState.Stand;
                        //mSpeed = Vector2.zero;
                        SoundManager.instance.PlaySingle(mHitWallSfx);
                    }
                    else	//either go right or go left are pressed so we change the state to walk
                    {
                        movementState = MovementState.Walk;
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

        //Attacks

        if (Input.playerButtonInput[(int)ButtonInput.Attack] && !Input.previousButtonInput[(int)ButtonInput.Attack])
        {
            AttackManager.meleeAttacks[0].Activate();
        }

        Vector2 aim;
        if (Input.playerAxisInput[(int)AxisInput.RightStickX] != 0 || Input.playerAxisInput[(int)AxisInput.RightStickY] != 0)
        {
            aim = GetAim();
            ((PlayerRenderer)Renderer).ShowWeapon(true);

            ((PlayerRenderer)Renderer).SetWeaponRotation(aim);

        } else
        {
            aim = Vector2.right * (int)mDirection;
        }

        if (Input.playerButtonInput[(int)ButtonInput.Fire])
        {
            ((PlayerRenderer)Renderer).ShowWeapon(true);

            ((PlayerRenderer)Renderer).SetWeaponRotation(aim);

            //Debug.Log("Pressed Fire");
            AttackManager.rangedAttacks[0].Activate(aim, Position);
        }
        else
        {
            ((PlayerRenderer)Renderer).ShowWeapon(false);
        }

        if (Input.playerButtonInput[(int)ButtonInput.Block])
        {
            //blockbox.mAABB.SetAngle(Vector2.Angle(Vector2.right, aim));
            blockbox.UpdatePosition();
            blockbox.mState = ColliderState.Open;

            ((PlayerRenderer)Renderer).SetWeaponBlock(true);

        }
        else
        {
            ((PlayerRenderer)Renderer).SetWeaponBlock(false);
            blockbox.mState = ColliderState.Closed;

        }

        //Shield


        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();


        CollisionManager.UpdateAreas(HurtBox);
        CollisionManager.UpdateAreas(blockbox);

        //HurtBox.mCollisions.Clear();

        //Pretty sure this lets use jump forever

        if (Body.mPS.pushedBottom && !Body.mPS.pushesBottom || Body.mPS.isClimbing)
        {
            mFramesFromJumpStart = 0;
        }



        //Update the animator last
        UpdateAnimator();


    }

    public void Jump()
    {
        Debug.Log("Jumping while in air - " + mJumpCount + " ~ " + mNumJumps);
        if(Body.mPS.inWater)
        {
            mJumpCount = 0;
        }

        if (movementState == MovementState.Jump) {

            if (mCanHover)
            {
                JetMode();
                return;
            }
            if(mJumpCount >= mNumJumps)
            {
                return;
            }
        }

        mJumpCount++;


        if (Body.mPS.inWater)
        {
            movementState = MovementState.Swimming;
            Body.mSpeed.y = mJumpSpeed/1.5f;
            SoundManager.instance.PlaySingle(mJumpSfx);
        } else
        {
            movementState = MovementState.Jump;
            Body.mSpeed.y = mJumpSpeed;
            SoundManager.instance.PlaySingle(mJumpSfx);
        }

    }

    public void JetMode()
    {
        if (movementState == MovementState.Jetting)
            return;
        Body.mSpeed = Vector2.zero;
        movementState = MovementState.Jetting;

    }

    public void ClimbLadder()
    {
        Body.mPS.isClimbing = true;
        movementState = MovementState.Climb;
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
                    movementState = MovementState.GrabLedge;
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

        //Should be something like itemObject.pickup(Inventory inventory);
        if (Inventory.AddItemToInventory(itemObject.mItemData))
        {
            itemObject.Destroy();
        }
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

        if (movementState == MovementState.Climb)
        {
            //Update the animator
            if (Mathf.Abs(Body.mSpeed.y) > 0)
                Renderer.SetAnimState("Climb");
            else
                Renderer.SetAnimState("LadderIdle");
        }
        else
        {
            Renderer.SetAnimState(movementState.ToString());

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
        blockbox.UpdatePosition();


        if (MiniMapIcon != null)
        {
            MiniMapIcon.UpdateIcon(Map.GetMapTileAtPoint(Position));
        }
    }

    public void Interact()
    {

    }

    public void GetHurt(Attack attack)
    {
        if(Random.Range(0, 100) < 5 + mStats.getStat(StatType.Luck).value)
        {
            ShowFloatingText("Missed", Color.blue);
            return;
        }

        int damage = attack.GetDamage();
        //Take 1 less damage for each point of defense
        damage -= mStats.getStat(StatType.Defense).GetValue();
        if (damage < 0)
        {
            damage = 0;
        }
        Health.LoseHP(damage);
        ShowFloatingText(damage.ToString(), Color.red);

        foreach(Effect effect in itemEffects)
        {
            effect.OnDamagedTrigger(attack);
        }

        if (Health.currentHealth == 0)
        {
            Die();
        }

        //Debug.Log("Hurt by " + );

    }

    public void GainLife(int health)
    {
        int life = (int)this.Health.GainHP(health);
        ShowFloatingText(life.ToString(), Color.green);
        foreach(Effect effect in itemEffects)
        {
            effect.OnHealTrigger(this, life);
        }


    }


    public override void Die()
    {

        //The player never dies
        Body.mState = ColliderState.Closed;
        mCurrentState = PlayerState.Dead;
        HurtBox.mState = ColliderState.Closed;

        foreach(Effect effect in itemEffects)
        {
            effect.OnPlayerDeath(this);
        }

        Renderer.SetAnimState("Dead");
    }

    public void Ressurect()
    {
        Body.mState = ColliderState.Open;
        mCurrentState = PlayerState.Idle;
        movementState = MovementState.Stand;
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

        CollisionManager.RemoveObjectFromAreas(HurtBox);
        //CollisionManager.RemoveObjectFromAreas(sight);

        //Do other stuff first because the base destroys the object
        base.ActuallyDie();
    }

    public ILootable CheckForItems()
    {
        //ItemObject item = null;
        for (int i = 0; i < Body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (Body.mCollisions[i].other.mEntity is ILootable)
            {
                return (ILootable)Body.mCollisions[i].other.mEntity;
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

    public IInteractable CheckForInteractables()
    {
        //ItemObject item = null;
        for (int i = 0; i < Body.mCollisions.Count; ++i)
        {
            //Debug.Log(mAllCollidingObjects[i].other.name);
            if (Body.mCollisions[i].other.mEntity is IInteractable)
            {
                return (IInteractable)Body.mCollisions[i].other.mEntity;
            }
        }

        return null;
    }
    //TODO: put this in the invetory
    public bool UseFirstHealingItem()
    {
        //ItemObject item = null;
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i].item is ConsumableItem temp)
            {
                temp.Use(this);
                return true;
            }
        }

        return false;
    }

    public Entity GetEntity()
    {
        return this;
    }
}
