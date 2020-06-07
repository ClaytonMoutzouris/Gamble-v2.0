using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterPreviewPortait : MonoBehaviour
{

    public ColorSwap colorSwapper;
    public List<Color> characterColors;
    public Image portrait;
    // Start is called before the first frame update
    void Start()
    {
        portrait = GetComponent<Image>();
        portrait.material = new Material(portrait.material);
        colorSwapper = new ColorSwap(GetComponent<Image>().material);
        colorSwapper.SetBaseColors(characterColors);

    }

    // Update is called once per frame
    void Update()
    {
        //colorSwapper.SetBaseColors(baseColors);

    }

    public List<Color> GetNewPalette()
    {
        List<Color> palette = new List<Color>();

        foreach(Color c in colorSwapper.mBaseColors)
        {
            palette.Add(c);
        }

        return palette;
    }
}
