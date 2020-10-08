using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffectUI : MonoBehaviour
{

    public StatusEffectUIIcon prefab;
    public List<StatusEffectUIIcon> icons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEffect(StatusEffect statusEffect)
    {
        StatusEffectUIIcon temp = Instantiate(prefab, transform);
        temp.SetEffect(statusEffect);
        icons.Add(temp);
    }

    public void RemoveEffect(StatusEffect statusEffect)
    {
        StatusEffectUIIcon toRemove = null;
        foreach (StatusEffectUIIcon icon in icons)
        {
            if(icon.effect == statusEffect)
            {
                toRemove = icon;
                break;
            }
        }
        icons.Remove(toRemove);
        Destroy(toRemove.gameObject);
    }
}
