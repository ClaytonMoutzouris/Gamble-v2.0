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

        RollMaps(Random.Range(3, 5));


        

    }

    public void Open(int playerIndex)
    {
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


    public void RollMaps(int numMaps)
    {

        ClearMaps();

        WorldNode temp;
        for (int i = 0; i < numMaps; i++)
        {
            temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
            temp.SetUp((WorldType)Random.Range(0, (int)WorldType.Count));
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
    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(null);
        gameObject.SetActive(false);
        LevelManager.instance.players[pausedIndex].Input.inputState = PlayerInputState.Game;
        pausedIndex = -1;

    }

    public void SelectWorld(WorldType type)
    {
        Debug.Log(type + " was selected");
        LevelManager.instance.levelIndex = (int)type;
        LevelManager.instance.mMapChangeFlag = true;
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
