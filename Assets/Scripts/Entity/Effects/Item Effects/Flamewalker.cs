using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamewalker : Effect
{
    Vector2i prevTile;
    RangedAttackPrototype attackPrototype;
    float tickFrequency = 32;
    Vector2 previousPosition;
    float distanceTravelled = 0;

    public Flamewalker()
    {
        effectName = "Flamewalker";
        type = EffectType.Flamewalker;
        attackPrototype = Resources.Load<RangedAttackPrototype>("Prototypes/Attacks/Effects/GroundFlame");
        //currentPosition = 
    }

    public override void OnWalkTrigger(Player player)
    {
        base.OnWalkTrigger(player);
        Debug.Log("On Walk Trigger");
        distanceTravelled += Vector2.Distance(player.Position, previousPosition);
        previousPosition = player.Position;

        if (distanceTravelled > tickFrequency)
        {
            Projectile shot = new Projectile(attackPrototype.projectilePrototype, new RangedAttack(player, attackPrototype), Vector2.zero);
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
