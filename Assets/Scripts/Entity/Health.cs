using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public float baseHP;
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthbar;
    public Entity entity;

    public Health(Entity entity, int hp, HealthBar bar = null)
    {
        this.entity = entity;
        baseHP = hp;
        currentHealth = hp;
        maxHealth = hp;
        healthbar = bar;
        if(healthbar != null)
        {
            Debug.Log("Setting health to " + currentHealth);
            healthbar.SetHealth(this);
        }

        UpdateHealth();

    }

    public void UpdateHealth()
    {
        maxHealth = baseHP;
        if (entity is Player player)
        {
            maxHealth = baseHP + 10 * player.mStats.getStat(StatType.Constitution).GetValue();

        }
        //Debug.Log(entity.Name + " Updating max health: " + maxHealth);

        if (healthbar != null)
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
