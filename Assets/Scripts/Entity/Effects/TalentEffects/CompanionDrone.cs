using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionDrone : Effect
{

    Vector2 offset = new Vector2(4, 10);
    public CompanionDrone()
    {
        effectName = "Companion Drone";
        type = EffectType.CompanionDrone;
    }

    public override void OnLearned(Player player)
    {
        Drone drone;

        drone = new Drone(Resources.Load<DronePrototype>("Prototypes/Entity/Drones/Drone") as DronePrototype);
        drone.owner = player;
        drone.Spawn(player.Position + offset);
    }


}
