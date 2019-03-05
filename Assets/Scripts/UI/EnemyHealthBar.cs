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
    }

    public override void SetHealth(Health health)
    {
        healthbar.transform.localScale = new Vector3(health.currentHealth / health.maxHealth, 1);
        switch (enemy.Hostility)
        {
            case Hostility.Friendly:
                healthbar.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case Hostility.Neutral:
                healthbar.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case Hostility.Hostile:
                healthbar.GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
    }
}
