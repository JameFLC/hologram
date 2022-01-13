using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCameraBounds : MonoBehaviour
{
    private Camera cam;
    
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    // Change virtual screen width to match screen ratio
    void Update()
    {
        float heightRatio = (float)Screen.height / Screen.width;
    }
}
