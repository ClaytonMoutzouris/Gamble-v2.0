using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIPanels : MonoBehaviour
{
    public static PlayerUIPanels instance;
    public PlayerPanel prefab;
    public PlayerPanel[] playerPanels = new PlayerPanel[4];
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(int index)
    {
        playerPanels[index] = Instantiate(prefab, transform);
        playerPanels[index].playerIndex = index;
    }

    public void RemovePlayer(int index)
    {
        Destroy(playerPanels[index].gameObject);

    }

    public void OpenClosePanel(int index)
    {
        if (playerPanels[index].isOpen)
        {
            playerPanels[index].ClosePlayerPanel();
        }
        else
        {
            playerPanels[index].OpenPlayerPanel();
        }
    }


}
