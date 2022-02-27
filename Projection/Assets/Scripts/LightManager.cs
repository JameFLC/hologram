using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Transform origin;
    public void UpdateLightWhileScaling(float newScale, float oldScale, Transform origin)
    {
        float lightMultiplier = (1.0f * newScale) / (oldScale);
        Debug.LogWarning("Ligth power boosted by " + lightMultiplier);
        SetupLight(origin, 0, lightMultiplier);
    }

    // Find all light in hologram and change all intensities and ranges to match hologram scale
    public void SetupLight(Transform target, int currentRecursion, float lightMultiplier)
    {
        int MaxRecurtion = 200;
        if (currentRecursion < MaxRecurtion)
        {

            foreach (Transform item in target)
            {

                SetupLight(item, currentRecursion + 1, lightMultiplier);

            }
            Light light = target.GetComponent<Light>();

            if (light == null)
                return;

            Debug.LogWarning("Multiplied light values of " + target + " to " + lightMultiplier);

            light.range *= lightMultiplier;
        }
        else
        {
            Debug.LogError("Recurtion level greater than " + MaxRecurtion);
        }
    }
}
