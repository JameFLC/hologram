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
    // Update is called once per frame
    void Update()
    {
        float heightRatio = (float)Screen.height / Screen.width;
    }
}
