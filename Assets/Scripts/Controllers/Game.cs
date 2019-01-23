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
        CollisionManager.InitializeCollisionManager();
        ItemDatabase.InitializeDatabase();
        EnemyDatabase.InitializeDatabase();
        MapGenerator.LoadRooms();
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
            CollisionManager.RemoveObjectFromAreas(mEntities[i].Body);
        }

        //Flag each object for removal before we switch to the new map
        foreach(Entity entity in mEntities)
        {
            if(!(entity is Player))
            entity.mToRemove = true;
            

        }

        MapType mapType;

        if (mMap.mCurrentMap != null)
        {
            mapType = mMap.mCurrentMap.mapType;
            if (mapType == MapType.Hub)
            {
                mapType = MapType.World;
            }
            else if (mapType == MapType.World)
            {
                mapType = MapType.BossMap;
            }
            else if (mapType == MapType.BossMap)
            {
                mapType = MapType.World;
            }
        } else
        {
            mapType = MapType.Hub;
        }
        


        mMap.NewMap(mapType);
        foreach(Player player in players)
        player.Body.SetTilePosition(mMap.mCurrentMap.startTile);


    }

    public void NewEditorMap()
    {
        mMap.Init();
    }

    void HandleInputs()
    {
        for(int p = 0; p < players.Count; p++)
        {
            switch (p)
            {
                case 0:
            playerInputs[p][(int)KeyInput.GoRight] = Input.GetAxisRaw("Horizontal") > 0;
            playerInputs[p][(int)KeyInput.GoLeft] = Input.GetAxisRaw("Horizontal") < 0;
            playerInputs[p][(int)KeyInput.GoDown] = Input.GetAxisRaw("Vertical") < 0;
            playerInputs[p][(int)KeyInput.Climb] = Input.GetAxisRaw("Vertical") > 0;
            playerInputs[p][(int)KeyInput.Jump] = Input.GetButton("Jump");
            playerInputs[p][(int)KeyInput.Shoot] = Input.GetButton("Fire1");
            playerInputs[p][(int)KeyInput.Attack] = Input.GetButton("Fire2");
            playerInputs[p][(int)KeyInput.Item] = Input.GetKey(KeyCode.V);

                    break;
                case 1:
            playerInputs[p][(int)KeyInput.GoRight] = Input.GetKey(KeyCode.D);
            playerInputs[p][(int)KeyInput.GoLeft] = Input.GetKey(KeyCode.A);
            playerInputs[p][(int)KeyInput.GoDown] = Input.GetKey(KeyCode.S);
            playerInputs[p][(int)KeyInput.Climb] = Input.GetKey(KeyCode.W);
            playerInputs[p][(int)KeyInput.Jump] = Input.GetKey(KeyCode.F);
            playerInputs[p][(int)KeyInput.Shoot] = Input.GetKey(KeyCode.Q);
            playerInputs[p][(int)KeyInput.Attack] = Input.GetKey(KeyCode.R);
            playerInputs[p][(int)KeyInput.Item] = Input.GetKey(KeyCode.V);


                    break;
            }
            
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
    public virtual void FlagObjectForRemoval(Entity obj)
    {
        obj.Die();
        
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

    public void ClearMap()
    {

    }

    void FixedUpdate()
    {

        //The game doesnt run in editor mode
        if (mGameMode == GameMode.Editor)
            return;



        //Right now, this update loop is nice and generic, I hope to keep it that way

        //Update all the entities
        for (int i = 0; i < mEntities.Count; ++i)
        {
            mEntities[i].EntityUpdate();          
        }

        //Check for entities flagged for removal
        for (int i = mEntities.Count - 1; i >= 0; i--)
        {
            if (mEntities[i].mToRemove)
            {
                mEntities[i].ActuallyDie();
                
            }
        }

        //Update the collisions of all objects
        CollisionManager.CheckCollisions();

        //Second handles everything that needs to be done AFTER physics and collisions have been checked
        for (int i = 0; i < mEntities.Count; ++i)
        {
            //if(!mObjects[i].mToRemove)
            mEntities[i].SecondUpdate();
        }

        /*If we want to change the map, we have to either abort everything or wait until we're finished updating
        * to perform the change. This method waits until everything is updated.
        */ 
        if (mMapChangeFlag)
        {
            NewGameMap();
            mMapChangeFlag = false;
        }
        
    }
}
