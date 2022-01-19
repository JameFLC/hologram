using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraData
{
    public float screenH;
    public float[] cameraPos;

    public CameraData(float screenHeight,Vector3 cameraPosition)
    {
        screenH = screenHeight;
        cameraPos = new float[3];
        
        cameraPos[0] = cameraPosition.x;
        cameraPos[1] = cameraPosition.y;
        cameraPos[2] = cameraPosition.z;

        Debug.Log("CameraData input : " + cameraPosition + ",output : " + cameraPos[0] + " " + cameraPos[1] + " " + cameraPos[2]);
    }
}
