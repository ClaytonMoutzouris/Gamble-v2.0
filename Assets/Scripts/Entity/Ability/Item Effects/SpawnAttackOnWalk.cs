using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttackOnWalk : Ability
{
    Vector2i prevTile;
    public RangedAttackPrototype attackPrototype;
    public float tickFrequency = 32;
    Vector2 previousPosition;
    float distanceTravelled = 0;

    public override void OnWalkTrigger(Player player)
    {
        base.OnWalkTrigger(player);

        distanceTravelled += Vector2.Distance(player.Position, previousPosition);
        previousPosition = player.Position;

        if (distanceTravelled > tickFrequency)
        {
            Projectile shot = new Projectile(attackPrototype.projectilePrototype, new RangedAttack(player, attackPrototype), Vector2.up);
            shot.Spawn(player.Position);
            distanceTravelled = 0;
        }
        /*
        if(player.Map.GetMapTileAtPoint(player.Position) != prevTile)
        {
            Projectile shot = new Projectile(attackPrototype.projectilePrototype, new RangedAttack(player, attackPrototype), Vector2.zero);
            shot.Spawn(player.Map.GetMapTilePosition(player.Map.GetMapTileAtPoint(player.Position)));
        }
        */

        prevTile = player.Map.GetMapTileAtPoint(player.Position);
    }

}
