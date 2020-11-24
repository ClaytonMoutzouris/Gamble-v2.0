using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{
    public float baseHP;
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthbar;
    public Entity entity;

    public Health(Entity entity, int baseHP, HealthBar bar = null)
    {
        this.entity = entity;
        this.baseHP = baseHP;
        UpdateHealth();

        currentHealth = baseHP;
        maxHealth = baseHP;
        healthbar = bar;
        if(healthbar != null)
        {
            Debug.Log("Setting health to " + currentHealth);
            healthbar.SetHealth(this);
        }


    }

    public void UpdateHealth()
    {
        maxHealth = baseHP;


        maxHealth = baseHP + 10 * entity.mStats.GetStat(StatType.Constitution).GetValue();

        //Debug.Log(entity.Name + " Updating max health: " + maxHealth);

        if (healthbar != null)
        {
            healthbar.SetHealth(this);
        }
    }

    public void SetHealthBar(HealthBar bar)
    {
        this.healthbar = bar;
        bar.SetHealth(this);
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
