using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    public Transform healthbar;
    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual void SetHealth(Health health)
    {

    }

}


