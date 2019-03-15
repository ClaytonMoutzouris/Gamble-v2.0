using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputMode { Game, Pause, Inventory };

public class InputManager : MonoBehaviour
{
    public InputMode mode;
    public Player player;

    public bool[] playerInputs;
    public bool[] playerPrevInputs;
    public int[] playerAxis;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputs = new bool[(int)KeyInput.Count];
        playerPrevInputs = new bool[(int)KeyInput.Count];
        //playerAxis = new int[(int)StickInput.Count];

        GetComponent<Player>().SetInputs(playerInputs, playerPrevInputs);
    }

    // Update is called once per frame
    void Update()
    {

        switch (mode)
        {
            case InputMode.Game:
                playerInputs[(int)KeyInput.LeftStick_Right] = Input.GetAxisRaw("LeftHorizontal") > 0;
                playerInputs[(int)KeyInput.LeftStick_Left] = Input.GetAxisRaw("LeftHorizontal") < 0;
                playerInputs[(int)KeyInput.LeftStick_Down] = Input.GetAxisRaw("LeftVertical") < 0;
                playerInputs[(int)KeyInput.LeftStick_Up] = Input.GetAxisRaw("LeftVertical") > 0;
                playerInputs[(int)KeyInput.RightStick_Right] = Input.GetAxisRaw("RightHorizontal") > 0;
                playerInputs[(int)KeyInput.RightStick_Left] = Input.GetAxisRaw("RightHorizontal") < 0;
                playerInputs[(int)KeyInput.RightStick_Down] = Input.GetAxisRaw("RightVertical") < 0;
                playerInputs[(int)KeyInput.RightStick_Up] = Input.GetAxisRaw("RightVertical") > 0;
                playerInputs[(int)KeyInput.Jump] = Input.GetButton("Jump");
                playerInputs[(int)KeyInput.Shoot] = Input.GetAxisRaw("Fire1") > 0;
                playerInputs[(int)KeyInput.Attack] = Input.GetButton("Fire2");
                playerInputs[(int)KeyInput.Item] = Input.GetKey(KeyCode.V);
                playerInputs[(int)KeyInput.Inventory] = Input.GetKey(KeyCode.Alpha1);
                break;
            case InputMode.Inventory:

                break;
            case InputMode.Pause:

                break;
        }




        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerUIPanels.instance.OpenClosePanel(0);
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (LevelManager.instance.mGameMode == GameMode.Game)
            {
                LevelManager.instance.PauseGame();
                EventSystem.current.SetSelectedGameObject(PauseMenu.instance.defaultObject);
            }
            else
            {
                LevelManager.instance.UnpauseGame();
            }
        }
    }

}
