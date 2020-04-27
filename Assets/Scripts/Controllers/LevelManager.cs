﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LocalCoop;

public enum GameMode
{
    Start,
    Game,
    Paused,
    GameOver
}

public class LevelManager : MonoBehaviour
{
    public GameMode mGameMode = GameMode.Start;
    public static LevelManager instance;
    public Camera gameCamera;
    public Player[] players = new Player[4];
    public int levelIndex = 0;
    int lastMouseTileX = -1;
    int lastMouseTileY = -1;

    public GameObject expBar;

    public MapManager mMap;

    [SerializeField]
    protected List<Entity> mEntities = new List<Entity>();

    public int partyEXP = 0;
    public int partyLevel = 1;

    public SpriteRenderer mMouseSprite;
    public bool mMapChangeFlag = false;
    public MapType changeToType;

    public bool warpToHubFlag = false;
    public List<bool> completedWorlds;
    public int currentWorldIndex = 0;

    private void Awake()
    {
        instance = this;
        CollisionManager.InitializeCollisionManager();
        ItemDatabase.InitializeDatabase();
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

        players = new Player[4];



        //player2.GetComponent<SpriteRenderer>().color = Color.gray;

        StartNewGame();        
    }

    public void StartNewGame()
    {
        foreach (Player player in players)
        {
            if (player != null)
            {
                DropPlayer(player.mPlayerIndex);
            }
        }
        partyLevel = 1;
        partyEXP = 0;
        completedWorlds = new List<bool>();

        levelIndex = 0;
        NewGameMap(MapType.Hub);
        if(mGameMode != GameMode.Start)
        {
            NavigationMenu.instance.ClearMaps();
        }


        mGameMode = GameMode.Game;
    }

    public void AddPlayer(int index, PlayerGamepadInput input)
    {
        PlayerUIPanels.instance.AddPlayer(index);

        PlayerClassType classType = (PlayerClassType)Random.Range(0, (int)PlayerClassType.Medic + 1);

        Player newPlayer = new Player(Instantiate(Resources.Load("Prototypes/Entity/Player/PlayerPrototype") as PlayerPrototype), Resources.Load<PlayerClass>("Prototypes/Player/Classes/"+classType.ToString()), index);


        //newPlayer.SetInputs(newPlayer.GetComponent<InputManager>().playerInputs, newPlayer.GetComponent<InputManager>().playerPrevInputs);
    }

    public void AddPlayer(int index, Player newPlayer)
    {
        players[index] = newPlayer;
        newPlayer.Spawn(MapManager.instance.GetMapTilePosition(5, 5));
        newPlayer.playerClass.LoadClass(newPlayer);
        newPlayer.talentTree.skillPoints = partyLevel;
    }

    public void DropPlayer(int index)
    {
        //players[index].mToRemove = true;
        PlayerUIPanels.instance.RemovePlayer(index);
        players[index].ActuallyDie();
        players[index] = null;


    }

    public void PartyLevelUp()
    {
        partyLevel++;

        foreach (Player player in players)
        {
            if(player != null)
            {
                player.talentTree.skillPoints++;
            }
        }
    }

    public void GainPartyEXP(int xp)
    {
        int levelThreshold = partyLevel * partyLevel * 10 + 50;
        partyEXP += xp;

        if (partyEXP >= levelThreshold)
        {
            partyEXP = partyEXP - levelThreshold;
            PartyLevelUp();
        }

        expBar.transform.localScale = new Vector3((float)partyEXP/(float)(partyLevel * partyLevel * 10 + 50), 1, 1);
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
            if(!(entity is Player) && !(entity is Drone))
            entity.mToRemove = true;

        }

        if(type == MapType.Hub)
        {
            mMap.NewMap(MapDatabase.GetHubMap());
        }
        else if(type == MapType.BossMap)
        {
            
            mMap.NewBossMap(MapDatabase.GetBossMap((WorldType)levelIndex), (WorldType)levelIndex);
        }
        else if(type == MapType.World)
        {
            mMap.NewMap(MapDatabase.GetMap((WorldType)levelIndex));
        }
        
        foreach (Player player in players)
        {
            if(player != null)
            {
                player.Body.SetTilePosition(mMap.mCurrentMap.startTile);
                if (player.IsDead)
                    player.Ressurect();
            }
        }

        MiniMap.instance.SetMap(mMap.mCurrentMap, players);

        SoundManager.instance.PlayLevelMusic((int)mMap.mCurrentMap.worldType);

        foreach(Player player in players)
        {
            if (player == null)
                continue;
            foreach (Effect effect in player.itemEffects)
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
        foreach(Player player in players)
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

    public int NumCompletedWorlds()
    {
        int completed = 0;

        for (int i = 0; i < completedWorlds.Count; i++)
        {
            if (completedWorlds[i])
            {
                completed++;
            }
        }
        return completed;

    }

    public void WorldCleared(int index)
    {
        completedWorlds[index] = true;
        NavigationMenu.instance.worldNodes[index].SetCleared();
    }

    void FixedUpdate()
    {

        if(mGameMode == GameMode.GameOver)
        {
            foreach (Player p in players)
            {
                if (p != null)
                {
                    if (p.Input.playerButtonInput[(int)ButtonInput.Pause])
                    {
                        StartNewGame();
                        GameOverScreen.instance.DisplayScreen(false);
                    }

                    if (p.Input.playerButtonInput[(int)ButtonInput.ZoomIn])
                    {
                        GameCamera.instance.mMinOrthographicSize -= 10;
                    }

                    if (p.Input.playerButtonInput[(int)ButtonInput.ZoomOut])
                    {
                        GameCamera.instance.mMinOrthographicSize += 10;

                    }
                }
            }
            return;
        }
        //The game doesnt run in editor mode
        if (mGameMode == GameMode.Paused)
            return;

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
        foreach (Player player in players)
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
            if (mMap.mCurrentMap.mapType == MapType.Hub)
            {
                NewGameMap(MapType.World);
                mMapChangeFlag = false;
            }
            else if (mMap.mCurrentMap.mapType == MapType.World){
                NewGameMap(MapType.BossMap);
                mMapChangeFlag = false;
            }
            else if(mMap.mCurrentMap.mapType == MapType.BossMap)
            {
                NewGameMap(MapType.Hub);
                mMapChangeFlag = false;
            }

        }

        if(warpToHubFlag)
        {
            NewGameMap(MapType.Hub);

            warpToHubFlag = false;
        }


        
    }
}
