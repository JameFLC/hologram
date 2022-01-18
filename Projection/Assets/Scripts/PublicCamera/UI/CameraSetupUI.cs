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

    [SerializeField] private InputField IFRenderScale;

    [SerializeField] private InputField IFHoloScale;
    [SerializeField] private InputField IFHoloOffsetX;
    [SerializeField] private InputField IFHoloOffsetY;
    [SerializeField] private InputField IFHoloOffsetZ;


    [SerializeField] float virtualScreenHeight = 10;
    [SerializeField] Transform publicCamera;
    [SerializeField] Transform hologramOrigin;
    [SerializeField] Cameramap cameramap;

    private Vector3 cameraPosition = new Vector3(0,0.5f,-1.5f);
    private Vector3 holoOffset = new Vector3(0,0,0);

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
            screenHeight = SanetizeInput(IFScreenHeight.text) / 100;
            IFScreenHeight.text = distanceDisplay(screenHeight);
            UpdateCameraPosition();
        }
    }
    public void SetViwerXOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetX.text))
        {
            cameraPosition.x = SanetizeInput(IFViewerOffsetX.text) / 100;
            IFViewerOffsetX.text = distanceDisplay(cameraPosition.x);

            UpdateCameraPosition();
        }
    }

    public void SetViwerYOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetY.text))
        {
            cameraPosition.y = SanetizeInput(IFViewerOffsetY.text) / 100;
            IFViewerOffsetY.text = distanceDisplay(cameraPosition.y);

            UpdateCameraPosition();
        }
    }
    public void SetViwerZOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetZ.text))
        {
            // Invert z to make more sense to the user (since camera is in front of the screen, its z coordinate are negative)
            cameraPosition.z = -SanetizeInput(IFViewerOffsetZ.text) / 100;
            IFViewerOffsetZ.text = distanceDisplay(-cameraPosition.z);

            UpdateCameraPosition();
        }
    }

    public void SetRenderScale()
    {
        if (!string.IsNullOrEmpty(IFRenderScale.text))
        {

            cameramap.RenderScale = SanetizeInput(IFRenderScale.text)/100;
            string upscaleText = (cameramap.RenderScale*100).ToString();
            IFRenderScale.text = upscaleText + " %";
            Debug.Log("BogosBinted " + cameramap.RenderScale);
        }
    }


    public void SetHoloScale()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloScale.text))
        {
            float scale = SanetizeInput(IFHoloScale.text);
            hologramOrigin.localScale = new Vector3(scale, scale, scale);
        }
    }
    public void SetHoloXOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetX.text))
        {
            holoOffset.x = SanetizeInput(IFHoloOffsetX.text) / 100;
            IFHoloOffsetX.text = distanceDisplay(holoOffset.x);

            hologramOrigin.position = holoOffset;
        }
    }
    public void SetHoloYOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetY.text))
        {
            holoOffset.y = SanetizeInput(IFHoloOffsetY.text) / 100;
            IFHoloOffsetY.text = distanceDisplay(holoOffset.y);

            hologramOrigin.position = holoOffset;
        }
    }
    public void SetHoloZOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetZ.text))
        {
            holoOffset.z = -SanetizeInput(IFHoloOffsetZ.text) / 100;
            IFHoloOffsetZ.text = distanceDisplay(-holoOffset.z);

            hologramOrigin.position = holoOffset;
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


        return result;
        
    }
    private string distanceDisplay(float distance)
    {
        distance *= 100;
        string text = distance.ToString();
        text += " cm";
        return text;
    }
}
