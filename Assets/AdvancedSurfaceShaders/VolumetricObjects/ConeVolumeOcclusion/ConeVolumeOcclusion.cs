using UnityEngine;

[ExecuteInEditMode]
public class ConeVolumeOcclusion : VolumetricObjectBase
{
    public float coneHeight = 2f;
    public float coneAngle = 30f;
    public float startOffset = 0f;
    public int occlusionTextureSize = 128;

    private float previousConeHeight = 0f;
    private float previousConeAngle = 0f;
    private float previousStartOffset = 0f;
    private int previousOcclusionTextureSize = 128;

    private Transform coneCameraTransform = null;
    private Camera coneCamera = null;
    private RenderTexture coneCameraRT;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/Create Other/Volumetric Objects/Cone Volume Occlusion")]
    static public void CreateSphereVolume()
    {
        GameObject newObject = new GameObject("Cone Volume Occlusion");
        if (UnityEditor.SceneView.currentDrawingSceneView) UnityEditor.SceneView.currentDrawingSceneView.MoveToView(newObject.transform);
        ConeVolumeOcclusion coneVolume = (ConeVolumeOcclusion)newObject.AddComponent<ConeVolumeOcclusion>();
        coneVolume.enabled = false;
        coneVolume.enabled = true;
    }
#endif

    protected override void OnEnable()
    {
        if (volumeShader == "") PopulateShaderName();
        base.OnEnable();

        SetupCamera();
    }

    protected override void CleanUp()
    {
        if (coneCameraRT)
        {
            DestroyImmediate(coneCameraRT);
        }
        base.CleanUp();
    }

    public override void PopulateShaderName()
    {
        volumeShader = "Advanced SS/Volumetric/Cone Volume Occlusion";
    }

    public override bool HasChanged()
    {
        if (coneHeight != previousConeHeight ||
            coneAngle != previousConeAngle ||
            startOffset != previousStartOffset ||
            occlusionTextureSize != previousOcclusionTextureSize ||
            base.HasChanged())
        {
            return true;
        }
        return false;
    }

    protected override void SetChangedValues()
    {
        if (coneHeight < 0f) coneHeight = 0f;
        if (coneAngle >= 89f) coneAngle = 89f;
        if (occlusionTextureSize < 16) occlusionTextureSize = 16;
        previousConeHeight = coneHeight;
        previousConeAngle = coneAngle;
        previousStartOffset = startOffset;
        previousOcclusionTextureSize = occlusionTextureSize;
        base.SetChangedValues();

        SetupCamera();
    }

    //private void OnDrawGizmos()
    //{
    //    float angleRads = coneAngle * Mathf.Deg2Rad;
    //    float bottomRadius = ((Mathf.Atan(angleRads) * coneHeight) + (Mathf.Tan(angleRads) * coneHeight)) * 0.25f;

    //    Gizmos.DrawLine(transform.position, transform.position - Vector3.up * coneHeight);
    //    Gizmos.DrawLine(transform.position, transform.position - Vector3.up * coneHeight + Vector3.right * bottomRadius);
    //    Gizmos.DrawLine(transform.position - Vector3.up * coneHeight, transform.position - Vector3.up * coneHeight + Vector3.right * bottomRadius);
    //}

    public override void UpdateVolume()
    {
        float angleRads = coneAngle * 0.5f * Mathf.Deg2Rad;
        float bottomRadius = Mathf.Tan(angleRads) * (coneHeight * 2f);
        //float bottomRadius = ((Mathf.Atan(angleRads) * coneHeight) + (Mathf.Tan(angleRads) * coneHeight)) * 0.5f;
        float bottomRadiusHalf = bottomRadius * 0.5f;

        Vector3 halfBoxSize = new Vector3(bottomRadius, coneHeight, bottomRadius);

        if (meshInstance)
        {
            ScaleMesh(meshInstance, halfBoxSize, -Vector3.up * coneHeight * 0.5f);

            // Set bounding volume so modified vertices don't get culled
            Bounds bounds = new Bounds();
            bounds.SetMinMax(-halfBoxSize, halfBoxSize);
            meshInstance.bounds = bounds;
        }

        if (materialInstance)
        {
            materialInstance.SetVector("_ConeData", new Vector4(bottomRadiusHalf, coneHeight, startOffset, Mathf.Cos(angleRads)));
            materialInstance.SetVector("_TextureData", new Vector4(-textureMovement.x, -textureMovement.y, -textureMovement.z, (1f / textureScale)));
            materialInstance.SetFloat("_Visibility", visibility);
            materialInstance.SetColor("_Color", volumeColor);
            materialInstance.SetTexture("_MainTex", texture);
        }
    }

    private void CreateDepthCamera()
    {
        if (!coneCameraTransform) coneCameraTransform = transform.FindChild("ConeCamera");
        if (!coneCameraTransform)
        {
            GameObject camOb = new GameObject("ConeCamera");
            coneCameraTransform = camOb.transform;
            coneCameraTransform.parent = transform;
        }

        if (!coneCamera)
        {
            coneCamera = coneCameraTransform.camera;
        }
        if (!coneCamera)
        {
            coneCamera = coneCameraTransform.gameObject.AddComponent<Camera>();
            coneCamera.gameObject.AddComponent<DrawDepth>();
            coneCamera.enabled = false;
        }
    }

    private RenderTexture ConeRenderTexture()
    {
        if (coneCameraRT)
        {
            if (coneCameraRT.width != occlusionTextureSize || coneCameraRT.height != occlusionTextureSize)
            {
#if UNITY_EDITOR
                DestroyImmediate(coneCameraRT);
#else
                Destroy(coneCameraRT);
#endif
            }
            else
            {
                return coneCameraRT;
            }
        }

        coneCameraRT = new RenderTexture(occlusionTextureSize, occlusionTextureSize, 0);
        materialInstance.SetTexture("_OcclusionTex", coneCameraRT);
        return coneCameraRT;
    }

    private void SetupCamera()
    {
        CreateDepthCamera();

        coneCameraTransform.position = transform.position;
        coneCameraTransform.rotation = Quaternion.LookRotation(-transform.up, transform.forward);
        coneCamera.farClipPlane = coneHeight;
        coneCamera.nearClipPlane = 0.01f;
        coneCamera.fieldOfView = coneAngle;
        coneCamera.clearFlags = CameraClearFlags.SolidColor;
        coneCamera.backgroundColor = Color.black;
        coneCamera.targetTexture = ConeRenderTexture();

#if UNITY_EDITOR
        //UnityEditor.EditorUtility.SetDirty(coneCameraTransform);
        //UnityEditor.SceneView.RepaintAll();
        DrawDepth drawDepth = coneCameraTransform.GetComponent<DrawDepth>();
        if (drawDepth == null)
        {
            drawDepth = coneCameraTransform.gameObject.AddComponent<DrawDepth>();
        }
        drawDepth.DoDepthRender();
#endif
    }
}