using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NavigationMenu : MonoBehaviour
{
    public static NavigationMenu instance;
    public GameObject defaultObject;
    public int pausedIndex = -1;
    public GameObject worldContainer;
    public WorldNode prefab;
    public List<WorldNode> worldNodes = new List<WorldNode>();
    public Button backButton;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
        worldNodes = new List<WorldNode>();

        WorldNode temp;


        /*
        temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
        temp.SetUp(WorldType.Tundra);
        worldNodes.Add(temp);

        temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
        temp.SetUp(WorldType.Lava);
        worldNodes.Add(temp);

        temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
        temp.SetUp(WorldType.Purple);
        worldNodes.Add(temp);

        temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
        temp.SetUp(WorldType.Yellow);
        worldNodes.Add(temp);
        */

        RollMaps();


        //LevelManager.instance.completedWorlds = new bool[worldNodes.Count];


    }

    public void Open(int playerIndex)
    {
        if (LevelManager.instance.AllWorldsCleared())
        {
            Debug.Log("This was true");
            SetVoidMap();
        } 

        pausedIndex = playerIndex;
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(defaultObject);
        defaultObject.GetComponent<Button>().OnSelect(null);
        gameObject.SetActive(true);
        LevelManager.instance.players[playerIndex].Input.inputState = PlayerInputState.NavigationMenu;


    }

    public void ClearMaps()
    {
        foreach(WorldNode world in worldNodes)
        {
            Destroy(world.gameObject);
        }

        worldNodes.Clear();
    }

    public void RollMaps()
    {
        ClearMaps();

        WorldNode temp;
        for (int i = 0; i < LevelManager.instance.numMaps; i++)
        {
            temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
            temp.SetUp((WorldType)Random.Range(0, LevelManager.instance.numMaps));
            worldNodes.Add(temp);
        }

        defaultObject = worldNodes[0].gameObject;


        Navigation nav = backButton.navigation;
        nav.selectOnUp = worldNodes[0].button;
        backButton.navigation = nav;

        for (int i = 0; i < worldNodes.Count; i++)
        {
            nav = worldNodes[i].button.navigation;
            if (i == 0)
            {
                nav.selectOnLeft = worldNodes[worldNodes.Count - 1].button;
                nav.selectOnRight = worldNodes[i + 1].button;

            }
            else if (i == worldNodes.Count - 1)
            {
                nav.selectOnLeft = worldNodes[i - 1].button;
                nav.selectOnRight = worldNodes[0].button;

            }
            else
            {
                nav.selectOnLeft = worldNodes[i - 1].button;
                nav.selectOnRight = worldNodes[i + 1].button;

            }
            nav.selectOnDown = backButton;
            worldNodes[i].button.navigation = nav;
        }
    }


    public void RollMaps(int numMaps)
    {

        ClearMaps();

        WorldNode temp;
        for (int i = 0; i < numMaps; i++)
        {
            temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
            temp.SetUp((WorldType)Random.Range(0, (int)WorldType.Void));
            defaultObject = temp.gameObject;
            worldNodes.Add(temp);
        }

        Navigation nav = backButton.navigation;
        nav.selectOnUp = worldNodes[0].button;
        backButton.navigation = nav;

        for (int i = 0; i < worldNodes.Count; i++)
        {
            nav = worldNodes[i].button.navigation;
            if (i == 0)
            {
                nav.selectOnLeft = worldNodes[worldNodes.Count - 1].button;
                nav.selectOnRight = worldNodes[i + 1].button;

            }
            else if (i == worldNodes.Count - 1)
            {
                nav.selectOnLeft = worldNodes[i - 1].button;
                nav.selectOnRight = worldNodes[0].button;

            }
            else
            {
                nav.selectOnLeft = worldNodes[i - 1].button;
                nav.selectOnRight = worldNodes[i + 1].button;

            }
            nav.selectOnDown = backButton;
            worldNodes[i].button.navigation = nav;
        }
    }

    public void SetVoidMap()
    {
        ClearMaps();
        WorldNode temp;

        temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
        temp.SetUp(WorldType.Void);
        defaultObject = temp.gameObject;
        

        Navigation nav = backButton.navigation;
        nav.selectOnUp = temp.button;
        backButton.navigation = nav;

        nav.selectOnDown = backButton;
        temp.button.navigation = nav;

        worldNodes.Add(temp);

    }

    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(null);
        gameObject.SetActive(false);
        LevelManager.instance.players[pausedIndex].Input.inputState = PlayerInputState.Game;
        pausedIndex = -1;

    }

    public void SelectWorld(WorldNode node)
    {
        Debug.Log(node.worldType + " was selected");
        LevelManager.instance.levelIndex = (int)node.worldType;
        LevelManager.instance.currentWorldIndex = (int)worldNodes.IndexOf(node);
        LevelManager.instance.mMapChangeFlag = true;
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
