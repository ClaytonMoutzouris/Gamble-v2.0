using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public static Game instance;
    public Camera gameCamera;
    public Character player1;
    public Character player2;
    bool[] inputs;
    bool[] prevInputs;

    bool[] p2inputs;
    bool[] p2prevInputs;

    public KeyCode goLeftKey = KeyCode.A;
    public KeyCode goRightKey = KeyCode.D;
    public KeyCode goJumpKey = KeyCode.Space;
    public KeyCode goDownKey = KeyCode.S;
    public KeyCode goUpKey = KeyCode.W;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    public Map mMap;

    public TileType mPlacedTileType;

    public Transform mCharacterPrefab;
    public Transform mMovingPlatformPrefab;
    public Transform mSlimePrefab;
    [SerializeField]
    protected List<MovingObject> mObjects = new List<MovingObject>();

    public SpriteRenderer mMouseSprite;

    private void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        Application.targetFrameRate = 60;

        //init player 1
        inputs = new bool[(int)KeyInput.Count];
        prevInputs = new bool[(int)KeyInput.Count];

        player1.mGame = this;
        player1.mMap = mMap;
        player1.Init(inputs, prevInputs);
        player1.mType = ObjectType.Player;
        // player.Scale = Vector2.one * 1.5f;

        //init player 2
        p2inputs = new bool[(int)KeyInput.Count];
        p2prevInputs = new bool[(int)KeyInput.Count];

        player2.mGame = this;
        player2.mMap = mMap;
        player2.Init(p2inputs, p2prevInputs);
        player2.mType = ObjectType.Player;
        player2.GetComponent<SpriteRenderer>().color = Color.gray;

        mMap.Init();

        player1.SetTilePosition(mMap.mMapData.startTile);
        player2.SetTilePosition(mMap.mMapData.startTile);

    }

    void HandleInputs()
    {
        inputs[(int)KeyInput.GoRight] = Input.GetKey(KeyCode.RightArrow);
        inputs[(int)KeyInput.GoLeft] = Input.GetKey(KeyCode.LeftArrow);
        inputs[(int)KeyInput.GoDown] = Input.GetKey(KeyCode.DownArrow);
        inputs[(int)KeyInput.Climb] = Input.GetKey(KeyCode.UpArrow);
        inputs[(int)KeyInput.Jump] = Input.GetKey(KeyCode.Space);

        p2inputs[(int)KeyInput.GoRight] = Input.GetKey(KeyCode.D);
        p2inputs[(int)KeyInput.GoLeft] = Input.GetKey(KeyCode.A);
        p2inputs[(int)KeyInput.GoDown] = Input.GetKey(KeyCode.S);
        p2inputs[(int)KeyInput.Climb] = Input.GetKey(KeyCode.W);
        p2inputs[(int)KeyInput.Jump] = Input.GetKey(KeyCode.F);
    }

    void Update()
    {
        HandleInputs();

        if (Input.GetKeyDown(KeyCode.D))
        {
            mMap.DebugPrintAreas();
        }

        //UpdateCursor();

    }
    
    public void SwapUpdateIds(MovingObject a, MovingObject b)
    {
        int tmp = a.mUpdateId;
        a.mUpdateId = b.mUpdateId;
        b.mUpdateId = tmp;
        mObjects[a.mUpdateId] = a;
        mObjects[b.mUpdateId] = b;
    }

    public int AddToUpdateList(MovingObject obj)
    {
        mObjects.Add(obj);
        return mObjects.Count - 1;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < mObjects.Count; ++i)
        {
            switch (mObjects[i].mType)
            {
                case ObjectType.Player:
                
                case ObjectType.NPC:
                    ((Character)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
                case ObjectType.Enemy:
                    ((Slime)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
                case ObjectType.MovingPlatform:
                    ((MovingPlatform)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
                case ObjectType.FallingRock:
                    ((FallingRock)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
                case ObjectType.RollingBoulder:
                    ((RollingBoulder)mObjects[i]).CustomUpdate();
                    mMap.UpdateAreas(mObjects[i]);
                    mObjects[i].mAllCollidingObjects.Clear();
                    break;
            }
        }

        mMap.CheckCollisions();

        for (int i = 0; i < mObjects.Count; ++i)
            mObjects[i].UpdatePhysicsP2();
    }
}
