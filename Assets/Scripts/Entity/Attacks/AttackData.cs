using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Fire, Water, Electric, Physical, Poison, Ice }

public class AttackData : ScriptableObject
{
    public int damage;
    public Vector2 offset;
    public DamageType damageType;
    public bool pureDamage = false;

    public int CalculateDamage(Entity entity)
    {
        int damage = this.damage;

        if (entity is Player player)
        {
            damage += player.mStats.GetStat(StatType.Attack).GetValue() / 4;
        }
        else if (entity is Enemy enemy)
        {
            damage += enemy.mStats.GetStat(StatType.Attack).GetValue() / 4;
        }

        return damage;
    }
}
