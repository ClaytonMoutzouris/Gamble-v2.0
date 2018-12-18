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
    public List<Player> players;

    public List<bool[]> playerInputs;
    public List<bool[]> playerPrevInputs;

    public KeyCode goLeftKey = KeyCode.A;
    public KeyCode goRightKey = KeyCode.D;
    public KeyCode goJumpKey = KeyCode.Space;
    public KeyCode goDownKey = KeyCode.S;
    public KeyCode goUpKey = KeyCode.W;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    public MapManager mMap;

    public TileType mPlacedTileType;

    public Transform mCharacterPrefab;
    public Transform mMovingPlatformPrefab;
    public Transform mSlimePrefab;
    [SerializeField]
    protected List<Entity> mEntities = new List<Entity>();

    public SpriteRenderer mMouseSprite;
    public bool mMapChangeFlag = false;

    private void Awake()
    {
        instance = this;
        ItemDatabase.InitializeDatabase();
        EnemyDatabase.InitializeDatabase();
    }

    void Start ()
    {
        Application.targetFrameRate = 60;



        switch (mGameMode)
        {
            case GameMode.Game:
                playerInputs = new List<bool[]>();
                playerPrevInputs = new List<bool[]>();

                foreach (Player player in players)
                {
                    //init player 1
                    bool[] inputs = new bool[(int)KeyInput.Count];
                    bool[] prevInputs = new bool[(int)KeyInput.Count];
                    playerInputs.Add(inputs);
                    playerPrevInputs.Add(prevInputs);

                    player.EntityInit();
                    player.SetInputs(inputs, prevInputs);
                    player.Body.mCollisionType = CollisionType.Player;
                }
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
        for (int i = mEntities.Count - 1; i >= 0; i--)
        {
            mMap.RemoveObjectFromAreas(mEntities[i].Body);
        }
        mMap.Init();
        foreach(Player player in players)
        player.Body.SetTilePosition(mMap.mMapData.startTile);
    }

    public void NewEditorMap()
    {
        mMap.Init();
    }

    void HandleInputs()
    {
        for(int p = 0; p < players.Count; p++)
        {
            playerInputs[p][(int)KeyInput.GoRight] = Input.GetAxisRaw("Horizontal") > 0;
            playerInputs[p][(int)KeyInput.GoLeft] = Input.GetAxisRaw("Horizontal") < 0;
            playerInputs[p][(int)KeyInput.GoDown] = Input.GetAxisRaw("Vertical") < 0;
            playerInputs[p][(int)KeyInput.Climb] = Input.GetAxisRaw("Vertical") > 0;
            playerInputs[p][(int)KeyInput.Jump] = Input.GetButton("Jump");
            playerInputs[p][(int)KeyInput.Shoot] = Input.GetButton("Fire1");
            playerInputs[p][(int)KeyInput.Attack] = Input.GetButton("Fire2");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            //PauseMenu.instance.Open();
        }

    }

    void Update()
    {
        if (mGameMode == GameMode.Editor)
            return;

        HandleInputs();

        

        //UpdateCursor();

    }
    
    public void SwapUpdateIds(Entity a, Entity b)
    {
        int tmp = a.mUpdateId;
        a.mUpdateId = b.mUpdateId;
        b.mUpdateId = tmp;
        mEntities[a.mUpdateId] = a;
        mEntities[b.mUpdateId] = b;
    }

    public int AddToUpdateList(Entity obj)
    {
        mEntities.Add(obj);
        return mEntities.Count - 1;
    }
    public void FlagObjectForRemoval(Entity obj)
    {
        obj.mToRemove = true;
    }

    public void RemoveFromUpdateList(Entity obj)
    {
        mEntities.Remove(obj);
        int index = 0;
        foreach(Entity p in mEntities)
        {
            p.mUpdateId = index;
            index++;
        }
    }

    void FixedUpdate()
    {
        if (mGameMode == GameMode.Editor)
            return;

        for (int i = 0; i < mEntities.Count; ++i)
        {
            mEntities[i].EntityUpdate();
            mMap.UpdateAreas(mEntities[i].Body);
            mEntities[i].Body.mAllCollidingObjects.Clear();
        }

        for (int i = mEntities.Count - 1; i >= 0; i--)
        {
            if (mEntities[i].mToRemove)
            {
                Debug.Log("Removing " + mEntities[i].name + " at index " + i);
                Destroy(mEntities[i].gameObject);
                //THIS VVVVV HAS TO BE REMOVED BEFORE..
                mMap.RemoveObjectFromAreas(mEntities[i].Body);
                //THIS!!!! OTHERWISE IT FUCKS SHIT UP
                //FUCK THIS ERROR IT TOOK ME SO GODDAMN LONG TO FIX
                RemoveFromUpdateList(mEntities[i]);
                
            }
        }

        mMap.CheckCollisions();

        for (int i = 0; i < mEntities.Count; ++i)
        {
            //if(!mObjects[i].mToRemove)
            mEntities[i].Body.UpdatePhysicsP2();
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
