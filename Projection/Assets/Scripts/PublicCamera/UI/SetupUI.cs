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
    [SerializeField] private InputField IFXSensPort;
    [SerializeField] private InputField IFHoloScale;
    [SerializeField] private InputField IFHoloOffsetX;
    [SerializeField] private InputField IFHoloOffsetY;
    [SerializeField] private InputField IFHoloOffsetZ;
    [SerializeField] private InputField IFHoloYRot;

    [SerializeField] float virtualScreenHeight = 10;
    [SerializeField] Transform publicCamera;
    [SerializeField] Transform hologramOrigin;
    //[SerializeField] Transform hologramSetupOrigin;
    [SerializeField] Cameramap cameramap;
    [SerializeField] ImportManager importManager;
    [SerializeField] xsens.XsStreamReader streamReader;
    [SerializeField] MocapManager mocapManager;
    [SerializeField] LightManager lightManager;
    private Vector3 cameraPosition = new Vector3(0, 0.5f, -1.5f);


    private Vector3 holoOffset = new Vector3(0, 0, 0);
    private float holoYRotation = 0;
    private float holoScale = 1;
    private float oldHoloScale = 1;
    private CanvasGroup UICanvasGroup;
    private bool isUIVisible = false;
    private float screenHeight = 1;
    private float proportion = 1;
    [HideInInspector]
    public bool isArtNetEnabled = false;
    [HideInInspector]
    public bool isMoCapEnabled = false;




    // Start is called before the first frame update
    void Awake()
    {
        UICanvasGroup = GetComponent<CanvasGroup>();
        Hide();



    }

    private void Update()
    {
        // Check to switch ui visbility
        if (Input.GetButtonDown("Cancel"))
        {
            ToggleUI();
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
        if (Input.GetButtonDown("ToggleArtNet"))
        {
            isArtNetEnabled = !isArtNetEnabled;
        }
        if (Input.GetButtonDown("ToggleMocap"))
        {
            isMoCapEnabled = !isMoCapEnabled;
            Debug.Log("Mocape enabled = " + isMoCapEnabled);
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
    public void ToggleUI()
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
            UpdateScreenHeight(SanetizeInput(IFScreenHeight.text) / 100);
        }
    }
    public void UpdateScreenHeight(float height)
    {
        screenHeight = height;
        IFScreenHeight.text = distanceDisplay(screenHeight);
        UpdateCameraPosition();
    }


    public void SetViewerXOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetX.text))
        {
            UpdateViewerXOffset(SanetizeInput(IFViewerOffsetX.text) / 100);
        }
    }
    public void UpdateViewerXOffset(float posX)
    {
        cameraPosition.x = posX;
        IFViewerOffsetX.text = distanceDisplay(cameraPosition.x);
        UpdateCameraPosition();
    }



    public void SetViewerYOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetY.text))
        {
            UpdateViewerYOffset(SanetizeInput(IFViewerOffsetY.text) / 100);
        }
    }
    public void UpdateViewerYOffset(float posY)
    {
        cameraPosition.y = posY;
        IFViewerOffsetY.text = distanceDisplay(cameraPosition.y);

        UpdateCameraPosition();
    }

    public void SetViewerZOffset()
    {
        if (!string.IsNullOrWhiteSpace(IFViewerOffsetZ.text))
        {
            // Invert z to make more sense to the user (since camera is in front of the screen, its z coordinate are negative)
            UpdateViewerZOffset(-SanetizeInput(IFViewerOffsetZ.text) / 100);
        }
    }

    public void UpdateViewerZOffset(float posZ)
    {
        cameraPosition.z = posZ;
        IFViewerOffsetZ.text = distanceDisplay(-cameraPosition.z);
        UpdateCameraPosition();
    }

    private void UpdateViewerOffset(Vector3 viewerOffset)
    {
        UpdateViewerXOffset(viewerOffset.x);
        UpdateViewerYOffset(viewerOffset.y);
        UpdateViewerZOffset(viewerOffset.z);
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
            UpdateScreenHeight(camData.screenH);

            UpdateViewerOffset(new Vector3(camData.cameraPos[0], camData.cameraPos[1], camData.cameraPos[2]));
        }
    }

    public void SetXSensPort()
    {
        if (!string.IsNullOrWhiteSpace(IFXSensPort.text))
        {
            UpdateXSensPort(Mathf.RoundToInt(SanetizeInput(IFXSensPort.text)));
        }
    }
    public void UpdateXSensPort(int port)
    {
        streamReader.listenPort = Mathf.Clamp(port, 0, 9999);
    }
    public void SetHoloScale()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloScale.text))
        {
            UpdateHoloScale(SanetizeInput(IFHoloScale.text));
        }
    }



    public void UpdateHoloScale(float scale)
    {
        SetNewHoloScale(scale);
        IFHoloScale.text = scale.ToString();
        lightManager.UpdateLightWhileScaling(holoScale, oldHoloScale, hologramOrigin);
    }

    public void SetNewHoloScale(float scale)
    {
        if (scale == 0)
        {
            oldHoloScale = holoScale;
            holoScale = 0.001f;
            hologramOrigin.localScale = new Vector3(holoScale, holoScale, holoScale);
        }
        else
        {
            oldHoloScale = holoScale;
            holoScale = scale;
            hologramOrigin.localScale = new Vector3(holoScale, holoScale, holoScale);
            IFHoloScale.text = hologramOrigin.localScale.x.ToString();
        }
        mocapManager.ScaleMocap(hologramOrigin, 0, holoScale);

    }

    


    public void SetHoloXOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetX.text))
        {
            UpdateHoloXOffset(SanetizeInput(IFHoloOffsetX.text) / 100);
        }
    }
    public void UpdateHoloXOffset(float posX)
    {
        holoOffset.x = posX;
        IFHoloOffsetX.text = distanceDisplay(holoOffset.x);

        hologramOrigin.position = holoOffset;
    }

    public void SetHoloYOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetY.text))
        {

            UpdateHoloYOffset(SanetizeInput(IFHoloOffsetY.text) / 100);
        }
    }
    public void UpdateHoloYOffset(float posY)
    {
        holoOffset.y = posY;
        IFHoloOffsetY.text = distanceDisplay(holoOffset.y);

        hologramOrigin.position = holoOffset;
    }

    public void SetHoloZOffset()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloOffsetZ.text))
        {

            UpdateHoloZOffset(-SanetizeInput(IFHoloOffsetZ.text) / 100);
        }
    }
    public void UpdateHoloZOffset(float posZ)
    {
        holoOffset.z = posZ;
        IFHoloOffsetZ.text = distanceDisplay(-holoOffset.z);

        hologramOrigin.position = holoOffset;
    }
    public void UpdateHoloOffset(Vector3 offset)
    {
        UpdateHoloXOffset(offset.x);
        UpdateHoloXOffset(offset.y);
        UpdateHoloXOffset(offset.z);
    }
    public void SetYRotation()
    {
        if (!string.IsNullOrWhiteSpace(IFHoloYRot.text))
        {

            UpdateYRotation(SanetizeInput(IFHoloYRot.text));
        }
    }
    public void UpdateYRotation(float rotY)
    {
        holoYRotation = rotY;
        IFHoloYRot.text = holoYRotation + " °";

        hologramOrigin.rotation = Quaternion.Euler(hologramOrigin.rotation.x, holoYRotation, hologramOrigin.rotation.z);

    }



    public void UpdateCameraPosition()
    {
        if (screenHeight != 0)
        {
            proportion = virtualScreenHeight / screenHeight;

            publicCamera.localPosition = (cameraPosition * proportion);
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
            SetNewHoloScale(1);
            HoloData holoData = SaveManager.LoadHologramData(saveID);
            importManager.LoadHologram(holoData.bundlePath , holoData.scaleFactor);
            Debug.LogError("Holoscale before update = " + hologramOrigin.localScale);
            UpdateHoloOffset(new Vector3(holoData.holoOffset[0], holoData.holoOffset[1], holoData.holoOffset[2]));



            UpdateYRotation(holoData.YRot);

            holoScale = 1.0f; 
            SetNewHoloScale(holoData.scaleFactor);
            Debug.LogError("Holoscale after update = " + hologramOrigin.localScale);
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
