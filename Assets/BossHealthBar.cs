using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : HealthBar
{
    [SerializeField]
    BossEnemy enemy;

    public void InitHealthbar(BossEnemy e)
    {
        enemy = e;
        if (enemy.mHealth != null)
        {
            enemy.mHealth.SetHealthBar(this);
        }
    }

    public override void SetHealth(Health health)
    {
        healthbar.transform.localScale = new Vector3(health.currentHealth / health.maxHealth, 1);

    }
}
