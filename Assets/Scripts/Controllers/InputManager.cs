using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputMode { Game, Pause, Inventory };

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public List<bool[]> playerInputs;
    public List<bool[]> playerPrevInputs;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;


        playerInputs = new List<bool[]>();
        playerPrevInputs = new List<bool[]>();

        for(int i = 0; i < Constants.MAX_NUM_PLAYERS; i++)
        { 
            //init player 1
            bool[] inputs = new bool[(int)KeyInput.Count];
            bool[] prevInputs = new bool[(int)KeyInput.Count];
            playerInputs.Add(inputs);
            playerPrevInputs.Add(prevInputs);

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int p = 0; p < Constants.MAX_NUM_PLAYERS; p++)
        {
            switch (p)
            {
                case 0:
                    playerInputs[p][(int)KeyInput.GoRight] = Input.GetAxisRaw("Horizontal") > 0;
                    playerInputs[p][(int)KeyInput.GoLeft] = Input.GetAxisRaw("Horizontal") < 0;
                    playerInputs[p][(int)KeyInput.GoDown] = Input.GetAxisRaw("Vertical") < 0;
                    playerInputs[p][(int)KeyInput.Climb] = Input.GetAxisRaw("Vertical") > 0;
                    playerInputs[p][(int)KeyInput.Jump] = Input.GetButton("Jump");
                    playerInputs[p][(int)KeyInput.Shoot] = Input.GetButton("Fire1");
                    playerInputs[p][(int)KeyInput.Attack] = Input.GetButton("Fire2");
                    playerInputs[p][(int)KeyInput.Item] = Input.GetKey(KeyCode.V);
                    playerInputs[p][(int)KeyInput.Inventory] = Input.GetKey(KeyCode.Alpha1);

                    break;
                case 1:
                    playerInputs[p][(int)KeyInput.GoRight] = Input.GetKey(KeyCode.D);
                    playerInputs[p][(int)KeyInput.GoLeft] = Input.GetKey(KeyCode.A);
                    playerInputs[p][(int)KeyInput.GoDown] = Input.GetKey(KeyCode.S);
                    playerInputs[p][(int)KeyInput.Climb] = Input.GetKey(KeyCode.W);
                    playerInputs[p][(int)KeyInput.Jump] = Input.GetKey(KeyCode.F);
                    playerInputs[p][(int)KeyInput.Shoot] = Input.GetKey(KeyCode.Q);
                    playerInputs[p][(int)KeyInput.Attack] = Input.GetKey(KeyCode.R);
                    playerInputs[p][(int)KeyInput.Item] = Input.GetKey(KeyCode.V);
                    playerInputs[p][(int)KeyInput.Inventory] = Input.GetKey(KeyCode.Alpha2);


                    break;
            }

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
