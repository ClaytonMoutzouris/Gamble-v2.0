using UnityEngine;
using System.Collections;

/// <summary>
/// Key input enumeration for easy input sending.
/// </summary>
public enum KeyInput
{
	LeftStick_Left = 0,
	LeftStick_Right,
	LeftStick_Down,
    LeftStick_Up,
    RightStick_Left,
    RightStick_Right,
    RightStick_Down,
    RightStick_Up,
    Jump,
    Shoot,
    Attack,
    Item,
    Inventory,
	Count
}

public enum StickInput
{
    LeftStickX = 0,
    LeftStickY,
    RightStickX,
    RightStickY,
    Count
}