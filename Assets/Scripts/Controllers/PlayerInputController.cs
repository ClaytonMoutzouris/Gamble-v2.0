using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;

public enum PlayerInputState { Game, Inventory, Paused, NavigationMenu, GameOver };


public class PlayerInputController
{
    Player player;
    public PlayerGamepadInput mGamepadInput;
    //Eventually, the playres will have this component. It will handle transitions between different input states and allow swapping between keyboard and gamepad and rebinding input
    public PlayerInputState inputState = PlayerInputState.Game;
    public float[] playerAxisInput = new float[(int)AxisInput.Count];
    public float[] previousAxisInput = new float[(int)AxisInput.Count];

    public bool[] playerButtonInput = new bool[(int)ButtonInput.Count];
    public bool[] previousButtonInput = new bool[(int)ButtonInput.Count];

    public PlayerInputController(Player p, PlayerGamepadInput gamepad)
    {
        player = p;
        mGamepadInput = gamepad;
    }

    void UpdatePreviousInputs()
    {
        var axisCount = (byte)AxisInput.Count;

        for (byte i = 0; i < axisCount; ++i)
        {
            previousAxisInput[i] = playerAxisInput[i];
        }

        var buttonCount = (byte)ButtonInput.Count;

        for (byte i = 0; i < buttonCount; ++i)
        {
            previousButtonInput[i] = playerButtonInput[i];
        }
    }
    // Update is called once per frame
    public void Update()
    {
        UpdatePreviousInputs();
        //Debug.Log("Player input " + player.mPlayerIndex + " in " + inputState.ToString());
        switch (inputState)
        {
            case PlayerInputState.Game:
                //Update the axis inputs
                playerAxisInput[(int)AxisInput.LeftStickX] = mGamepadInput.xAxisLeft;
                playerAxisInput[(int)AxisInput.LeftStickY] = mGamepadInput.yAxisLeft;
                playerAxisInput[(int)AxisInput.RightStickX] = mGamepadInput.xAxisRight;
                playerAxisInput[(int)AxisInput.RightStickY] = mGamepadInput.yAxisRight;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                playerButtonInput[(int)ButtonInput.LeftStick_Left] = mGamepadInput.LeftStickTapLeft();
                playerButtonInput[(int)ButtonInput.LeftStick_Right] = mGamepadInput.LeftStickTapRight();
                playerButtonInput[(int)ButtonInput.LeftStick_Down] = mGamepadInput.LeftStickTapDown();
                playerButtonInput[(int)ButtonInput.LeftStick_Up] = mGamepadInput.LeftStickTapUp();
                playerButtonInput[(int)ButtonInput.DPad_Left] = mGamepadInput.D_Pad_Left_Pressed();
                playerButtonInput[(int)ButtonInput.DPad_Right] = mGamepadInput.D_Pad_Right_Pressed();
                playerButtonInput[(int)ButtonInput.DPad_Down] = mGamepadInput.D_Pad_Down_Pressed();
                playerButtonInput[(int)ButtonInput.DPad_Up] = mGamepadInput.D_Pad_Up_Pressed();

                playerButtonInput[(int)ButtonInput.Jump] = mGamepadInput.ButtonAPressed();
                playerButtonInput[(int)ButtonInput.Attack] = mGamepadInput.ButtonBPressed();
                playerButtonInput[(int)ButtonInput.Item] = mGamepadInput.ButtonXPressed();
                playerButtonInput[(int)ButtonInput.Inventory] = mGamepadInput.ButtonYPressed();
                playerButtonInput[(int)ButtonInput.Swap] = mGamepadInput.RightBumperPressed();
                playerButtonInput[(int)ButtonInput.Pause] = mGamepadInput.StartDown();
                playerButtonInput[(int)ButtonInput.Select] = mGamepadInput.SelectDown();
                playerButtonInput[(int)ButtonInput.SkipLevel] = mGamepadInput.RightBumperDown() && mGamepadInput.LeftBumperDown();
                playerButtonInput[(int)ButtonInput.Teleport] = mGamepadInput.LeftBumperDown();
                playerButtonInput[(int)ButtonInput.BeamUp] = mGamepadInput.rightTrigger > 0 && mGamepadInput.leftTrigger > 0;
                playerButtonInput[(int)ButtonInput.Fire] = mGamepadInput.rightTrigger > 0;
                playerButtonInput[(int)ButtonInput.Block] = mGamepadInput.RightBumperPressed();
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;


                break;

            case PlayerInputState.Inventory:
                //Update the axis inputs
                playerAxisInput[(int)AxisInput.LeftStickX] = 0;
                playerAxisInput[(int)AxisInput.LeftStickY] = 0;
                playerAxisInput[(int)AxisInput.RightStickX] = 0;
                playerAxisInput[(int)AxisInput.RightStickY] = 0;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                playerButtonInput[(int)ButtonInput.LeftStick_Left] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Right] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Down] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Up] = false;
                playerButtonInput[(int)ButtonInput.DPad_Left] = false;
                playerButtonInput[(int)ButtonInput.DPad_Right] = false;
                playerButtonInput[(int)ButtonInput.DPad_Down] = false;
                playerButtonInput[(int)ButtonInput.DPad_Up] = false;

                playerButtonInput[(int)ButtonInput.Jump] = false;
                playerButtonInput[(int)ButtonInput.Attack] = false;
                playerButtonInput[(int)ButtonInput.Item] = false;
                playerButtonInput[(int)ButtonInput.Inventory] = mGamepadInput.ButtonYPressed();
                playerButtonInput[(int)ButtonInput.Swap] = false;
                playerButtonInput[(int)ButtonInput.Pause] = mGamepadInput.StartDown();
                playerButtonInput[(int)ButtonInput.Select] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Teleport] = mGamepadInput.LeftBumperDown();
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Block] = mGamepadInput.RightBumperPressed();
                playerButtonInput[(int)ButtonInput.InventoryDrop] = mGamepadInput.ButtonBDown();
                playerButtonInput[(int)ButtonInput.InventoryMove] = mGamepadInput.ButtonXDown();
                playerButtonInput[(int)ButtonInput.InventorySort] = mGamepadInput.SelectDown();
                break;

            case PlayerInputState.NavigationMenu:
                //Update the axis inputs
                playerAxisInput[(int)AxisInput.LeftStickX] = 0;
                playerAxisInput[(int)AxisInput.LeftStickY] = 0;
                playerAxisInput[(int)AxisInput.RightStickX] = 0;
                playerAxisInput[(int)AxisInput.RightStickY] = 0;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                playerButtonInput[(int)ButtonInput.LeftStick_Left] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Right] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Down] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Up] = false;
                playerButtonInput[(int)ButtonInput.DPad_Left] = false;
                playerButtonInput[(int)ButtonInput.DPad_Right] = false;
                playerButtonInput[(int)ButtonInput.DPad_Down] = false;
                playerButtonInput[(int)ButtonInput.DPad_Up] = false;

                playerButtonInput[(int)ButtonInput.Jump] = false;
                playerButtonInput[(int)ButtonInput.Attack] = mGamepadInput.ButtonBPressed();
                playerButtonInput[(int)ButtonInput.Item] = false;
                playerButtonInput[(int)ButtonInput.Inventory] = false;
                playerButtonInput[(int)ButtonInput.Swap] = false;
                playerButtonInput[(int)ButtonInput.Pause] = false;
                playerButtonInput[(int)ButtonInput.Select] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Teleport] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Block] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                break;

            case PlayerInputState.Paused:
               //Update the axis inputs
                playerAxisInput[(int)AxisInput.LeftStickX] = 0;
                playerAxisInput[(int)AxisInput.LeftStickY] = 0;
                playerAxisInput[(int)AxisInput.RightStickX] = 0;
                playerAxisInput[(int)AxisInput.RightStickY] = 0;

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                playerButtonInput[(int)ButtonInput.LeftStick_Left] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Right] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Down] = false;
                playerButtonInput[(int)ButtonInput.LeftStick_Up] = false;
                playerButtonInput[(int)ButtonInput.DPad_Left] = false;
                playerButtonInput[(int)ButtonInput.DPad_Right] = false;
                playerButtonInput[(int)ButtonInput.DPad_Down] = false;
                playerButtonInput[(int)ButtonInput.DPad_Up] = false;

                playerButtonInput[(int)ButtonInput.Jump] = false;
                playerButtonInput[(int)ButtonInput.Attack] = false;
                playerButtonInput[(int)ButtonInput.Item] = false;
                playerButtonInput[(int)ButtonInput.Inventory] = false;
                playerButtonInput[(int)ButtonInput.Swap] = false;
                playerButtonInput[(int)ButtonInput.Pause] = mGamepadInput.StartDown();
                playerButtonInput[(int)ButtonInput.Select] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Teleport] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Block] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                break;
        }
        
    }
}
