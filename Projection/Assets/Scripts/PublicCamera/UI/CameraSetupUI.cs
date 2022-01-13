using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Text.RegularExpressions;
public class CameraSetupUI : MonoBehaviour
{
    [SerializeField] private InputField IFScreenHeight;
    [SerializeField] private InputField IFViewerOffsetX;
    [SerializeField] private InputField IFViewerOffsetY;
    [SerializeField] private InputField IFViewerOffsetZ;


    [SerializeField] float virtualScreenHeight = 10;
    [SerializeField] Transform publicCamera;

    private Vector3 cameraPosition = new Vector3(0,0.5f,-1.5f);
    private float screenHeight = 1;
    private float proportion = 1;
    // Start is called before the first frame update
    void Awake()
    {
        Hide();

        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    
    public void SetScreenHeight()
    {
        if (!string.IsNullOrWhiteSpace(IFScreenHeight.text))
        {
            screenHeight = SanetizeInput(IFScreenHeight.text);
            IFScreenHeight.text = distanceDisplay(screenHeight);
            UpdateCameraPosition();
        }
    }
    public void SetViwerXOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetX.text))
        {
            cameraPosition.x = SanetizeInput(IFViewerOffsetX.text);
            IFViewerOffsetX.text = distanceDisplay(cameraPosition.x);

            UpdateCameraPosition();
        }
        
    }

    public void SetViwerYOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetY.text))
        {
            cameraPosition.y = SanetizeInput(IFViewerOffsetY.text);
            IFViewerOffsetY.text = distanceDisplay(cameraPosition.y);

            UpdateCameraPosition();
        }
    }
    public void SetViwerZOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetX.text))
        {
            // Invert z to make more sense to the user (since camera is in front of the screen, its z coordinate are negative)
            cameraPosition.z = -SanetizeInput(IFViewerOffsetZ.text);
            IFViewerOffsetZ.text = distanceDisplay(-cameraPosition.z);

            UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {
        if (screenHeight != 0)
        {
            proportion = virtualScreenHeight / screenHeight;

            publicCamera.position = (cameraPosition * proportion);
        }
        else
        {
            Debug.Log("Invalid Screen Height");
        }
        
    }

    // Clean string input and output float
    private float SanetizeInput(string str)
    {
        // Remove all non float usefull character
        str = Regex.Replace(str, "[^0-9,.+-]+", "");

        // Replace , by . to perform parse
        str = Regex.Replace(str, "[,]+", ".");
        // Check str to not parse null string
        
        float result;
        float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        // Convert CM in M
        result /= 100;

        return result;
        
    }
    private string distanceDisplay(float distance)
    {
        distance *= 100;
        string text = distance.ToString("F1");
        text += " cm";
        return text;
    }
}
