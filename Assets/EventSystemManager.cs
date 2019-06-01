using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemManager : MonoBehaviour
{
    public static EventSystemManager instance;
    [SerializeField]
    List<CustomEventSystem> eventSystems;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CustomEventSystem GetEventSystem(int index)
    {
        return eventSystems[index];
    }

}
