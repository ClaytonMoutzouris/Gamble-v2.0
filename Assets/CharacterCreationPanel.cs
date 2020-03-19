using LocalCoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationPanel : MonoBehaviour
{
    public PlayerClassType classType;
    public SwapIndex swapIndex;
    public CharacterPreviewPortait preview;
    public int playerIndex = 0;
    public GameObject inputAnchor;
    PlayerGamepadInput input;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //OpenPlayerPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewCharacter(PlayerGamepadInput i)
    {
        input = i;
        OpenCreationPanel();
    }

    public void CreateCharacter()
    {
        PlayerUIPanels.instance.AddPlayer(playerIndex);

        Player newPlayer = new Player(Instantiate(Resources.Load("Prototypes/Entity/Player/PlayerPrototype") as PlayerPrototype), Resources.Load<PlayerClass>("Prototypes/Player/Classes/" + classType.ToString()), playerIndex);

        newPlayer.SetColorPalette(preview.characterColors);

        newPlayer.SetInput(input);
        newPlayer.playerPanel = PlayerUIPanels.instance.playerPanels[playerIndex];
        LevelManager.instance.AddPlayer(playerIndex, newPlayer);
        Close();
    }

    public void OpenCreationPanel()
    {
        EventSystemManager.instance.GetEventSystem(playerIndex).SetSelectedGameObject(inputAnchor);
        CreationPanelsUI.instance.placeholders[playerIndex].SetActive(false);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        CreationPanelsUI.instance.placeholders[playerIndex].SetActive(true);

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
}
