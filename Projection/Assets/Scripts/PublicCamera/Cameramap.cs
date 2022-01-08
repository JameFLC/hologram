using UnityEngine;

public class Cameramap : MonoBehaviour
{
    [SerializeField] Mesh mesh;
    [SerializeField] Shader proj;
    [SerializeField] GameObject VirtualScreen;
    [SerializeField] float RenderUpscale = 1;


    #region private properties
    Renderer ScreenRenderer;
    Material matProjection_;
    Camera cam_;
    Matrix4x4 projMatrix_;
    Vector3 posCache_;
    Quaternion rotCache_ = Quaternion.identity;
    RenderTexture tmp_;
    float screenSize;
    float ScreenSize = Screen.width * Screen.height;
    #endregion


    private void Awake()
    {
        cam_ = GetComponent<Camera>();
        matProjection_ = new Material(proj);
        ScreenRenderer = VirtualScreen.GetComponent<Renderer>();
        screenSize = Screen.width * Screen.height;
    }
    
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        transform.LookAt(ScreenRenderer.GetComponentInParent<Transform>());
        CameraSetting();
    
        var local_ = transform.worldToLocalMatrix;
    
        
        var store_ = RenderTexture.active;
 
        float currentScreenSize = Screen.width * Screen.height;
        // if texture is missing or window sze changed update rendter texture
        if (tmp_ == null || ScreenSize != currentScreenSize) tmp_ = new RenderTexture((int)(Screen.width * RenderUpscale), (int)(Screen.height * RenderUpscale), 0) ;
    
    
    
        Graphics.SetRenderTarget(tmp_);
        matProjection_.SetPass(0);
        matProjection_.SetMatrix("_WorldToCam", local_);
        matProjection_.SetMatrix("_ProjMatrix", projMatrix_);
        matProjection_.SetTexture("_RtCamera", src);
        float heightRatio = (float)Screen.width / Screen.height;
    
        matProjection_.SetFloat("_ScreenRatio", heightRatio);
    
        Graphics.DrawMeshNow(mesh, posCache_, rotCache_);
    
        ScreenRenderer.material.SetTexture("_MainTex", tmp_);
    
        RenderTexture.active = store_;
        Graphics.Blit(src, dst);
    
        posCache_ = ScreenRenderer.transform.position;
        rotCache_ = ScreenRenderer.transform.rotation;
    }

    void OnDisable()
    {
        Destroy(matProjection_);
        Destroy(tmp_); tmp_ = null;
    }

   

    void CameraSetting()
    {
        var center_ = ScreenRenderer.bounds.center;
        var size_ = ScreenRenderer.bounds.size;
        var x = size_.x / 2;
        var y = size_.y / 2;
        var z = size_.z / 2;
        var vertices_ = 8;
        var vtxWorldPos_ = new Vector3[vertices_];
        var vtxLocalPos_ = new Vector3[vertices_];
        var matrix_ = transform.worldToLocalMatrix;

        vtxWorldPos_ = new Vector3[]
        {
                center_ + new Vector3( x,  y,  z),
                center_ + new Vector3( x, -y,  z),
                center_ + new Vector3( x,  y, -z),
                center_ + new Vector3( x, -y, -z),
                center_ + new Vector3(-x,  y,  z),
                center_ + new Vector3(-x, -y,  z),
                center_ + new Vector3(-x,  y, -z),
                center_ + new Vector3(-x, -y, -z),
        };

        for (int i = 0; i < vertices_; i++)
        {
            vtxLocalPos_[i] = matrix_.MultiplyPoint(vtxWorldPos_[i]);
            vtxLocalPos_[i] /= vtxLocalPos_[i].z;
        }

        var c_ = matrix_.MultiplyPoint(center_);
        c_ /= c_.z;

        float distantX_ = 0;
        float distantY_ = 0;
        for (int i = 0; i < vertices_; i++)
        {
            var x_ = Mathf.Abs(vtxLocalPos_[i].x - c_.x);
            var y_ = Mathf.Abs(vtxLocalPos_[i].y - c_.y);
            if (x_ > distantX_) distantX_ = x_;
            if (y_ > distantY_) distantY_ = y_;
        }

        cam_.ResetProjectionMatrix();
        cam_.aspect = distantX_ / distantY_;
        cam_.fieldOfView = 2f * Mathf.Atan2(distantY_, 1) * Mathf.Rad2Deg;

        var ndsOffset_ = new Vector2(c_.x / distantX_, c_.y / distantY_);

        // Thing that make vew corner snap to screen corner
        //var pj = cam_.projectionMatrix;
        //pj[0, 2] = ndsOffset_.x;
        //pj[1, 2] = ndsOffset_.y;
        //cam_.projectionMatrix = pj;
       
        // projection setting
        projMatrix_ = Matrix4x4.Perspective(cam_.fieldOfView, cam_.aspect, 0, 1000);
        projMatrix_[0, 2] = -ndsOffset_.x;
        projMatrix_[1, 2] = -ndsOffset_.y;
    }
}
