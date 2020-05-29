using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;

public class CrewManager : MonoBehaviour
{

    List<Player> crewMembers;
    public int partyEXP = 0;
    public int partyLevel = 1;
    public Player[] players = new Player[4];
    public GameObject expBar;

    public void ResetParty()
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
            if (player != null)
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

        expBar.transform.localScale = new Vector3((float)partyEXP / (float)(partyLevel * partyLevel * 10 + 50), 1, 1);
    }

}
