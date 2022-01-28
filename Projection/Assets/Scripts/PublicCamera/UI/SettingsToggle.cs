using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    [SerializeField] SetupUI setupScript;

    public void toggleSettings()
    {
        setupScript.ToggleUI();
    }
}
