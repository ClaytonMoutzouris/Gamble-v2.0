using UnityEngine;
using UnityEngine.UI;

public class CreationOptionSelector : MonoBehaviour
{
    public CharacterCreationPanel creationPanel;
    public Image optionDisplay;
    public Text optionText;
    public Button prev;
    public Button next;
    public int optionIndex = 0;
    public int numOptions;

    public void NextOption()
    {
        optionIndex++;
        if(optionIndex >= numOptions)
        {
            optionIndex = 0;
        }
        UpdateActiveOption();

    }

    public void PreviousOption()
    {
        optionIndex--;
        if (optionIndex < 0)
        {
            optionIndex = numOptions-1;
        }
        UpdateActiveOption();
    }

    public virtual void UpdateActiveOption()
    {
    }
}
