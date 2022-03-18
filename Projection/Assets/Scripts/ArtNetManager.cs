using UnityEngine;

public class ArtNetManager : MonoBehaviour
{
    private int firstChannel = 0;
    private ArtDotNet.ArtNetClient client;
    private SetupUI setupUI;
    private int bigScale = 50;
    // channel + 0 = screen height.big : 1 = 50 cm
    // channel + 1 = screen height.small : 1 = 1cm

    // channel + 2 = cam.x.big : 1 = 50cm
    // channel + 3 = cam.x.small : 1 = 1cm
    // channel + 4 = cam.y.big : 1  = 50cm
    // channel + 5 = cam.y.small : 1 = 1cm
    // channel + 6 = cam.z.big : 1 = 50cm
    // channel + 7 = cam.z.small : 1 = 1cm

    // channel + 8 = holo.x.big : 1 = 50cm
    // channel + 9 = holo.x.small : 1 = 1cm
    // channel + 10 = holo.y.big : 1  = 50cm
    // channel + 11 = holo.y.small : 1 = 1cm
    // channel + 12 = holo.z.big : 1 = 50cm
    // channel + 13 = holo.z.small : 1 = 1cm

    private float screenHeight = 1;
    private Vector3 camPosition = new Vector3(0, 0.5f, -1.5f);

    private Vector3 holoOffset = new Vector3(0, 0, 0);
    private float holoScale = 1;
    private float holoYRotation = 0;


    // Start is called before the first frame update
    private void Start()
    {
        client = GetComponent<ArtDotNet.ArtNetClient>();
        setupUI = GetComponent<SetupUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if ArtNet is enabled to update values 
        if (setupUI.isArtNetEnabled)
        {
            // Screen Height
            float newHeight = client.DMXdata[firstChannel] / (100.0f/ bigScale) + client.DMXdata[firstChannel + 1] / 100.0f;
            if (screenHeight  != newHeight)
            {
                screenHeight = newHeight;
                setupUI.UpdateScreenHeight(screenHeight);
            }
            // CamPosX
            float newPosX = (-128 + client.DMXdata[firstChannel + 2]) / (100.0f / bigScale) + (-128 +client.DMXdata[firstChannel + 3]) / 100.0f;
            if (camPosition.x != newPosX)
            {
                camPosition.x = newPosX;
                setupUI.UpdateViewerXOffset(camPosition.x);
            }
            // CamPosY
            float newPosY = (-128 + client.DMXdata[firstChannel + 4]) / (100.0f / bigScale) + (-128 + client.DMXdata[firstChannel + 5]) / 100.0f;
            if (camPosition.y != newPosY)
            {
                camPosition.y = newPosY;
                setupUI.UpdateViewerYOffset(camPosition.y);
            }
            // CamPosZ
            float newPosZ = -(( client.DMXdata[firstChannel + 6]) / (100.0f / bigScale) + (client.DMXdata[firstChannel + 7]) / 100.0f);
            if (camPosition.z != newPosZ)
            {
                camPosition.z = newPosZ;
                setupUI.UpdateViewerZOffset(camPosition.z);
            }
            // CamPosX
            float newHoloPosX = (-128 + client.DMXdata[firstChannel + 8]) / (100.0f / bigScale) + (-128 + client.DMXdata[firstChannel + 9]) / 100.0f;
            if (holoOffset.x != newHoloPosX)
            {
                holoOffset.x = newHoloPosX;
                setupUI.UpdateHoloXOffset(holoOffset.x);
            }
            // CamPosY
            float newHoloPosY = (-128 + client.DMXdata[firstChannel + 10]) / (100.0f / bigScale) + (-128 + client.DMXdata[firstChannel + 11]) / 100.0f;
            if (holoOffset.y != newHoloPosY)
            {
                holoOffset.y = newHoloPosY;
                setupUI.UpdateHoloYOffset(holoOffset.y);
            }
            // CamPosY
            float newHoloPosZ = (-128 + client.DMXdata[firstChannel + 12]) / (100.0f / bigScale) + (-128 + client.DMXdata[firstChannel + 13]) / 100.0f;
            if (holoOffset.z != newHoloPosZ)
            {
                holoOffset.z = newHoloPosZ;
                setupUI.UpdateHoloZOffset(holoOffset.z);
            }
            float newScale = client.DMXdata[firstChannel + 14] / 40.0f;
            if (holoScale != newScale)
            {
                holoScale = newScale;
                setupUI.UpdateHoloScale(holoScale);
            }
            const int steps = 4 * 6;
            float rotationByStep = -180 + (int)((((client.DMXdata[firstChannel +15] + 1) * steps) / 255) / (steps * 1.0f) * 360) ;
            float newRotation = rotationByStep + (-128 + client.DMXdata[firstChannel + 16])*2 + -128 + client.DMXdata[firstChannel + 17];
            if (holoYRotation != newRotation)
            {
                Debug.Log("Rotation = " + newRotation);
                holoYRotation = newRotation;
                setupUI.UpdateYRotation(holoYRotation);
            }
        }
        
    }
}
