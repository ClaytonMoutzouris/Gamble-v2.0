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


        //SetMaps();
        //RollMaps();


        //LevelManager.instance.completedWorlds = new bool[worldNodes.Count];
        defaultObject = backButton.gameObject;


    }

    public void Open(int playerIndex)
    {

        pausedIndex = playerIndex;
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(defaultObject);
        defaultObject.GetComponent<Button>().OnSelect(null);
        gameObject.SetActive(true);
        GameManager.instance.players[playerIndex].Input.inputState = PlayerInputState.NavigationMenu;


    }

    public void ClearMaps()
    {
        foreach(WorldNode world in worldNodes)
        {
            Destroy(world.gameObject);
        }

        worldNodes.Clear();
    }

    public void AddWorldNode(World world)
    {

        WorldNode temp;

        temp = Instantiate<WorldNode>(prefab, worldContainer.transform);
        temp.SetUp(world);
        worldNodes.Add(temp);
        Navigation nav;

        if (worldNodes.Count == 1)
        {


            nav = backButton.navigation;
            nav.selectOnUp = worldNodes[0].button;
            backButton.navigation = nav;
            nav = temp.button.navigation;
            nav.selectOnDown = backButton;
            temp.button.navigation = nav;

        } else
        {
            nav = temp.button.navigation;

            nav.selectOnLeft = worldNodes[worldNodes.Count - 2].button;
            nav.selectOnRight = worldNodes[0].button;
            nav.selectOnDown = backButton;
            temp.button.navigation = nav;

            Navigation nav2 = worldNodes[worldNodes.Count - 2].button.navigation;
            nav2.selectOnRight = temp.button;
            worldNodes[worldNodes.Count - 2].button.navigation = nav2;

            Navigation nav3 = worldNodes[0].button.navigation;
            nav3.selectOnLeft = temp.button;
            worldNodes[0].button.navigation = nav3;
        }

        defaultObject = temp.gameObject;

    }

    public void SetDefaultNode()
    {
        
    }

    public void Close()
    {
        EventSystemManager.instance.GetEventSystem(pausedIndex).SetSelectedGameObject(null);
        gameObject.SetActive(false);
        GameManager.instance.players[pausedIndex].Input.inputState = PlayerInputState.Game;
        pausedIndex = -1;

    }

    public void SelectWorld(WorldNode node)
    {
        Debug.Log(node + " was selected");

        WorldManager.instance.SelectWorld(worldNodes.IndexOf(node));
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
