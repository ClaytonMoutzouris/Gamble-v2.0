using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
                playerButtonInput[(int)ButtonInput.Interact] = mGamepadInput.buttonInputs[(int)GamepadButtons.WestButton];
                playerButtonInput[(int)ButtonInput.DPad_Up] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadUp];

                playerButtonInput[(int)ButtonInput.Jump] = mGamepadInput.buttonInputs[(int)GamepadButtons.SouthButton];
                playerButtonInput[(int)ButtonInput.MeleeAttack] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.QuickHeal] = mGamepadInput.buttonInputs[(int)GamepadButtons.NorthButton];
                playerButtonInput[(int)ButtonInput.PlayerMenu] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                playerButtonInput[(int)ButtonInput.Pause] = mGamepadInput.buttonInputs[(int)GamepadButtons.StartButton];
                playerButtonInput[(int)ButtonInput.Minimap] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadUp];
                playerButtonInput[(int)ButtonInput.Gadget1] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.Fire] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightTrigger];
                playerButtonInput[(int)ButtonInput.BeamUp] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadDown];
                playerButtonInput[(int)ButtonInput.Gadget2] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = false;
                playerButtonInput[(int)ButtonInput.Menu_Back] = false;
                playerButtonInput[(int)ButtonInput.FireMode] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightStickPress];
                playerButtonInput[(int)ButtonInput.Roll] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftTrigger];
                playerButtonInput[(int)ButtonInput.CycleQuickUseLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadLeft];
                playerButtonInput[(int)ButtonInput.CycleQuickUseRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.DpadRight];

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
                playerButtonInput[(int)ButtonInput.Interact] = false;
                playerButtonInput[(int)ButtonInput.DPad_Up] = false;

                playerButtonInput[(int)ButtonInput.Jump] = false;
                playerButtonInput[(int)ButtonInput.MeleeAttack] = false;
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.PlayerMenu] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                playerButtonInput[(int)ButtonInput.Pause] = false;
                playerButtonInput[(int)ButtonInput.Minimap] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.BeamUp] = false;

                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.InventoryMove] = mGamepadInput.buttonInputs[(int)GamepadButtons.WestButton];
                playerButtonInput[(int)ButtonInput.InventorySort] = mGamepadInput.buttonInputs[(int)GamepadButtons.SelectButton];
                playerButtonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                playerButtonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.Menu_Back] = false;
                playerButtonInput[(int)ButtonInput.FireMode] = false;
                playerButtonInput[(int)ButtonInput.Roll] = false;

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
                playerButtonInput[(int)ButtonInput.Interact] = false;
                playerButtonInput[(int)ButtonInput.DPad_Up] = false;
                playerButtonInput[(int)ButtonInput.BeamUp] = false;

                playerButtonInput[(int)ButtonInput.Jump] = false;
                playerButtonInput[(int)ButtonInput.MeleeAttack] = false;
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.PlayerMenu] = false;
                playerButtonInput[(int)ButtonInput.Pause] = false;
                playerButtonInput[(int)ButtonInput.Minimap] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                playerButtonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.FireMode] = false;
                playerButtonInput[(int)ButtonInput.Roll] = false;

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
                playerButtonInput[(int)ButtonInput.Interact] = false;
                playerButtonInput[(int)ButtonInput.DPad_Up] = false;
                playerButtonInput[(int)ButtonInput.BeamUp] = false;

                playerButtonInput[(int)ButtonInput.Jump] = false;
                playerButtonInput[(int)ButtonInput.MeleeAttack] = false;
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.PlayerMenu] = false;
                playerButtonInput[(int)ButtonInput.Pause] = false;
                playerButtonInput[(int)ButtonInput.Minimap] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                playerButtonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = mGamepadInput.buttonInputs[(int)GamepadButtons.LeftBumper];
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = mGamepadInput.buttonInputs[(int)GamepadButtons.RightBumper];
                playerButtonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.FireMode] = false;
                playerButtonInput[(int)ButtonInput.Roll] = false;

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
                playerButtonInput[(int)ButtonInput.Interact] = false;
                playerButtonInput[(int)ButtonInput.DPad_Up] = false;
                playerButtonInput[(int)ButtonInput.BeamUp] = false;

                playerButtonInput[(int)ButtonInput.Jump] = false;
                playerButtonInput[(int)ButtonInput.MeleeAttack] = false;
                playerButtonInput[(int)ButtonInput.QuickHeal] = false;
                playerButtonInput[(int)ButtonInput.PlayerMenu] = false;
                playerButtonInput[(int)ButtonInput.Pause] = mGamepadInput.buttonInputs[(int)GamepadButtons.StartButton];
                playerButtonInput[(int)ButtonInput.Minimap] = false;
                playerButtonInput[(int)ButtonInput.SkipLevel] = false;
                playerButtonInput[(int)ButtonInput.Gadget1] = false;
                playerButtonInput[(int)ButtonInput.Fire] = false;
                playerButtonInput[(int)ButtonInput.Gadget2] = false;
                playerButtonInput[(int)ButtonInput.InventoryDrop] = false;
                playerButtonInput[(int)ButtonInput.InventoryMove] = false;
                playerButtonInput[(int)ButtonInput.InventorySort] = false;
                playerButtonInput[(int)ButtonInput.CycleQuickUseLeft] = false;
                playerButtonInput[(int)ButtonInput.CycleQuickUseRight] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabLeft] = false;
                playerButtonInput[(int)ButtonInput.ChangeTabRight] = false;
                playerButtonInput[(int)ButtonInput.Menu_Back] = mGamepadInput.buttonInputs[(int)GamepadButtons.EastButton];
                playerButtonInput[(int)ButtonInput.FireMode] = false;
                playerButtonInput[(int)ButtonInput.Roll] = false;

                break;
        }
        
    }
}
