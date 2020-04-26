using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    public Color color;
    public SwapIndex colorIndex;

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        //player.Renderer.colorSwapper.SwapColor(colorIndex, color);
        //player.mAttackManager.AttackList[0] = attack;
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        //player.Renderer.colorSwapper.SwapIndexToBase(colorIndex);

    }

    public override void RandomizeStats()
    {
        base.RandomizeStats();
        color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}
