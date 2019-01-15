using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthbar;
    public Text health;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponentInChildren<Image>();
        health = GetComponentInChildren<Text>();
    }

    public void SetHealth(Health health)
    {
        this.health.text = health.ToString();
        healthbar.transform.localScale = new Vector3(health.currentHealth / health.maxHealth, 1);
    }

}
