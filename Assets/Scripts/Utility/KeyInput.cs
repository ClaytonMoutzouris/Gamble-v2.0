using UnityEngine;
using System.Collections;

/// <summary>
/// Key input enumeration for easy input sending.
/// </summary>
public enum ButtonInput
{
	LeftStick_Left = 0,
	LeftStick_Right,
	LeftStick_Down,
    LeftStick_Up,
    DPad_Left,
    DPad_Right,
    DPad_Down,
    DPad_Up,
    Jump,
    Attack,
    Item,
    Inventory,
    Swap,
    Pause,
    Select,
    SkipLevel,
    Teleport,
    BeamUp,
    Fire,
    Shield,
	Count
}

public enum AxisInput
{
    LeftStickX = 0,
    LeftStickY,
    RightStickX,
    RightStickY,
    Count
}