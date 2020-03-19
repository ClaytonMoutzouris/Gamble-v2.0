using LocalCoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationPanelsUI : MonoBehaviour
{
    public static CreationPanelsUI instance;
    public CharacterCreationPanel prefab;
    public CharacterCreationPanel[] creationPanels = new CharacterCreationPanel[4];
    public GameObject[] placeholders = new GameObject[4];
    public GameObject placeholderPrefab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        for (int i = 0; i < 4; i++)
        {
        }

        for (int i = 0; i < 4; i++)
        {
            creationPanels[i] = Instantiate(prefab, transform);
            placeholders[i] = Instantiate(placeholderPrefab, transform);
            creationPanels[i].playerIndex = i;
        }



    }

    public void AddPlayer(int index)
    {
        /*
        creationPanels[index] = Instantiate(prefab, transform);
        creationPanels[index].Initialize();
        creationPanels[index].playerIndex = index;
        */
    }

    public void RemovePlayer(int index)
    {
        //Destroy(playerPanels[index].gameObject);

    }

    public void OpenClosePanel(int index)
    {
        /*
        if (playerPanels[index].isOpen)
        {
            playerPanels[index].ClosePlayerPanel();
        }
        else
        {
            playerPanels[index].OpenPlayerPanel();
        }
        */
    }
}
