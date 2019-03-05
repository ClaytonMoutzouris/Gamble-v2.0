using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    public Text health;
    // Start is called before the first frame update
    void Start()
    {
        //healthbar = GetComponentInChildren<Transform>();
        //health = GetComponentInChildren<Text>();
    }

    public override void SetHealth(Health health)
    {
        this.health.text = health.ToString();
        healthbar.transform.localScale = new Vector3(health.currentHealth / health.maxHealth, 1);
    }
}