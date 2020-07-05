using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionDrone : Ability
{

    Vector2 offset = new Vector2(4, 10);
    public CompanionDrone()
    {
        abilityName = "Companion Drone";
        type = AbilityType.CompanionDrone;
    }

    public override void OnEquipTrigger(Player player)
    {
        Drone drone;

        drone = new Drone(Resources.Load<DronePrototype>("Prototypes/Entity/Drones/Drone") as DronePrototype);
        drone.owner = player;
        drone.Spawn(player.Position + offset);
    }


}
