using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionManager : MonoBehaviour
{
    [SerializeField] float realScreenHeight = 1;
    [SerializeField] float virtualScreenHeight = 10;

    [SerializeField] Vector3 viewerPosition;
    // Start is called before the first frame update
    void Awake()
    {
        float proportion = virtualScreenHeight / realScreenHeight;

        Vector3 virtualViewerPosition = viewerPosition * proportion;
        transform.position = virtualViewerPosition;
    }

    
}
