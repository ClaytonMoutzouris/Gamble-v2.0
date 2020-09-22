using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;

public class CrewManager : MonoBehaviour
{
    public static CrewManager instance;
    List<Player> crewMembers;
    public Player[] players = new Player[4];
    public int weird;
    //public GameObject expBar;

    public void Start()
    {
        instance = this;
        players = new Player[4];

    }

    public void ResetCrew()
    {
        foreach (Player player in players)
        {
            if (player != null)
            {
                DropPlayer(player.mPlayerIndex);
            }
        }
    }

    public void AddPlayer(int index, PlayerGamepadInput input)
    {
        PlayerUIPanels.instance.AddPlayer(index);

        PlayerClassType classType = (PlayerClassType)Random.Range(0, (int)PlayerClassType.Medic + 1);

        Player newPlayer = new Player(Instantiate(Resources.Load("Prototypes/Entity/Player/PlayerPrototype") as PlayerPrototype), Resources.Load<PlayerClass>("Prototypes/Player/Classes/" + classType.ToString()), index);


        //newPlayer.SetInputs(newPlayer.GetComponent<InputManager>().playerInputs, newPlayer.GetComponent<InputManager>().playerPrevInputs);
    }

    public void AddPlayer(int index, Player newPlayer)
    {
        players[index] = newPlayer;
        newPlayer.Spawn(MapManager.instance.GetMapTilePosition(5, 5));
        newPlayer.playerClass.LoadClass(newPlayer, false);
        newPlayer.talentTree.skillPoints = newPlayer.playerLevel;
    }

    public void AddPlayer(int index, Player loadPlayer, PlayerSaveData saveData)
    {

        players[index] = loadPlayer;
        loadPlayer.Spawn(MapManager.instance.GetMapTilePosition(5, 5));
        loadPlayer.playerClass.LoadClass(loadPlayer, true);
        saveData.SetPlayersData(loadPlayer);
    }

    public void DropPlayer(int index)
    {
        //players[index].mToRemove = true;
        PlayerUIPanels.instance.RemovePlayer(index);
        players[index].ActuallyDie();
        players[index] = null;


    }

    public void LevelUp(Player player)
    {
        player.playerLevel++;
        player.talentTree.skillPoints++;

    }

    public void GainEXP(Player player, int xp)
    {
        int levelThreshold = player.playerLevel * player.playerLevel * 10 + 50;

        player.playerExperience += xp;

        if (player.playerExperience >= levelThreshold)
        {
            player.playerExperience = player.playerExperience - levelThreshold;
            LevelUp(player);
        }
    }

    public void GainPartyEXP(int xp)
    {
        int count = 0;
        foreach(Player player in players)
        {
            count++;
            if (player != null)
            {
                Debug.Log(player.entityName + " gains " + xp + " experience.");
                GainEXP(player, xp);
            }
        }
        //expBar.transform.localScale = new Vector3((float)partyEXP / (float)(partyLevel * partyLevel * 10 + 50), 1, 1);
    }

}
