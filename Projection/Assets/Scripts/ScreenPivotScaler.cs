using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPivotScaler : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float heightRatio = (float) Screen.width / Screen.height;

        transform.localScale = new Vector3(heightRatio, 1, 1);


    }
}
