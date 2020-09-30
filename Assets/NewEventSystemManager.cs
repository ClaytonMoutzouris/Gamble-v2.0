using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewEventSystemManager : MonoBehaviour
{
    public static NewEventSystemManager instance;
    [SerializeField]
    List<EventSystem> eventSystems;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public EventSystem GetEventSystem(int index)
    {
        return eventSystems[index];
    }

}
