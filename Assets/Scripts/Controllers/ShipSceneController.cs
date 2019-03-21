using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSceneController : LevelManager
{
    public List<CharacterCreationPanel> panels;
    public bool[] joinedPlayers;

    void Start()
    {
        Application.targetFrameRate = 60;
        //panels = new List<CharacterCreationPanel>();
        joinedPlayers = new bool[4];






        //player2.GetComponent<SpriteRenderer>().color = Color.gray;

        mMap.CreationMap();

    }

    private void Update()
    {
        if (Input.GetButtonDown("Player1_Button0"))
        {
            if (!joinedPlayers[0])
            {
                PlayerJoinGame(0);

            }
            else if(players[0] == null)
            {
                AddPlayer(0);
                panels[0].Close();
            }

        }

        if (Input.GetButtonDown("Player1_Button1"))
        {
            RemovePlayer(0);
        }
    }

    public void PlayerJoinGame(int index)
    {
        panels[index].Open();
        joinedPlayers[index] = true;
    }

    public void PlayerLeaveGame(int index)
    {
        panels[index].Close();
        joinedPlayers[index] = false;
    }

    public void AddPlayer(int index)
    {
        if(players[index] != null)
        {
            RemovePlayer(index);
        }

        Player newPlayer = Instantiate(mPlayerPrefab) as Player;
        //newPlayer.mHealthBar = PlayerUIPanels.instance.playerPanels[0].healthBar;
        //newPlayer.InventoryUI = PlayerUIPanels.instance.playerPanels[0].inventoryUI;
        newPlayer.EntityInit();
        players[index] = newPlayer;
        newPlayer.Body.SetTilePosition(new Vector2i(1, 1));

    }

    public void RemovePlayer(int index)
    {
        if(players[index] == null)
        {
            return;
        }

        players[index].Destroy();
        players[index] = null;
        joinedPlayers[index] = false;
    }

    void FixedUpdate()
    {

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


    }

}
