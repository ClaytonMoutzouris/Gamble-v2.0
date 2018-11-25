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
    protected List<PhysicsObject> mObjects = new List<PhysicsObject>();

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
        //player2.GetComponent<SpriteRenderer>().color = Color.gray;

        NewMap();

    }

    public void NewMap()
    {
        mMap.Init();

        player1.SetTilePosition(mMap.mMapData.startTile);
        player2.SetTilePosition(mMap.mMapData.startTile);
    }

    void HandleInputs()
    {
        inputs[(int)KeyInput.GoRight] = Input.GetAxisRaw("Horizontal") > 0;
        inputs[(int)KeyInput.GoLeft] = Input.GetAxisRaw("Horizontal") < 0;
        inputs[(int)KeyInput.GoDown] = Input.GetAxisRaw("Vertical") < 0;
        inputs[(int)KeyInput.Climb] = Input.GetAxisRaw("Vertical") > 0;
        inputs[(int)KeyInput.Jump] = Input.GetButton("Jump");

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
    
    public void SwapUpdateIds(PhysicsObject a, PhysicsObject b)
    {
        int tmp = a.mUpdateId;
        a.mUpdateId = b.mUpdateId;
        b.mUpdateId = tmp;
        mObjects[a.mUpdateId] = a;
        mObjects[b.mUpdateId] = b;
    }

    public int AddToUpdateList(PhysicsObject obj)
    {
        mObjects.Add(obj);
        return mObjects.Count - 1;
    }
    public void FlagObjectForRemoval(PhysicsObject obj)
    {
        obj.mToRemove = true;
    }

    public void RemoveFromUpdateList(PhysicsObject obj)
    {
        mObjects.Remove(obj);
        int index = 0;
        foreach(PhysicsObject p in mObjects)
        {
            p.mUpdateId = index;
            index++;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < mObjects.Count; ++i)
        {
            mObjects[i].CustomUpdate();
            mMap.UpdateAreas(mObjects[i]);
            mObjects[i].mAllCollidingObjects.Clear();
        }

        mMap.CheckCollisions();

        for (int i = 0; i < mObjects.Count; ++i)
            mObjects[i].UpdatePhysicsP2();

        for (int i = mObjects.Count-1; i >= 0; i--)
        {
            if (mObjects[i].mToRemove)
            {
                Destroy(mObjects[i].gameObject);
                RemoveFromUpdateList(mObjects[i]);
                mMap.RemoveObjectFromAreas(mObjects[i]);
            }
        }
    }
}
