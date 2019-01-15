using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : HealthBar
{

    public override void SetHealth(Health health)
    {
        healthbar.transform.localScale = new Vector3(health.currentHealth / health.maxHealth, 1);
    }
}
