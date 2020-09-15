using UnityEngine;
using System.Collections.Generic;

public enum GameMode
{
    Start,
    Game,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public GameMode mGameMode = GameMode.Start;
    public static GameManager instance;
    public Camera gameCamera;
    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    [SerializeField]
    protected List<Entity> mEntities = new List<Entity>();
    public SpriteRenderer mMouseSprite;
    public bool mMapChangeFlag = false;

    public bool warpToHubFlag = false;

    private void Awake()
    {
        instance = this;
        CollisionManager.InitializeCollisionManager();
        ItemDatabase.InitializeDatabase();
        AbilityDatabase.InitializeDatabase();
        EnemyDatabase.LoadDatabase();
        MapDatabase.InitializeDatabase();
        RoomDatabase.InitializeDatabase();

    }

    void Start ()
    {
        Application.targetFrameRate = 60;

        /*
        for(int i = 0; i < numPlayers; i++)
        {
            Player newPlayer = Instantiate(mPlayerPrefab) as Player;
            newPlayer.mHealthBar = PlayerUIPanels.instance.playerPanels[0].healthBar;
            newPlayer.InventoryUI = PlayerUIPanels.instance.playerPanels[0].inventoryUI;
            newPlayer.EntityInit();
            newPlayer.playerIndex = i;
            //newPlayer.SetInputs(newPlayer.GetComponent<InputManager>().playerInputs, newPlayer.GetComponent<InputManager>().playerPrevInputs);
            players[i] = newPlayer;
            
        }
        */




        //player2.GetComponent<SpriteRenderer>().color = Color.gray;

        StartNewGame();        
    }

    public void StartNewGame()
    {
        CrewManager.instance.ResetCrew();

        WorldManager.instance.NewUniverse();
        BossUIManager.instance.ClearBoss();

        LoadMap(WorldManager.instance.GetHubWorld().GetFirstMap());
        mGameMode = GameMode.Game;
    }

    public List<Chest> GetChests()
    {
        List<Chest> chests = new List<Chest>();

        foreach(Entity entity in mEntities)
        {
            if(entity is Chest chest)
            {
                chests.Add(chest);
            }
        }

        return chests;
    }

    public void TravelToWorld(World destination)
    {

    }

    public void LoadMap(Map map)
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
            if(!(entity is Player) && !(entity is Companion))
            entity.mToRemove = true;

        }


        MapManager.instance.LoadMap(map);

        
        foreach (Player player in CrewManager.instance.players)
        {
            if(player != null)
            {
                player.Body.SetTilePosition(MapManager.instance.mCurrentMap.startTile);
                if (player.IsDead)
                    player.Ressurect();
            }
        }

        MiniMap.instance.SetMap(MapManager.instance.mCurrentMap, CrewManager.instance.players);

        SoundManager.instance.PlayLevelMusic((int)MapManager.instance.mCurrentMap.worldType);

        foreach(Player player in CrewManager.instance.players)
        {
            if (player == null)
                continue;
            foreach (Ability effect in player.abilities)
            {
                effect.OnMapChanged();
            }
        }


    }

    public void PauseGame(int index)
    {
        Debug.Log("Player index " + index + " pressed pause.");
        PauseMenu.instance.Open(index);
        mGameMode = GameMode.Paused;
    }

    public void UnpauseGame()
    {

        PauseMenu.instance.Close();
        mGameMode = GameMode.Game;

    }

    void Update()
    {
        foreach(Player player in CrewManager.instance.players)
        {
            if(player != null)
            {
                player.Input.Update();
            }
        }
        //UpdateCursor();

    }

    public void GameOver()
    {
        mGameMode = GameMode.GameOver;
        GameOverScreen.instance.DisplayScreen();
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

    public void GlobalPlayerInputs()
    {
        foreach (Player p in CrewManager.instance.players)
        {
            if (p != null)
            {

                if (p.Input.playerButtonInput[(int)ButtonInput.ZoomIn])
                {
                    GameCamera.instance.mMinOrthographicSize -= 5;
                }

                if (p.Input.playerButtonInput[(int)ButtonInput.ZoomOut])
                {
                    GameCamera.instance.mMinOrthographicSize += 5;

                }
            }
        }

    }

    void FixedUpdate()
    {
        GlobalPlayerInputs();

        if (mGameMode == GameMode.GameOver)
        {
            foreach (Player p in CrewManager.instance.players)
            {
                if (p != null)
                {
                    if (p.Input.playerButtonInput[(int)ButtonInput.Pause])
                    {
                        StartNewGame();
                        GameOverScreen.instance.DisplayScreen(false);
                    }

                }
            }
            return;
        }
        //The game doesnt run in editor mode
        if (mGameMode == GameMode.Paused)
        {

            foreach (Player p in CrewManager.instance.players)
            {
                if (p != null)
                {
                    if (p.Input.playerButtonInput[(int)ButtonInput.Pause] || p.Input.playerButtonInput[(int)ButtonInput.Menu_Back])
                    {
                        UnpauseGame();
                    }

                }
            }
            return;

        }

        /*
        if (players != null && players[0] != null && players[0].Input.playerButtonInput[(int)ButtonInput.Pause])
        {
            StartNewGame();
            GameOverScreen.instance.DisplayScreen(false);
        }
        */

        //Right now, this update loop is nice and generic, I hope to keep it that way

        //Update all the entities
        for (int i = 0; i < mEntities.Count; ++i)
        {
            if(!mMapChangeFlag)
                mEntities[i].EntityUpdate();          
        }

        //Check for entities flagged for removal
        for (int i = mEntities.Count - 1; i >= 0; i--)
        {
            if (mEntities[i].mToRemove)
            {
                //Debug.Log(mEntities[i].Renderer.name + " Is being removed");
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

        
        //Check for game over
        bool allPlayersDead = true;
        bool playersExist = false;
        foreach (Player player in CrewManager.instance.players)
        {
            if (player == null)
                continue;

            playersExist = true;

            if (!player.IsDead)
                allPlayersDead = false;
        }

        //Debug.Log("APD: " + allPlayersDead + ", PE: " + playersExist);
        if(allPlayersDead && playersExist)
        {
            GameOver();
            return;
        }

        /*If we want to change the map, we have to either abort everything or wait until we're finished updating
        * to perform the change. This method waits until everything is updated.
        */
        if (mMapChangeFlag)
        {
            LoadMap(WorldManager.instance.GetCurrentWorld().GetNextMap());
            mMapChangeFlag = false;
        }

        /*
        if(warpToHubFlag)
        {
            LoadMap(MapType.Hub);

            warpToHubFlag = false;
        }
        */

        
    }
}
