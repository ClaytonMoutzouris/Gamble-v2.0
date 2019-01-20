using UnityEngine;
using UnityEngine.UI;

public class SaveLoadItem : MonoBehaviour
{

    public SaveLoadMenu menu;

    public string MapName
    {
        get
        {
            return mapName;
        }
        set
        {
            mapName = value;
            transform.GetChild(0).GetComponent<Text>().text = value;
        }
    }

    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }

    string mapName;
    string path;

    public void Select()
    {
        menu.SelectItem(this);
    }
}