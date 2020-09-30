using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterLoadNode : MonoBehaviour, ISelectHandler
{
    public Text characterInfo;
    // Start is called before the first frame update
    public PlayerSaveData playerData;
    public CharacterCreationPanel mainPanel;

    public void SetCharacter(PlayerSaveData saveData)
    {
        playerData = saveData;
        characterInfo.text = playerData.playerName + "\n";
        characterInfo.text += playerData.classType + " - Level " + playerData.playerLevel;
    }

    public void SelectNode()
    {
        mainPanel.LoadCharacter(playerData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        mainPanel.loadMenu.SetCurrentNode(this);
    }

}
