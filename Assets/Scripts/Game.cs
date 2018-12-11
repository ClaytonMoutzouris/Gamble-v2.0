using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameMode
{
    Game,
    Editor
}
public class Game : MonoBehaviour
{
    public GameMode mGameMode;
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
    public bool mMapChangeFlag = false;

    private void Awake()
    {
        instance = this;
        ItemDatabase.InitializeDatabase();
    }

    void Start ()
    {
        Application.targetFrameRate = 60;



        switch (mGameMode)
        {
            case GameMode.Game:

                //init player 1
                inputs = new bool[(int)KeyInput.Count];
                prevInputs = new bool[(int)KeyInput.Count];


                player1.ObjectInit();
                player1.SetInputs(inputs, prevInputs);
                player1.mType = ObjectType.Player;
                // player.Scale = Vector2.one * 1.5f;

                //init player 2
                p2inputs = new bool[(int)KeyInput.Count];
                p2prevInputs = new bool[(int)KeyInput.Count];

                player2.ObjectInit();
                player2.SetInputs(p2inputs, p2prevInputs);
                player2.mType = ObjectType.Player;
                //player2.GetComponent<SpriteRenderer>().color = Color.gray;

                NewGameMap();

           break;
            case GameMode.Editor:
                NewEditorMap();
                break;
        }
        
    }

    public void NewGameMap()
    {
        //We have to clear/reset the players refence to the map
        //For now, clearing its areas does the job
        for (int i = mObjects.Count - 1; i >= 0; i--)
        {
            mMap.RemoveObjectFromAreas(mObjects[i]);
        }
        mMap.Init();
        player1.SetTilePosition(mMap.mMapData.startTile);
        player2.SetTilePosition(mMap.mMapData.startTile);
    }

    public void NewEditorMap()
    {
        mMap.Init();
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
        if (mGameMode == GameMode.Editor)
            return;

        HandleInputs();

        

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
        if (mGameMode == GameMode.Editor)
            return;

        for (int i = 0; i < mObjects.Count; ++i)
        {
            mObjects[i].CustomUpdate();
            mMap.UpdateAreas(mObjects[i]);
            mObjects[i].mAllCollidingObjects.Clear();
        }

        for (int i = mObjects.Count - 1; i >= 0; i--)
        {
            if (mObjects[i].mToRemove)
            {
                Debug.Log("Removing " + mObjects[i].name + " at index " + i);
                Destroy(mObjects[i].gameObject);
                //THIS VVVVV HAS TO BE REMOVED BEFORE..
                mMap.RemoveObjectFromAreas(mObjects[i]);
                //THIS!!!! OTHERWISE IT FUCKS SHIT UP
                //FUCK THIS ERROR IT TOOK ME SO GODDAMN LONG TO FIX
                RemoveFromUpdateList(mObjects[i]);
                
            }
        }

        mMap.CheckCollisions();

        for (int i = 0; i < mObjects.Count; ++i)
        {
            //if(!mObjects[i].mToRemove)
            mObjects[i].UpdatePhysicsP2();
        }

        /*If we want to change the map, we have to either abort everything or wait until we're finished updating
        * to perform the change. This method waits until everything is updated.
        * 
        * 
        */ 
        if (mMapChangeFlag)
        {
            NewGameMap();
            mMapChangeFlag = false;
        }
        
    }
}
