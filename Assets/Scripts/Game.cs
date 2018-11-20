using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public static Game instance;
    public Camera gameCamera;
    public Character player;
    bool[] inputs;
    bool[] prevInputs;

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

        inputs = new bool[(int)KeyInput.Count];
        prevInputs = new bool[(int)KeyInput.Count];

        player.mGame = this;
        player.mMap = mMap;
        player.Init(inputs, prevInputs);
        player.mType = ObjectType.Player;
       // player.Scale = Vector2.one * 1.5f;

        mMap.Init();

        player.SetTilePosition(mMap.mMapData.startTile);
    }
	
	void Update()
    {
        inputs[(int)KeyInput.GoRight] = Input.GetKey(KeyCode.RightArrow);
        inputs[(int)KeyInput.GoLeft] = Input.GetKey(KeyCode.LeftArrow);
        inputs[(int)KeyInput.GoDown] = Input.GetKey(KeyCode.DownArrow);
        inputs[(int)KeyInput.Climb] = Input.GetKey(KeyCode.UpArrow);
        inputs[(int)KeyInput.Jump] = Input.GetKey(KeyCode.Space);

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
