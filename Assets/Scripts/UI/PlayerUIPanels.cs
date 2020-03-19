using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIPanels : MonoBehaviour
{
    public static PlayerUIPanels instance;
    public PlayerPanel prefab;
    public PlayerPanel[] playerPanels = new PlayerPanel[4];
    public GameObject[] placeholders = new GameObject[4];
    public GameObject placeholderPrefab;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for (int i = 0; i < 4; i++)
        {
            placeholders[i] = Instantiate(placeholderPrefab, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayer(int index)
    {
        playerPanels[index] = Instantiate(prefab, transform);
        Destroy(placeholders[index]);
        playerPanels[index].transform.SetSiblingIndex(index);
        playerPanels[index].Initialize();
        playerPanels[index].playerIndex = index;
    }

    public void RemovePlayer(int index)
    {
        Destroy(playerPanels[index].gameObject);
        placeholders[index] = Instantiate(placeholderPrefab, transform);

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
