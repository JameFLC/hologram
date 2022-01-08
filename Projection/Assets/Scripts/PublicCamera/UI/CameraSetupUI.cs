using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        IFScreenHeight.characterValidation = InputField.CharacterValidation.Decimal;

        IFViewerOffsetX.characterValidation = InputField.CharacterValidation.Decimal;
        IFViewerOffsetY.characterValidation = InputField.CharacterValidation.Decimal;
        IFViewerOffsetZ.characterValidation = InputField.CharacterValidation.Decimal;
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
    public void Test(string UwU)
    {
        Debug.Log("Test at " + Time.time);
    }
    public void SetScreenHeight()
    {
        screenHeight = float.Parse(IFScreenHeight.text);
        UpdateCameraPosition();
    }
    public void SetViwerXOffset()
    {
        cameraPosition.x = float.Parse(IFViewerOffsetX.text);
        UpdateCameraPosition();
    }

    public void SetViwerYOffset()
    {
        cameraPosition.y = float.Parse(IFViewerOffsetY.text) ;
        UpdateCameraPosition();
    }
    public void SetViwerZOffset()
    {
        cameraPosition.z = -float.Parse(IFViewerOffsetZ.text);
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        proportion = virtualScreenHeight / screenHeight;
        
        publicCamera.position = (cameraPosition * proportion);
    }

    
}
