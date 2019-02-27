using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameMode
{
    Game,
    Paused
}
public class LevelManager : MonoBehaviour
{
    public GameMode mGameMode;
    public static LevelManager instance;
    public Camera gameCamera;
    public int numPlayers = 1;
    public List<Player> players;

    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    public MapManager mMap;

    public TileType mPlacedTileType;

    public Player mPlayerPrefab;
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
        MapDatabase.InitializeDatabase();
        RoomDatabase.InitializeDatabase();

    }

    void Start ()
    {
        Application.targetFrameRate = 60;

        for(int i = 0; i < numPlayers; i++)
        {
            Player newPlayer = Instantiate(mPlayerPrefab) as Player;
            newPlayer.mHealthBar = PlayerUIPanels.instance.playerPanels[0].healthBar;
            newPlayer.EntityInit();
            newPlayer.SetInputs(InputManager.instance.playerInputs[i], InputManager.instance.playerPrevInputs[i]);
            GameCamera.instance.targets.Add(newPlayer.transform);
            players.Add(newPlayer);
            
        }

        



        //player2.GetComponent<SpriteRenderer>().color = Color.gray;

        NewGameMap(MapType.Hub);
        
    }

    public void NewGameMap(MapType type)
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

        mMap.NewMap(MapDatabase.GetMap(type));
        int count = 0;
        foreach (Player player in players)
        {
            player.Body.SetTilePosition(mMap.mCurrentMap.startTile);
        }


    }

    public void PauseGame()
    {
        PauseMenu.instance.Open();
        mGameMode = GameMode.Paused;
    }

    public void UnpauseGame()
    {
        PauseMenu.instance.Close();
        mGameMode = GameMode.Game;

    }

    void Update()
    {

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
        if (mGameMode == GameMode.Paused)
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
            NewGameMap(MapType.World);
            mMapChangeFlag = false;
        }
        
    }
}
