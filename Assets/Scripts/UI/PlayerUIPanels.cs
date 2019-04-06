﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIPanels : MonoBehaviour
{
    public static PlayerUIPanels instance;
    public List<PlayerPanel> playerPanels;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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