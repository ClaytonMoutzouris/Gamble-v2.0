using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Health
{
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthbar;

    public float LoseHP(float damage)
    {
        Debug.Log("Dealing " + damage + " damage");
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
