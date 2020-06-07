using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadCharacterMenu : CreationMenuTab
{
    public CharacterCreationPanel mainPanel;
    public CharacterLoadNode loadNodePrefab;
    public List<CharacterLoadNode> loadNodes;
    public GameObject nodeContainer;

    public override void Open()
    {
        ClearNodes();

        foreach (PlayerSaveData saveData in SaveLoadManager.instance.FindCharacters())
        {

            CharacterLoadNode node = Instantiate<CharacterLoadNode>(loadNodePrefab, nodeContainer.transform);
            node.mainPanel = mainPanel;
            node.SetCharacter(saveData);
            loadNodes.Add(node);
        }

        if (loadNodes.Count > 0)
        {
            inputAnchor = loadNodes[0].gameObject;
        }
    }

    public void ClearNodes()
    {
        foreach(CharacterLoadNode node in loadNodes)
        {
            Destroy(node.gameObject);
        }

        loadNodes.Clear();
    }

    public override void Close()
    {

    }
}
