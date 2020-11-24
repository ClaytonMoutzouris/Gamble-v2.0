using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorIcon : MonoBehaviour
{
    public SpriteRenderer renderer;
    // Start is called before the first frame update

    public void SetIcon(Sprite sprite)
    {
        renderer.sprite = sprite;
    }
}
