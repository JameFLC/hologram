using UnityEngine;

public class ArtNetManager : MonoBehaviour
{
    private int firstChannel = 0;
    private ArtDotNet.ArtNetClient client;
    private SetupUI setupUI;

    // channel firste channel + 0 = screen height.big : 1 = 10 cm
    // channel firste channel + 1 = screen height.small : 1 = 1cm

    // channel firste channel + 2 = cam.x.big : 1 = 10cm
    // channel firste channel + 3 = cam.x.small : 1 = 1cm
    // channel firste channel + 4 = cam.y.big : 1  = 10cm
    // channel firste channel + 5 = cam.y.small : 1 = 1cm
    // channel firste channel + 6 = cam.z.big : 1 = 10cm
    // channel firste channel + 7 = cam.z.small : 1 = 1cm

    // channel firste channel + 8 = holo.x.big : 1 = 10cm
    // channel firste channel + 9 = holo.x.small : 1 = 1cm
    // channel firste channel + 10 = holo.y.big : 1  = 10cm
    // channel firste channel + 11 = holo.y.small : 1 = 1cm
    // channel firste channel + 12 = holo.z.big : 1 = 10cm
    // channel firste channel + 13 = holo.z.small : 1 = 1cm

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
            float newHeight = client.DMXdata[firstChannel] / 10.0f + client.DMXdata[firstChannel + 1] / 100.0f;
            if (screenHeight  != newHeight)
            {
                screenHeight = newHeight;
                setupUI.UpdateScreenHeight(screenHeight);
            }
            // CamPosX
            float newPosX = (-128 + client.DMXdata[firstChannel + 2]) / 10.0f + (-128 +client.DMXdata[firstChannel + 3]) / 100.0f;
            if (camPosition.x != newPosX)
            {
                camPosition.x = newPosX;
                setupUI.UpdateViewerXOffset(camPosition.x);
            }
            // CamPosY
            float newPosY = (-128 + client.DMXdata[firstChannel + 4]) / 10.0f + (-128 + client.DMXdata[firstChannel + 5]) / 100.0f;
            if (camPosition.y != newPosY)
            {
                camPosition.y = newPosY;
                setupUI.UpdateViewerYOffset(camPosition.y);
            }
            // CamPosZ
            float newPosZ = -(( client.DMXdata[firstChannel + 6]) / 10.0f + (client.DMXdata[firstChannel + 7]) / 100.0f);
            if (camPosition.z != newPosZ)
            {
                camPosition.z = newPosZ;
                setupUI.UpdateViewerZOffset(camPosition.z);
            }
        }
    }
}
