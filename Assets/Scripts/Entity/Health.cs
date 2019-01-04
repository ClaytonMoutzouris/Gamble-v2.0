using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Health
{
    public float currentHealth;
    public float maxHealth;

    public void LoseHP(float damage)
    {
        Debug.Log("Dealing " + damage + " damage");
        currentHealth -= damage;
        Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void GainHP(float gainz)
    {
        currentHealth += gainz;
        Mathf.Clamp(currentHealth, 0, maxHealth);
    }

}
