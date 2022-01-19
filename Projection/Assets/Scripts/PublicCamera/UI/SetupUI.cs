using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Text.RegularExpressions;
public class SetupUI : MonoBehaviour
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
    [SerializeField] private InputField IFHoloYRot;

    [SerializeField] float virtualScreenHeight = 10;
    [SerializeField] Transform publicCamera;
    [SerializeField] Transform hologramOrigin;
    [SerializeField] Cameramap cameramap;
    [SerializeField] ImportManager importManager;


    private Vector3 cameraPosition = new Vector3(0,0.5f,-1.5f);
    
    
    private Vector3 holoOffset = new Vector3(0,0,0);
    private float holoYRotation = 0;
    private float holoScale = 1;
    private CanvasGroup UICanvasGroup;
    private bool isUIVisible = false;
    private float screenHeight = 1;
    private float proportion = 1;
    




    // Start is called before the first frame update
    void Awake()
    {
        UICanvasGroup = GetComponent<CanvasGroup>();
        Hide();



    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Toggle();
        }
        if (Input.GetButtonDown("LoadCam"))
        {
            LoadCameraData();
        }
        if (Input.GetButtonDown("LoadHolo1"))
        {
            LoadHologramData(0);
        }
        if (Input.GetButtonDown("LoadHolo2"))
        {
            LoadHologramData(1);
        }
        if (Input.GetButtonDown("LoadHolo3"))
        {
            LoadHologramData(2);
        }
    }

    public void Show()
    {
        UICanvasGroup.interactable = isUIVisible;
        UICanvasGroup.alpha = 1;
        UICanvasGroup.blocksRaycasts = true;
        isUIVisible = true;
    }
    public void Hide()
    {
        UICanvasGroup.interactable = isUIVisible;
        UICanvasGroup.alpha = 0;
        UICanvasGroup.blocksRaycasts = false;
        isUIVisible = false;
    }
    public void Toggle()
    {
        isUIVisible = !isUIVisible;
        if (isUIVisible)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    
    public void SetScreenHeight()
    {
        if (!string.IsNullOrWhiteSpace(IFScreenHeight.text))
        {
            screenHeight = SanetizeInput(IFScreenHeight.text) / 100;
            UpdateScreenHeight();
        }
    }
    private void UpdateScreenHeight()
    {
        IFScreenHeight.text = distanceDisplay(screenHeight);
        UpdateCameraPosition();
    }


    public void SetViewerXOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetX.text))
        {
            cameraPosition.x = SanetizeInput(IFViewerOffsetX.text) / 100;
            UpdateViewerXOffset();
        }
    }
    private void UpdateViewerXOffset()
    {
        IFViewerOffsetX.text = distanceDisplay(cameraPosition.x);

        UpdateCameraPosition();
    }



    public void SetViewerYOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetY.text))
        {
            cameraPosition.y = SanetizeInput(IFViewerOffsetY.text) / 100;
            UpdateViewerYOffset();
        }
    }
    private void UpdateViewerYOffset()
    {
        IFViewerOffsetY.text = distanceDisplay(cameraPosition.y);

        UpdateCameraPosition();
    }

    public void SetViewerZOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetZ.text))
        {
            // Invert z to make more sense to the user (since camera is in front of the screen, its z coordinate are negative)
            cameraPosition.z = -SanetizeInput(IFViewerOffsetZ.text) / 100;
            UpdateViewerZOffset();
        }
    }

    private void UpdateViewerZOffset()
    {
        IFViewerOffsetZ.text = distanceDisplay(-cameraPosition.z);

        UpdateCameraPosition();
    }

    private void UpdateViewerOffset()
    {
        UpdateViewerXOffset();
        UpdateViewerYOffset();
        UpdateViewerZOffset();
    }

    public void SaveCameraData()
    {
        Debug.Log("camData Save : screen height = " + screenHeight + " Cam position = " + cameraPosition.x + " " + cameraPosition.y + " " + " " + cameraPosition.z);

        SaveManager.SaveCameraData(screenHeight, cameraPosition);
    }

    public void LoadCameraData()
    {
        if (SaveManager.isCameraSaved())
        {
            CameraData camData = SaveManager.LoadCameraData();
            Debug.Log("camData Loaded : screen height = " + camData.screenH + " Cam position = " + camData.cameraPos[0] + " " + camData.cameraPos[1] + " " + " " + camData.cameraPos[2]);
            screenHeight = camData.screenH;
            UpdateScreenHeight();

            cameraPosition = new Vector3(camData.cameraPos[0], camData.cameraPos[1], camData.cameraPos[2]);
            UpdateViewerOffset();
        }
    }

    public void SetHoloScale()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloScale.text))
        {
            holoScale = SanetizeInput(IFHoloScale.text);
            UpdateHoloScale();
        }
    }
    private void UpdateHoloScale()
    {
        hologramOrigin.localScale = new Vector3(holoScale, holoScale, holoScale);
        IFHoloScale.text = hologramOrigin.localScale.x.ToString();
    }
    public void SetHoloXOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetX.text))
        {
            holoOffset.x = SanetizeInput(IFHoloOffsetX.text) / 100;
            UpdateHoloOffset();
        }
    }
    private void UpdateHoloOffset()
    {
        IFHoloOffsetX.text = distanceDisplay(holoOffset.x);

        hologramOrigin.position = holoOffset;
    }

    public void SetHoloYOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetY.text))
        {
            holoOffset.y = SanetizeInput(IFHoloOffsetY.text) / 100;
            UpdateHoloYOffset();
        }
    }
    private void UpdateHoloYOffset()
    {
        IFHoloOffsetY.text = distanceDisplay(holoOffset.y);

        hologramOrigin.position = holoOffset;
    }

    public void SetHoloZOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetZ.text))
        {
            holoOffset.z = -SanetizeInput(IFHoloOffsetZ.text) / 100;
            UpdateHoloZOffset();
        }
    }
    private void UpdateHoloZOffset()
    {
        IFHoloOffsetZ.text = distanceDisplay(-holoOffset.z);

        hologramOrigin.position = holoOffset;
    }

    public void SetYRotation()
    {
        if (!string.IsNullOrWhiteSpace(IFHoloYRot.text))
        {
            holoYRotation = SanetizeInput(IFHoloYRot.text);
            UpdateYRotation();
        }
    }
    private void UpdateYRotation()
    {
        IFHoloYRot.text = holoYRotation + " °";

        hologramOrigin.rotation = Quaternion.Euler(hologramOrigin.rotation.x, holoYRotation, hologramOrigin.rotation.z);
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


    public void SaveHologramData(int saveID)
    {
        SaveManager.SaveHologram(importManager.GetHologramPath(), holoOffset, hologramOrigin.localScale.x, holoYRotation, saveID);
    }

    public void LoadHologramData(int saveID)
    {
        if (SaveManager.isHologramSaved(saveID))
        {
            HoloData holoData = SaveManager.LoadHologramData(saveID);

            importManager.LoadHologram(holoData.bundlePath);
            holoOffset = new Vector3(holoData.holoOffset[0], holoData.holoOffset[1], holoData.holoOffset[2]);
            holoScale = holoData.scaleFactor;
            holoYRotation = holoData.YRot;
            UpdateHoloOffset();
            UpdateHoloScale();
            UpdateYRotation();
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

    public void SetRenderScale()
    {
        if (!string.IsNullOrEmpty(IFRenderScale.text))
        {

            cameramap.RenderScale = SanetizeInput(IFRenderScale.text) / 100;
            string upscaleText = (cameramap.RenderScale * 100).ToString();
            IFRenderScale.text = upscaleText + " %";

        }
    }
}
