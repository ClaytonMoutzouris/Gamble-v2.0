using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputMode { Game, Pause, Inventory };

public class InputManager : MonoBehaviour
{
    public InputMode mode;
    public Player player;
    public bool[] playerInputs;
    public bool[] playerPrevInputs;
    public int[] playerAxis;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputs = new bool[(int)KeyInput.Count];
        playerPrevInputs = new bool[(int)KeyInput.Count];
        //playerAxis = new int[(int)StickInput.Count];

        GetComponent<Player>().SetInputs(playerInputs, playerPrevInputs);
    }

    // Update is called once per frame
    void Update()
    {


        switch (mode)
        {
            case InputMode.Game:
                playerInputs[(int)KeyInput.LeftStick_Right] = Input.GetAxisRaw("Player" + player.playerIndex + "_LeftStickX") > 0;
                playerInputs[(int)KeyInput.LeftStick_Left] = Input.GetAxisRaw("Player" + player.playerIndex + "_LeftStickX") < 0;
                playerInputs[(int)KeyInput.LeftStick_Down] = Input.GetAxisRaw("Player" + player.playerIndex + "_LeftStickY") < 0;
                playerInputs[(int)KeyInput.LeftStick_Up] = Input.GetAxisRaw("Player" + player.playerIndex + "_LeftStickY") > 0;
                playerInputs[(int)KeyInput.RightStick_Right] = Input.GetAxisRaw("Player" + player.playerIndex + "_RightStickX") > 0;
                playerInputs[(int)KeyInput.RightStick_Left] = Input.GetAxisRaw("Player" + player.playerIndex + "_RightStickX") < 0;
                playerInputs[(int)KeyInput.RightStick_Down] = Input.GetAxisRaw("Player" + player.playerIndex + "_RightStickY") < 0;
                playerInputs[(int)KeyInput.RightStick_Up] = Input.GetAxisRaw("Player" + player.playerIndex + "_RightStickY") > 0;
                playerInputs[(int)KeyInput.Jump] = Input.GetButton("Player" + player.playerIndex + "_Button0");
                playerInputs[(int)KeyInput.Shoot] = Input.GetAxisRaw("Player" + player.playerIndex + "_RightTrigger") > 0;
                playerInputs[(int)KeyInput.Attack] = Input.GetButton("Player" + player.playerIndex + "_Button1");
                playerInputs[(int)KeyInput.Item] = Input.GetButton("Player" + player.playerIndex + "_Button2");
                playerInputs[(int)KeyInput.Inventory] = Input.GetButton("Player" + player.playerIndex + "_Button3");
                playerInputs[(int)KeyInput.Pause] = Input.GetButton("Player" + player.playerIndex + "_Button7");
                playerInputs[(int)KeyInput.RangeSwap] = Input.GetButton("Player" + player.playerIndex + "_Button4");
                break;
            case InputMode.Inventory:

                break;
            case InputMode.Pause:

                break;
        }




        
    }

}
