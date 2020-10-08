using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUIIcon : MonoBehaviour
{

    public Image image;
    public StatusEffect effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEffect(StatusEffect effect)
    {
        this.effect = effect;
        image.sprite = effect.icon;
    }
}
