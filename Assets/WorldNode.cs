using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WorldNode : MonoBehaviour, ISelectHandler
{

    public World world;
    public Image image;
    public int worldID;
    public Button button;
    public bool completed = false;
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetUp(World world)
    {
        world.WorldNode = this;
        this.world = world;
        SetColor();
    }

    public void SetCleared()
    {
        completed = true;
    }

    public void SetColor()
    {
        switch (world.WorldType)
        {
            case WorldType.Forest:
                image.color = Color.green;
                break;
            case WorldType.Tundra:
                image.color = Color.cyan;
                break;
            case WorldType.Lava:
                image.color = Color.red;
                break;
            case WorldType.Purple:
                image.color = Color.magenta;
                break;
            case WorldType.Yellow:
                image.color = Color.yellow;
                break;
            case WorldType.Void:
                image.color = Color.grey;
                break;

        }
    }

    public void SelectWorld()
    {
        if(completed)
        {
            return;
        }

        NavigationMenu.instance.SelectWorld(this);
    }

    public void OnSelect(BaseEventData eventData)
    {
        NavigationMenu.instance.currentNode = this;
        NavigationMenu.instance.SetWorldInfo();
    }
}
