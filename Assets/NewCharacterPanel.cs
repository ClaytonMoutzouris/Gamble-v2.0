using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPanelStatus { Waiting, Creation, Standby };

public class NewCharacterPanel : MonoBehaviour
{
    public string playerName;
    public PlayerPanelStatus status;

    public void SetName(string name)
    {
        playerName = name;
        Debug.Log(playerName);
    }

    public void ChangeStatus(PlayerPanelStatus p)
    {
        status = p;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Handle input from controller here
        
    }
}
