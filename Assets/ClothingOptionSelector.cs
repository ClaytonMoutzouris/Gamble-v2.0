using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingOptionSelector : CreationOptionSelector
{
    public List<SwapIndex> clothingTypes;

    // Start is called before the first frame update
    void Start()
    {
        clothingTypes = new List<SwapIndex>();
        foreach (SwapIndex classType in System.Enum.GetValues(typeof(SwapIndex)))
        {
            clothingTypes.Add(classType);
        }

        optionIndex = 0;
        numOptions = clothingTypes.Count;
        UpdateActiveOption();
    }

    public override void UpdateActiveOption()
    {
        optionText.text = clothingTypes[optionIndex].ToString();
        creationPanel.swapIndex = clothingTypes[optionIndex];
    }


}
