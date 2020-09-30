using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NavigationMenu : MonoBehaviour
{
    public static NavigationMenu instance;
    public GameObject defaultObject;
    public int pausedIndex = -1;
    public ScrollRect scrollRect;
    public GameObject worldContainer;
    public WorldNode prefab;
    public List<WorldNode> worldNodes = new List<WorldNode>();
    public Button backButton;
    public Text worldName;
    public Text worldInfo;

    public WorldNode currentNode;

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


    private void Update()
    {
        if (currentNode != null)
        {
            scrollRect.ScrollRepositionX(currentNode.GetComponent<RectTransform>());
            Navigation nav = backButton.navigation;
            nav.selectOnUp = currentNode.button;
            backButton.navigation = nav;
        }

    }

    public void Open(int playerIndex)
    {
        if(pausedIndex != -1)
        {
            return;
        }

        pausedIndex = playerIndex;

        if (worldNodes.Count > 0)
        {
            currentNode = worldNodes[0];
            SetWorldInfo();
            defaultObject = currentNode.gameObject;
        }


        if (defaultObject != null)
        {
            Button button = defaultObject.GetComponent<Button>();
            if(button != null)
            {
                button.OnSelect(null);

            }
        }

        GamepadInputManager.instance.gamepadInputs[pausedIndex].GetEventSystem().SetSelectedGameObject(defaultObject);

        gameObject.SetActive(true);
        CrewManager.instance.players[playerIndex].Input.inputState = PlayerInputState.NavigationMenu;


    }

    public void ClearMaps()
    {
        foreach(WorldNode world in worldNodes)
        {
            Destroy(world.gameObject);
        }

        worldNodes.Clear();
    }


    public void SetWorldInfo()
    {
        worldName.text = currentNode.world.worldName;
        if(currentNode.completed)
        {
            worldName.color = Color.green;
        }
        else
        {
            worldName.color = Color.white;
        }

        worldInfo.text = "Type: " + currentNode.world.WorldType;
        
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


    }

    public void Close()
    {
        GamepadInputManager.instance.gamepadInputs[pausedIndex].GetEventSystem().SetSelectedGameObject(null);
        gameObject.SetActive(false);
        CrewManager.instance.players[pausedIndex].Input.inputState = PlayerInputState.Game;
        pausedIndex = -1;

    }

    public void SelectWorld(WorldNode node)
    {
        Debug.Log(node + " was selected");

        WorldManager.instance.SelectWorld(worldNodes.IndexOf(node));
        Close();
    }

}
