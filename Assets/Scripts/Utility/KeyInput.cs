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
    Interact,
    DPad_Up,
    Jump,
    MeleeAttack,
    QuickHeal,
    PlayerMenu,
    Pause,
    Minimap,
    SkipLevel,
    Gadget1,
    BeamUp,
    Fire,
    Gadget2,
    InventoryDrop,
    InventoryMove,
    InventorySort,
    CycleQuickUseLeft,
    CycleQuickUseRight,
    ChangeTabLeft,
    ChangeTabRight,
    Menu_Back,
    FireMode,
    Roll,
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