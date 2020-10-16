// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""52514382-ce06-420c-b165-f2d7b8989f2c"",
            ""actions"": [
                {
                    ""name"": ""LeftStickX"",
                    ""type"": ""Value"",
                    ""id"": ""3096efea-5f25-4ad9-a029-568eb6c127db"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftStickY"",
                    ""type"": ""Value"",
                    ""id"": ""6323394f-a1df-42cd-866c-dd435abe97de"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftStickPress"",
                    ""type"": ""Button"",
                    ""id"": ""6d06ab83-aff3-4c6a-86b8-32fcb48f6cbe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightStickX"",
                    ""type"": ""Value"",
                    ""id"": ""f158928a-76f5-4aa8-b123-78661c8aac93"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightStickY"",
                    ""type"": ""Value"",
                    ""id"": ""194b9395-732b-4195-8d67-62c7dc888c0a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightStickPress"",
                    ""type"": ""Button"",
                    ""id"": ""48e984ed-2503-4623-8f6d-4818c3277008"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""NorthButton"",
                    ""type"": ""Button"",
                    ""id"": ""653e039b-eada-4026-ae78-31d575029e86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""EastButton"",
                    ""type"": ""Button"",
                    ""id"": ""24139483-3410-4c30-970c-0eedf33fca5f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SouthButton"",
                    ""type"": ""Button"",
                    ""id"": ""843468d3-6ad3-4daf-ac77-282917a5af59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""WestButton"",
                    ""type"": ""Button"",
                    ""id"": ""df647fbd-6209-4dee-b2d6-646b9d81d8a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""LeftBumper"",
                    ""type"": ""Button"",
                    ""id"": ""9353a448-9bf6-4071-904c-c178a4b24908"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""RightBumper"",
                    ""type"": ""Button"",
                    ""id"": ""4736091c-8196-496b-9d72-1839f96865c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""LeftTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""2be4f7ca-ad61-4e9e-8240-1e2ca303bcb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""RightTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""7c6f8b4a-dd7d-4dc4-bc67-641135830a64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SelectButton"",
                    ""type"": ""Button"",
                    ""id"": ""46321fbd-e412-4233-a8e6-e93b2b353965"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""StartButton"",
                    ""type"": ""Button"",
                    ""id"": ""ce04415b-f828-48e0-8b99-40fd9463bbf8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""UIMove"",
                    ""type"": ""Value"",
                    ""id"": ""6aa77bca-3d1c-4740-9241-d284964d2cf2"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""8fcf2b4a-f1b7-46c3-a9b9-acb6fdc04200"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DpadLeft"",
                    ""type"": ""Button"",
                    ""id"": ""aa42ac63-52e2-48ed-8cd7-95c7e1aab79b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""DpadRight"",
                    ""type"": ""Button"",
                    ""id"": ""c5a04e8e-aff0-4b1b-8153-c3d3a1cbd0ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""DpadUp"",
                    ""type"": ""Button"",
                    ""id"": ""f627521c-728c-451e-a13f-ef4196368f16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""DpadDown"",
                    ""type"": ""Button"",
                    ""id"": ""9db72526-1dc0-43a4-aa59-b45d8718286e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""54c60521-ebba-4081-9cf4-13f47fc5fe15"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""LeftStickX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3860d59f-082d-4f69-97ab-a26de864307f"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""LeftStickY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0794a7c1-89ab-4ba5-9518-d6ae9225e4d5"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""RightStickX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d05b989-3744-4154-9b58-a78aa847edbc"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""RightStickY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5e9efa6-8b65-4503-9939-f9ec5ca8b293"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""NorthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a0011c7-d95d-4f20-9f65-2d4798e8eeab"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""EastButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1edef835-a71e-44c9-989e-9b2cc27750f9"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""SouthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd8c7b2d-4b9a-4fd6-adcc-a7a4265151e3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""WestButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""703c1bb1-9688-4a8c-8e8d-5dd39d982a07"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""LeftStickPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff8ce817-2aa7-43aa-9092-799b9cefea14"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""RightStickPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""052ad282-e9f4-40b3-8495-19bf4c45b549"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""LeftBumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c6c1941-d06d-4a25-9a16-58783d4bb266"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""RightBumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fbc2676-fcf1-4cef-a7bb-a74550367da2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""RightTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c37a4797-81c5-404e-9c04-363e88080b58"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""LeftTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1993caca-41b2-4c7e-b2c9-e00c4fe6e935"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""SelectButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee4f1d36-d742-4b7f-8fde-b584c75687ee"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""StartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87da7dd5-47bf-4b03-a5bf-1df4b82ab57c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""UIMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""c0117255-84fe-41fe-a011-3913d2e6b865"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7190d7c1-4aa2-44b2-a41b-e4855810126a"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bd877459-1d1d-46a3-8629-aa8d997dda4d"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""91989f4b-a4d0-4f43-8f5c-67fc35cbb989"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b73f74e7-a722-47b7-a906-52c7fe1e1657"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2231e1f4-e415-47b9-8c6e-8e6c88e5485f"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""73e6960b-73a2-4d32-9735-f25b5227616f"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""02de8575-002e-4b95-a244-ffed28ec6684"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e5a65e7a-16f6-4a8e-ba81-4a3c9d5a07b7"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""16f27e0a-a810-4b31-ac5c-b250ca1ecf73"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""0081e92d-4bfd-4b8f-8d2f-393022fffdd5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f48708ee-8ed4-489b-bfaf-ba8cfbfa1112"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f92ebc8b-c6ba-4640-85f6-bde7e0e1fb63"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""02c4b3d1-38cd-490f-89e7-684068cdf52a"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6ac2e4eb-0cea-4826-9286-4aaa8928fc92"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;GamepadControlScheme"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bd82ff8f-d48c-4a67-ab7c-4f27e8e76434"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""DpadLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8c5825f-18de-4338-94f2-08463e98b6fe"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""DpadRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f28a1822-9cb6-4ef3-8595-da0b9cda67cb"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""DpadUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""adfc4043-a047-4e5c-8a0c-cab9be5f733d"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamepadControlScheme"",
                    ""action"": ""DpadDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""GamepadControlScheme"",
            ""bindingGroup"": ""GamepadControlScheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_LeftStickX = m_Player.FindAction("LeftStickX", throwIfNotFound: true);
        m_Player_LeftStickY = m_Player.FindAction("LeftStickY", throwIfNotFound: true);
        m_Player_LeftStickPress = m_Player.FindAction("LeftStickPress", throwIfNotFound: true);
        m_Player_RightStickX = m_Player.FindAction("RightStickX", throwIfNotFound: true);
        m_Player_RightStickY = m_Player.FindAction("RightStickY", throwIfNotFound: true);
        m_Player_RightStickPress = m_Player.FindAction("RightStickPress", throwIfNotFound: true);
        m_Player_NorthButton = m_Player.FindAction("NorthButton", throwIfNotFound: true);
        m_Player_EastButton = m_Player.FindAction("EastButton", throwIfNotFound: true);
        m_Player_SouthButton = m_Player.FindAction("SouthButton", throwIfNotFound: true);
        m_Player_WestButton = m_Player.FindAction("WestButton", throwIfNotFound: true);
        m_Player_LeftBumper = m_Player.FindAction("LeftBumper", throwIfNotFound: true);
        m_Player_RightBumper = m_Player.FindAction("RightBumper", throwIfNotFound: true);
        m_Player_LeftTrigger = m_Player.FindAction("LeftTrigger", throwIfNotFound: true);
        m_Player_RightTrigger = m_Player.FindAction("RightTrigger", throwIfNotFound: true);
        m_Player_SelectButton = m_Player.FindAction("SelectButton", throwIfNotFound: true);
        m_Player_StartButton = m_Player.FindAction("StartButton", throwIfNotFound: true);
        m_Player_UIMove = m_Player.FindAction("UIMove", throwIfNotFound: true);
        m_Player_Navigate = m_Player.FindAction("Navigate", throwIfNotFound: true);
        m_Player_DpadLeft = m_Player.FindAction("DpadLeft", throwIfNotFound: true);
        m_Player_DpadRight = m_Player.FindAction("DpadRight", throwIfNotFound: true);
        m_Player_DpadUp = m_Player.FindAction("DpadUp", throwIfNotFound: true);
        m_Player_DpadDown = m_Player.FindAction("DpadDown", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_LeftStickX;
    private readonly InputAction m_Player_LeftStickY;
    private readonly InputAction m_Player_LeftStickPress;
    private readonly InputAction m_Player_RightStickX;
    private readonly InputAction m_Player_RightStickY;
    private readonly InputAction m_Player_RightStickPress;
    private readonly InputAction m_Player_NorthButton;
    private readonly InputAction m_Player_EastButton;
    private readonly InputAction m_Player_SouthButton;
    private readonly InputAction m_Player_WestButton;
    private readonly InputAction m_Player_LeftBumper;
    private readonly InputAction m_Player_RightBumper;
    private readonly InputAction m_Player_LeftTrigger;
    private readonly InputAction m_Player_RightTrigger;
    private readonly InputAction m_Player_SelectButton;
    private readonly InputAction m_Player_StartButton;
    private readonly InputAction m_Player_UIMove;
    private readonly InputAction m_Player_Navigate;
    private readonly InputAction m_Player_DpadLeft;
    private readonly InputAction m_Player_DpadRight;
    private readonly InputAction m_Player_DpadUp;
    private readonly InputAction m_Player_DpadDown;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftStickX => m_Wrapper.m_Player_LeftStickX;
        public InputAction @LeftStickY => m_Wrapper.m_Player_LeftStickY;
        public InputAction @LeftStickPress => m_Wrapper.m_Player_LeftStickPress;
        public InputAction @RightStickX => m_Wrapper.m_Player_RightStickX;
        public InputAction @RightStickY => m_Wrapper.m_Player_RightStickY;
        public InputAction @RightStickPress => m_Wrapper.m_Player_RightStickPress;
        public InputAction @NorthButton => m_Wrapper.m_Player_NorthButton;
        public InputAction @EastButton => m_Wrapper.m_Player_EastButton;
        public InputAction @SouthButton => m_Wrapper.m_Player_SouthButton;
        public InputAction @WestButton => m_Wrapper.m_Player_WestButton;
        public InputAction @LeftBumper => m_Wrapper.m_Player_LeftBumper;
        public InputAction @RightBumper => m_Wrapper.m_Player_RightBumper;
        public InputAction @LeftTrigger => m_Wrapper.m_Player_LeftTrigger;
        public InputAction @RightTrigger => m_Wrapper.m_Player_RightTrigger;
        public InputAction @SelectButton => m_Wrapper.m_Player_SelectButton;
        public InputAction @StartButton => m_Wrapper.m_Player_StartButton;
        public InputAction @UIMove => m_Wrapper.m_Player_UIMove;
        public InputAction @Navigate => m_Wrapper.m_Player_Navigate;
        public InputAction @DpadLeft => m_Wrapper.m_Player_DpadLeft;
        public InputAction @DpadRight => m_Wrapper.m_Player_DpadRight;
        public InputAction @DpadUp => m_Wrapper.m_Player_DpadUp;
        public InputAction @DpadDown => m_Wrapper.m_Player_DpadDown;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @LeftStickX.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickX;
                @LeftStickX.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickX;
                @LeftStickX.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickX;
                @LeftStickY.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickY;
                @LeftStickY.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickY;
                @LeftStickY.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickY;
                @LeftStickPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickPress;
                @LeftStickPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickPress;
                @LeftStickPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftStickPress;
                @RightStickX.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickX;
                @RightStickX.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickX;
                @RightStickX.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickX;
                @RightStickY.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickY;
                @RightStickY.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickY;
                @RightStickY.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickY;
                @RightStickPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickPress;
                @RightStickPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickPress;
                @RightStickPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightStickPress;
                @NorthButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNorthButton;
                @NorthButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNorthButton;
                @NorthButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNorthButton;
                @EastButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEastButton;
                @EastButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEastButton;
                @EastButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEastButton;
                @SouthButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSouthButton;
                @SouthButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSouthButton;
                @SouthButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSouthButton;
                @WestButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWestButton;
                @WestButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWestButton;
                @WestButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWestButton;
                @LeftBumper.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftBumper;
                @LeftBumper.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftBumper;
                @LeftBumper.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftBumper;
                @RightBumper.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightBumper;
                @RightBumper.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightBumper;
                @RightBumper.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightBumper;
                @LeftTrigger.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTrigger;
                @LeftTrigger.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTrigger;
                @LeftTrigger.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTrigger;
                @RightTrigger.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTrigger;
                @RightTrigger.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTrigger;
                @RightTrigger.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTrigger;
                @SelectButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectButton;
                @SelectButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectButton;
                @SelectButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectButton;
                @StartButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStartButton;
                @StartButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStartButton;
                @StartButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStartButton;
                @UIMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUIMove;
                @UIMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUIMove;
                @UIMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUIMove;
                @Navigate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNavigate;
                @DpadLeft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadLeft;
                @DpadLeft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadLeft;
                @DpadLeft.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadLeft;
                @DpadRight.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadRight;
                @DpadRight.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadRight;
                @DpadRight.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadRight;
                @DpadUp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadUp;
                @DpadUp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadUp;
                @DpadUp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadUp;
                @DpadDown.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadDown;
                @DpadDown.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadDown;
                @DpadDown.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDpadDown;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftStickX.started += instance.OnLeftStickX;
                @LeftStickX.performed += instance.OnLeftStickX;
                @LeftStickX.canceled += instance.OnLeftStickX;
                @LeftStickY.started += instance.OnLeftStickY;
                @LeftStickY.performed += instance.OnLeftStickY;
                @LeftStickY.canceled += instance.OnLeftStickY;
                @LeftStickPress.started += instance.OnLeftStickPress;
                @LeftStickPress.performed += instance.OnLeftStickPress;
                @LeftStickPress.canceled += instance.OnLeftStickPress;
                @RightStickX.started += instance.OnRightStickX;
                @RightStickX.performed += instance.OnRightStickX;
                @RightStickX.canceled += instance.OnRightStickX;
                @RightStickY.started += instance.OnRightStickY;
                @RightStickY.performed += instance.OnRightStickY;
                @RightStickY.canceled += instance.OnRightStickY;
                @RightStickPress.started += instance.OnRightStickPress;
                @RightStickPress.performed += instance.OnRightStickPress;
                @RightStickPress.canceled += instance.OnRightStickPress;
                @NorthButton.started += instance.OnNorthButton;
                @NorthButton.performed += instance.OnNorthButton;
                @NorthButton.canceled += instance.OnNorthButton;
                @EastButton.started += instance.OnEastButton;
                @EastButton.performed += instance.OnEastButton;
                @EastButton.canceled += instance.OnEastButton;
                @SouthButton.started += instance.OnSouthButton;
                @SouthButton.performed += instance.OnSouthButton;
                @SouthButton.canceled += instance.OnSouthButton;
                @WestButton.started += instance.OnWestButton;
                @WestButton.performed += instance.OnWestButton;
                @WestButton.canceled += instance.OnWestButton;
                @LeftBumper.started += instance.OnLeftBumper;
                @LeftBumper.performed += instance.OnLeftBumper;
                @LeftBumper.canceled += instance.OnLeftBumper;
                @RightBumper.started += instance.OnRightBumper;
                @RightBumper.performed += instance.OnRightBumper;
                @RightBumper.canceled += instance.OnRightBumper;
                @LeftTrigger.started += instance.OnLeftTrigger;
                @LeftTrigger.performed += instance.OnLeftTrigger;
                @LeftTrigger.canceled += instance.OnLeftTrigger;
                @RightTrigger.started += instance.OnRightTrigger;
                @RightTrigger.performed += instance.OnRightTrigger;
                @RightTrigger.canceled += instance.OnRightTrigger;
                @SelectButton.started += instance.OnSelectButton;
                @SelectButton.performed += instance.OnSelectButton;
                @SelectButton.canceled += instance.OnSelectButton;
                @StartButton.started += instance.OnStartButton;
                @StartButton.performed += instance.OnStartButton;
                @StartButton.canceled += instance.OnStartButton;
                @UIMove.started += instance.OnUIMove;
                @UIMove.performed += instance.OnUIMove;
                @UIMove.canceled += instance.OnUIMove;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @DpadLeft.started += instance.OnDpadLeft;
                @DpadLeft.performed += instance.OnDpadLeft;
                @DpadLeft.canceled += instance.OnDpadLeft;
                @DpadRight.started += instance.OnDpadRight;
                @DpadRight.performed += instance.OnDpadRight;
                @DpadRight.canceled += instance.OnDpadRight;
                @DpadUp.started += instance.OnDpadUp;
                @DpadUp.performed += instance.OnDpadUp;
                @DpadUp.canceled += instance.OnDpadUp;
                @DpadDown.started += instance.OnDpadDown;
                @DpadDown.performed += instance.OnDpadDown;
                @DpadDown.canceled += instance.OnDpadDown;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_GamepadControlSchemeSchemeIndex = -1;
    public InputControlScheme GamepadControlSchemeScheme
    {
        get
        {
            if (m_GamepadControlSchemeSchemeIndex == -1) m_GamepadControlSchemeSchemeIndex = asset.FindControlSchemeIndex("GamepadControlScheme");
            return asset.controlSchemes[m_GamepadControlSchemeSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnLeftStickX(InputAction.CallbackContext context);
        void OnLeftStickY(InputAction.CallbackContext context);
        void OnLeftStickPress(InputAction.CallbackContext context);
        void OnRightStickX(InputAction.CallbackContext context);
        void OnRightStickY(InputAction.CallbackContext context);
        void OnRightStickPress(InputAction.CallbackContext context);
        void OnNorthButton(InputAction.CallbackContext context);
        void OnEastButton(InputAction.CallbackContext context);
        void OnSouthButton(InputAction.CallbackContext context);
        void OnWestButton(InputAction.CallbackContext context);
        void OnLeftBumper(InputAction.CallbackContext context);
        void OnRightBumper(InputAction.CallbackContext context);
        void OnLeftTrigger(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
        void OnSelectButton(InputAction.CallbackContext context);
        void OnStartButton(InputAction.CallbackContext context);
        void OnUIMove(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnDpadLeft(InputAction.CallbackContext context);
        void OnDpadRight(InputAction.CallbackContext context);
        void OnDpadUp(InputAction.CallbackContext context);
        void OnDpadDown(InputAction.CallbackContext context);
    }
}
