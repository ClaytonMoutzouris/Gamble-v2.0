using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOptionSelector : CreationOptionSelector
{
    public List<Color> colors;

    // Start is called before the first frame update
    void Start()
    {

        optionIndex = 0;
        numOptions = colors.Count;
        UpdateActiveOption();
    }

    public override void UpdateActiveOption()
    {
        optionDisplay.color = colors[optionIndex];
        creationPanel.SwapColor(colors[optionIndex]);
    }


}
