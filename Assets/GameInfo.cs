using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameInfo : MonoBehaviour
{
    public Text text;
    public GameInfo instance;

    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        string info = "";
        foreach(Player player in GameManager.instance.players)
        {
            if(player != null)
            {
                info += "Player " + player.mPlayerIndex + "\n";
                info += "State: " + player.mCurrentState + "\n";
                info += "Move state: " + player.movementState + "\n";
                info += "Position: " + player.Position + "\n";
                info += "Speed: " + player.Body.mSpeed + "\n";
                info += "Ignores Gravity: " + player.Body.mIgnoresGravity + "\n";
                info += "Push Top: " + player.Body.mPS.pushesTop + "\n";
                info += "Push Top Tile: " + player.Body.mPS.pushesTopTile + "\n";
                info += "Push Top Object: " + player.Body.mPS.pushesTopObject + "\n";

                info += "\n";


            }
        }
        text.text = info;
    }

    public void UpdateGameInfo()
    {

    }
}
