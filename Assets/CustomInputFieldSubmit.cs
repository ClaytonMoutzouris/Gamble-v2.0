using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomInputFieldSubmit : MonoBehaviour
{
    public InputField inputField;
    private bool wasFocused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        wasFocused = inputField.isFocused;
    }
}
