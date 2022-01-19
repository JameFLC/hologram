using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HoloData
{
    public string bundlePath;
    public float[] holoOffset;
    public float scaleFactor;
    public float YRot;

    public HoloData(string holoPath,Vector3 holoOffset,float scale,float YRotation)
    {
        bundlePath = holoPath;
        this.holoOffset = new float[3];
        this.holoOffset[0] = holoOffset.x;
        this.holoOffset[1] = holoOffset.y;
        this.holoOffset[2] = holoOffset.z;
        scaleFactor = scale;
        YRot = YRotation;
    }
}
