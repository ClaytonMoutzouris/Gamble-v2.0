﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthbar;


    public Health(int hp, HealthBar bar = null)
    {
        currentHealth = hp;
        maxHealth = hp;
        healthbar = bar;
        if(healthbar != null)
        {
            healthbar.SetHealth(this);
        }
    }

    public void SetHealthBar(HealthBar bar)
    {
        this.healthbar = bar;
    }

    public float LoseHP(float damage)
    {
        //Debug.Log("Dealing " + damage + " damage");
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if(healthbar != null)
        {
            healthbar.SetHealth(this);
        }

        return damage;
    }

    public float GainHP(float gainz)
    {
        currentHealth = Mathf.Clamp(currentHealth + gainz, 0, maxHealth);
        if (healthbar != null)
        {
            healthbar.SetHealth(this);
        }

        return gainz;
    }

    public override string ToString()
    {
        return currentHealth + " / " + maxHealth;
    }


}
