using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MinimapIconType { Player, Door, Shop, Boss, Chest };

public class MiniMapIcon : MonoBehaviour
{
    public Image image;
    public MinimapIconType type = MinimapIconType.Player;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        switch (type)
        {
            case MinimapIconType.Player:
                image.color = Color.red;
                break;
            case MinimapIconType.Door:
                image.color = Color.blue;
                break;
            case MinimapIconType.Chest:
                image.color = Color.yellow;
                break;
            case MinimapIconType.Boss:
                image.color = Color.magenta;
                break;
        }
    }

    public void UpdateIcon(Vector2i tilePos)
    {
        transform.localPosition = MiniMap.TileToMinimap(tilePos);
    }




}
