using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectContainer : MonoBehaviour
{
    public List<UiEffectElement> effectList;
    public UiEffectElement prefab;
    public GameObject container;
    public UiEffectElement currentNode;
    ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        effectList = new List<UiEffectElement>();
        scrollRect = GetComponent<ScrollRect>();
    }

    private void Update()
    {
        if (currentNode != null)
        { 
            scrollRect.ScrollRepositionY(currentNode.GetComponent<RectTransform>());
        }
    }

    public void AddEffect(Ability ability)
    {
        UiEffectElement element = Instantiate(prefab, container.transform) as UiEffectElement;
        element.SetAbility(this, ability);
        effectList.Add(element);

    }

    public void RemoveEffect(Ability ability)
    {
        foreach(UiEffectElement element in effectList)
        {
            if(element.ability == ability)
            {
                effectList.Remove(element);
                Destroy(element.gameObject);
                return;
            }
        }

    }

    public void SetCurrentNode(UiEffectElement node)
    {
        currentNode = node;
    }
}
