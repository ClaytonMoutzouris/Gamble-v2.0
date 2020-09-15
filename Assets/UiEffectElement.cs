using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiEffectElement : MonoBehaviour, ISelectHandler
{
    UIEffectContainer container;
    public Button button;
    public Ability ability;
    public Text text;

    public void SetAbility(UIEffectContainer container, Ability ability)
    {
        this.container = container;
        this.ability = ability;
        text.text = ability.abilityName;
    }

    public void OnSelect(BaseEventData eventData)
    {
        container.SetCurrentNode(this);
    }

    public void SelectOption()
    {
    }
}
