using UnityEngine;
using UnityEngine.UI;


public class WorldNode : MonoBehaviour
{

    public WorldType worldType;
    public Image image;
    public int worldID;
    public Button button;
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetUp(WorldType type)
    {

        worldType = type;
        SetColor();
    }

    public void SetColor()
    {
        switch (worldType)
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

        }
    }

    public void SelectWorld()
    {
        NavigationMenu.instance.SelectWorld(worldType);
    }
}
