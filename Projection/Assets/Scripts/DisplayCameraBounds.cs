using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCameraBounds : MonoBehaviour
{
    [SerializeField]
    private float screenBaseSize = 5;
    
    private Camera cam;
    
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        float heightRatio = (float)Screen.height / Screen.width;

        
       // camera.orthographicSize = screenBaseSize * heightRatio;
       // transform.localPosition = new Vector3 (0,screenBaseSize * heightRatio,-1.0f);
    }
}
