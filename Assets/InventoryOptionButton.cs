using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryOptionButton : MonoBehaviour
{
    InventoryOptionList list;
    public Button button;
    public Text text;
    public InventoryOption option;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetOption(InventoryOption o, InventoryOptionList list)
    {
        this.list = list;
        option = o;
        text.text = option.ToString();
    }

    public void SelectOption()
    {
        list.OptionSelected(option);
    }

}
