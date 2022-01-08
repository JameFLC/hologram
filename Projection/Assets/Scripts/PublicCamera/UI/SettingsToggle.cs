using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    [SerializeField] CameraSetupUI setupScript;

    public void toggleSettings()
    {
        setupScript.Toggle();
    }
}
