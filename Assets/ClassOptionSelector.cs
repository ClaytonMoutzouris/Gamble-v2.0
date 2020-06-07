using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassOptionSelector : CreationOptionSelector
{
    public List<PlayerClassType> classTypes;

    // Start is called before the first frame update
    void Start()
    {
        classTypes = new List<PlayerClassType>();
        foreach(PlayerClassType classType in System.Enum.GetValues(typeof(PlayerClassType)))
        {
            classTypes.Add(classType);
        }

        optionIndex = 0;
        numOptions = classTypes.Count;
        UpdateActiveOption();
    }

    public override void UpdateActiveOption()
    {
        optionText.text = classTypes[optionIndex].ToString();
        creationPanel.classType = classTypes[optionIndex];
    }


}
