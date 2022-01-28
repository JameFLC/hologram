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
    


    private Vector3 cameraPosition = new Vector3(0,0.5f,-1.5f);
    
    
    private Vector3 holoOffset = new Vector3(0,0,0);
    private float holoYRotation = 0;
    private float holoScale = 1;
    private CanvasGroup UICanvasGroup;
    private bool isUIVisible = true;
    private float screenHeight = 1;
    private float proportion = 1;
    [HideInInspector]
    public bool isArtNetEnabled = false;




    // Start is called before the first frame update
    void Awake()
    {
        UICanvasGroup = GetComponent<CanvasGroup>();




    }

    private void Update()
    {   
        

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

   

    public void SetHoloScale()
    {

        if (!string.IsNullOrWhiteSpace(IFHoloScale.text))
        {
            UpdateHoloScale(SanetizeInput(IFHoloScale.text));
        }
    }
    public void UpdateHoloScale(float scale)
    {
        holoScale = scale;
        
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


    }



    public void UpdateCameraPosition()
    {
        if (screenHeight != 0)
        {
            proportion = virtualScreenHeight / screenHeight;


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
