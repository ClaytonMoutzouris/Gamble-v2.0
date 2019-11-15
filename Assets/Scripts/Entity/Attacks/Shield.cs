using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield
{

    public Entity owner;
    public Blockbox blockbox;
    
    public Shield(Entity o, Blockbox b)
    {
        owner = o;
        blockbox = b;
    }

    public void UpdateShield()
    {

    }

}
