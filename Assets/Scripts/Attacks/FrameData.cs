using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FrameType { StartUp, Active, End };

public class FrameData
{
    List<FrameType> frames;
    int frameCount;

    public FrameData()
    {

    }

}
