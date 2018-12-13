using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity {

    private PhysicsObject body;

    #region Accesors
    public PhysicsObject Body
    {
        get
        {
            return body;
        }

        set
        {
            body = value;
        }
    }

    #endregion

    public Entity()
    {

    }

    public void Spawn()
    {
        
    }

    public void CustomUpdate()
    {

    }


}

