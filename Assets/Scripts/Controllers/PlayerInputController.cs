using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;

public enum PlayerInputState { Game, Inventory, Paused, NavigationMenu, GameOver, Shop };


public class PlayerInputController
{
    Player player;
    public NewGamepadInput mGamepadInput;
    //Eventually, the playres will have this component. It will handle transitions between different input states and allow swapping between keyboard and gamepad and rebinding input
    public PlayerInputState inputState = PlayerInputState.Game;
    public float[] playerAxisInput = new float[(int)AxisInput.Count];
    public float[] previousAxisInput = new float[(int)AxisInput.Count];

    public bool[] playerButtonInput = new bool[(int)ButtonInput.Count];
    public bool[] previousButtonInput = new bool[(int)ButtonInput.Count];

    public PlayerInputController(Player p, NewGamepadInput gamepad)
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
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Delete all these states and just make more buttons...
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void Update()
    {
        UpdatePreviousInputs();
        //Debug.Log("Player input " + player.mPlayerIndex + " in " + inputState.ToString());
        switch (inputState)
        {
            case PlayerInputState.Game:
                //Update the axis inputs
                playerAxisInput[(int)AxisInput.LeftStickX] = mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickX];
                playerAxisInput[(int)AxisInput.LeftStickY] = mGamepadInput.axisInputs[(int)GamepadAxis.LeftStickY];
                playerAxisInput[(int)AxisInput.RightStickX] = mGamepadInput.axisInputs[(int)GamepadAxis.RightStickX];
                playerAxisInput[(int)AxisInput.RightStickY] = mGamepadInput.axisInputs[(int)GamepadAxis.RightStickY];

                //Update the button inputs
                //These are for reading in "taps" of the stick buttons
                playerButtonInput[(int)ButtonInput.DPad_Left] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadLeft];
                playerButtonInput[(int)ButtonInput.DPad_Right] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadRight];
                playerButtonInput[(int)ButtonInput.DPad_Down] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadDown];
                playerButtonInput[(int)ButtonInput.DPad_Up] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadUp];

                playerButtonInput[(int)ButtonInput.Jump] = mGamepadInput.buttonInputs[(int)GamepadButtons.SouthButton];
                playerButtonInput[(int)ButtonInput.Attack] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.QuickHeal] = mGamepadInput.buttonInputs[(int)GamepadButtons.WestButton];
                playerButtonInput[(int)ButtonInput.OpenCloseInventory] = mGamepadInput.buttonInputs[(int)GamepadButtons.NorthButton];
                playerButtonInput[(int)ButtonInput.Pause] = mGamepadInput.buttonInputs[(int)GamepadButtons.StartButton];
                playerButtonInput[(int)ButtonInput.Select] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                playerButtonInput[(int)ButtonInput.Gadget1] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.Fire] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightTrigger];
                playerButtonInput[(int)ButtonInput.Gadget2] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.ZoomIn] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadLeft];
                playerButtonInput[(int)ButtonInput.ZoomOut] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadRight];
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = false;
                playerButtonInput[(int)ButtonInput.Menu_Back] = false;
                playerButtonInput[(int)ButtonInput.FireMode] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightStickPress];



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
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.OpenCloseInventory] = mGamepadInput.buttonInputs[(int)GamepadButtons.NorthButton];
                playerButtonInput[(int)ButtonInput.Pause] = false;
                playerButtonInput[(int)ButtonInput.Select] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.InventoryMove] = mGamepadInput.buttonInputs[(int)GamepadButtons.WestButton];
                playerButtonInput[(int)ButtonInput.InventorySort] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                playerButtonInput[(int)ButtonInput.ZoomIn] = false;
                playerButtonInput[(int)ButtonInput.ZoomOut] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.Menu_Back] = false;
                playerButtonInput[(int)ButtonInput.FireMode] = false;

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
                playerButtonInput[(int)ButtonInput.Attack] = false;
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.OpenCloseInventory] = false;
                playerButtonInput[(int)ButtonInput.Pause] = false;
                playerButtonInput[(int)ButtonInput.Select] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.ZoomIn] = false;
                playerButtonInput[(int)ButtonInput.ZoomOut] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.FireMode] = false;

                break;
            case PlayerInputState.Shop:
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
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.OpenCloseInventory] = false;
                playerButtonInput[(int)ButtonInput.Pause] = false;
                playerButtonInput[(int)ButtonInput.Select] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.ZoomIn] = false;
                playerButtonInput[(int)ButtonInput.ZoomOut] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.FireMode] = false;

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
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.OpenCloseInventory] = false;
                playerButtonInput[(int)ButtonInput.Pause] = mGamepadInput.buttonInputs[(int)GamepadButtons.StartButton];
                playerButtonInput[(int)ButtonInput.Select] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.ZoomIn] = false;
                playerButtonInput[(int)ButtonInput.ZoomOut] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = false;
                playerButtonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.FireMode] = false;

                break;
        }
        
    }
}
