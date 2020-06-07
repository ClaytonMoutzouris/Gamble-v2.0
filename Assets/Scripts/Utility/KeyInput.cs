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
    Gadget1,
    BeamUp,
    Fire,
    Gadget2,
    InventoryDrop,
    InventoryMove,
    InventorySort,
    ZoomIn,
    ZoomOut,
    ChangeTabLeft,
    ChangeTabRight,
    Menu_Back,
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