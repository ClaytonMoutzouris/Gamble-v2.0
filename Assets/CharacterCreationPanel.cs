
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterCreationPanel : MonoBehaviour
{
    public PlayerClassType classType;
    public SwapIndex swapIndex;
    public CharacterPreviewPortait preview;
    public int playerIndex = 0;
    public GameObject inputAnchor;
    public Image background;
    NewGamepadInput gamepadInput;
    public List<CreationMenuTab> tabs;
    public LoadCharacterMenu loadMenu;
    public int activeTabIndex = 0;
    public bool isOpen = false;
    public string characterName;
    public InputField characterNameField;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        ChangeTab(activeTabIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen && characterNameField.isFocused
            && (GamepadInputManager.instance.gamepadInputs[playerIndex].buttonInputs[(int)GamepadButtons.EastButton]
            || GamepadInputManager.instance.gamepadInputs[playerIndex].buttonInputs[(int)GamepadButtons.SouthButton]))
        {
            characterNameField.DeactivateInputField();
        }

    }

    public void ChangeTab(int index)
    {
        foreach(CreationMenuTab tab in tabs)
        {
            tab.gameObject.SetActive(false);
        }

        activeTabIndex = index;
        tabs[activeTabIndex].gameObject.SetActive(true);
        tabs[activeTabIndex].Open();

        //EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(tabs[activeTabIndex].inputAnchor);
        //NewEventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(tabs[activeTabIndex].inputAnchor);
        if (gamepadInput != null)
        {
            gamepadInput.GetComponent<EventSystem>().SetSelectedGameObject(tabs[activeTabIndex].inputAnchor);
            Debug.Log("Selected object " + gamepadInput.GetComponent<EventSystem>().currentSelectedGameObject);
        }
            tabs[activeTabIndex].inputAnchor.GetComponent<Button>().OnSelect(null);

    }

    public void NewCharacter(NewGamepadInput i)
    {

        gamepadInput = i;
        OpenCreationPanel();
    }

    public void CreateCharacter()
    {
        PlayerUIPanels.instance.AddPlayer(playerIndex);

        Player newPlayer = new Player(Instantiate(Resources.Load("Prototypes/Entity/Player/PlayerPrototype") as PlayerPrototype), Resources.Load<PlayerClass>("Prototypes/Player/Classes/" + classType.ToString()), playerIndex);
        newPlayer.entityName = characterName;
        newPlayer.SetColorPalette(preview.characterColors);


        newPlayer.SetInput(gamepadInput);
        newPlayer.playerPanel = PlayerUIPanels.instance.playerPanels[playerIndex];
        CrewManager.instance.AddPlayer(playerIndex, newPlayer);
        Close();
    }

    public void LoadCharacter(PlayerSaveData saveData)
    {
        PlayerUIPanels.instance.AddPlayer(playerIndex);

        Player newPlayer = new Player(Instantiate(Resources.Load("Prototypes/Entity/Player/PlayerPrototype") as PlayerPrototype), Resources.Load<PlayerClass>("Prototypes/Player/Classes/" + saveData.classType), playerIndex);
        newPlayer.playerPanel = PlayerUIPanels.instance.playerPanels[playerIndex];
        newPlayer.SetInput(gamepadInput);
        CrewManager.instance.AddPlayer(playerIndex, newPlayer, saveData);
        Close();
    }

    public void OpenCreationPanel()
    {

        CreationPanelsUI.instance.placeholders[playerIndex].SetActive(false);
        gameObject.SetActive(true);
        ChangeTab(0);
        isOpen = true;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        CreationPanelsUI.instance.placeholders[playerIndex].SetActive(true);
        isOpen = false;


    }

    public void CancelCreation()
    {
        Close();
        GamepadInputManager.instance.RemovePlayerAtIndex(playerIndex);
    }

    public void SwapColor(Color color)
    {
        preview.colorSwapper.SwapColor(swapIndex, color);

        switch(swapIndex)
        {
            case SwapIndex.Skin:
                preview.characterColors[0] = color;
                break;
            case SwapIndex.HoodPrimary:
                preview.characterColors[1] = color;
                break;
            case SwapIndex.HoodSecondary:
                preview.characterColors[2] = color;
                break;
            case SwapIndex.ShirtPrimary:
                preview.characterColors[3] = color;
                break;
            case SwapIndex.ShirtSecondary:
                preview.characterColors[4] = color;
                break;
            case SwapIndex.Shoes:
                preview.characterColors[5] = color;
                break;
            case SwapIndex.Pants:
                preview.characterColors[6] = color;

                break;
        }
        
    }

    public void SetName(string name)
    {
        characterName = name;
    }
}
