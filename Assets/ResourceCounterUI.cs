using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounterUI : MonoBehaviour
{
    public Image icon;
    public Text counter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCounter(int count)
    {
        counter.text = ": "+count;
    }

}
