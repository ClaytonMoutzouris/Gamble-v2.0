using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#
using LocalCoop;

/// <summary>
/// A manager that can be used to add players without having pre-assigned controlled ID's to the input
/// </summary>
public class PlayerInputManager : MonoBehaviour {

    GamePadState[] controllerStates = new GamePadState[4];
    PlayerIndex[] controllerIDs = new PlayerIndex[4];
    public PlayerGamepadInput[] playerInputs = new PlayerGamepadInput[4];

    public GameManager levelManager;
    public bool use_X_Input = true;
    public int connectedControllers = 0;   //if this variable changes, we need to call an update on the gamepads

    public static PlayerInputManager singleton = null;

    void Awake() {
        //Check if instance already exists
        if (singleton == null) {
            //if not, set instance to this
            singleton = this;
        }

        //If instance already exists and it's not this:
        else if (singleton != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        connectedControllers = CheckControllerAmount();
        Assign_X_Input_Controllers();
    }

    void Assign_X_Input_Controllers() {
        for (int i = 0; i < 4; ++i) {
            PlayerIndex controllerID = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(controllerID);
            if (testState.IsConnected) {
                switch (i) {
                    case 0:
                        controllerIDs[i] = controllerID;
                        controllerStates[i] = testState;
                        break;
                    case 1:
                        controllerIDs[i] = controllerID;
                        controllerStates[i] = testState;
                        break;
                    case 2:
                        controllerIDs[i] = controllerID;
                        controllerStates[i] = testState;
                        break;
                    case 3:
                        controllerIDs[i] = controllerID;
                        controllerStates[i] = testState;
                        break;
                    default:
                        break;
                }

                Debug.Log(string.Format("GamePad found {0}", controllerID));
            }
        }
    }

    //Checks if the amount of controllers changed when connecting/unplugging new controllers
    int CheckControllerAmount() {
        int amount = 0;

        for (int i = 0; i < 4; ++i) {
            PlayerIndex controllerID = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(controllerID);
            if (testState.IsConnected) {
                amount++;
            }
        }

        return amount;
    }

    // Update is called once per frame
    void Update() {
        if (use_X_Input) {
            if (connectedControllers != CheckControllerAmount()) {
                connectedControllers = CheckControllerAmount();
                print("update controllers");
                Assign_X_Input_Controllers();
            }

            for (int i = 0; i < 4; i++)
            {
                if (controllerStates[i].IsConnected)
                {
                    controllerStates[i] = GamePad.GetState(controllerIDs[i]);
                    if (controllerStates[i].Buttons.Start == ButtonState.Pressed)
                    {
                        if (levelManager.players[i] == null)
                        {
                            playerInputs[i].controllerID = (int)controllerIDs[i];
                            CreationPanelsUI.instance.creationPanels[i].NewCharacter(playerInputs[i]);
                            //levelManager.AddPlayer(i, playerInputs[i]);
                            //you can call a function here to instantiate a player and then assign this ID to the player input's script to connect the player to the controller that pressed start for example
                            //you then also need to assign that player input script to one of the X input modules to connect it with unity's input system
                        }
                    }
                }
            }
                
        }
        else {
            //join game
            if (Input.GetButtonDown("Start1")) {
                print("start1");
            }
            if (Input.GetButtonDown("Start2")) {
                print("Start2");
            }
            if (Input.GetButtonDown("Start3")) {
                print("Start3");
            }
            if (Input.GetButtonDown("Start4")) {
                print("Start4");
            }
        }
    }
}
