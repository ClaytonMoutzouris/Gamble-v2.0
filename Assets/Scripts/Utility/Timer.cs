using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public float duration;

    public Timer(float duration)
    {
        this.duration = duration;
    }

    // Update is called once per frame
    public void Update()
    {
        duration -= Time.deltaTime;
    }

    public bool Expired()
    {
        return (duration <= 0);
    }
}
