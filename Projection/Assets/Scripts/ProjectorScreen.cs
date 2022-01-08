using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorScreen : MonoBehaviour
{
    // Variables   
    
    [SerializeField]
    private MeshRenderer screen;
    [SerializeField]
    private Camera publicCamera;

    private RenderTexture viewTexture;

    // Start is called before the first frame update
    void Awake()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        CreateViewTexture();
    }

    // Function based on Portals by https://github.com/SebLague (MIT Liscence)

    void CreateViewTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            // Render the view from the portal camera to the view texture
            publicCamera.targetTexture = viewTexture;
            // Display the view texture on the screen of the linked portal
            screen.material.SetTexture("_MainTex", viewTexture);
        }
    }

    public void Render()
    {
        screen.enabled = false;
        CreateViewTexture();

        publicCamera.enabled = true;
        screen.enabled = true;
    }
    
}
