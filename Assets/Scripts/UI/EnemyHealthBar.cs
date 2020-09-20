using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : HealthBar
{
    [SerializeField]
    Enemy enemy;
    
    public void InitHealthbar(Enemy e)
    {
        enemy = e;
        if(enemy.mHealth != null)
        {
            enemy.mHealth.SetHealthBar(this);
        }
    }

    public override void SetHealth(Health health)
    {
        healthbar.transform.localScale = new Vector3(health.currentHealth / health.maxHealth, 1);

    }
}
