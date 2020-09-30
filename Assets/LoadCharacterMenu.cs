using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacterMenu : CreationMenuTab
{
    public CharacterCreationPanel mainPanel;
    public CharacterLoadNode loadNodePrefab;
    public List<CharacterLoadNode> loadNodes;
    public GameObject nodeContainer;
    public CharacterLoadNode currentNode;
    public ScrollRect maskRect;

    private void Update()
    {
        if(currentNode != null)
        {
            maskRect.ScrollRepositionY(currentNode.GetComponent<RectTransform>());
        }

    }

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
            currentNode = loadNodes[0];
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
        base.Close();
    }

    public void SetCurrentNode(CharacterLoadNode node)
    {
        currentNode = node;
    }
}
