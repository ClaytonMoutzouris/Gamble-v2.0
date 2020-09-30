using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum GamepadButtons { LeftStickPress, RightStickPress, NorthButton, EastButton, SouthButton, WestButton, LeftBumper, LeftTrigger, RightBumper, RightTrigger, StartButton, SelectButton, DpadLeft, DpadRight, DpadUp, DpadDown, Count };
public enum GamepadAxis { LeftStickX, LeftStickY, RightStickX, RightStickY, Count };

public class NewGamepadInput : MonoBehaviour
{
    public static int playerCount = 0;
    public int playerIndex;
    public PlayerInput input;
    public float[] axisInputs = new float[(int)GamepadAxis.Count];
    public bool[] buttonInputs = new bool[(int)GamepadButtons.Count];
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        GamepadInputManager.instance.gamepadInputs[input.playerIndex] = this;
        CreationPanelsUI.instance.creationPanels[input.playerIndex].NewCharacter(this);
        for(int buttons = 0; buttons < (int)GamepadButtons.Count; buttons++)
        {
            buttonInputs[buttons] = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {


    }

    public EventSystem GetEventSystem()
    {
        return GetComponent<EventSystem>();
    }

    private void OnLeftStickX(InputValue input)
    {
        axisInputs[(int)GamepadAxis.LeftStickX] = input.Get<float>();
    }

    private void OnLeftStickY(InputValue input)
    {
        axisInputs[(int)GamepadAxis.LeftStickY] = input.Get<float>();

    }

    private void OnRightStickX(InputValue input)
    {
        axisInputs[(int)GamepadAxis.RightStickX] = input.Get<float>();

    }

    private void OnRightStickY(InputValue input)
    {
        axisInputs[(int)GamepadAxis.RightStickY] = input.Get<float>();

    }

    private void OnLeftStickPress(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.LeftStickPress] = input.Get<float>() > 0.5;

    }

    private void OnRightStickPress(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.RightStickPress] = input.Get<float>() > 0.5;

    }

    private void OnNorthButton(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.NorthButton] = input.Get<float>() > 0.5;

    }

    private void OnEastButton(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.EastButton] = input.Get<float>() > 0.5;

    }

    private void OnSouthButton(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.SouthButton] = input.Get<float>() > 0.5;
    }

    private void OnWestButton(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.WestButton] = input.Get<float>() > 0.5;

    }

    private void OnLeftBumper(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.LeftBumper] = input.Get<float>() > 0.5;

    }

    private void OnRightBumper(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.RightBumper] = input.Get<float>() > 0.5;

    }

    private void OnLeftTrigger(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.LeftTrigger] = input.Get<float>() > 0.5;

    }

    private void OnRightTrigger(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.RightTrigger] = input.Get<float>() > 0.5;

    }

    private void OnSelectButton(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.SelectButton] = input.Get<float>() > 0.5;

    }

    private void OnStartButton(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.StartButton] = input.Get<float>() > 0.5;

    }

    private void OnUIMove()
    {
    }

    private void OnSubmit()
    {
    }

    private void OnPoint()
    {
    }

    private void OnDpadLeft(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.DpadLeft] = input.Get<float>() > 0.5;

    }

    private void OnDpadRight(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.DpadRight] = input.Get<float>() > 0.5;

    }

    private void OnDpadUp(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.DpadUp] = input.Get<float>() > 0.5;

    }

    private void OnDpadDown(InputValue input)
    {
        buttonInputs[(int)GamepadButtons.DpadDown] = input.Get<float>() > 0.5;

    }
}
