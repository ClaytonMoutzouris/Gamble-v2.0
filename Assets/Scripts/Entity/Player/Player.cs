﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;


public enum PlayerState { Idle, Attacking, Blocking, Dead, Warping };
public enum MovementState { Stand, Walk, Jump, GrabLedge, Climb, Jetting, Swimming, Hooking, Knockback, Rolling, Crouching, Mounted };

public class Player : Entity, IHurtable
{
    private Health health;
    PlayerPrototype prototype;
    public float mJumpSpeed;
    public float mWalkSpeed;
    public float mClimbSpeed;
    public int mNumJumps = 1;
    public int mJumpCount = 0;


    public float mTimeToExit = 1;
    public HealthBar mHealthBar;
    private PlayerInventory inventory;
    private PlayerEquipment equipment;
    private PlayerInputController input;
    public PlayerState mCurrentState = PlayerState.Idle;
    public MovementState movementState = MovementState.Stand;
    public int mPlayerIndex;
    public PlayerClass playerClass;
    private AttackManager attackManager;
    public MeleeAttack defaultMelee;
    public RangedAttack defaultRanged;
    public bool freeAxis = true;
    //Level up stuff
    public int playerLevel = 1;
    public int playerExperience = 0;
    public TalentTree talentTree;
    public Companion companion;
    public CompanionManager companionManager;
    public List<WeaponAttributeBonus> weaponBonuses;

    public AudioClip mHitWallSfx;
    public AudioClip mJumpSfx;
    public AudioClip mWalkSfx;
    //public AudioSource mAudioSource;
    public MiniMapIcon MiniMapIcon;
    public bool warpingIn;
    public bool warpingOut;
    public float rollDuration = 0.5f;
    public float rollTimestamp = 0;
    public float rollCooldown = 0.25f;
    public float rollCooldownTimestamp = 0;
    public float rollSpeed = 180;
    public float climbCooldown = 0.25f;
    public float climbCooldownTimestamp = 0;
    public float beamUpTime = 2.5f;
    public float beamUpTimestamp = 0;

    GameObject warpingEffect = null;

    public int quickUseIndex = 0;

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

    public void SetInput(NewGamepadInput input)
    {
        this.Input = new PlayerInputController(this, input);
    }

    public Player(PlayerPrototype proto, PlayerClass playerClass, int index) : base(proto)
    {
        prototype = ScriptableObject.Instantiate<PlayerPrototype>(proto);
        this.playerClass = ScriptableObject.Instantiate<PlayerClass>(playerClass);
        mPlayerIndex = index;
        Inventory = new PlayerInventory(this);
        Equipment = new PlayerEquipment(this);
        abilities = new List<Ability>();
        weaponBonuses = new List<WeaponAttributeBonus>();
        PlayerUIPanels.instance.playerPanels[mPlayerIndex].uiPlayerTab.player = this;

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

        companionManager = new CompanionManager();

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
        }
        SetColorPalette();

        Position = spawnPoint;
        Renderer.Draw();
        Body.UpdatePosition();
        isSpawned = true;

        HurtBox.UpdatePosition();
    }

    //Function for deriving the speed value from the speed stat
    public override float GetSpeed()
    {
        float waterReduction = 1;

        if(Body.mPS.inWater)
        {
            waterReduction = 0.8f;
        }
        return (mWalkSpeed + mMovingSpeed * 0.01f * mStats.GetSecondaryStat(SecondaryStatType.MoveSpeedBonus).GetValue()) *waterReduction;
    }

    public void HandlePlayerPanelInput()
    {
        //Swap between panel tabs
        if (Input.playerButtonInput[(int)ButtonInput.ChangeTabLeft] && !Input.previousButtonInput[(int)ButtonInput.ChangeTabLeft])
        {
            playerPanel.NextTabLeft();
        }

        if (Input.playerButtonInput[(int)ButtonInput.ChangeTabRight] && !Input.previousButtonInput[(int)ButtonInput.ChangeTabRight])
        {
            playerPanel.NextTabRight();
        }

        //Inventory specific
        if (playerPanel.isOpen && playerPanel.selectedTabIndex == PlayerPanelTabType.Inventory)
        {
            if (Input.playerButtonInput[(int)ButtonInput.InventoryDrop] && !Input.previousButtonInput[(int)ButtonInput.InventoryDrop])
            {
                if(Inventory.inventoryUI.currentSlot != null)
                Inventory.slots[Inventory.inventoryUI.currentSlot.slotID].DropItem();
            }

            if (Input.playerButtonInput[(int)ButtonInput.InventoryMove] && !Input.previousButtonInput[(int)ButtonInput.InventoryMove])
            {
                if (Inventory.inventoryUI.currentSlot != null)
                    Inventory.inventoryUI.MoveItem(Inventory.inventoryUI.currentSlot.slotID);
            }


            if (Input.playerButtonInput[(int)ButtonInput.InventorySort] && !Input.previousButtonInput[(int)ButtonInput.InventorySort])
            {
                Inventory.SortInventory();
            }
        }
        
    }

    public bool GetCurrency(Item currencyType, int amount)
    {
        InventorySlot slot = Inventory.GetSlotWithItemType(currencyType);
        if (slot != null)
        {
            if (slot.amount >= amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    slot.GetOneItem();

                }

                return true;
            }
        }


        return false;
    }

    public int GetCurrencyCount(Item currencyType)
    {
        int count = 0;
        InventorySlot slot = Inventory.GetSlotWithItemType(currencyType);
        if (slot != null)
        {
            count += slot.amount;
        }

        return count;
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
            if (Input.playerButtonInput[(int)ButtonInput.Menu_Back])
            {
                NavigationMenu.instance.Close();
            }
        }

        if (Input.inputState == PlayerInputState.Shop)
        {
            if (Input.playerButtonInput[(int)ButtonInput.Menu_Back])
            {
                ShopScreenUI.instance.Close();
            }
        }

        if (Input.playerButtonInput[(int)ButtonInput.Pause] && !Input.previousButtonInput[(int)ButtonInput.Pause])
        {
            GameManager.instance.PauseGame(mPlayerIndex);
            //PauseMenu.instance.defaultObject;
        }


        if (warpingIn)
        {

        }

        if (mCurrentState == PlayerState.Dead)
        {
            return;
        }

        if (Input.playerButtonInput[(int)ButtonInput.Minimap] && !Input.previousButtonInput[(int)ButtonInput.Minimap])
        {
            MiniMap.instance.Toggle();
        }

        if(Input.inputState == PlayerInputState.Inventory)
        {
            HandlePlayerPanelInput();
        }

        if (Input.playerButtonInput[(int)ButtonInput.FireMode] && !Input.previousButtonInput[(int)ButtonInput.FireMode])
        {
            freeAxis = !freeAxis;
        }

        //Update UI things
        //Toolbelt
        if (Input.playerButtonInput[(int)ButtonInput.CycleQuickUseLeft] && !Input.previousButtonInput[(int)ButtonInput.CycleQuickUseLeft])
        {
            Debug.Log("Cycle Left");

            quickUseIndex--;

            if (quickUseIndex < 0)
            {
                int slotsCount = inventory.GetQuickuseSlots().Count;

                quickUseIndex = slotsCount - 1;
                Mathf.Clamp(quickUseIndex, 0, slotsCount);
            }
        }

        if (Input.playerButtonInput[(int)ButtonInput.CycleQuickUseRight] && !Input.previousButtonInput[(int)ButtonInput.CycleQuickUseRight])
        {
            Debug.Log("Cycle Right");
            quickUseIndex++;
            int slotsCount = inventory.GetQuickuseSlots().Count;

            if (quickUseIndex >= slotsCount)
            {
                quickUseIndex = 0;
            }
        }

        UpdateToolbelt();

        //
        if (Input.playerButtonInput[(int)ButtonInput.PlayerMenu] && !Input.previousButtonInput[(int)ButtonInput.PlayerMenu])
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
        if(Equipment.GetGadget1() != null)
        {
            ((Gadget)Equipment.GetGadget1()).GadgetUpdate(this);
        }

        if (Equipment.GetGadget2() != null)
        {
            ((Gadget)Equipment.GetGadget2()).GadgetUpdate(this);
        }
        

        //playerPanel.toolbeltUI.UpdateQuickUseNode();
        if (Input.playerButtonInput[(int)ButtonInput.QuickHeal] && !Input.previousButtonInput[(int)ButtonInput.QuickHeal])
        {
            QuickUse();
        }

        
        //Check to see if a player is trying to pick up an item
        if (Input.playerAxisInput[(int)AxisInput.LeftStickY] == -1 && Input.previousAxisInput[(int)AxisInput.LeftStickY] != -1)
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
            ((PlayerRenderer)Renderer).ShowButtonTooltip(true, interactable.InteractLabel);
                    //Check to see if a player is trying to pick up an item
            if (Input.playerButtonInput[(int)ButtonInput.Interact] && !Input.previousButtonInput[(int)ButtonInput.Interact])
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


                Body.mPS.tmpIgnoresOneWay = false;


                Body.mSpeed.x = 0;



                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    break;
                }

                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    Roll();
                    break;
                }

                //if left or right key is pressed, but not both
                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] != 0)
                {
                    movementState = MovementState.Walk;
                    break;
                }

                //Check to see if the player is trying to pass through a one way
                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] <= -0.5)
                {
                    movementState = MovementState.Crouching;
                    break;

                }


                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0.5f)
                {
                    if (Body.mPS.onLadder && ClimbLadder())
                    {
                        break;
                    }


                }

                //Check to see if the player is trying to pass through a one way
                if (Input.playerButtonInput[(int)ButtonInput.BeamUp] && Map.mCurrentMap.Data.mapType == MapType.World)
                {
                    if(!Input.previousButtonInput[(int)ButtonInput.BeamUp])
                    {
                        beamUpTimestamp = Time.time;
                        warpingEffect = Renderer.AddVisualEffect(Resources.Load<ParticleSystem>("Prefabs/ParticleEffects/WarpingEffect"), Body.mOffset);
                    }


                    if(Time.time > beamUpTimestamp + beamUpTime)
                    {
                        GameManager.instance.TravelToShip();

                        break;
                    }

                } else
                {
                    if(warpingEffect != null)
                    {
                        Renderer.RemoveVisualEffect(warpingEffect);
                        warpingEffect = null;
                    }

                }

                break;
            case MovementState.Crouching:

                Body.mAABB.ScaleY = 0.5f;
                HurtBox.mAABB.ScaleY = 0.5f;
                //Body.mAABB.HalfSizeY = Body.mAABB.HalfSizeY / 2;
                if (!Body.mPS.pushesBottom)
                {
                    movementState = MovementState.Jump;
                    Body.mAABB.ScaleY = 1f;
                    HurtBox.mAABB.ScaleY = 1f;

                    break;
                }


                Body.mPS.tmpIgnoresOneWay = false;


                Body.mSpeed.x = 0;

                if(Input.playerAxisInput[(int)AxisInput.LeftStickY] > -0.5)
                {
                    movementState = MovementState.Stand;
                    Body.mAABB.ScaleY = 1f;
                    HurtBox.mAABB.ScaleY = 1f;


                    break;
                }

                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    Roll();
                    Body.mAABB.ScaleY = 1f;
                    HurtBox.mAABB.ScaleY = 1f;
                    break;
                }

                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    if (Body.mPS.onOneWay)
                    {
                        Body.mPS.tmpIgnoresOneWay = true;
                        break;
                    } else
                    {
                        Body.mAABB.ScaleY = 1f;
                        HurtBox.mAABB.ScaleY = 1f;
                        Jump();
                    }
                }

                //if left or right key is pressed, but not both
                if (Input.playerAxisInput[(int)AxisInput.LeftStickX] != 0)
                {
                    movementState = MovementState.Walk;
                    Body.mAABB.ScaleY = 1f;
                    HurtBox.mAABB.ScaleY = 1f;


                    break;
                }


                break;

            case MovementState.Rolling:

                Body.mIgnoresGravity = false;
                Body.mPS.tmpIgnoresOneWay = false;
                Body.mSpeed.x = GetSpeed() * 2 * (int)mDirection;

                if (Time.time > rollTimestamp + rollDuration)
                {
                    StopRoll();
                }

                if (Body.mSpeed.y <= 0.0f && !Body.mPS.pushesTop
                    && ((Body.mPS.pushesRight && Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0) || (Body.mPS.pushesLeft && Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)))
                {
                    if (TryGrabLedge())
                    {
                        StopRoll();
                        movementState = MovementState.GrabLedge;

                    }

                }
                break;
            case MovementState.Walk:


                if (Body.mPS.inUpdraft)
                {
                    Jump();
                    break;
                }

                if (!Body.mPS.pushesBottom)
                {
                    movementState = MovementState.Jump;
                    break;
                }

                Body.mPS.tmpIgnoresOneWay = false;


                mWalkSfxTimer += Time.deltaTime;

                if (mWalkSfxTimer > cWalkSfxTime)
                {
                    mWalkSfxTimer = 0.0f;
                    SoundManager.instance.PlaySingle(mWalkSfx);
                }

                //Trigger on walk effects
                foreach (Ability effect in abilities)
                {
                    effect.OnWalkTrigger(this);
                }

                //Check to see if the player is trying to pass through a one way
                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] == -1 && Input.playerButtonInput[(int)ButtonInput.Jump])
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

                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    Roll();
                    break;
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


                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0.5f && Body.mPS.onLadder)
                {
                    ClimbLadder();

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
                        Body.mSpeed.y = Mathf.Min(mJumpSpeed, -Constants.cMaxFallingSpeed);


                }

                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    Roll();
                    break;
                }

                // we can climb ladders from this state
                if ((Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0.5f || Input.playerAxisInput[(int)AxisInput.LeftStickY] < -0.5f) && Body.mPS.onLadder && ClimbLadder())
                {
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
                if (Body.mPS.pushesBottom && !Body.mPS.pushedBottom && !Body.mPS.inUpdraft)
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


                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < -0.5f)
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

                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    Roll();
                    break;
                }

                //when the tile we grab onto gets destroyed
                if (!Map.IsObstacle(mLedgeTile.x, mLedgeTile.y))
                    movementState = MovementState.Jump;

                break;
            case MovementState.Climb:
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

                    StopClimb();
                    break;
                }

                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    StopClimb();
                    Roll();
                    break;
                }


                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    //the speed is positive so we don't have to worry about hero grabbing an edge
                    //right after he jumps because he doesn't grab if speed.y > 0
                    StopClimb();
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
                        StopClimb();
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
                if(Body.mPS.inWater)
                {
                    movementState = MovementState.Swimming;
                    return;
                }
                Body.mSpeed = Vector2.zero;
                Body.mIgnoresGravity = true;
                Body.mPS.tmpIgnoresOneWay = true;


                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    Roll();
                    break;
                }


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
                if ((Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0.5f || Input.playerAxisInput[(int)AxisInput.LeftStickY] < -0.5f) && Body.mPS.onLadder && ClimbLadder())
                {
                    break;
                }


                if (Input.playerButtonInput[(int)ButtonInput.Jump] && !Input.previousButtonInput[(int)ButtonInput.Jump])
                {
                    Jump();
                    //break;
                }

                if (Input.playerButtonInput[(int)ButtonInput.Roll] && !Input.previousButtonInput[(int)ButtonInput.Roll])
                {
                    Roll();
                    break;
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


                if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < -0.5f)
                {
                    Body.mPS.tmpIgnoresOneWay = true;
                }




                break;
            case MovementState.Hooking:

                Body.mIgnoresGravity = true;
                Body.mPS.isClimbing = false;


                break;
            case MovementState.Knockback:
                if(Body.mPS.pushesBottomTile && !Body.mPS.pushedBottomTile)
                {
                    movementState = MovementState.Stand;
                }

                if(Body.mPS.pushesLeftTile || Body.mPS.pushesRightTile)
                {
                    movementState = MovementState.Jump;
                }

                if(Body.mSpeed == Vector2.zero)
                {
                    movementState = MovementState.Stand;
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

        if (Input.playerButtonInput[(int)ButtonInput.MeleeAttack] && !Input.previousButtonInput[(int)ButtonInput.MeleeAttack])
        {
            //AttackManager.meleeAttacks[0].Activate();
            if (Equipment.GetSlot(EquipmentSlotType.Offhand).GetContents() is MeleeWeapon weapon)
            {
                weapon.Attack();
                //((PlayerRenderer)Renderer).UpdateAmmo(weapon);

            }
        }

        Vector2 aim;
        if (Input.playerAxisInput[(int)AxisInput.RightStickX] != 0 || Input.playerAxisInput[(int)AxisInput.RightStickY] != 0)
        {
            aim = GetAimRight();
            ((PlayerRenderer)Renderer).ShowWeapon(true);

            ((PlayerRenderer)Renderer).SetWeaponRotation(aim);

        } else
        {
            aim = Vector2.right * (int)mDirection;
        }

        if (Input.playerButtonInput[(int)ButtonInput.Fire])
        {

            if (Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon weapon)
            {
                weapon.Attack();
                //((PlayerRenderer)Renderer).UpdateAmmo(weapon);

            }

             ((PlayerRenderer)Renderer).ShowWeapon(true);

            ((PlayerRenderer)Renderer).SetWeaponRotation(aim);

            //Debug.Log("Pressed Fire");
            //AttackManager.rangedAttacks[0].Activate(aim, Position);
        }
        else
        {
            ((PlayerRenderer)Renderer).ShowWeapon(false);

        }

        if (Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon rangedWeapon)
        {
            rangedWeapon.OnUpdate();
            ((PlayerRenderer)Renderer).UpdateAmmo(rangedWeapon);

        }

        if (Input.playerButtonInput[(int)ButtonInput.Gadget1] && !Input.previousButtonInput[(int)ButtonInput.Gadget1])
        {
            if (Equipment.GetGadget1() != null)
            {
                ((Gadget)Equipment.GetGadget1()).Activate(this, 1);
            }
            //Game.mMapChangeFlag = true;
        }

        if (Input.playerButtonInput[(int)ButtonInput.Gadget2] && !Input.previousButtonInput[(int)ButtonInput.Gadget2])
        {
            if (Equipment.GetGadget2() != null)
            {
                ((Gadget)Equipment.GetGadget2()).Activate(this, 2);
            }
            //Game.mMapChangeFlag = true;
        }
        //Shield


        //Calling this pretty much just updates the body
        //Let's seee if we can make it update collision stuff aswell
        base.EntityUpdate();


        CollisionManager.UpdateAreas(HurtBox);

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

        if (Body.mPS.inWater)
        {
            mJumpCount = 0;
        }

        if (movementState == MovementState.Jump) {

            if (abilityFlags.GetFlag(AbilityFlag.Hover))
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

    public void Roll()
    {
        if(Time.time < rollCooldownTimestamp + rollCooldown)
        {
            return;
        }
        rollTimestamp = Time.time;
        movementState = MovementState.Rolling;
        Body.mSpeed.x = rollSpeed*(int)mDirection;
        HurtBox.mState = ColliderState.Closed;
    }

    public void StopRoll()
    {
        movementState = MovementState.Stand;
        Body.mSpeed.x = 0;
        HurtBox.mState = ColliderState.Open;
        rollCooldownTimestamp = Time.time;
    }

    public void JetMode()
    {
        if (movementState == MovementState.Jetting)
            return;
        Body.mSpeed = Vector2.zero;
        movementState = MovementState.Jetting;

    }

    public bool ClimbLadder()
    {
        if (Time.time < climbCooldownTimestamp + climbCooldown)
        {
            return false;
        }
        Body.mPS.isClimbing = true;
        movementState = MovementState.Climb;
        Body.mSpeed = Vector2.zero;
        Body.mIgnoresGravity = true;
        return true;
    }

    public void StopClimb()
    {
        climbCooldownTimestamp = Time.time;
        Body.mPS.isClimbing = false;
        movementState = MovementState.Stand;
    }

    public bool TryGrabLedge()
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
                    return true;
                    //mGame.PlayOneShot(SoundType.Character_LedgeGrab, mPosition, Game.sSfxVolume);
                }
            }
        }

        return false;
    }

    public bool PickUp(ItemObject itemObject)
    {
        //mAllCollidingObjects.Remove(item);

        //Should be something like itemObject.pickup(Inventory inventory);
        if (Inventory.AddItemToInventory(itemObject.mItemData))
        {
            itemObject.Destroy();
            return true;
        }
        return false;
    }

    public void UpdateAnimator()
    {

        if (movementState != MovementState.Rolling)
        {
            foreach (Attack attack in AttackManager.meleeAttacks)
            {
                if (attack.mIsActive)
                {
                    Renderer.SetAnimState("Attack");
                    return;
                }
            }
        }

        if (movementState == MovementState.Climb)
        {
            //Update the animator
            if (Mathf.Abs(Body.mSpeed.y) > 0)
                Renderer.SetAnimState("Climb");
            else
                Renderer.SetAnimState("LadderIdle");
        } else if(movementState == MovementState.Hooking)
        {
            Renderer.SetAnimState("Jump");
        }
        else
        {
            Renderer.SetAnimState(movementState.ToString());

        }



    }

    public Vector2 GetAimRight()
    {
        if (Input.playerAxisInput[(int)AxisInput.RightStickY] == 0 && Input.playerAxisInput[(int)AxisInput.RightStickX] == 0)
        {
            return Vector2.right * (int)mDirection;
        }

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

    public Vector2 GetAimLeft(bool freeAxis = true)
    {

        if (Input.playerAxisInput[(int)AxisInput.LeftStickX] == 0 && Input.playerAxisInput[(int)AxisInput.LeftStickY] == 0)
        {
            return Vector2.right * (int)mDirection;
        }

        if (freeAxis)
        {
            return new Vector2(Input.playerAxisInput[(int)AxisInput.LeftStickX], Input.playerAxisInput[(int)AxisInput.LeftStickY]).normalized;
        }

        Vector2 aim = Vector2.zero;

        if (Input.playerAxisInput[(int)AxisInput.LeftStickY] < 0)
        {
            aim += Vector2.down;

        }
        else if (Input.playerAxisInput[(int)AxisInput.LeftStickY] > 0)
        {
            aim += Vector2.up;

        }

        if (Input.playerAxisInput[(int)AxisInput.LeftStickX] < 0)
        {
            aim += Vector2.left;

        }
        else if (Input.playerAxisInput[(int)AxisInput.LeftStickX] > 0)
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

    public void Interact()
    {

    }

    public void GetHurt(Attack attack)
    {
        if(Random.Range(0, 100) < 5 + mStats.GetSecondaryStat(SecondaryStatType.DodgeChance).GetValue())
        {
            ShowFloatingText("Missed", Color.grey);
            return;
        }

        int damage = attack.GetDamage();
        //Take 5% less damage for each point of defense
        //Limit defense to 80% reduction
        damage -= (int)(damage * Mathf.Min(0.8f, 0.01f*mStats.GetSecondaryStat(SecondaryStatType.DamageReduction).GetValue()));

        if (damage <= 0)
        {
            damage = 0;
        }
        else
        {
            //Crits
            if (attack.mEntity != null && Random.Range(0, 100) < attack.mEntity.mStats.GetSecondaryStat(SecondaryStatType.CritChance).GetValue())
            {
                damage *= 2;
                ShowFloatingText(damage.ToString(), Color.yellow, 2, 20, 2);
            }
            else
            {
                ShowFloatingText(damage.ToString(), Color.red);
            }

            health.LoseHP(damage);

            foreach (Ability effect in abilities)
            {
                effect.OnDamagedTrigger(attack);
            }
        }




        if (Health.currentHealth == 0)
        {
            Die();
        }

        //Debug.Log("Hurt by " + );

    }

    public void GainLife(int health, bool fromTrigger = false)
    {
        int life = (int)this.Health.GainHP(health);
        ShowFloatingText(life.ToString(), Color.green);

        //heals from triggers shouldnt also trigger
        if(!fromTrigger)
        {
            foreach (Ability effect in abilities)
            {
                effect.OnHealTrigger(this, life);
            }
        }
    }


    public override void Die()
    {

        //The player never dies
        Body.mState = ColliderState.Closed;
        mCurrentState = PlayerState.Dead;
        HurtBox.mState = ColliderState.Closed;

        List<Ability> tempAbilities = new List<Ability>();
        tempAbilities.AddRange(abilities);
 
        foreach (Ability effect in tempAbilities)
        {
            effect.OnOwnerDeath(this);
        }

        foreach(Talent talent in talentTree.GetAllTalents())
        {
            if(talent.isLearned)
            {
                foreach(Ability ability in talent.abilities)
                {
                    ability.OnRemoveTrigger(this);
                }
            }
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
        health.UpdateHealth();

        foreach (EquipmentSlot slot in Equipment.GetSlots())
        {
            if(slot.GetContents() != null)
            {
                foreach(Ability ability in slot.GetContents().Effects)
                {
                    ability.OnGainTrigger(this);
                }
            }
        }

        foreach (Talent talent in talentTree.GetAllTalents())
        {
            if (talent.isLearned)
            {
                foreach (Ability ability in talent.abilities)
                {
                    ability.OnGainTrigger(this);
                }
            }
        }
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
        List<Ability> tempAbilities = new List<Ability>();
        tempAbilities.AddRange(abilities);
        foreach (Ability effect in tempAbilities)
        {
            effect.OnOwnerDeath(this);
        }

        foreach(Talent talent in talentTree.GetAllTalents())
        {
            if(talent.isLearned)
            {
                foreach(Ability ability in talent.abilities)
                {
                    ability.OnRemoveTrigger(this);
                }
            }
        }
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


    public void UpdateToolbelt()
    {
        List<InventorySlot> slots = inventory.GetQuickuseSlots();

        if(slots.Count <= 0)
        {
            playerPanel.toolbeltUI.UpdateQuickUseNode(null, 0);
            return;
        }

        if(quickUseIndex >= slots.Count)
        {
            quickUseIndex = slots.Count - 1;
        }

        if (inventory.GetQuickuseSlots()[quickUseIndex].item is ConsumableItem consumable)
        {
            playerPanel.toolbeltUI.UpdateQuickUseNode(consumable, inventory.GetQuickuseSlots()[quickUseIndex].amount);
        }
    }

    public void QuickUse()
    {
        //playerPanel.toolbeltUI.UpdateQuickUseNode(temp);
        List<InventorySlot> slots = inventory.GetQuickuseSlots();

        if (slots.Count <= 0)
        {
            return;
        }

        if (slots[quickUseIndex].item is ConsumableItem consumable && consumable.Use(this))
        {
            slots[quickUseIndex].GetOneItem();
        }

    }

    public void AddWeaponBonus(WeaponAttributeBonus bonus)
    {
        //This null checks right?
        if (Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged)
        {
            ranged.attributes.AddBonus(bonus);

        }

        if (Equipment.GetSlot(EquipmentSlotType.Offhand).GetContents() is MeleeWeapon melee)
        {
            melee.attributes.AddBonus(bonus);

        }

        weaponBonuses.Add(bonus);
    }

    public void RemoveWeaponBonus(WeaponAttributeBonus bonus)
    {
        //This null checks right?
        if (Equipment.GetSlot(EquipmentSlotType.Mainhand).GetContents() is RangedWeapon ranged)
        {
            ranged.attributes.RemoveBonus(bonus);

        }

        if (Equipment.GetSlot(EquipmentSlotType.Offhand).GetContents() is MeleeWeapon melee)
        {
            melee.attributes.RemoveBonus(bonus);

        }

        weaponBonuses.Remove(bonus);
    }

    public Entity GetEntity()
    {
        return this;
    }
}
